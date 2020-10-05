using System;
using System.Collections.Generic;
using System.Text;

namespace SystemWspomaganiaDecyzji.Models
{
    public class AllRows
    {
        private static readonly object _lock = new object();
        public List<RowView> FullFile { get; set; }
        private static AllRows _instance;
        public string tmp = "dupa";
        private AllRows()
        {
            FullFile = new List<RowView>();
        }
        public static void ClearFullFile()
        {
            _instance = new AllRows();
        }
        public static AllRows GetInstance()
        {
            if(_instance == null)
            {
                lock (_lock)
                {
                    _instance = new AllRows();
                }
            }

            return _instance;            
        }
        

    }

    public class RowView
    {
        public RowView()
        {
            Value = new List<string>();
        }
        public List<string> Value { get; set; }        
    }
}
