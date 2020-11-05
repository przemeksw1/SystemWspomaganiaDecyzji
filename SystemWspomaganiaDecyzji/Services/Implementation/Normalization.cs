using System;
using System.Collections.Generic;
using System.Text;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class Normalization
    {
        public static void DoNormalization(int numOfColumn)
        {
            //dyskretyzacja przez dodanie nowej kolumny
            AllRows.GetInstance().HeaderName.Insert(numOfColumn + 1, AllRows.GetInstance().HeaderName[numOfColumn] + "__norm");
            string cell = "";
            decimal avg = MathHelper.CalcAverage(AllRows.GetInstance().FullFile, numOfColumn);
            decimal deviation = MathHelper.CalcStandardDeviation(AllRows.GetInstance().FullFile, numOfColumn);
            decimal result = 0;
            decimal value = 0;
            //decimal testAvg = 0; //zmienna do testowania wyniku - dla danych poprawnie znormalizowanych średnia=0
            for (int i = 0; i < AllRows.GetInstance().FullFile.Count; i++)
            {
                value = Convert.ToDecimal(AllRows.GetInstance().FullFile[i].Value[numOfColumn]);
                result = (value - avg) / deviation;
                //testAvg += result;
                result = Math.Round(result, 3);
                cell = result.ToString();
                AllRows.GetInstance().FullFile[i].Value.Insert(numOfColumn + 1, cell);
            }
            //testAvg /= AllRows.GetInstance().FullFile.Count; //dla danych poprawnie znormalizowanych wynik=0
        }
    }
}
