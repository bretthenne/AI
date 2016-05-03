namespace AINav
{
    public class Counters
    {        
        private int steps;       
        private int nodesEvaluated;
        private int nodesShared;

        public void reset()
        {   
            this.steps = 0;            
            this.nodesEvaluated = 0;
            this.nodesShared = 0;
        }
         
        public int getNodesShared()
        {
            return this.nodesShared;
        }

        public void setNodesShared(int nodesShared)
        {
            this.nodesShared = nodesShared;
        }

        public void incNodesShared(int nodesShared)
        {
            this.nodesShared += nodesShared;
        }

        public int getSteps()
        {
            return this.steps;
        }

        public void setSteps(int steps)
        {
            this.steps = steps;
        }

        public void incSteps()
        {
            this.steps++;
        }

        public int getNodesEvaluated()
        {
            return this.nodesEvaluated;
        }

        public void setNodesEvaluated(int nodesEvaluated)
        {
            this.nodesEvaluated = nodesEvaluated;
        }

        public void incNodesEvaluated()
        {
            this.nodesEvaluated++;
        }

        public void incNodesEvaluated(int delta)
        {
            this.nodesEvaluated += delta;
        }
    }
}
