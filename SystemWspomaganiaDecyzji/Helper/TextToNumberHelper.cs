using System;
using System.Collections.Generic;
using System.Text;

namespace SystemWspomaganiaDecyzji.Models
{
    public class TextToNumberHelper
    {
        public string text { get; set; }
        public int number { get; set; }

        public int SortByTextAscending(string text1, string text2)
        {

            return text1.CompareTo(text2);
        }
    }
}
