using System;
using System.Collections.Generic;
using System.Text;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class Discretization
    {
        public static void DoDiscretization(int numOfColumn, int countOfIntervals)
        {
            //tworzenie listy przedziałów
            List<decimal> intervals = new List<decimal>();
            decimal[] minmax = MathHelper.FindMinMax(AllRows.GetInstance().FullFile, numOfColumn);
            decimal intervalSize = (minmax[1] - minmax[0]) / countOfIntervals;
            decimal leftLimit = minmax[0];
            while (leftLimit < minmax[1] && intervals.Count<countOfIntervals)
            {
                intervals.Add(leftLimit);
                leftLimit += intervalSize;
            }
            //dyskretyzacja przez dodanie nowej kolumny
            AllRows.GetInstance().HeaderName.Insert(numOfColumn + 1, AllRows.GetInstance().HeaderName[numOfColumn] + "__dyskret"+countOfIntervals);
            string cell = "";
            for (int i = 0; i < AllRows.GetInstance().FullFile.Count; i++)
            {
                for (int j = 0; j < intervals.Count; j++)
                {
                    if (j == intervals.Count - 1)
                    {
                        cell = (j + 1).ToString();
                        AllRows.GetInstance().FullFile[i].Value.Insert(numOfColumn + 1, cell);
                        break;
                    }
                    else if (Convert.ToDecimal(AllRows.GetInstance().FullFile[i].Value[numOfColumn]) < intervals[j + 1])
                    {
                        cell = (j + 1).ToString();
                        AllRows.GetInstance().FullFile[i].Value.Insert(numOfColumn + 1, cell);
                        break;
                    }
                }
            }
        }
    }
}
