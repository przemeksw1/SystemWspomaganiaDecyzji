using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class TextToNumeric
    {
        public static void AlfabeticTextToNumber(int collumn)
        {
            AllRows allRows = AllRows.GetInstance();
            List<TextToNumberHelper> tmpHelperList = new List<TextToNumberHelper>();
            TextToNumberHelper textToNumberHealper = new TextToNumberHelper();

            allRows.HeaderName.Insert(collumn + 1, allRows.HeaderName[collumn] + "__num__a");

            for (int i = 0; i < allRows.FullFile.Count(); i++)
            {
                var text = tmpHelperList.SingleOrDefault(s => s.text.Equals(allRows.FullFile[i].Value[collumn]));
                if (text == null)
                {
                    textToNumberHealper = new TextToNumberHelper();
                    textToNumberHealper.text = allRows.FullFile[i].Value[collumn];
                    tmpHelperList.Add(textToNumberHealper);
                }
            }
            tmpHelperList.Sort(delegate (TextToNumberHelper x, TextToNumberHelper y)
            {
                if (x.text == null && y.text == null) return 0;
                else if (x.text == null) return -1;
                else if (y.text == null) return 1;
                else return x.text.CompareTo(y.text); 
            });
            for (int i = 0; i< tmpHelperList.Count(); i++)
            {
                tmpHelperList[i].number = i + 1;
            }
            for (int i = 0; i < allRows.FullFile.Count(); i++)
            {
                var text = tmpHelperList.SingleOrDefault(s => s.text.Equals(allRows.FullFile[i].Value[collumn]));
                allRows.FullFile[i].Value.Insert(collumn + 1, text.number.ToString());
            }
        }

        public static void OrderTextToNumber(int collumn)
        {
            
            AllRows allRows = AllRows.GetInstance();
            List<TextToNumberHelper> tmpHealerList = new List<TextToNumberHelper>();
            TextToNumberHelper textToNumberHealper = new TextToNumberHelper();
            int counter = 1;
            
            allRows.HeaderName.Insert(collumn + 1, allRows.HeaderName[collumn] + "__num__k");

            for (int i= 0; i<allRows.FullFile.Count(); i++)
            {
                var text = tmpHealerList.SingleOrDefault(s => s.text.Equals(allRows.FullFile[i].Value[collumn]));
                if (text == null)
                {
                    textToNumberHealper = new TextToNumberHelper();
                    textToNumberHealper.text = allRows.FullFile[i].Value[collumn];
                    textToNumberHealper.number = counter;
                    allRows.FullFile[i].Value.Insert(collumn + 1, counter.ToString()) ;
                    counter++;
                    tmpHealerList.Add(textToNumberHealper);
                }
                else
                {
                    allRows.FullFile[i].Value.Insert(collumn + 1, text.number.ToString());
                }
               // if (allRows.FullFile[i].Value[collumn])
            }

        }
    }
}
