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
            Dictionary<string, double> minDistance = new Dictionary<string, double>();
            double finalResult;
            string finalDecision = "";

            List<RowView> allRows = AllRows.GetInstance().FullFile;

            //wybór metryki do zliczenia odleglosci miedzy wszystkimi obiektami
            switch (Metric)
            {
                case MetricName.Euklides:
                    results = ClasificationMetrics.Euklides(NewObject, allRows, DecisionColNumber);
                    break;
                case MetricName.Manhattan:
                    results = ClasificationMetrics.Manhatan(NewObject, allRows, DecisionColNumber);
                    break;
                case MetricName.Czebyszew:
                    results = ClasificationMetrics.Czebyszew(NewObject, allRows, DecisionColNumber);
                    break;
                case MetricName.Mahalanobis:
                    results = ClasificationMetrics.Mahalanobis(NewObject, allRows, DecisionColNumber);
                    break;
                default:
                    results = ClasificationMetrics.Euklides(NewObject, allRows, DecisionColNumber);
                    break;
            }

            //wybor k najblizszych sasiadow
            sortedResults = results.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            for (int k=0; k<NeighboursCount; k++)
            {
                objectID = sortedResults.ElementAt(k).Key;
                neighbours.Add(allRows[objectID]);
                string tempDecision = neighbours[k].Value[DecisionColNumber];
                if (minDistance.Where(c => c.Key == tempDecision).ToList().Count > 0)
                {
                    double tempMinResult = minDistance.Where(c => c.Key == tempDecision).First().Value;
                    if (sortedResults.ElementAt(k).Value < tempMinResult)
                    {
                        minDistance.Remove(tempDecision);
                        tempMinResult = sortedResults.ElementAt(k).Value;
                        minDistance.Add(tempDecision, tempMinResult);
                    }
                }
                else
                {
                    double tempMinResult = sortedResults.ElementAt(k).Value;
                    minDistance.Add(tempDecision, tempMinResult);
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
            int neighboursCount = decisionsCounter.ElementAt(0).Value;
            if (decisionsCounter.Where(x => x.Value == neighboursCount).ToList().Count > 1)
            {            
                foreach(var val in minDistance)
                {
                    if (decisionsCounter.Where(x => x.Key == val.Key && x.Value == neighboursCount).ToList().Count == 0)
                        minDistance.Remove(val.Key);
                }
                finalDecision = minDistance.Where(c => c.Value == minDistance.Min(x => x.Value)).First().Key;
            }
            else finalDecision = decisionsCounter.ElementAt(0).Key;

            return finalDecision;
        }

        public decimal GetClassificationQuality()
        {
            List<bool> results = new List<bool>();
            string classifyResult;
            decimal correctCount;
            decimal quality;

            for (int rowID = 0; rowID < AllRows.GetInstance().FullFile.Count; rowID++)
            {
                var row = AllRows.GetInstance().FullFile[rowID];
                AllRows.GetInstance().FullFile.Remove(row);
                List<RowView> rows = AllRows.GetInstance().FullFile;
                classifyResult = Classify(row);
                AllRows.GetInstance().FullFile.Insert(rowID, row);
                List<RowView> rows1 = AllRows.GetInstance().FullFile;
                if (classifyResult == row.Value[DecisionColNumber]) results.Add(true);
                else results.Add(false);
            }

            correctCount = results.Where(x => x == true).ToList().Count();
            quality = correctCount / results.Count();
            return quality;
        }

        public List<decimal> GetClassificationQualityForAllNeighbours()
        {
            List<decimal> qualities = new List<decimal>();
            int maxK = AllRows.GetInstance().FullFile.Count-1;
            for (int k=1; k<=maxK; k++)
            {
                NeighboursCount = k;
                qualities.Add(GetClassificationQuality());
            }
            return qualities;
        }

    }
}
