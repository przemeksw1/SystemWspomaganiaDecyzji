using System;
using System.Collections.Generic;
using System.Text;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class PercentMinMax
    {
        public static List<List<RowView>> DoPercentMinMaxCut(double min, double max, int columnNumber)
        {
            List<double> sortedRows = MathHelper.SortRowsByColumn(AllRows.GetInstance().FullFile, columnNumber);
            List<RowView> minResultRows = new List<RowView>();
            List<RowView> maxResultRows = new List<RowView>();
            List<List<RowView>> resultRows = new List<List<RowView>>();

            int minInterval = Convert.ToInt32(Math.Round(min/100 * sortedRows.Count));
            int maxInterval = sortedRows.Count - Convert.ToInt32(Math.Round(max/100 * sortedRows.Count));
            double minLeft = sortedRows[0];
            double minRight = sortedRows[minInterval - 1];
            double maxLeft = sortedRows[maxInterval];
            double maxRight = sortedRows[sortedRows.Count - 1];

            double cell;
            foreach(var row in AllRows.GetInstance().FullFile)
            {
                cell = Convert.ToDouble(row.Value[columnNumber]);
                if (cell >= minLeft && cell <= minRight) minResultRows.Add(row);
                else if (cell >= maxLeft && cell <= maxRight) maxResultRows.Add(row);
            }

            resultRows.Add(minResultRows);
            resultRows.Add(maxResultRows);
            return resultRows;
        }
    }
}
