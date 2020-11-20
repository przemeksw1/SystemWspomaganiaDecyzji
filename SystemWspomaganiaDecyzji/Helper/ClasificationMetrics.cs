using Accord.Statistics;
using Accord.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Media;
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
        public static Dictionary<int, double> Euklides(RowView newRow, List<RowView> allRows, int decisionColumn)
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
                for(int i=0; i<newRow.Value.Count ; i++)
                {
                    if (i != decisionColumn)
                    {
                        value = Convert.ToDouble(row.Value[i]);
                        newValue = Convert.ToDouble(newRow.Value[i]);
                        pow = value - newValue;
                        pow = Math.Pow(pow, 2);
                        sum += pow;
                    }
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

        public static Dictionary<int, double> Manhatan(RowView newRow, List<RowView> allRows, int decisionColumn)
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
                for (int i = 0; i < newRow.Value.Count; i++)
                {
                    if (i != decisionColumn)
                    {
                        value = Convert.ToDouble(row.Value[i]);
                        newValue = Convert.ToDouble(newRow.Value[i]);
                        mod = value - newValue;
                        mod = Math.Abs(mod);
                        sum += mod;
                    }
                }
                results.Add(rowId, sum);
                sum = 0;
                mod = 0;
                //results.Add(sum);
                rowId++;
            }
            return results;
        }

        public static Dictionary<int, double> Czebyszew(RowView newRow, List<RowView> allRows, int decisionColumn)
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
                for (int i = 0; i < newRow.Value.Count; i++)
                {
                    if (i != decisionColumn)
                    {
                        value = Convert.ToDouble(row.Value[i]);
                        newValue = Convert.ToDouble(newRow.Value[i]);
                        mod = value - newValue;
                        mod = Math.Abs(mod);
                        max = Math.Max(max, mod);
                    }
                }
                results.Add(rowId, max);
                max = 0;
                mod = 0;
                //results.Add(sum);
                rowId++;
            }
            return results;
        }

        public static Dictionary<int, double> Mahalanobis(RowView newRow, List<RowView> allRows, int decisionColumn)
        {
            Dictionary<int, double> results = new Dictionary<int, double>();
            double value;
            double newValue;
            int rowId = 0;

            double[] vector = new double[allRows[0].Value.Count-1];
            double[,] matrix = new double[allRows.Count, allRows[0].Value.Count-1];
            double[,] covMatrix;
            double cell = 0;
            List<double> vectorTimesMatrix = new List<double>();

            //stowrzenie macierzy danych do obliczenia macierzy kowariancji
            for (int i=0; i<allRows.Count; i++)
            {
                for (int j = 0, listj = 0; j < allRows[i].Value.Count-1; j++, listj++)
                {
                    if (listj == decisionColumn) listj++;
                    matrix[i, j] = Convert.ToDouble(allRows[i].Value[listj]);
                }
            }
            //obliczanie macierzy kowariancji i jej odwrotności
            covMatrix = Measures.Covariance(matrix);
            covMatrix = Accord.Math.Matrix.Inverse(covMatrix);


            foreach (var row in allRows)
            {
                vectorTimesMatrix = new List<double>();

                //obliczanie wektora (x-y)
                for (int i = 0, listi=0; i < newRow.Value.Count-1; i++, listi++)
                {
                    if (listi == decisionColumn) listi++;
                    value = Convert.ToDouble(row.Value[listi]);
                    newValue = Convert.ToDouble(newRow.Value[listi]);
                    vector[i] = value - newValue;
                }

                // vectorTimesMatrix = (x-y)^T * Sigmna^-1  => mnożenie transponowanego wektora razy odwórcona macierz kowariancji
                for (int i=0; i < vector.Length; i++)
                {
                    cell = 0;
                    for (int j=0; j<vector.Length; j++)
                        cell += vector[j] * covMatrix[i, j];
                    vectorTimesMatrix.Add(cell);
                }

                // vectorTimesMatrix * (x-y) = (x-y)^T * Sigmna^-1 * (x-y)   => mnożenie uzyskanego wyniku razy wektor
                cell = 0;
                for (int i=0; i< vectorTimesMatrix.Count; i++)
                    cell += vectorTimesMatrix[i] * vector[i];

                //result = sqrt(MD^2)
                cell = Math.Sqrt(cell);

                //dodanie numeru wiersza i wyniku do zwracanej listy
                results.Add(rowId, cell);
                rowId++;
            }

            return results;
        }



    }


}
