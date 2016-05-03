using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace AINav
{
    class Synchronization
    {
        /* private synchronization variables */
        private int[] activeCounter;
        private int[] activeCompare;
        private int[] activeIndex;
        private int activeAgentCount;
        private int numberOfAgents;

        private const int WAIT_DELAY = 100;

        /* private static variables */
        private static object syncLock;

        static Synchronization()
        {
            syncLock = new object();
        }

        public Synchronization(int numberOfAgents)
        {
            this.numberOfAgents = numberOfAgents;
            this.activeCounter = new int[2] { 0, 0 };
            this.activeCompare = new int[2] { numberOfAgents, numberOfAgents };
            this.activeIndex = new int[numberOfAgents];
            this.activeAgentCount = numberOfAgents;
        }

        public void RemoveActiveAgent(int agentId)
        {
            lock (syncLock)
            {   
                activeCompare[activeIndex[agentId]]--;
                activeAgentCount--;
            }
        }
        
        public void Wait(int agentId)
        {
            int compare, counter;

            /* increment the active counter */
            lock (syncLock)            
                activeCounter[activeIndex[agentId]]++;            

            /* busy wait loop */
            do
            {
                lock (syncLock)
                {
                    compare = activeCompare[activeIndex[agentId]];
                    counter = activeCounter[activeIndex[agentId]];                    
                }
            } while (compare != counter);

            /* decrement the active counter and active compare and toggle the active index */
            lock (syncLock)
            {
                activeCounter[activeIndex[agentId]]--;
                activeCompare[activeIndex[agentId]]--;
                activeIndex[agentId] = (activeIndex[agentId] == 0) ? 1 : 0;
                activeCompare[activeIndex[agentId]] = activeAgentCount;
            }
        }
    }
}
