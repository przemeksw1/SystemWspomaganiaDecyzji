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

        public static decimal[] FindMinMax(List<RowView> allRows, int columnNumber)
        {
            List<double> sortedColumn = SortRowsByColumn(allRows, columnNumber);
            decimal[] minmax = new decimal[2];
            minmax[0] = Convert.ToDecimal(sortedColumn[0]);
            minmax[1] = Convert.ToDecimal(sortedColumn[sortedColumn.Count - 1]);
            return minmax;
        }

        public static decimal CalcAverage(List<RowView> allRows, int columnNumber)
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
            decimal avg = CalcAverage(allRows, columnNumber);
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
