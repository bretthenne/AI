using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace AINav
{
    public partial class View : Form
    {        
        private Controller controller;
        
        private const int STEP_DELAY_INIT = 0;
        private const int VISUALIZATION_DELAY_INIT = 0;
        private const int MAP_SIZE_INIT = 25;
        private const int CELL_SIZE_INT = 400 / MAP_SIZE_INIT;
        private const int PERCENT_WALKABLE_INIT = 80;
        private const int NUMBER_OF_AGENTS_INIT = 2;
        private const int FIELD_OF_VIEW_INIT = 2;
        private const int MAX_STEPS_INIT = MAP_SIZE_INIT * MAP_SIZE_INIT;

        public readonly int PANEL_SIZE;

        public enum ViewButton
        {
            RandomMap,
            EmptyMap,            
            ResetMap,            
            Start,
            Cancel,            
            ExportCsv,
            ClearResults,
        }

        public enum ViewRadioButton
        {            
            EditStart,
            EditFinish,
            EditWalkable,
            EditNonWalkable,            
        }  

        public enum ViewNumericUpDown
        {
            FieldOfView,
            MapSize,
            MaxSteps, 
            NumberOfAgents, 
            PercentWalkable,            
            StepDelay,
            VisualizationDelay,
        }

        public enum ViewComboBox
        {
            ViewableAgent,
            AgentCooperation, 
            AlgorithmType,
            FovMethod, 
            HeuristicMethod,
            PathPersistence,
            Visualizations,
        }

        public enum PathPersistence
        {
            Disabled,
            Enabled,
        }

        public enum AgentCooperation
        {
            Disabled,
            Enabled,
        }

        public enum Visualizations
        {
            Disabled,
            Enabled,
        }

        public View()
        {
            InitializeComponent();

            PANEL_SIZE = panelMap.Size.Width;
            Simulation simulation = new Simulation();
            simulation.setAlgorithmType((AlgorithmType)algorithmType.SelectedIndex);
            simulation.setHeuristicMethod((Node.Method)heuristicMethod.SelectedIndex);
            simulation.set(ViewNumericUpDown.FieldOfView, FIELD_OF_VIEW_INIT);
            
            simulation.set(ViewNumericUpDown.NumberOfAgents, NUMBER_OF_AGENTS_INIT);
            simulation.set(ViewNumericUpDown.PercentWalkable, PERCENT_WALKABLE_INIT);
            simulation.set(ViewNumericUpDown.MaxSteps, MAX_STEPS_INIT);
            simulation.set(ViewNumericUpDown.StepDelay, STEP_DELAY_INIT);
            simulation.set(ViewNumericUpDown.MapSize, MAP_SIZE_INIT);
            simulation.setCellSize(CELL_SIZE_INT);

            Results results = new Results(this.simulationListView, new String[] { 
                "Agent ID", 
                "Map ID",
                "Field of View", 
                "Steps",                 
                "Visible",
                "Visible Walkable",
                "Visible Non-Walkable",
                "Evaluated",  
                "Shared",
                "Possible",
                "Success", 

                "Algorithm", 
                "Heuristic Method", 
                
                "Field of View Method",
                "Cooperation",
                "Map Size",
                "% Walkalbe",
                "Agents", 
            });
            Model model = new Model(simulation, results);
            controller = new Controller(model, this);            

            eventHandlerInit(model);
            customViewInit(model);
        }

        private void customViewInit(Model model)
        {
            /* Section:  AI Parameters */
            algorithmType.Items.Clear();
            algorithmType.Items.Add(AlgorithmType.Dijkstra); 
            algorithmType.Items.Add(AlgorithmType.AStar);            
            algorithmType.SelectedItem = AlgorithmType.AStar;            
            heuristicMethod.Items.Clear(); 
            heuristicMethod.Items.Add(Node.Method.Euclidean);
            heuristicMethod.Items.Add(Node.Method.EuclideanSquared);
            heuristicMethod.Items.Add(Node.Method.Manhattan);
            heuristicMethod.Items.Add(Node.Method.Chebyshev);
            heuristicMethod.SelectedItem = Node.Method.Euclidean;            
            fieldOfView.Value = FIELD_OF_VIEW_INIT;            
            fovMethod.Items.Clear();            
            fovMethod.Items.Add(Node.Method.Euclidean);
            fovMethod.Items.Add(Node.Method.EuclideanSquared);
            fovMethod.Items.Add(Node.Method.Manhattan);
            fovMethod.Items.Add(Node.Method.Chebyshev);
            fovMethod.SelectedItem = Node.Method.Euclidean;
            agentCooperation.Items.Clear();
            agentCooperation.Items.Add(AgentCooperation.Disabled);
            agentCooperation.Items.Add(AgentCooperation.Enabled);
            agentCooperation.SelectedItem = AgentCooperation.Enabled;

            /* Section:  Map Generator */
            mapSize.Value = MAP_SIZE_INIT;
            percentWalkable.Value = PERCENT_WALKABLE_INIT;
            numberOfAgents.Value = NUMBER_OF_AGENTS_INIT;             
            
            /* Section:  Simulation Preferences */
            viewableAgent.Items.Clear(); 
            pathPersistence.Items.Clear();
            pathPersistence.Items.Add(PathPersistence.Disabled);
            pathPersistence.Items.Add(PathPersistence.Enabled);
            pathPersistence.SelectedItem = PathPersistence.Enabled;
            visualizations.Items.Clear();
            visualizations.Items.Add(Visualizations.Disabled);
            visualizations.Items.Add(Visualizations.Enabled);
            visualizations.SelectedItem = Visualizations.Disabled;
            maxSteps.Value = MAX_STEPS_INIT;            
            stepDelay.Value = STEP_DELAY_INIT;
            visualizationDelay.Value = VISUALIZATION_DELAY_INIT;
            buttonStart.Enabled = false;
            buttonCancel.Enabled = false;                                        
      
            /* Section:  Map */
            radioButtonEditWalkable.Enabled = false;
            radioButtonEditNonWalkable.Enabled = false;
            radioButtonEditStart.Enabled = false;
            radioButtonEditFinish.Enabled = false; 
            buttonResetMap.Enabled = false;

            /* Section:  Help */
            this.status.Text = "Select Parameters then click Random or Empty Map";            
        }

        private void eventHandlerInit(Model model)
        {
            fieldOfView.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(fieldOfView, ViewNumericUpDown.FieldOfView);
            mapSize.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(mapSize, ViewNumericUpDown.MapSize);
            percentWalkable.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(percentWalkable, ViewNumericUpDown.PercentWalkable);
            numberOfAgents.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(numberOfAgents, ViewNumericUpDown.NumberOfAgents);
            maxSteps.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(maxSteps, ViewNumericUpDown.MaxSteps);
            stepDelay.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(stepDelay, ViewNumericUpDown.StepDelay);
            visualizationDelay.ValueChanged += (sender, eventArgs) => controller.NumericUpDown_ValueChanged(visualizationDelay, ViewNumericUpDown.VisualizationDelay);
                        
            algorithmType.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(algorithmType, ViewComboBox.AlgorithmType);
            heuristicMethod.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(heuristicMethod, ViewComboBox.HeuristicMethod);
            fovMethod.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(fovMethod, ViewComboBox.FovMethod);
            agentCooperation.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(agentCooperation, ViewComboBox.AgentCooperation);
            viewableAgent.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(viewableAgent, ViewComboBox.ViewableAgent);
            pathPersistence.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(pathPersistence, ViewComboBox.PathPersistence);
            visualizations.SelectedIndexChanged += (sender, eventArgs) => controller.ComboBox_SelectedIndexChanged(visualizations, ViewComboBox.Visualizations);

            buttonRandomMap.Click += (sender, eventArgs) => controller.Button_Click(buttonRandomMap, ViewButton.RandomMap);
            buttonEmptyMap.Click += (sender, eventArgs) => controller.Button_Click(buttonEmptyMap, ViewButton.EmptyMap);
            buttonStart.Click += (sender, eventArgs) => controller.Button_Click(buttonStart, ViewButton.Start);
            buttonCancel.Click += (sender, eventArgs) => controller.Button_Click(buttonCancel, ViewButton.Cancel);
            buttonClearResults.Click += (sender, eventArgs) => controller.Button_Click(buttonClearResults, ViewButton.ClearResults);
            buttonExportCSV.Click += (sender, eventArgs) => controller.Button_Click(buttonExportCSV, ViewButton.ExportCsv);
            buttonResetMap.Click += (sender, eventArgs) => controller.Button_Click(buttonResetMap, ViewButton.ResetMap);
                    
            radioButtonEditWalkable.Click += (sender, eventArgs) => controller.RadioButton_Click(radioButtonEditWalkable, ViewRadioButton.EditWalkable);
            radioButtonEditNonWalkable.Click += (sender, eventArgs) => controller.RadioButton_Click(radioButtonEditNonWalkable, ViewRadioButton.EditNonWalkable);
            radioButtonEditStart.Click += (sender, eventArgs) => controller.RadioButton_Click(radioButtonEditStart, ViewRadioButton.EditStart);
            radioButtonEditFinish.Click += (sender, eventArgs) => controller.RadioButton_Click(radioButtonEditFinish, ViewRadioButton.EditFinish);
            
            /* view class events */
            simulationListView.ColumnWidthChanging += new ColumnWidthChangingEventHandler(simulationListView_ColumnWidthChanging);
            simulationListView.ColumnClick += new ColumnClickEventHandler(simulationListView_ColumnClick);
        }
        
        public void buttonEditStartUpdate(Color backColor, Color foreColor, string text, bool enable)
        {
            radioButtonEditStart.BackColor = backColor;
            radioButtonEditStart.ForeColor = foreColor;
            radioButtonEditStart.Text = text;
            radioButtonEditStart.Enabled = enable;
        }

        public void buttonEditFinishUpdate(Color backColor, Color foreColor, string text, bool enable)
        {
            radioButtonEditFinish.BackColor = backColor;
            radioButtonEditFinish.ForeColor = foreColor;
            radioButtonEditFinish.Text = text;
            radioButtonEditFinish.Enabled = enable;
        }

        public void updateAgentItems()
        {
            /* update viewable agent and move agent combo box items */
            viewableAgent.Items.Clear(); 
            viewableAgent.Items.Add(Agent.AgentID.Agent_All);

            /* update the combo box list of available agents */
            Agent.AgentID id = Agent.AgentID.Agent_0;

            for (int i = 0; i < (int)this.numberOfAgents.Value; i++)
            {
                viewableAgent.Items.Add(id);                
                id++;
            }

            viewableAgent.SelectedItem = Agent.AgentID.Agent_All;
        }

        public void updateHeuristicMethodItems(AlgorithmType algorithmMethod)
        {
            switch (algorithmMethod)
            {
                case AlgorithmType.Dijkstra:                    
                    heuristicMethod.Items.Clear();
                    heuristicMethod.Items.Add(Node.Method.None);                    
                    break;                    
                case AlgorithmType.AStar:                    
                    heuristicMethod.Items.Clear();                    
                    heuristicMethod.Items.Add(Node.Method.Euclidean);
                    heuristicMethod.Items.Add(Node.Method.Manhattan);
                    break;
            }            
        }

        public void controlEnable(bool enable)
        {
            algorithmType.Enabled = enable;
            heuristicMethod.Enabled = enable;
            fovMethod.Enabled = enable;
            percentWalkable.Enabled = enable;
            fieldOfView.Enabled = enable;
            agentCooperation.Enabled = enable;
            numberOfAgents.Enabled = enable;
            viewableAgent.Enabled = enable;
            maxSteps.Enabled = enable;
            stepDelay.Enabled = enable;
            visualizationDelay.Enabled = enable;
            mapSize.Enabled = enable;
            pathPersistence.Enabled = enable;            
            radioButtonEditWalkable.Enabled = enable;
            radioButtonEditNonWalkable.Enabled = enable;
            radioButtonEditStart.Enabled = enable;
            radioButtonEditFinish.Enabled = enable;
            buttonResetMap.Enabled = enable;            
        }

        public RadioButton getRadioButton(ViewRadioButton radioButton)
        {
            switch (radioButton)
            {
                case ViewRadioButton.EditStart:
                    return this.radioButtonEditStart;
                case ViewRadioButton.EditFinish:
                    return this.radioButtonEditFinish;
                case ViewRadioButton.EditWalkable:
                    return this.radioButtonEditWalkable;
                case ViewRadioButton.EditNonWalkable:
                    return this.radioButtonEditNonWalkable;
                default:
                    throw new Exception("invalid radio button index");
            }            
        }

        public Button getButton(View.ViewButton button)
        {
            switch (button)
            {
                case ViewButton.RandomMap:
                    return this.buttonRandomMap;
                case ViewButton.EmptyMap:
                    return this.buttonEmptyMap;
                case ViewButton.ResetMap:
                    return this.buttonResetMap;                
                case ViewButton.Start:
                    return this.buttonStart;
                case ViewButton.Cancel:
                    return this.buttonCancel;                
                case ViewButton.ExportCsv:
                    return this.buttonExportCSV;
                case ViewButton.ClearResults:
                    return this.buttonClearResults;
                default:
                    throw new Exception("invalid button index");
            }            
        }

        public NumericUpDown getNumericUpDown(ViewNumericUpDown numericUpDown)
        {
            switch (numericUpDown)
            {
                case ViewNumericUpDown.FieldOfView:
                    return this.fieldOfView;
                case ViewNumericUpDown.MapSize:
                    return this.mapSize;
                case ViewNumericUpDown.MaxSteps:
                    return this.maxSteps;
                case ViewNumericUpDown.NumberOfAgents:
                    return this.numberOfAgents;
                case ViewNumericUpDown.PercentWalkable:
                    return this.percentWalkable;
                case ViewNumericUpDown.StepDelay:
                    return this.stepDelay;
                case ViewNumericUpDown.VisualizationDelay:
                    return this.visualizationDelay;
                default:
                    throw new Exception("invalid numeric up/down index");
            }            
        }

        public ComboBox getComboBox(ViewComboBox comboBox)
        {
            switch (comboBox)
            {
                case ViewComboBox.ViewableAgent:
                    return this.viewableAgent;
                case ViewComboBox.AgentCooperation:
                    return this.agentCooperation;
                case ViewComboBox.AlgorithmType:
                    return this.algorithmType;
                case ViewComboBox.FovMethod:
                    return this.fovMethod;
                case ViewComboBox.HeuristicMethod:
                    return this.heuristicMethod;
                case ViewComboBox.PathPersistence:
                    return this.pathPersistence;
                case ViewComboBox.Visualizations:
                    return this.visualizations; 
                default:
                    throw new Exception("invalid combo box");
            }            
        }        

        public ListView getSimulationListView()
        {
            return this.simulationListView;
        }

        public void setStatusText(string text)
        {
            this.status.Text = text;
        }                
        
        public Panel getPanelMap()
        {
            return this.panelMap;
        }
        
        void simulationListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            /* prevent the listview from changing column width */            
            e.NewWidth = this.simulationListView.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        public void simulationListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            Debug.WriteLine("column click: " + e.Column.ToString());
            ListView listView = sender as ListView;
            listView.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listView.Sort();
        }                      
    }

    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;

        public ListViewItemComparer()
        {
            col = 0;
        }

        public ListViewItemComparer(int column)
        {
            col = column;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
            ((ListViewItem)y).SubItems[col].Text);
            return returnVal;
        }
    }
}
