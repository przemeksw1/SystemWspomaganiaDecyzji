using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Helper
{
    public static class MathHelper
    { 
        public static List<double> SortRowsByColumn(List<RowView> allRows, int columnNumber)
        {
            List<double> sortedColumn = new List<double>();
            foreach (RowView row in allRows)
            {
                sortedColumn.Add(Convert.ToDouble(row.Value[columnNumber]));
            }
            sortedColumn.Sort();
            return sortedColumn;
        }

        public static double[] FindMinMaxInColumn(List<RowView> allRows, int columnNumber)
        {
            List<double> sortedColumn = SortRowsByColumn(allRows, columnNumber);
            double[] minmax = new double[2];
            minmax[0] = sortedColumn[0];
            minmax[1] = sortedColumn[sortedColumn.Count - 1];
            return minmax;
        }

        public static decimal CalcAverageOfColumn(List<RowView> allRows, int columnNumber)
        {
            decimal sum = 0;
            foreach (var row in allRows)
            {
                sum += Convert.ToDecimal(row.Value[columnNumber]);
            }
            return (sum / allRows.Count);
        }

        public static decimal CalcStandardDeviation(List<RowView> allRows, int columnNumber)
        {
            decimal avg = CalcAverageOfColumn(allRows, columnNumber);
            decimal deviation = 0;
            decimal value;
            foreach (var row in allRows)
            {
                value = Convert.ToDecimal(row.Value[columnNumber]);
                deviation += Convert.ToDecimal(Math.Pow(Convert.ToDouble(value - avg), 2));
            }
            deviation = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(deviation) / allRows.Count));
            return deviation;
        }
    }
}
