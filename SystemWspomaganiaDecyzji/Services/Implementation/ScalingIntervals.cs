using System;
using System.Collections.Generic;
using System.Text;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class ScalingIntervals
    {
        public static void DoScalingIntervals(int numOfColumn, decimal newMin, decimal newMax)
        {
            double[] minmax = MathHelper.FindMinMax(AllRows.GetInstance().FullFile, numOfColumn);
            decimal oldMin = Convert.ToDecimal(minmax[0]);
            decimal oldMax = Convert.ToDecimal(minmax[1]);
            decimal k = (newMax - newMin) / (oldMax - oldMin); //skala rzutowania przedziałów
            //decimal k = newMax / Convert.ToDecimal(minmax[1]); 


            //rzutowanie przedziału na nowy przez dodanie nowej kolumny
            AllRows.GetInstance().HeaderName.Insert(numOfColumn + 1, AllRows.GetInstance().HeaderName[numOfColumn] + "__skal");
            string cell = "";
            decimal newValue;
            decimal value = 0;
            for (int i = 0; i < AllRows.GetInstance().FullFile.Count; i++)
            {
                value = Convert.ToDecimal(AllRows.GetInstance().FullFile[i].Value[numOfColumn]);
                newValue = newMin + k * (value - oldMin); //wzor na przerzutowana wartosc
                newValue = Math.Round(newValue,3);
                cell = newValue.ToString();
                AllRows.GetInstance().FullFile[i].Value.Insert(numOfColumn + 1, cell);
            }
        }
    }
}
