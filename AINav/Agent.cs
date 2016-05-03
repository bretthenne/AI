using System;
using System.Collections.Generic;
using System.Drawing;

namespace AINav
{
    public class Agent
    {
        private AgentID id;        
        private Map map;
        private Node start;
        private Node target;
        private Node finish;        
        private Node current;        
        private Color backColor;
        private Color foreColor;              
        private Counters counters;
        private bool active;
        private List<AgentID> meetingList;
        private object aLock;

        public enum AgentID
        {
            Agent_All = -1, 
            Agent_0,
            Agent_1,
            Agent_2,
            Agent_3,
            Agent_4,
            Agent_5,
            Agent_6,
            Agent_7,
            Agent_8,
            Agent_9,
            Agent_10,            
        }

        public enum ColorType
        {
            BackColor,
            ForeColor,
        }

        public enum NodeType
        {
            Start,
            Finish,
            Current,
            Target,
        }

        public Agent(AgentID id, int mapSize, int cellSize, Color backColor, Color foreColor)
        {
            this.id = id;
            this.map = new Map();
            this.map.create(Map.MapType.Empty, mapSize, cellSize, 100);
            this.start = null;
            this.finish = null;
            this.current = null;
            this.target = null;
            this.backColor = backColor;
            this.foreColor = foreColor;
            this.counters = new Counters();
            this.active = false;            
            this.meetingList = new List<AgentID>();
            this.aLock = new object();
        }

        #region Getters

        public AgentID getId()
        {
            return this.id;
        }

        public Map getMap()
        {
            return this.map;
        }

        public Node getNode(NodeType type)
        {
            switch (type)
            {
                case NodeType.Start:
                    return this.start;
                case NodeType.Finish:
                    return this.finish;
                case NodeType.Current:
                    return this.current;
                case NodeType.Target:
                    return this.target;
                default:
                    throw new Exception("invalid node type");
            }
        }        

        public bool isActive()
        {
            return this.active;
        }

        public Counters getCounters()
        {
            return this.counters;
        }

        public Color getColor(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.BackColor:
                    return this.backColor;
                case ColorType.ForeColor:
                    return this.foreColor;
                default:
                    throw new Exception("invalid color type");
            }
        }        

        #endregion Getters

        #region Setters

        public void setNode(NodeType type, Node node)
        {
            switch (type)
            {
                case NodeType.Start:
                    this.start = node;
                    break;
                case NodeType.Finish:
                    this.finish = node;
                    break;
                case NodeType.Current:
                    this.current = node;
                    break;
                case NodeType.Target:
                    this.target = node;
                    break;
                default:
                    throw new Exception("invalid node type");
            }
        }        

        public void setActive(bool active)
        {
            this.active = active;
        }

        #endregion Setters

        #region HelperMethods               

        public void repaint(Map map)
        {
            Node sharedMapNode;            
            sharedMapNode =  map.getNodeAtLocation(start.getPosition());
            sharedMapNode.repaintNode(this.backColor, this.foreColor, "S");
            sharedMapNode = map.getNodeAtLocation(finish.getPosition());
            sharedMapNode.repaintNode(this.backColor, this.foreColor, "F");
        }        

        public string isSuccess()
        {
            return (current.isEqual(finish)) ? "Yes" : "No";
        }

        public bool meetingListCheck(AgentID id)
        {
            foreach (AgentID agentId in meetingList)
            {
                if (agentId == id)
                    return true;
            }
            return false;
        }

        public void meetingListAdd(AgentID id)
        {
            this.meetingList.Add(id);
        }

        public void meetingListClear()
        {
            meetingList.Clear();
        }

        #endregion HelperMethods                 
    }
}
