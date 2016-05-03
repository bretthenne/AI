using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace AINav
{
    public class Controller
    {
        delegate Node GetNodeDelegate();        

        public enum NodeEditState
        {
            DISABLED,
            WALKABLE,
            NON_WALKABLE,
            START,
            FINISH,
        }        

        #region Members 
            
        /* private member fields */
        private View view;
        private Model model;
        private List<BackgroundWorker> workers;        
        private bool cancelled;
        private int running;        
        private Map.MapType mapType;        
        private Color nodeEditBackColor;
        private Color nodeEditForeColor;
        private NodeEditState nodeEditState;
        private Agent.AgentID nodeEditFinishAgentId;        
        private Agent.AgentID nodeEditStartAgentId;
        private volatile int tempMapSize;
        private volatile int tempPercentWalkable;
        private volatile int tempNumberofAgents;
        private List<Node> mouseDownNodes;
        private BackgroundWorker mouseDownBackgroundWorker;
        private bool mouseDownRunning;
        private Synchronization synchronization;

        /* static variables */
        private static ManualResetEvent mre;        
        private static object aLock;

        /* private constants */
        private const string parametersChanged = "Map parameters changed, click Random or Empty Map to apply changes";

        #endregion Members

        #region Constructor

        static Controller()
        {
            mre = new ManualResetEvent(false);
            aLock = new object();
        }

        public Controller(Model model, View view)
        {            
            /* initialize class fields */
            this.model = model;
            this.view = view;            
            this.workers = new List<BackgroundWorker>();
            this.cancelled = false;
            this.running = 0; 
            this.mapType = Map.MapType.Empty;            
            this.nodeEditBackColor = Color.White;
            this.nodeEditForeColor = Color.White;
            this.nodeEditBackColor = Color.White;
            this.nodeEditState = NodeEditState.DISABLED;
            this.nodeEditFinishAgentId = Agent.AgentID.Agent_0;
            this.nodeEditStartAgentId = Agent.AgentID.Agent_0;

            /* initialize the volatile variables */
            tempMapSize = model.getSimulation().get(View.ViewNumericUpDown.MapSize);
            tempPercentWalkable = model.getSimulation().get(View.ViewNumericUpDown.PercentWalkable);
            tempNumberofAgents = model.getSimulation().get(View.ViewNumericUpDown.NumberOfAgents);

            /* generate an empty map without agents */
            generateMapCommon(null, false);
            model.getSimulation().setMapID(0);

            /* mouse event handlers */
            view.getPanelMap().MouseDown += new MouseEventHandler(Controller_MouseDown);
            view.getPanelMap().MouseUp += new MouseEventHandler(Controller_MouseUp);
            view.getPanelMap().MouseClick += new MouseEventHandler(Controller_MouseClick);
            
            this.mouseDownNodes = new List<Node>();
            mouseDownRunning = false;
            mouseDownBackgroundWorker = new BackgroundWorker();            
            mouseDownBackgroundWorker.DoWork += new DoWorkEventHandler(collectNodes_DoWork);            
        }

        #endregion Constructor

        #region EventHandlers

        public void Controller_MouseDown(object sender, MouseEventArgs e)
        {
            /* Check if mouse down is within map region */
            if (this.nodeEditState == NodeEditState.WALKABLE | 
                this.nodeEditState == NodeEditState.NON_WALKABLE)
            {
                this.mouseDownRunning = true;                
                mouseDownBackgroundWorker.RunWorkerAsync();
            }
        }

        #region ComboBox_SelectedIndexChanged                

        public void ComboBox_SelectedIndexChanged(object sender, View.ViewComboBox eventHandler)
        {
            ComboBox cb = sender as ComboBox;
            Debug.WriteLine(eventHandler.ToString() + " " + cb.SelectedItem.ToString());

            switch (eventHandler)
            {
                case View.ViewComboBox.AlgorithmType:
                    AlgorithmType algorithmType = (AlgorithmType)cb.SelectedItem;                    
                    model.getSimulation().setAlgorithmType(algorithmType);
                    view.updateHeuristicMethodItems(algorithmType);
                    ComboBox heuristicMethodComboBox = view.getComboBox(View.ViewComboBox.HeuristicMethod);
                    switch (algorithmType)
                    {
                        case AlgorithmType.Dijkstra:
                            heuristicMethodComboBox.SelectedItem = Node.Method.None;
                            model.getSimulation().setHeuristicMethod(Node.Method.None);
                            break;
                        case AlgorithmType.AStar:
                            heuristicMethodComboBox.SelectedItem = Node.Method.Euclidean;
                            model.getSimulation().setHeuristicMethod(Node.Method.Euclidean);
                            break;
                        default:
                            break;
                    }
                    break;
                case View.ViewComboBox.HeuristicMethod:
                    Node.Method heuristicMethod = (Node.Method)cb.SelectedItem;                    
                    model.getSimulation().setHeuristicMethod(heuristicMethod);  
                    break;
                case View.ViewComboBox.FovMethod:
                    Node.Method fovMethod = (Node.Method)cb.SelectedItem;                    
                    model.getSimulation().setFieldOfViewMethod(fovMethod);
                    break;
                case View.ViewComboBox.AgentCooperation:
                    View.AgentCooperation agentCooperation = (View.AgentCooperation)cb.SelectedItem;                    
                    model.getSimulation().setAgentCooperation(agentCooperation);
                    break;
                case View.ViewComboBox.ViewableAgent:
                    Agent.AgentID agentID = (Agent.AgentID)cb.SelectedItem;                    
                    model.getSimulation().setViewableAgent(agentID);
                    switch (agentID)
                    {
                        case Agent.AgentID.Agent_All:
                            /* set all agents to active */
                            foreach (Agent agent in model.getAgents())
                                agent.setActive(true);                            
                            break;
                        default:
                            foreach (Agent agent in model.getAgents())
                                agent.setActive(false);
                            Agent a = model.getAgent(agentID);
                            if (a != null)
                                a.setActive(true);                            
                            break;
                    }
                    break;
                case View.ViewComboBox.Visualizations:
                    View.Visualizations visualizations = ((View.Visualizations)cb.SelectedItem);                    
                    model.getSimulation().setVisualizations(visualizations);
                    break;
                case View.ViewComboBox.PathPersistence:
                    View.PathPersistence pathPersistence = ((View.PathPersistence)cb.SelectedItem);                    
                    model.getSimulation().setPersistenceEnabled(pathPersistence);
                    break;
                default:
                    throw new Exception("combo box event handler invalid");
            }
        }

        #endregion ComboBox_SelectedIndexChanged

        #region NumericUpDown_ValueChanged        

        public void NumericUpDown_ValueChanged(object sender, View.ViewNumericUpDown eventHandler)
        {
            NumericUpDown nud = sender as NumericUpDown;
            int value = (int)nud.Value;
            Debug.WriteLine(eventHandler.ToString() + " " + value.ToString());

            switch (eventHandler)
            {
                case View.ViewNumericUpDown.FieldOfView:
                    model.getSimulation().set(View.ViewNumericUpDown.FieldOfView, value);
                    break;
                case View.ViewNumericUpDown.MapSize: 
                    tempMapSize = value;            
                    view.setStatusText(parametersChanged);
                    break;
                case View.ViewNumericUpDown.PercentWalkable: 
                    tempPercentWalkable = value;
                    view.setStatusText(parametersChanged);
                    break;
                case View.ViewNumericUpDown.NumberOfAgents:
                    tempNumberofAgents = value;
                    view.setStatusText(parametersChanged);
                    break;
                case View.ViewNumericUpDown.MaxSteps:
                    model.getSimulation().set(View.ViewNumericUpDown.MaxSteps, value);
                    break;
                case View.ViewNumericUpDown.StepDelay:
                    model.getSimulation().set(View.ViewNumericUpDown.StepDelay, value);
                    break;
                case View.ViewNumericUpDown.VisualizationDelay:
                    model.getSimulation().set(View.ViewNumericUpDown.VisualizationDelay, value);
                    break;
                default:
                    throw new Exception("invalid numeric up/down event handler");
            } 
        }

        #endregion NumericUpDown_ValueChanged

        #region Button_Click                

        private Agent.AgentID incNodeEditAgentId(Model model, Agent.AgentID init)
        {
            int index = (int)init;            
            int maxIndex = (model.getSimulation().get(View.ViewNumericUpDown.NumberOfAgents) - 1);

            index = (index == maxIndex) ? (int)Agent.AgentID.Agent_0 : index + 1;

            this.nodeEditBackColor = model.getAgent((Agent.AgentID)index).getColor(Agent.ColorType.BackColor);
            this.nodeEditForeColor = model.getAgent((Agent.AgentID)index).getColor(Agent.ColorType.ForeColor);

            return (Agent.AgentID)index;
        }

        public void Button_Click(object sender, View.ViewButton eventHandler)
        {
            Debug.WriteLine(eventHandler.ToString() + " Clicked"); 
            
            switch (eventHandler)
            {
                case View.ViewButton.RandomMap:
                    mapType = Map.MapType.Random;                    
                    break;
                case View.ViewButton.EmptyMap:
                    mapType = Map.MapType.Empty;                   
                    break;                
                case View.ViewButton.ResetMap:
                    DrawingControl.SuspendDrawing(view.getPanelMap());  
                    model.Reset(true); 
                    DrawingControl.ResumeDrawing(view.getPanelMap());
                    break;                
            }
            
            switch (eventHandler)
            {   
                case View.ViewButton.RandomMap:
                case View.ViewButton.EmptyMap: 
                    /* save the temp variables */
                    model.getSimulation().set(View.ViewNumericUpDown.MapSize, tempMapSize);
                    model.getSimulation().setCellSize(view.PANEL_SIZE / tempMapSize);
                    model.getSimulation().set(View.ViewNumericUpDown.PercentWalkable, tempPercentWalkable);
                    model.getSimulation().set(View.ViewNumericUpDown.NumberOfAgents, tempNumberofAgents);

                    /* generate map */
                    generateMapCommon(null, true);

                    /* initialize the node edit variables */
                    nodeEditStartAgentId = Agent.AgentID.Agent_0;
                    nodeEditFinishAgentId = Agent.AgentID.Agent_0;
                    nodeEditBackColor = model.getAgent(Agent.AgentID.Agent_0).getColor(Agent.ColorType.BackColor);
                    nodeEditForeColor = model.getAgent(Agent.AgentID.Agent_0).getColor(Agent.ColorType.ForeColor);

                    /* update view */
                    view.updateAgentItems();
                    view.getButton(View.ViewButton.Start).Enabled = true;
                    view.getRadioButton(View.ViewRadioButton.EditWalkable).Enabled = true;
                    view.getRadioButton(View.ViewRadioButton.EditNonWalkable).Enabled = true;
                    view.setStatusText("Change Parameters, Edit Map, or Click Start");
                    view.buttonEditStartUpdate(nodeEditBackColor, nodeEditForeColor, "S", true);
                    view.buttonEditFinishUpdate(nodeEditBackColor, nodeEditForeColor, "F", true);
                    break;                
                case View.ViewButton.Start:                    
                    cancelled = false;
                    nodeEditState = NodeEditState.DISABLED;
                    model.getMap().setNodes(Node.Cost.Movement, 0.0);
                    model.getMap().setNodes(Node.Cost.Heuristic, 0.0);
                    model.getMap().setNodes(Node.Cost.Total, 0.0);

                    foreach (Agent agent in model.getAgents())
                    {
                        agent.getMap().setNodes(Node.Cost.Movement, 0.0);
                        agent.getMap().setNodes(Node.Cost.Heuristic, 0.0);
                        agent.getMap().setNodes(Node.Cost.Total, 0.0);
                        agent.getMap().setNodes(Node.Flag.IsWalkable, true);
                        agent.getMap().setNodes(Node.Flag.IsVisible, false);
                        agent.getMap().setNodes(Node.Flag.IsShared, false);
                    }

                    /* update the view */
                    view.setStatusText("");
                    view.getButton(View.ViewButton.Cancel).Enabled = true;
                    view.getButton(View.ViewButton.Start).Enabled = false;
                    view.getButton(View.ViewButton.RandomMap).Enabled = false;
                    view.getButton(View.ViewButton.EmptyMap).Enabled = false;                             
                    view.controlEnable(false);
                    view.getComboBox(View.ViewComboBox.Visualizations).Enabled = false;

                    DrawingControl.SuspendDrawing(view.getPanelMap());            
                    model.Reset(false);                    
                    DrawingControl.ResumeDrawing(view.getPanelMap());

                    synchronization = new Synchronization(model.getSimulation().get(View.ViewNumericUpDown.NumberOfAgents));

                    /* start the agent background threads */
                    foreach (Agent agent in model.getAgents())
                    {
                        running++;
                        agent.setNode(Agent.NodeType.Current, agent.getNode(Agent.NodeType.Start));
                        agent.setNode(Agent.NodeType.Target, agent.getNode(Agent.NodeType.Finish));
                        agent.getMap().setNodes(Node.Flag.IsPath, false);                        
                        agent.getNode(Agent.NodeType.Start).setFlag(Node.Flag.IsWalkable, true);
                        agent.getNode(Agent.NodeType.Finish).setFlag(Node.Flag.IsWalkable, true);
                        BackgroundWorker bgw = new BackgroundWorker();
                        bgw.WorkerSupportsCancellation = true;                        
                        bgw.DoWork += new DoWorkEventHandler(Start_DoWork);
                        bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Start_RunWorkerCompleted);                        
                        bgw.RunWorkerAsync(agent);
                        workers.Add(bgw);
                    }

                    /* init the parameters for un-applied map changes */
                    view.getNumericUpDown(View.ViewNumericUpDown.MapSize).Value = model.getSimulation().get(View.ViewNumericUpDown.MapSize);
                    view.getNumericUpDown(View.ViewNumericUpDown.PercentWalkable).Value = model.getSimulation().get(View.ViewNumericUpDown.PercentWalkable);
                    view.getNumericUpDown(View.ViewNumericUpDown.NumberOfAgents).Value = model.getSimulation().get(View.ViewNumericUpDown.NumberOfAgents);
                    break;
                case View.ViewButton.Cancel:                    
                    cancelled = true;
                    view.getButton(View.ViewButton.Cancel).Enabled = false;
                    view.getButton(View.ViewButton.Start).Enabled = true;
                    view.getButton(View.ViewButton.RandomMap).Enabled = true;
                    view.getButton(View.ViewButton.EmptyMap).Enabled = true;                               
                    view.controlEnable(true);
                    foreach (BackgroundWorker bgw in workers) { bgw.CancelAsync(); }
                    break;
                case View.ViewButton.ExportCsv:                    
                    StringBuilder result = new StringBuilder(); 
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "*.csv|";
                    saveFileDialog.OverwritePrompt = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != null)
                    {  
                        ListView listView = view.getSimulationListView();
                    
                        /* export the column headers */
                        foreach (ColumnHeader ch in listView.Columns)                    
                            result.Append(ch.Text + ",");
                        result.AppendLine();

                        /* export the data rows */
                        foreach (ListViewItem listItem in listView.Items)
                        {
                            foreach (ListViewItem.ListViewSubItem lvs in listItem.SubItems)                    
                                result.Append(lvs.Text + ",");                     
                            result.AppendLine();
                        }

                        try
                        {
                            File.WriteAllText(saveFileDialog.FileName + ".csv", result.ToString());                    
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "CSV Export Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                case View.ViewButton.ClearResults:
                    model.getResults().clear();
                    break;                
            }
        }

        #endregion Button_Click

        #region RadioButton_Click

        public void RadioButton_Click(object sender, View.ViewRadioButton eventHandler)
        {
            Debug.WriteLine(eventHandler.ToString() + " Clicked");

            DrawingControl.SuspendDrawing(view.getPanelMap());
            model.getMap().setNodes(Node.Flag.IsVisible, true);
            model.getMap().repaint();
            model.paintAgents();
            DrawingControl.ResumeDrawing(view.getPanelMap());

            switch (eventHandler)
            {
                case View.ViewRadioButton.EditStart:                    
                    nodeEditStartAgentId = incNodeEditAgentId(model, nodeEditStartAgentId);
                    view.buttonEditStartUpdate(nodeEditBackColor, nodeEditForeColor, "S", true);
                    this.nodeEditState = NodeEditState.START;
                    view.setStatusText("Click Map to Change Agent " + (int)nodeEditStartAgentId + " Start Node");
                    break;
                case View.ViewRadioButton.EditFinish:
                    this.nodeEditState = NodeEditState.FINISH;
                    nodeEditFinishAgentId = incNodeEditAgentId(model, nodeEditFinishAgentId);
                    view.buttonEditFinishUpdate(nodeEditBackColor, nodeEditForeColor, "F", true);
                    view.setStatusText("Click Map to Change Agent " + (int)nodeEditFinishAgentId + " Finish Node");
                    break;
                case View.ViewRadioButton.EditWalkable:
                    this.nodeEditState = NodeEditState.WALKABLE;
                    this.nodeEditBackColor = Color.White;
                    this.nodeEditForeColor = Color.White;
                    view.setStatusText("Click Map to Change Selected Node to Walkable");
                    break;
                case View.ViewRadioButton.EditNonWalkable:
                    this.nodeEditState = NodeEditState.NON_WALKABLE;
                    this.nodeEditBackColor = Color.Gray;
                    this.nodeEditForeColor = Color.Gray;
                    view.setStatusText("Click Map to Change Selected Node to Non-Walkable");
                    break;                
            }
        }

        #endregion RadioButton_Click

        #endregion EventHandlers

        #region HelperMethods

        private void PaintAgentMap(Agent agent)
        {
            Debug.WriteLine("Painting " + agent.getId().ToString() + " Map");

            for (int i = 0; i < agent.getMap().getNodes().Count; i++)
            {
                Node agentMapNode = agent.getMap().getNode(i);
                Node sharedMapNode = model.getMap().getNode(i);
                sharedMapNode.setFlag(Node.Flag.IsPath, agentMapNode.getFlag(Node.Flag.IsPath));
                sharedMapNode.setFlag(Node.Flag.IsSpecial, agentMapNode.getFlag(Node.Flag.IsSpecial));
                sharedMapNode.setFlag(Node.Flag.IsVisible, agentMapNode.getFlag(Node.Flag.IsVisible));
                sharedMapNode.setFlag(Node.Flag.IsWalkable, agentMapNode.getFlag(Node.Flag.IsWalkable));
                sharedMapNode.setCost(Node.Cost.Movement, agentMapNode.getCost(Node.Cost.Movement));
                sharedMapNode.setCost(Node.Cost.Heuristic, agentMapNode.getCost(Node.Cost.Heuristic));
                sharedMapNode.setCost(Node.Cost.Total, agentMapNode.getCost(Node.Cost.Total));
            }
            model.Reset(true);
        }

        private void setVisualization(View.Visualizations visualizations)
        {
            ComboBox cb = view.getComboBox(View.ViewComboBox.Visualizations);
            cb.Enabled = ((View.Visualizations)(cb.SelectedItem) == View.Visualizations.Enabled) ? true : false;
            cb.SelectedItem = visualizations;
            model.getSimulation().setVisualizations(visualizations);
        }

        private Node getClickedNode()
        {
            Point p1 = view.getPanelMap().FindForm().PointToScreen(
                        view.getPanelMap().Parent.PointToScreen(view.getPanelMap().Location));
            Point p2 = view.PointToScreen(Cursor.Position);
            int cellSize = model.getSimulation().getCellSize();
            Point p = new Point((p2.X - p1.X) / cellSize, (p2.Y - p1.Y) / cellSize);            

            /* return null if outside map */
            int max = model.getSimulation().get(View.ViewNumericUpDown.MapSize);
            if (p.X >= max | p.Y >= max | p.X < 0 | p.Y < 0)
                return null;
            else
                return model.getMap().getNodeAtLocation(p);
        }
        
        private void generateMapCommon(object sender, bool withAgents)
        {
            Panel panelMap = view.getPanelMap();

            DrawingControl.SuspendDrawing(panelMap);

            /* disable controls generating map */
            view.getButton(View.ViewButton.Start).Enabled = false;
            view.controlEnable(false);

            /* remove nodes from panel map */
            panelMap.Controls.Clear();

            /* create the new map */
            model.getMap().create(mapType, model.getSimulation().get(View.ViewNumericUpDown.MapSize), model.getSimulation().getCellSize(), model.getSimulation().get(View.ViewNumericUpDown.PercentWalkable));
            model.getSimulation().incMapID();

            /* create and paint the agents */
            if (withAgents)
            {
                int mapSize = model.getSimulation().get(View.ViewNumericUpDown.MapSize);
                int cellSize = view.PANEL_SIZE / mapSize;
                model.createAgents(mapSize, cellSize);
                model.paintAgents();                
            }

            /* reset all agent nodes to visible */
            foreach (Agent agent in model.getAgents())            
                agent.getMap().setNodes(Node.Flag.IsVisible, true);

            /* add nodes to panel map */
            for (int x = 0; x < model.getSimulation().get(View.ViewNumericUpDown.MapSize); x++)
                for (int y = 0; y < model.getSimulation().get(View.ViewNumericUpDown.MapSize); y++)
                    view.getPanelMap().Controls.Add((Label)model.getMap().getNodeAtLocation(new Point(x, y)));                
            
            view.controlEnable(true);
            view.getButton(View.ViewButton.Start).Enabled = true;
            DrawingControl.ResumeDrawing(panelMap);
        }               
          
        public void Controller_MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseDownRunning = false;
        }

        public void Controller_MouseHover(object sender, EventArgs e)
        {
            Debug.WriteLine("mouse hover");
        }

        public void Controller_MouseClick(object sender, MouseEventArgs e)
        {
            Agent agent;
            Node sharedMapNode = null;
            Node agentMapNode = null;
            Color backColor, foreColor;

            int cellSize = model.getSimulation().getCellSize();
            int x = e.Location.X / cellSize;
            int y = e.Location.Y / cellSize;
            Point position = new Point(x, y);

            switch (e.Button)
            {
                case MouseButtons.Left:

                    sharedMapNode = model.getMap().getNodeAtLocation(position);
                    Debug.WriteLine("mouse click left:  (" + sharedMapNode.getPosition().X + "," + sharedMapNode.getPosition().Y + ")");
                    
                    switch (this.nodeEditState)
                    {
                        case NodeEditState.DISABLED:                           
                        case NodeEditState.WALKABLE:
                        case NodeEditState.NON_WALKABLE:
                            return;
                        case NodeEditState.START:
                            if (!model.isSpecialNode(sharedMapNode))
                            {
                                /* old start node */
                                agent = model.getAgent(nodeEditStartAgentId);
                                Node sharedMapNodeStartCurrent = model.getMap().getNodeAtLocation(agent.getNode(Agent.NodeType.Start).getPosition());
                                backColor = Color.White;
                                foreColor = Color.White;
                                sharedMapNodeStartCurrent.repaintNode(backColor, foreColor, "");
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartCurrent.getPosition()).repaintNode(backColor, foreColor, "");
                                sharedMapNodeStartCurrent.setFlag(Node.Flag.IsWalkable, true);
                                sharedMapNodeStartCurrent.setFlag(Node.Flag.IsSpecial, false);
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartCurrent.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartCurrent.getPosition()).setFlag(Node.Flag.IsSpecial, false);

                                /* new start node */
                                agent.setNode(Agent.NodeType.Start, sharedMapNode);

                                /* paint the new start node */
                                Node sharedMapNodeStartNew = model.getMap().getNodeAtLocation(sharedMapNode.getPosition());
                                backColor = agent.getColor(Agent.ColorType.BackColor);
                                foreColor = agent.getColor(Agent.ColorType.ForeColor);
                                sharedMapNodeStartNew.repaintNode(backColor, foreColor, "S");
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartNew.getPosition()).repaintNode(backColor, foreColor, "S"); 
                                sharedMapNodeStartNew.setFlag(Node.Flag.IsWalkable, true);
                                sharedMapNodeStartNew.setFlag(Node.Flag.IsSpecial, true);
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartNew.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                                agent.getMap().getNodeAtLocation(sharedMapNodeStartNew.getPosition()).setFlag(Node.Flag.IsSpecial, true);
                            }
                            break;
                        case NodeEditState.FINISH:
                            if (!model.isSpecialNode(sharedMapNode))
                            {
                                /* old finish node */
                                agent = model.getAgent(nodeEditFinishAgentId);
                                Node nodeFinishCurrent = model.getMap().getNodeAtLocation(agent.getNode(Agent.NodeType.Finish).getPosition());
                                backColor = Color.White;
                                foreColor = Color.White;
                                nodeFinishCurrent.repaintNode(backColor, foreColor, "");
                                agent.getMap().getNodeAtLocation(nodeFinishCurrent.getPosition()).repaintNode(backColor, foreColor, "");
                                nodeFinishCurrent.setFlag(Node.Flag.IsWalkable, true);
                                nodeFinishCurrent.setFlag(Node.Flag.IsSpecial, false);
                                agent.getMap().getNodeAtLocation(nodeFinishCurrent.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                                agent.getMap().getNodeAtLocation(nodeFinishCurrent.getPosition()).setFlag(Node.Flag.IsSpecial, false);
                                
                                /* set the new finish node */
                                agent.setNode(Agent.NodeType.Finish, sharedMapNode);

                                /* new finish node */
                                Node nodeFinishNew = model.getMap().getNodeAtLocation(sharedMapNode.getPosition());
                                backColor = agent.getColor(Agent.ColorType.BackColor);
                                foreColor = agent.getColor(Agent.ColorType.ForeColor);
                                nodeFinishNew.repaintNode(backColor, foreColor, "F");
                                agent.getMap().getNodeAtLocation(nodeFinishNew.getPosition()).repaintNode(backColor, foreColor, "F");                                
                                nodeFinishNew.setFlag(Node.Flag.IsWalkable, true);
                                nodeFinishNew.setFlag(Node.Flag.IsSpecial, true);
                                agent.getMap().getNodeAtLocation(nodeFinishNew.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                                agent.getMap().getNodeAtLocation(nodeFinishNew.getPosition()).setFlag(Node.Flag.IsSpecial, true);
                            }
                            break;
                    }                    
                                      
                    break; 
               
                case MouseButtons.Right:
                    try
                    {
                        ToolTip toolTip;
                        IWin32Window win;

                        switch (model.getSimulation().getViewableAgent())
                        {
                            case Agent.AgentID.Agent_All:
                                toolTip = new ToolTip();
                                win = view.getPanelMap();
                                toolTip.Show("Select Viewable Agent in order to see node information", win, 415, 300, 5000);
                                break;                            
                            default:
                                Agent.AgentID agentId = model.getSimulation().getViewableAgent();
                                agent = model.getAgent(agentId);
                                agentMapNode = agent.getMap().getNodeAtLocation(position);
                                Debug.WriteLine("mouse click right:  (" + agentMapNode.getPosition().X + "," + agentMapNode.getPosition().Y + ")");
                                /* show tooltip on right mouse click */
                                toolTip = new ToolTip();
                                win = view.getPanelMap();
                                toolTip.Show("Agent:  " + agentId.ToString() + "\n" + agentMapNode.toString(), win, 415, 300, 5000);
                                break;
                        }                        
                    }
                    catch { }
                    break;                          
            }            
        }        
        
        #endregion HelperMethods

        #region BackgroundWorkerMethods

        private void collectNodes_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;            

            while (this.mouseDownRunning)
            {
                /* Thread safe call to get the node clicked */
                Node sharedMapNode = ((Node)view.Invoke(new GetNodeDelegate(getClickedNode)));
                if (sharedMapNode != null)
                {
                    //Debug.WriteLine("(" + sharedMapNode.getPosition().X + "," + sharedMapNode.getPosition().Y + ")");
                    if (!model.isSpecialNode(sharedMapNode))
                    {
                        if (this.nodeEditState == NodeEditState.NON_WALKABLE)
                        {                            
                            sharedMapNode.repaintNode(Color.Gray, Color.Gray, "");
                            sharedMapNode.setFlag(Node.Flag.IsWalkable, false);                            
                            
                            /* update node to non-walkable on all agents maps */
                            foreach (Agent agent in model.getAgents())
                            {
                                agent.getMap().getNodeAtLocation(sharedMapNode.getPosition()).repaintNode(Color.Gray, Color.Gray, "");
                                agent.getMap().getNodeAtLocation(sharedMapNode.getPosition()).setFlag(Node.Flag.IsWalkable, false);
                            }
                        }
                        else if (this.nodeEditState == NodeEditState.WALKABLE)
                        {                            
                            sharedMapNode.repaintNode(Color.White, Color.White, "");
                            sharedMapNode.setFlag(Node.Flag.IsWalkable, true);                             

                            /* update node to non-walkable on all agents maps */
                            foreach (Agent agent in model.getAgents())
                            {
                                agent.getMap().getNodeAtLocation(sharedMapNode.getPosition()).repaintNode(Color.White, Color.White, "");
                                agent.getMap().getNodeAtLocation(sharedMapNode.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                            }
                        }
                    }
                }
            }
        }
        
        private void Start_DoWork(object sender, DoWorkEventArgs e)
        {
            Agent agent = (Agent)e.Argument;
            e.Result = (Agent)agent;
            BackgroundWorker worker = sender as BackgroundWorker;
            Algorithm algorithm = null; 
            int watchdog = 0;
            bool newNonWalkableNodesInView;

            switch (model.getSimulation().getAlgorithmType())
            {
                case AlgorithmType.AStar:
                    algorithm = new AStarAlgorithm(agent, model, 
                        model.getSimulation().getHeuristicMethod(), model.getSimulation().getFieldOfViewMethod());
                    break;  
                case AlgorithmType.Dijkstra:
                    algorithm = new DijkstraAlgorithm(agent, model,
                        model.getSimulation().getFieldOfViewMethod());
                    break;                
            }           
                
            algorithm.Init(agent.getNode(Agent.NodeType.Start), true);

            lock (aLock)
                newNonWalkableNodesInView = model.UpdateAgentMap(agent); 

            while (!algorithm.isSuccess(agent))
            {
                /* check to see if the background thread has be asynchronously cancelled */
                if (worker.CancellationPending)
                    return;
                        
                /* check to see if the max steps has been reached */
                if (agent.getCounters().getSteps() >= model.getSimulation().get(View.ViewNumericUpDown.MaxSteps))
                    return;                                           

                /* set the new target node */
                bool newTargetNodeSet;
                lock (aLock)
                    newTargetNodeSet = model.SetAgentTarget(agent);
                      
                /* initialize algorithm with current node if new non-walkable nodes were discovered or a new target node was set */
                if (newNonWalkableNodesInView | newTargetNodeSet)
                    algorithm.Init(agent.getNode(Agent.NodeType.Current), false);
                
                synchronization.Wait((int)agent.getId());                                        

                /* wait for all agents to set target before taking step */
                bool stepSuccess = false;
                stepSuccess = algorithm.Step(sender);
                
                /* step the agent one node */
                if (stepSuccess)
                {
                    synchronization.Wait((int)agent.getId());

                    /* identify node as a path node in the agent map and the shared map */
                    Node agentMapNode = agent.getNode(Agent.NodeType.Current);
                    agentMapNode.setFlag(Node.Flag.IsPath, true);

                    /* only update shared map if agent is active */
                    if (agent.isActive())
                        model.getMap().getNodeAtLocation(agentMapNode.getPosition()).setFlag(Node.Flag.IsPath, true);                    

                    /* paint the parent and current node */
                    algorithm.PostStep(agent, model);

                    /* update the agent's map and paint the newly viewed node in the shared map */
                    
                    lock (aLock)
                        newNonWalkableNodesInView = model.UpdateAgentMap(agent);     
                }
                else
                {     
                    /* watchdog */
                    if (++watchdog > model.getSimulation().get(View.ViewNumericUpDown.MaxSteps))                        
                        return;                        
                }

                /* synchronize steps by waiting for all background threads */
                synchronization.Wait((int)agent.getId());

                /* only delay if step was successfull */
                if (stepSuccess)                        
                    Thread.Sleep(model.getSimulation().get(View.ViewNumericUpDown.StepDelay));
            }                        
        } 

        private void Start_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Agent agent = (Agent)e.Result;
            int visibleWalkable = agent.getMap().getCount(new List<Node.Flag> { Node.Flag.IsVisible, Node.Flag.IsWalkable });
            int visible = agent.getMap().getCount(Node.Flag.IsVisible);
            int visibleNonWalkable = visible - visibleWalkable;

            /* build the result string for the UI */
            String[] resultString = new String[] {                
                ((int)agent.getId()).ToString(), 
                model.getSimulation().getMapID().ToString(),                
                model.getSimulation().get(View.ViewNumericUpDown.FieldOfView).ToString(),
                agent.getCounters().getSteps().ToString(),                
                visible.ToString(),
                visibleWalkable.ToString(),                
                visibleNonWalkable.ToString(),                             
                agent.getCounters().getNodesEvaluated().ToString(),
                agent.getCounters().getNodesShared().ToString(),
                Math.Pow(model.getSimulation().get(View.ViewNumericUpDown.MapSize),2).ToString(),
                agent.isSuccess(),

                model.getSimulation().getAlgorithmType().ToString(),
                model.getSimulation().getHeuristicMethod().ToString(),
                
                model.getSimulation().getFieldOfViewMethod().ToString(),
                model.getSimulation().getAgentCooperation().ToString(),
                model.getSimulation().get(View.ViewNumericUpDown.MapSize).ToString(),
                model.getSimulation().get(View.ViewNumericUpDown.PercentWalkable).ToString(),
                model.getSimulation().get(View.ViewNumericUpDown.NumberOfAgents).ToString(),
            };

            /* update the simulation results */
            model.getResults().add(resultString);
            
            /* update the help box */
            view.setStatusText("Change Parameters, Edit Map, or Click Start");

            Debug.WriteLine("Run Worker Completed on Agent: " + agent.getId().ToString());

            /* decrement the active agent count */
            synchronization.RemoveActiveAgent((int)agent.getId());

            if (Interlocked.Decrement(ref running) == 0)
            {
                Debug.WriteLine("running = 0");
                view.getButton(View.ViewButton.Cancel).Enabled = false;
                view.getButton(View.ViewButton.Start).Enabled = true;
                view.getButton(View.ViewButton.RandomMap).Enabled = true;
                view.getButton(View.ViewButton.EmptyMap).Enabled = true;                
                view.controlEnable(true);
                view.getComboBox(View.ViewComboBox.Visualizations).Enabled = true;

                /* reset the map with fog of war disabled if cancelled */
                if (cancelled)
                {                    
                    DrawingControl.SuspendDrawing(view.getPanelMap());
                    model.Reset(true);
                    DrawingControl.ResumeDrawing(view.getPanelMap());
                }
            }
        }
        #endregion BackgroundWorkerMethods
    }
}