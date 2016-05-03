using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace AINav
{    
    public class Model
    {        
        private Simulation simulation;
        private Results results;
        private Map map;
        private List<Agent> agents;
        private Random random;
        private int seed;        

        public Color[,] agentColorPair = 
        {
            /* path color, text color */
            {Color.Blue, Color.White},
            {Color.Green, Color.White},
            {Color.Red, Color.White},
            {Color.Yellow, Color.Black},
            {Color.Brown, Color.White},
            {Color.Aqua, Color.Black},
            {Color.Teal, Color.Black},
            {Color.Magenta, Color.Black},
            {Color.Purple, Color.White},
            {Color.Pink, Color.Black}, 
        };
            
        public Model(Simulation simulation, Results results)
        {
            this.simulation = simulation;
            this.results = results;
            this.map = new Map();            
            this.agents = new List<Agent>();
        }

        public Simulation getSimulation()
        {
            return this.simulation;
        }

        public Results getResults()
        {
            return this.results;
        }

        public List<Agent> getAgents()
        {
            return this.agents;
        }

        public Map getMap()
        {
            return this.map;
        }

        public Agent isAgentInView(Agent me)
        {
            foreach (Agent agent in agents)
            {
                if (agent.getId() != me.getId())
                {
                    double distance = agent.getNode(Agent.NodeType.Current).getDistance(me.getNode(Agent.NodeType.Current), simulation.getFieldOfViewMethod());

                    if (distance <= simulation.get(View.ViewNumericUpDown.FieldOfView))
                    {                        
                        return agent;
                    }
                }
            }
            return null;
        }
        
        public void Reset(bool visible)
        {
            /* reset the shared map */
            map.setNodes(Node.Flag.IsVisible, visible);
            map.setNodes(Node.Flag.IsPath, false);
            
            map.repaint();
            
            /* reset agent maps  */
            foreach (Agent agent in agents)
            {
                agent.getMap().setNodes(Node.Flag.IsVisible, false);
                agent.getMap().setNodes(Node.Flag.IsPath, false);
                agent.getMap().setNodes(Node.Flag.IsWalkable, true);
                agent.getMap().setNodes(Node.Cost.Movement, 0.0);
                agent.getMap().setNodes(Node.Cost.Heuristic, 0.0);
                agent.getMap().setNodes(Node.Cost.Total, 0.0);
                agent.repaint(map);
            }

            /* reset the agents meeting list */
            foreach (Agent agent in agents)
                agent.meetingListClear();
        }

        public void paintAgents()
        {
            foreach (Agent agent in agents)
            {
                agent.repaint(map);
            }
        }

        public Agent getAgent(Agent.AgentID agentID)
        {
            foreach (Agent agent in agents)
            {
                if (agent.getId() == agentID)
                    return agent;
            }
            return null;
        }

        public bool isVisibleNode(Node node)
        {
            foreach (Agent agent in agents)
            {
                if (agent.getMap().isNodeFlag(node, Node.Flag.IsVisible))
                    return true;
            }
            return false;
        }

        public bool isPathNode(Node node)
        {
            foreach (Agent agent in agents)
            {
                if (agent.getMap().isNodeFlag(node, Node.Flag.IsPath))
                    return true;
            }
            return false;
        }        

        public bool isSpecialNode(Node node)
        {
            foreach (Agent a in agents)
            {
                if (node.isEqual(a.getNode(Agent.NodeType.Start)) | node.isEqual(a.getNode(Agent.NodeType.Finish)))
                    return true;                    
            }
            return false;
        }

        public Agent getSpecialNodeAgent(Node node)
        {
            foreach (Agent a in agents)
            {
                if (node.isEqual(a.getNode(Agent.NodeType.Start)) | node.isEqual(a.getNode(Agent.NodeType.Finish)))
                    return a;
            }
            return null;
        }       

        public void createAgents(int mapSize, int cellSize)
        {
            int x, y;

            seed = unchecked(DateTime.Now.Ticks.GetHashCode());
            random = new Random(seed);
                        
            /* create the agents */
            agents.Clear();
            for (int i = 0; i < simulation.get(View.ViewNumericUpDown.NumberOfAgents); i++)
            {
                /* create the agent */
                Color backColor = agentColorPair[i, 0];
                Color foreColor = agentColorPair[i, 1];
                Agent agent = new Agent((Agent.AgentID)i, simulation.get(View.ViewNumericUpDown.MapSize), simulation.getCellSize(), backColor, foreColor);

                /* select random start node */
                x = random.Next(simulation.get(View.ViewNumericUpDown.MapSize));
                y = random.Next(simulation.get(View.ViewNumericUpDown.MapSize));
                Node start = agent.getMap().getNodeAtLocation(new Point(x, y));                
                agent.setNode(Agent.NodeType.Start, start);
                start.setFlag(Node.Flag.IsWalkable, true);
                start.setFlag(Node.Flag.IsSpecial, true);
                map.getNodeAtLocation(start.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                map.getNodeAtLocation(start.getPosition()).setFlag(Node.Flag.IsSpecial, true);

                /* select random finish node */
                x = random.Next(simulation.get(View.ViewNumericUpDown.MapSize));
                y = random.Next(simulation.get(View.ViewNumericUpDown.MapSize));
                Node finish = agent.getMap().getNodeAtLocation(new Point(x, y));
                agent.setNode(Agent.NodeType.Finish, finish);                
                finish.setFlag(Node.Flag.IsWalkable, true);
                finish.setFlag(Node.Flag.IsSpecial, true);
                map.getNodeAtLocation(finish.getPosition()).setFlag(Node.Flag.IsWalkable, true);
                map.getNodeAtLocation(finish.getPosition()).setFlag(Node.Flag.IsSpecial, true);


                /* add agent to list */
                agents.Add(agent);                
            }
        }

        public bool SetAgentTarget(Agent agent)
        {
            Agent agentInView = this.isAgentInView(agent);

            /* check if agent is in view */
            if (agentInView != null && (simulation.getAgentCooperation() == View.AgentCooperation.Enabled))
            {
                /* agent in view, check if meeting has already occurred */
                if (agent.meetingListCheck(agentInView.getId()))
                {
                    /* meeting has already occurred so set the target node to finish */
                    agent.setNode(Agent.NodeType.Target, agent.getNode(Agent.NodeType.Finish));
                }
                else
                {
                    /* agent in view, check if both agents occupy the same same node or adjacent node */
                    if (agentInView.getNode(Agent.NodeType.Current).isEqual(agent.getNode(Agent.NodeType.Current)) ||
                        agentInView.getNode(Agent.NodeType.Current).isAdjacent(agent.getNode(Agent.NodeType.Current), simulation.get(View.ViewNumericUpDown.MapSize)))
                    {
                        /* agent in view & agents are in sharing distance, therefore update the visible flags */
                        int sharedNodeCount = 0;

                        foreach (Node agentInViewMapNode in agentInView.getMap().getNodes())
                        {
                            /* get the agent map node */
                            Node agentMapNode = agent.getMap().getNodeAtLocation(agentInViewMapNode.getPosition());

                            /* skip nodes that are not visible */
                            if (!agentInViewMapNode.getFlag(Node.Flag.IsVisible))
                                continue;

                            /* skip nodes that are already visible in the agent's map */
                            if (agentMapNode.getFlag(Node.Flag.IsVisible))
                                continue;

                            /* don't share shared nodes (IsShared flag is cleared after both agents complete the sharing) */
                            if (agentInViewMapNode.getFlag(Node.Flag.IsShared))
                                continue;

                            


                            /* set flags flags */
                            agentMapNode.setFlag(Node.Flag.IsVisible, true);
                            agentMapNode.setFlag(Node.Flag.IsWalkable, agentInViewMapNode.getFlag(Node.Flag.IsWalkable));
                            agentMapNode.setFlag(Node.Flag.IsShared, true);

                            /* increment the shared node counter */
                            sharedNodeCount++;

                            /* paint the shared map if the agent is active */
                            if (agent.isActive())
                            {
                                /* get the shared map node */
                                Node sharedMapNode = map.getNodeAtLocation(agentMapNode.getPosition());

                                bool isWalkable = agentMapNode.getFlag(Node.Flag.IsWalkable);
                                bool isPathNode = map.isNodeFlag(sharedMapNode, Node.Flag.IsPath);
                                bool isSpecialNode = this.isSpecialNode(sharedMapNode);
                                bool isVisualizations = (simulation.getVisualizations() == View.Visualizations.Enabled) ? true : false;

                                /* dont repaint if its the start node, finish node, or a path node */
                                if (!isSpecialNode && !isPathNode)
                                {
                                    if (isWalkable)
                                    {
                                        if (isVisualizations)
                                            sharedMapNode.repaintNode(Color.White, Color.Black, sharedMapNode.Text);
                                        else
                                            sharedMapNode.repaintNode(Color.White, Color.White, "");
                                    }
                                    else
                                        sharedMapNode.repaintNode(Color.Gray, Color.Gray, "");
                                }                             
                            }
                        }

                        agent.meetingListAdd(agentInView.getId());
                        agent.getCounters().incNodesShared(sharedNodeCount);
                        
                        /* set the target node to the finish node */
                        agent.setNode(Agent.NodeType.Target, agent.getNode(Agent.NodeType.Finish));

                        /* indicate that an algorithm reset is required because the target node was changed */
                        return true;
                    }
                    else
                    {
                        /* agent in view but both agents are not occupying the same node yet */
                        /* set the agent target node to the agent in view's current node */

                        Node target = agent.getMap().getNodeAtLocation(agentInView.getNode(Agent.NodeType.Current).getPosition());
                        agent.setNode(Agent.NodeType.Target, target);

                        /* indicate that an algorithm reset is required because a new target node has been set */
                        return true;
                    }
                }
            }
            else
            {
                /* agent not in view so just keep going to the agents finish node */
                agent.setNode(Agent.NodeType.Target, agent.getNode(Agent.NodeType.Finish));
            }

            /* indicate that an algorithm reset is not required because the target node was not changed */
            return false;
        }  

        public bool UpdateAgentMap(Agent agent)
        {
            int nonWalkableNonVisibleCount = 0;
            List<Node> sharedMapFovNodes;
            
            /* get viewable shared map nodes based on the agent's current location */
            sharedMapFovNodes = map.getNodesWithinDistance(
                agent.getNode(Agent.NodeType.Current), 
                simulation.getFieldOfViewMethod(), 
                simulation.get(View.ViewNumericUpDown.FieldOfView));
            
            foreach (Node sharedMapNode in sharedMapFovNodes)
            {    
                /* update the agent's map */
                bool isWalkable = sharedMapNode.getFlag(Node.Flag.IsWalkable);
                Node agentMapNode = agent.getMap().getNodeAtLocation(sharedMapNode.getPosition());
                agentMapNode.setFlag(Node.Flag.IsVisible, true);
                agentMapNode.setFlag(Node.Flag.IsWalkable, isWalkable);

                /* count the nodes that are not already visible and non walkable in order to reset the algorithm for the next step */
                if (!sharedMapNode.getFlag(Node.Flag.IsVisible) && !sharedMapNode.getFlag(Node.Flag.IsWalkable))
                    nonWalkableNonVisibleCount++;  

                /* only paint the shared map if the agent is active */
                if (agent.isActive())
                {
                    bool isPathNode = map.isNodeFlag(sharedMapNode, Node.Flag.IsPath);                    
                    bool isSpecialNode = this.isSpecialNode(sharedMapNode);
                    bool isVisualizations = (simulation.getVisualizations() == View.Visualizations.Enabled) ? true : false;
                    
                    /* dont repaint if its the start node, finish node, or a path node */
                    if (!isSpecialNode && !isPathNode)
                    {                        
                        if (isWalkable)
                        {
                            if (isVisualizations)
                                sharedMapNode.repaintNode(Color.White, Color.Black, sharedMapNode.Text);
                            else
                                sharedMapNode.repaintNode(Color.White, Color.White, "");
                        }
                        else
                            sharedMapNode.repaintNode(Color.Gray, Color.Gray, "");
                    }
                }
            }

            return (nonWalkableNonVisibleCount == 0) ? false : true;
        }
    }
}
