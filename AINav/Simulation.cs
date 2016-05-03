using System;

namespace AINav
{    
    public class Simulation
    {
        /* combo box variables */
        private Agent.AgentID viewableAgent;
        private View.AgentCooperation agentCooperation; 
        private AlgorithmType algorithmType;
        private Node.Method fieldOfViewMethod; 
        private Node.Method heuristicMethod;
        private View.PathPersistence pathPersistence;
        private View.Visualizations visualizations;
                
        /* numeric up/down variables */
        private int fieldOfView;
        private int mapSize;
        private int maxSteps;
        private int numberOfAgents;
        private int percentWalkable;
        private int stepDelay;
        private int visualizationDelay;
        
        /* private variables */
        private int cellSize;
        private int mapID;

        public void set(View.ViewNumericUpDown type, int value)
        {
            switch (type)
            {
                case View.ViewNumericUpDown.FieldOfView:
                    this.fieldOfView = value;
                    break;
                case View.ViewNumericUpDown.MapSize:
                    this.mapSize = value;
                    break;
                case View.ViewNumericUpDown.MaxSteps:
                    this.maxSteps = value;
                    break;
                case View.ViewNumericUpDown.NumberOfAgents:
                    this.numberOfAgents = value;
                    break;
                case View.ViewNumericUpDown.PercentWalkable:
                    this.percentWalkable = value;
                    break;
                case View.ViewNumericUpDown.StepDelay:
                    this.stepDelay = value;
                    break;
                case View.ViewNumericUpDown.VisualizationDelay:
                    this.visualizationDelay = value;
                    break;
                default:
                    throw new Exception("simulation type invalid on set");
            }
        }

        public int get(View.ViewNumericUpDown type)
        {
            switch (type)
            {
                case View.ViewNumericUpDown.FieldOfView:
                    return this.fieldOfView;
                case View.ViewNumericUpDown.MapSize:
                    return this.mapSize;
                case View.ViewNumericUpDown.MaxSteps:
                    return this.maxSteps;
                case View.ViewNumericUpDown.NumberOfAgents:
                    return this.numberOfAgents;
                case View.ViewNumericUpDown.PercentWalkable:
                    return this.percentWalkable;
                case View.ViewNumericUpDown.StepDelay:
                    return this.stepDelay;
                case View.ViewNumericUpDown.VisualizationDelay:
                    return this.visualizationDelay;
                default:
                    throw new Exception("simulation type invalid on get");
            }
        }

        public Agent.AgentID getViewableAgent()
        {
            return this.viewableAgent;
        }

        public void setViewableAgent(Agent.AgentID viewableAgent)
        {
            this.viewableAgent = viewableAgent;
        }

        public View.PathPersistence getPersistenceEnabled()
        {
            return this.pathPersistence;
        }

        public void setPersistenceEnabled(View.PathPersistence pathPersistence)
        {
            this.pathPersistence = pathPersistence;
        }

        public View.Visualizations getVisualizations()
        {
            return visualizations;
        }

        public void setVisualizations(View.Visualizations visualizations)
        {
            this.visualizations = visualizations;
        }

        public View.AgentCooperation getAgentCooperation()
        {
            return this.agentCooperation;
        }

        public void setAgentCooperation(View.AgentCooperation agentCooperation)
        {
            this.agentCooperation = agentCooperation;
        }

        public AlgorithmType getAlgorithmType()
        {
            return this.algorithmType;
        }

        public void setAlgorithmType(AlgorithmType algorithmType)
        {
            this.algorithmType = algorithmType;
        }

        public Node.Method getHeuristicMethod()
        {
            return this.heuristicMethod;
        }

        public void setHeuristicMethod(Node.Method method)
        {
            this.heuristicMethod = method;
        }

        public Node.Method getFieldOfViewMethod()
        {            
            return this.fieldOfViewMethod;
        }

        public void setFieldOfViewMethod(Node.Method method)
        {
            this.fieldOfViewMethod = method;
        }

        public void setMapID(int value)
        {
            this.mapID = value;
        }

        public int getMapID()
        {
            return this.mapID;
        }

        public void incMapID()
        {
            this.mapID++;
        }

        public int getCellSize()
        {
            return this.cellSize;
        }

        public void setCellSize(int cellSize)
        {
            this.cellSize = cellSize;
        }        
    }
}
