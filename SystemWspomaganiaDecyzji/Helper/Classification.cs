using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Helper
{
    public class Classification
    {
        int NeighboursCount;
        MetricName Metric;
        RowView NewObject;
        int DecisionColNumber;

        public Classification(int k, MetricName metric, int decisionColNumber)
        {
            NeighboursCount = k;
            Metric = metric;
            DecisionColNumber = decisionColNumber;
        }

        public string Classify(RowView newObject)
        {
            NewObject = newObject;
            Dictionary<int, double> results = new Dictionary<int, double>();
            //List<double> results = new List<double>();
            List<RowView> neighbours = new List<RowView>();
            Dictionary<int, double> sortedResults = new Dictionary<int, double>();
            //List<double> sortedResults = new List<double>();
            Dictionary<string, int> decisionsCounter = new Dictionary<string, int>();
            int objectID;
            string decisionValue;
            double minDistance=0;
            string minResult="";
            string finalDecision = "";

            //wybór metryki do zliczenia odleglosci miedzy wszystkimi obiektami
            switch (Metric)
            {
                case MetricName.Euklides:
                    results = ClasificationMetrics.Euklides(NewObject, AllRows.GetInstance().FullFile);
                    break;
                case MetricName.Manhattan:
                    results = ClasificationMetrics.Manhatan(NewObject, AllRows.GetInstance().FullFile);
                    break;
                case MetricName.Czebyszew:
                    results = ClasificationMetrics.Czebyszew(NewObject, AllRows.GetInstance().FullFile);
                    break;
                case MetricName.Mahalanobis:
                    results = ClasificationMetrics.Mahalanobis(NewObject, AllRows.GetInstance().FullFile);
                    break;
                default:
                    results = ClasificationMetrics.Euklides(NewObject, AllRows.GetInstance().FullFile);
                    break;
            }

            //wybor k najblizszych sasiadow
            sortedResults = results.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            for (int k=0; k<NeighboursCount; k++)
            {
                objectID = sortedResults.ElementAt(k).Key;
                neighbours.Add(AllRows.GetInstance().FullFile[objectID]);
                if (sortedResults.ElementAt(k).Value < minDistance || k==0)
                {
                    minDistance = sortedResults.ElementAt(k).Value;
                    minResult = neighbours[k].Value[DecisionColNumber];
                }
            }

            //zliczanie ktora decyzja wsrod sasiadow dominuje
            int temp;
            for (int i = 0; i < neighbours.Count; i++)
            {
                decisionValue = neighbours[i].Value[DecisionColNumber];
                if (decisionsCounter.Where(x => x.Key == decisionValue).ToList().Count == 0)
                {
                    temp = neighbours.Where(x => x.Value[DecisionColNumber] == decisionValue).ToList().Count;
                    decisionsCounter.Add(decisionValue, temp);
                }
            }
            decisionsCounter = decisionsCounter.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            int distance = decisionsCounter.ElementAt(0).Value;
            if (decisionsCounter.Where(x => x.Value == distance).ToList().Count > 1)
                finalDecision = minResult;
            else finalDecision = decisionsCounter.ElementAt(0).Key;

            return finalDecision;
        }

        public decimal GetClassificationQuality()
        {
            List<bool> results = new List<bool>();
            string classifyResult;
            decimal correctCount;
            decimal quality;

            foreach (RowView row in AllRows.GetInstance().FullFile)
            {
                classifyResult = Classify(row);
                if (classifyResult == row.Value[DecisionColNumber]) results.Add(true);
                else results.Add(false);
            }

            correctCount = results.Where(x => x == true).ToList().Count();
            quality = correctCount / results.Count();
            return quality;
        }

    }
}
