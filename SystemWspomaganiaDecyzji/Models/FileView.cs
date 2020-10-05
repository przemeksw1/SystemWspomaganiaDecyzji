using System;
using System.Collections.Generic;
using System.Text;

namespace SystemWspomaganiaDecyzji.Models
{
    public class AllColumns
    {
        private static readonly object _lock = new object();
        public List<ColumnView> FullFile { get; set; }
        private static AllColumns _instance;
        public string tmp = "dupa";
        private AllColumns()
        {
            FullFile = new List<ColumnView>();
        }
        public static void ClearFullFile()
        {
            _instance = new AllColumns();
        }
        public static AllColumns GetInstance()
        {
            if(_instance == null)
            {
                lock (_lock)
                {
                    _instance = new AllColumns();
                }
            }

            return _instance;            
        }
        

    }

    public class ColumnView
    {
        public ColumnView()
        {
            Value = new List<string>();
        }
        public List<string> Value { get; set; }        
    }
}
