using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace AINav
{
    public class Node : Label
    {
        private Point position;
        private Node parent;
        private bool isWalkable;        
        private bool isVisible;
        private bool isPath;
        private bool isSpecial;
        private bool isShared;

        private double heuristicCost;
        private double movementCost;
        private double totalCost;
        
        #region Constructor

        public Node(int x, int y, int width, int height)
        {
            this.position = new Point(x, y);
            this.parent = null;
            this.isWalkable = true;            
            this.isVisible = false;
            this.isPath = false;
            this.isSpecial = false;
            this.isShared = false;
            this.heuristicCost = 0;
            this.movementCost = 0;
            this.totalCost = 0;            
            
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font("Arial", 4);
            this.Location = new Point(position.X * width, position.Y * height);
            this.Size = new Size(width, height);
        }

        #endregion Constructor

        #region Enum

        public enum Cost
        {
            Heuristic,
            Movement,
            Total,
        }

        public enum Flag
        {           
            IsVisible,
            IsWalkable,
            IsPath,
            IsSpecial,
            IsShared,
        }

        public enum Method
        {
            None,
            Euclidean, 
            EuclideanSquared,
            Manhattan,
            Chebyshev,
        }

        #endregion Enum

        #region Getters

        public double getCost(Cost cost)
        {
            switch (cost)
            {
                case Cost.Heuristic:
                    return this.heuristicCost;
                case Cost.Movement:
                    return this.movementCost;
                case Cost.Total:
                    return this.totalCost;
                default:
                    throw new Exception("invalid cost type get");
            }
        }

        public bool getFlag(Flag flag)
        {
            switch (flag)
            {                
                case Flag.IsVisible:
                    return this.isVisible;
                case Flag.IsWalkable:
                    return this.isWalkable;
                case Flag.IsPath:
                    return this.isPath;
                case Flag.IsSpecial:
                    return this.isSpecial;
                case Flag.IsShared:
                    return this.isShared;
                default:
                    throw new Exception("invalid node boolean type get");
            }
        }        

        public Point getPosition()
        {
            return this.position;
        }

        public Node getParent()
        {
            return this.parent;
        }

        #endregion Getters

        #region Setters

        public void setCost(Cost cost, double value)
        {
            switch (cost)
            {
                case Cost.Heuristic:
                    this.heuristicCost = value;
                    break;
                case Cost.Movement:
                    this.movementCost = value;
                    break;
                case Cost.Total:
                    this.totalCost = value;
                    break;
                default:
                    throw new Exception("invalid cost type get");
            }
        }

        public void setFlag(Flag flag, bool value)
        {
            switch (flag)
            {                
                case Flag.IsVisible:
                    this.isVisible = value;
                    break;
                case Flag.IsWalkable:
                    this.isWalkable = value;
                    break;
                case Flag.IsPath:
                    this.isPath = value;
                    break;
                case Flag.IsSpecial:
                    this.isSpecial = value;
                    break;
                case Flag.IsShared:
                    this.isShared = value;
                    break;
                default:
                    throw new Exception("invalid node boolean type set");
            }
        }

        public delegate void RepaintNodeDelegate(Color backColor, Color foreColor, String text); 
        
        public void repaintNode(Color backColor, Color foreColor, String text)
        {
            if (this.InvokeRequired)
                this.Invoke(new RepaintNodeDelegate(repaintNode), backColor, foreColor, text);
            else
            {
                bool refresh = false;
                
                if (!this.BackColor.Equals(backColor))
                {
                    this.BackColor = backColor;
                    refresh = true;
                }

                if (!this.ForeColor.Equals(foreColor))
                {
                    this.ForeColor = foreColor;
                    refresh = true;
                }

                if (!this.Text.Equals(text))
                {
                    this.Text = text;
                    refresh = true;
                }

                if (refresh)
                    this.Refresh();
            }
        }

        public void setParent(Node parent)
        {
            this.parent = parent;
        }

        #endregion Setters                

        #region HelperMethods        

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }

        public String toString()
        {
            String str = "node = (" + this.position.X + "," + this.position.Y + ")";
            if (this.parent != null)
                str += "\nparent = (" + this.parent.getPosition().X + "," + this.parent.getPosition().Y + ")";
            else
                str += "\nparent = null";
            str += "\nheuristic cost = " + this.heuristicCost;
            str += "\nmovement cost = " + this.movementCost;
            str += "\ntotal cost = " + this.totalCost;
            str += "\nIs Walkable = " + isWalkable.ToString();            
            str += "\nIs Visible = " + isVisible.ToString();
            str += "\nIs Path = " + isPath.ToString();
            str += "\nIs Special = " + isSpecial.ToString();
            str += "\nIs Shared = " + isShared.ToString(); 
            str += "\nLocation = (" + Location.X + "," + Location.Y + ")";
            str += "\nSize = " + Size;
            str += "\nBack Color = " + BackColor;
            str += "\nFore Color = " + ForeColor;
            str += "\nText = " + Text;
            return str;
        }

        public double getDistance(Node a, Method method)
        {
            double dx = Math.Abs(a.getPosition().X - this.position.X);
            double dy = Math.Abs(a.getPosition().Y - this.position.Y);

            switch (method)
            {
                case Method.Manhattan:
                    return dx + dy;
                case Method.Euclidean:
                    return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                case Method.EuclideanSquared:
                    return Math.Pow(dx, 2) + Math.Pow(dy, 2);
                case Method.Chebyshev:
                    return Math.Max(dx, dy);
                default:
                    throw new Exception("method not defined");
            }
        }

        public bool isEqual(Node a)
        {
            return position.Equals(a.getPosition());
        }

        public bool isAdjacent(Node node, int max)
        {
            /*     0 1 2
             *     _ _ _
             * 0  |_|_|_|
             * 1  |_|_|_|
             * 2  |_|_|_|
             * 
             *   */
            try
            {
                if (node.getPosition().X - 1 == this.position.X && node.getPosition().Y + 1 == this.position.Y)
                    return true;
                if (node.getPosition().X - 0 == this.position.X && node.getPosition().Y + 1 == this.position.Y)
                    return true;
                if (node.getPosition().X + 1 == this.position.X && node.getPosition().Y + 1 == this.position.Y)
                    return true;
                if (node.getPosition().X - 1 == this.position.X && node.getPosition().Y + 0 == this.position.Y)
                    return true;
                if (node.getPosition().X - 0 == this.position.X && node.getPosition().Y + 0 == this.position.Y)
                    return true;
                if (node.getPosition().X + 1 == this.position.X && node.getPosition().Y + 0 == this.position.Y)
                    return true;
                if (node.getPosition().X - 1 == this.position.X && node.getPosition().Y - 1 == this.position.Y)
                    return true;
                if (node.getPosition().X - 0 == this.position.X && node.getPosition().Y - 1 == this.position.Y)
                    return true;
                if (node.getPosition().X + 1 == this.position.X && node.getPosition().Y - 1 == this.position.Y)
                    return true;
            } 
            catch
            {
                return false;
            }
            return false;
        }

        #endregion HelperMethods   
    }
}
