using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Helper
{

    public enum MetricName{
        Euklides,
        Manhattan,
        Czebyszew,
        Mahalanobis
    }


    public static class ClasificationMetrics
    {
        public static Dictionary<int, double> Euklides(RowView newRow, List<RowView> allRows)
        {
            double sum = 0;
            double pow = 0;
            Dictionary<int, double> results = new Dictionary<int, double>();
            //List<double> results = new List<double>();
            double value;
            double newValue;
            int rowId = 0;
            foreach (var row in allRows)
            {
                for(int i=0; i<newRow.Value.Count-1 ; i++)
                {
                    value = Convert.ToDouble(row.Value[i]);
                    newValue = Convert.ToDouble(newRow.Value[i]);
                    pow = value - newValue;
                    pow = Math.Pow(pow, 2);
                    sum += pow;
                }
                sum = Math.Sqrt(sum);
                results.Add(rowId, sum);
                sum = 0;
                pow = 0;
                //results.Add(sum);
                rowId++;
            }
            return results;
        }

        public static Dictionary<int, double> Manhatan(RowView newRow, List<RowView> allRows)
        {
            double sum = 0;
            double mod = 0;
            Dictionary<int, double> results = new Dictionary<int, double>();
            //List<double> results = new List<double>();
            double value;
            double newValue;
            int rowId = 0;
            foreach (var row in allRows)
            {
                for (int i = 0; i < newRow.Value.Count - 1; i++)
                {
                    value = Convert.ToDouble(row.Value[i]);
                    newValue = Convert.ToDouble(newRow.Value[i]);
                    mod = value - newValue;
                    mod = Math.Abs(mod);
                    sum += mod;
                }
                results.Add(rowId, sum);
                sum = 0;
                mod = 0;
                //results.Add(sum);
                rowId++;
            }
            return results;
        }

        public static Dictionary<int, double> Czebyszew(RowView newRow, List<RowView> allRows)
        {
            double max = 0;
            double mod = 0;
            Dictionary<int, double> results = new Dictionary<int, double>();
            //List<double> results = new List<double>();
            double value;
            double newValue;
            int rowId = 0;
            foreach (var row in allRows)
            {
                for (int i = 0; i < newRow.Value.Count - 1; i++)
                {
                    value = Convert.ToDouble(row.Value[i]);
                    newValue = Convert.ToDouble(newRow.Value[i]);
                    mod = value - newValue;
                    mod = Math.Abs(mod);
                    max = Math.Max(max, mod);
                }
                results.Add(rowId, max);
                max = 0;
                mod = 0;
                //results.Add(sum);
                rowId++;
            }
            return results;
        }



    }


}
