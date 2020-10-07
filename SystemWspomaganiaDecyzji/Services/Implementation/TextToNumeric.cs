using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public class TextToNumeric : ITextToNumeric
    {
        public void AlfabeticTextToNumber(int collumn, bool firstRowHaveHeader)
        {
            AllRows allRows = AllRows.GetInstance();
            List<TextToNumberHealper> tmpHelperList = new List<TextToNumberHealper>();
            TextToNumberHealper textToNumberHealper = new TextToNumberHealper();

            allRows.HeaderName.Insert(collumn + 1, allRows.HeaderName[collumn] + "__num__a");

            for (int i = 0; i < allRows.FullFile.Count(); i++)
            {
                var text = tmpHelperList.SingleOrDefault(s => s.text.Equals(allRows.FullFile[i].Value[collumn]));
                if (text == null)
                {
                    textToNumberHealper = new TextToNumberHealper();
                    textToNumberHealper.text = allRows.FullFile[i].Value[collumn];
                    tmpHelperList.Add(textToNumberHealper);
                }
            }
            tmpHelperList.Sort(delegate (TextToNumberHealper x, TextToNumberHealper y)
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

        public void OrderTextToNumber(int collumn, bool firstRowHaveHeader)
        {
            
            AllRows allRows = AllRows.GetInstance();
            List<TextToNumberHealper> tmpHealerList = new List<TextToNumberHealper>();
            TextToNumberHealper textToNumberHealper = new TextToNumberHealper();
            int counter = 1;
            
            allRows.HeaderName.Insert(collumn + 1, allRows.HeaderName[collumn] + "__num__k");

            for (int i= 0; i<allRows.FullFile.Count(); i++)
            {
                var text = tmpHealerList.SingleOrDefault(s => s.text.Equals(allRows.FullFile[i].Value[collumn]));
                if (text == null)
                {
                    textToNumberHealper = new TextToNumberHealper();
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
