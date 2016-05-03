using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace AINav
{    
    abstract public class Algorithm
    {             
        abstract public void Init(Node node, bool resetCounters);
        abstract public bool Step(object sender);             

        public bool isSuccess(Agent agent)
        {
            return agent.getNode(Agent.NodeType.Current).isEqual(agent.getNode(Agent.NodeType.Finish));
        }

              

        public void PostStep(Agent agent, Model model)
        {
            Agent.AgentID id = model.getSimulation().getViewableAgent();

            /* only update the selected agent or all agents if selected */
            
            Node currentNode = model.getMap().getNodeAtLocation(agent.getNode(Agent.NodeType.Current).getPosition());

            Color backColor = agent.getColor(Agent.ColorType.BackColor);
            Color foreColor = agent.getColor(Agent.ColorType.ForeColor);

            if (currentNode.isEqual(agent.getNode(Agent.NodeType.Finish)))
                currentNode.repaintNode(backColor, foreColor, "F");
            else
            {
                if (model.getSimulation().getVisualizations() == View.Visualizations.Enabled)
                    currentNode.repaintNode(backColor, foreColor, currentNode.Text);
                else
                    currentNode.repaintNode(backColor, foreColor, "");
            }

            if (agent.getNode(Agent.NodeType.Current).getParent() != null)
            {
                Node parentNode = model.getMap().getNodeAtLocation(agent.getNode(Agent.NodeType.Current).getParent().getPosition());

                if (model.getSimulation().getPersistenceEnabled() == View.PathPersistence.Disabled | !agent.isActive())
                {
                    if (model.isSpecialNode(parentNode))
                    {
                        Agent specialAgent = model.getSpecialNodeAgent(parentNode);
                        backColor = specialAgent.getColor(Agent.ColorType.BackColor);
                        foreColor = specialAgent.getColor(Agent.ColorType.ForeColor);
                        parentNode.repaintNode(backColor, foreColor, "");
                    }
                    else
                    {
                        if (agent.isActive())
                            if (parentNode.getFlag(Node.Flag.IsWalkable))
                                parentNode.repaintNode(Color.White, Color.White, "");
                            else
                                parentNode.repaintNode(Color.Gray, Color.Gray, "");
                        else
                        {
                            /* is node visible on active agent's map */                            
                            Agent viewAgent = model.getAgent(model.getSimulation().getViewableAgent());
                            bool isVisibleOnActiveAgentsMap = viewAgent.getMap().isNodeFlag(parentNode, Node.Flag.IsVisible);

                            if (isVisibleOnActiveAgentsMap)
                            {
                                /* is node special */
                                bool isSpecial = viewAgent.getMap().isNodeFlag(parentNode, Node.Flag.IsSpecial);
                                if (isSpecial)
                                {
                                    if (parentNode.isEqual(viewAgent.getNode(Agent.NodeType.Start)))
                                        parentNode.repaintNode(viewAgent.getColor(Agent.ColorType.BackColor), viewAgent.getColor(Agent.ColorType.ForeColor), "S");
                                    else
                                        parentNode.repaintNode(viewAgent.getColor(Agent.ColorType.BackColor), viewAgent.getColor(Agent.ColorType.ForeColor), "F");
                                }
                                else
                                {
                                    /* is node walkable on active agent's map */
                                    bool isWalkable = viewAgent.getMap().isNodeFlag(parentNode, Node.Flag.IsWalkable);
                                    if (isWalkable)
                                    {
                                        bool isPathNode = viewAgent.getMap().isNodeFlag(parentNode,Node.Flag.IsPath);
                                        if (isPathNode)
                                        {
                                            if (model.getSimulation().getPersistenceEnabled() == View.PathPersistence.Enabled)
                                            {
                                                /* non active agent's parent node is also the active agent's path node */
                                                Color b = viewAgent.getColor(Agent.ColorType.BackColor);
                                                Color f = viewAgent.getColor(Agent.ColorType.ForeColor);
                                                parentNode.repaintNode(b, f, "");
                                            }
                                            else
                                            {
                                                /* non active agent's walkable path node with persistence disabled */
                                                parentNode.repaintNode(Color.White, Color.White, "");
                                            }
                                        }
                                        else
                                        {
                                            /* non active agent's parent node is just a visible walkable node */
                                            parentNode.repaintNode(Color.White, Color.White, "");
                                        }
                                    }
                                    else
                                    {
                                        /* non active agent's parent node is just a visible non-walkable node */
                                        parentNode.repaintNode(Color.Gray, Color.Gray, "");
                                    }
                                }
                            }
                            else
                            {
                                /* node is not visible on active agent's map therefore we can just paint it black */
                                parentNode.repaintNode(Color.Black, Color.Black, "");
                            }
                        }
                    }
                }
            }
                        
        }

        protected bool checkTarget(Model model, Agent agent, Node algorithmCurrent)
        {           
            bool targetReached;

            /* target is target node if target is not null */
            targetReached = algorithmCurrent.isEqual(agent.getNode(Agent.NodeType.Target));
            
            /* take step if target is reached */
            if (targetReached)
                takeStep(agent, algorithmCurrent, model);            

            return targetReached;
        }

        protected Node getNext(List<Node> list)
        {
            Node next = null;
            double minTotalCost = double.MaxValue;

            foreach (Node node in list)
            {
                if (node.getCost(Node.Cost.Total) < minTotalCost)
                {
                    minTotalCost = node.getCost(Node.Cost.Total);
                    next = node;
                }
            }

            return next;
        }

        protected void takeStep(Agent agent, Node algorithmCurrent, Model model)
        {            
            /* check to see if we didn't move at all */
            if (algorithmCurrent.isEqual(agent.getNode(Agent.NodeType.Current)))
                return;

            /* check to see if we need to backtrack */
            while (!algorithmCurrent.getParent().isEqual(agent.getNode(Agent.NodeType.Current)))
                algorithmCurrent = algorithmCurrent.getParent();
                
            /* take a step! */
            agent.setNode(Agent.NodeType.Current, algorithmCurrent);

            /* increment step counter */
            agent.getCounters().incSteps();            
        }
        
        protected bool visualizationPaintOK(Node node, Agent agent, Model model)
        {
            bool isActive = agent.isActive();
            View.Visualizations visualizations = model.getSimulation().getVisualizations();
            bool notSpecial = !model.isSpecialNode(node);
            bool notPath = !model.isPathNode(node);
            if (isActive && visualizations == View.Visualizations.Enabled && notSpecial && notPath)
                return true;
            else
                return false;
        }
    }

    public enum AlgorithmType
    {
        Dijkstra, 
        AStar,        
    }
    
    public class AStarAlgorithm : Algorithm
    {        
        private Agent agent;        
        private Model model;        
        private Node.Method heuristicMethod;
        private Node.Method fovMethod;
        private List<Node> open;
        private List<Node> closed;
        private List<Node> neighbors;
        private Node current;        

        public AStarAlgorithm(Agent agent, Model model, Node.Method heuristicMethod, Node.Method fovMethod)
        {
            this.agent = agent;
            this.model = model;
            this.heuristicMethod = heuristicMethod;
            this.fovMethod = fovMethod;
            open = new List<Node>();
            closed = new List<Node>();
            neighbors = new List<Node>();            
        }
                
        override public void Init(Node node, bool resetCounters)
        {
            closed.Clear(); 
            open.Clear();            
            neighbors.Clear(); 
            if (resetCounters)
                agent.getCounters().reset();
            this.current = node;
            node.setParent(node.getParent());
            node.setCost(Node.Cost.Movement, 0);
            node.setCost(Node.Cost.Heuristic, node.getCost(Node.Cost.Movement) + node.getDistance(agent.getNode(Agent.NodeType.Target), heuristicMethod));
            open.Add(node);
        }        

        override public bool Step(object sender)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int visualizationDelay = model.getSimulation().get(View.ViewNumericUpDown.VisualizationDelay);
            
            /* run algorithm on agent's viewable map to determine the next step */
            while (open.Count > 0)            
            {
                /* check if the run has been cancelled */
                if (worker.CancellationPending)
                    return false;
                
                /* get the next node */
                current = getNext(open);

                if (current == null)
                    return false;

                /* check if target node has been reached */
                Node target = agent.getNode(Agent.NodeType.Target);
                if (target != null && current.isEqual(target))
                    break;               

                /* move node from open list to closed list */
                open.Remove(current);
                closed.Add(current);                

                /* update current node visualizations when put on closed list */
                if (visualizationPaintOK(current, agent, model))
                {
                    Node sharedMapNode = model.getMap().getNodeAtLocation(current.getPosition());
                    int count = agent.getCounters().getNodesEvaluated();
                    sharedMapNode.repaintNode(Color.Pink, Color.Black, sharedMapNode.Text);
                    Thread.Sleep(visualizationDelay);
                }

                /* get the neighbors */
                neighbors.Clear();
                neighbors = agent.getMap().getNeighbors(agent, current, null, fovMethod);                

                /* add neighbor's to open list if not already there */
                foreach (Node node in neighbors)
                {
                    /* skip nodes that are already on the closed list */
                    if (closed.Contains(node))                    
                        continue;

                    /* calculate the new movement cost */
                    double newMovementCost = current.getCost(Node.Cost.Movement) + current.getDistance(node, fovMethod);
                   
                    if (!open.Contains(node) || newMovementCost < node.getCost(Node.Cost.Movement))
                    {
                        /* save the parent node */
                        node.setParent(current);

                        /* update the movement cost */
                        node.setCost(Node.Cost.Movement, newMovementCost);                        

                        /* calculate the heuristic cost */
                        node.setCost(Node.Cost.Heuristic, node.getDistance(agent.getNode(Agent.NodeType.Target), heuristicMethod));

                        /* update the f-score */
                        node.setCost(Node.Cost.Total, node.getCost(Node.Cost.Movement) + node.getCost(Node.Cost.Heuristic));                       

                        /* add to open list if not already there */
                        if (!open.Contains(node))
                        {
                            open.Add(node);
                            
                            /* increment nodes evaluated only when nodes are added to open list */
                            agent.getCounters().incNodesEvaluated();
                            
                            /* visualizations */
                            if (visualizationPaintOK(node, agent, model))
                            {
                                Node sharedMapNode = model.getMap().getNodeAtLocation(node.getPosition());
                                int count = agent.getCounters().getNodesEvaluated();
                                sharedMapNode.repaintNode(Color.LightGreen, Color.Black, count.ToString());
                                Thread.Sleep(visualizationDelay);
                            }
                        }
                    }
                }
            } 

            return checkTarget(model, agent, current); ;           
        }
    }

    public class DijkstraAlgorithm : Algorithm
    {
        private Agent agent;        
        private Model model;
        private Node.Method fovMethod;        
        private List<Node> open;
        private List<Node> closed;
        private List<Node> neighbors;
        private Node current;        

        public DijkstraAlgorithm(Agent agent, Model model, Node.Method fovMethod)
        {
            this.agent = agent;
            this.model = model;
            this.fovMethod = fovMethod;            
            open = new List<Node>();
            closed = new List<Node>();
            neighbors = new List<Node>();
        }      
  
        override public void Init(Node current, bool resetCounters)
        {
            open.Clear();
            closed.Clear();
            neighbors.Clear();
            if (resetCounters)
                agent.getCounters().reset();            
            open = agent.getMap().getWalkableNodes(); 
            foreach (Node node in open)
            {
                if (node.isEqual(current))                
                    node.setCost(Node.Cost.Total, 0);
                else
                    node.setCost(Node.Cost.Total, double.MaxValue);
                    
                node.setParent(null);
            } 
        }        

        override public bool Step(object sender)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int visualizationDelay = model.getSimulation().get(View.ViewNumericUpDown.VisualizationDelay);  
  
            while (open.Count > 0)
            {
                /* check if the run has been cancelled */
                if (worker.CancellationPending)
                    return false;

                /* pick the node in the open list with the shortest distance */
                current = getNext(open);                

                if (current == null)
                    return false;

                if (current.isEqual(agent.getNode(Agent.NodeType.Target)) | (current.getCost(Node.Cost.Total) == double.MaxValue))
                    break;

                open.Remove(current);
                closed.Add(current);

                agent.getCounters().incNodesEvaluated();

                /* update current node visualizations when put on closed list */
                if (visualizationPaintOK(current, agent, model))
                {
                    Node sharedMapNode = model.getMap().getNodeAtLocation(current.getPosition());
                    int count = agent.getCounters().getNodesEvaluated();
                    sharedMapNode.repaintNode(Color.Pink, Color.Black, agent.getCounters().getNodesEvaluated().ToString());
                    Thread.Sleep(visualizationDelay);
                }

                /* get neighbors that are on the open list only */
                neighbors.Clear();
                neighbors = agent.getMap().getNeighbors(agent, current, open, fovMethod);

                foreach (Node node in neighbors)
                {
                    /* calculate the new distance */
                    double distance = current.getCost(Node.Cost.Total) + current.getDistance(node, fovMethod);

                    /* if distance is lower then replace */
                    if (distance < node.getCost(Node.Cost.Total))
                    {
                        node.setCost(Node.Cost.Total, distance);
                        node.setParent(current);

                        if (!closed.Contains(node))
                        {
                            if (visualizationPaintOK(node, agent, model))
                            {
                                Node sharedMapNode = model.getMap().getNodeAtLocation(node.getPosition());
                                int count = agent.getCounters().getNodesEvaluated();
                                sharedMapNode.repaintNode(Color.LightGreen, Color.Black, agent.getCounters().getNodesEvaluated().ToString());
                                Thread.Sleep(visualizationDelay);
                            }
                        }
                    }
                }
            }

            return checkTarget(model, agent, current);            
        }          
    }
}
