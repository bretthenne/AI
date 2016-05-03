using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace AINav
{
    public class Map
    {
        private List<Node> nodes; 
        private object aLock;
        private int mapSize;

        public enum MapType
        {
            Empty,
            Random,
        }        

        public Map()
        { 
            this.nodes = new List<Node>();
            this.aLock = new object();
            this.mapSize = 0;
        }

        /* do not lock this method, locked in controller */
        public List<Node> getNodes()
        {
            return this.nodes;
        }

        public int getCount(Node.Flag type)
        {
            int count = 0;
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    if (node.getFlag(type))
                        count++;
                }
            }
            return count;
        }

        public int getCount(List<Node.Flag> types)
        {
            int count = 0;
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    int typeCount = 0;
                    foreach (Node.Flag type in types)
                    {
                        if (node.getFlag(type))
                            typeCount++;
                    }
                    if (typeCount == types.Count)
                        count++;
                }
            }
            return count;
        }        
        /* IsVisible,
            IsWalkable,
            IsPath,
            IsSpecial,
            IsShared,
         * */
        public override string ToString()
        {
            int visibleCount = this.getCount(Node.Flag.IsVisible);
            int walkableCount = this.getCount(Node.Flag.IsWalkable);            
            int pathCount = this.getCount(Node.Flag.IsPath);
            int specialCount = this.getCount(Node.Flag.IsSpecial);
            int sharedCount = this.getCount(Node.Flag.IsShared);
            
            return "visible: " + visibleCount + " walkable: " + walkableCount + " path: " + pathCount + " special: " + specialCount + " shared: " + sharedCount;
        }        
        
        public void create(MapType mapType, int mapSize, int cellSize, int percentWalkable)
        {
            lock (aLock)
            {
                Node node = null;
                this.nodes.Clear();
                this.mapSize = mapSize;

                int seed = unchecked(DateTime.Now.Ticks.GetHashCode());
                Random random = new Random(seed);

                for (int x = 0; x < mapSize; x++)
                {
                    for (int y = 0; y < mapSize; y++)
                    {
                        node = new Node(x, y, cellSize, cellSize);

                        switch (mapType)
                        {
                            case MapType.Empty:
                                node.repaintNode(Color.White, Color.White, "");
                                node.setFlag(Node.Flag.IsWalkable, true);
                                break;
                            case MapType.Random:
                                if (random.Next(0, 100) < percentWalkable)
                                {
                                    node.repaintNode(Color.White, Color.White, "");
                                    node.setFlag(Node.Flag.IsWalkable, true);
                                }
                                else
                                {
                                    node.repaintNode(Color.Gray, Color.Gray, "");
                                    node.setFlag(Node.Flag.IsWalkable, false);
                                }
                                break;
                        }

                        /* common to all map types */
                        node.BorderStyle = BorderStyle.FixedSingle;

                        /* add to list */
                        nodes.Add(node);
                    }
                }
            }
        }

        public void resetText()
        {
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    node.repaintNode(node.BackColor, node.ForeColor, "");
                }
            }
        }
        public List<Node> getNodesWithinDistance(Node origin, Node.Method method, int distance)
        {
            List<Node> temp = new List<Node>();
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    if (node.getDistance(origin, method) <= distance)
                        temp.Add(node);
                }
            }
            return temp;
        }

        public List<Node> getWalkableNodes()
        {
            List<Node> walkableNodes = new List<Node>();
            lock (aLock)
            {   
                foreach (Node node in nodes)
                {
                    if (node.getFlag(Node.Flag.IsWalkable))
                    {
                        walkableNodes.Add(node);
                    }
                }
            }
            return walkableNodes;
        }
               
        public List<Node> getNeighbors(Agent agent, Node current, List<Node> inclusionList, Node.Method method)
        {
            List<Node> nodeList = new List<Node>();
            List<Node> tempList = new List<Node>();
            lock (aLock)
            {
                int x = current.getPosition().X;
                int y = current.getPosition().Y;

                
                /* adjacent nodes */
                if ((x - 1 >= 0) & (y + 1 < mapSize))
                    tempList.Add(nodes[(x - 1) * mapSize + (y + 1)]);
                if ((y + 1 < mapSize)) 
                    tempList.Add(nodes[(x - 0) * mapSize + (y + 1)]);
                if ((x + 1 < mapSize) & (y + 1 < mapSize)) 
                    tempList.Add(nodes[(x + 1) * mapSize + (y + 1)]);                
                if ((x - 1 >= 0)) 
                    tempList.Add(nodes[(x - 1) * mapSize + (y - 0)]);
                if ((x + 1 < mapSize)) 
                    tempList.Add(nodes[(x + 1) * mapSize + (y - 0)]);
                if ((x - 1 >= 0) & (y - 1 >= 0)) 
                    tempList.Add(nodes[(x - 1) * mapSize + (y - 1)]);
                if (y - 1 >= 0) 
                    tempList.Add(nodes[(x - 0) * mapSize + (y - 1)]);
                if ((x + 1 < mapSize) & (y - 1 >= 0)) 
                    tempList.Add(nodes[(x + 1) * mapSize + (y - 1)]);


                foreach (Node node in tempList)
                {   
                    if (inclusionList != null)
                    {
                        if (inclusionList.Contains(node))
                        {
                            if (node.getFlag(Node.Flag.IsWalkable))
                                nodeList.Add(node);
                        }
                    }
                    else
                    {
                        if (node.getFlag(Node.Flag.IsWalkable))
                            nodeList.Add(node);
                    }                    
                }
            }
            return nodeList;
        }        
           
        public void setNodes(Node.Flag type, bool value)
        {
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    node.setFlag(type, value);
                }
            }
        }

        public void setNodes(Node.Cost type, double value)
        {
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    node.setCost(type, value);
                }
            }
        }

        public Node getNodeAtLocation(Point position)
        {
            lock (aLock)
            {
                return nodes[position.X * this.mapSize + position.Y];
            }

            throw new Exception("node does not exist");
        }

        public Node getNode(int index)
        {
            lock (aLock)
            {
                return nodes[index];
            }
            throw new Exception("node does not exist");
        }

        public bool isNodeFlag(Node n, Node.Flag flag)
        {
            lock (aLock)
            {
                return nodes[n.getPosition().X * this.mapSize + n.getPosition().Y].getFlag(flag);
            }
        }        

        public void repaint()
        {
            lock (aLock)
            {
                foreach (Node node in nodes)
                {
                    /* agent class paints the agent start and finish nodes */ 
                    if (node.getFlag(Node.Flag.IsSpecial)) 
                        continue;                    

                    /* visible walkable node */
                    if (node.getFlag(Node.Flag.IsVisible) & node.getFlag(Node.Flag.IsWalkable))
                    {
                        node.repaintNode(Color.White, Color.White, "");
                        continue;
                    }

                    /* visible non-walkable node */
                    if (node.getFlag(Node.Flag.IsVisible) & !node.getFlag(Node.Flag.IsWalkable))
                    {
                        node.repaintNode(Color.Gray, Color.Gray, "");                         
                        continue;
                    }

                    /* non walkable nodes */
                    node.repaintNode(Color.Black, Color.Black, "");
                }                   
            }
        } 
    }
}
