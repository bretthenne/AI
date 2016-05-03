using System;
using System.Windows.Forms;

namespace AINav
{
    public class Results
    { 
        private ListView listView;

        public Results(ListView lv, String[] headers)
        {
            this.listView = lv;
            foreach (String header in headers)            
                listView.Columns.Add(new ColumnHeader() { 
                    Text = header, 
                    TextAlign = HorizontalAlignment.Left });
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);            
        }

        public void add(String[] str)
        {
            listView.Items.Add(new ListViewItem(str));
            listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(6, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.AutoResizeColumn(7, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.Items[listView.Items.Count - 1].EnsureVisible();
        }

        public void clear()
        {
            listView.Items.Clear();
        }              
    }
}
