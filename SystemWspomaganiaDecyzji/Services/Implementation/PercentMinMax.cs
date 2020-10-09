using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    public static class PercentMinMax
    {
        public static List<List<RowView>> DoPercentMinMaxCut(double min, double max, int columnNumber)
        {
            List<RowView> allRows = AllRows.GetInstance().FullFile;
            allRows = allRows.OrderBy(o => Convert.ToDouble(o.Value[columnNumber])).ToList();
            List<RowView> minResultRows = new List<RowView>();
            List<RowView> maxResultRows = new List<RowView>();
            List<List<RowView>> resultRows = new List<List<RowView>>();

            int minInterval = Convert.ToInt32(Math.Round(min/100 * allRows.Count));
            int maxInterval = allRows.Count - Convert.ToInt32(Math.Round(max/100 * allRows.Count));

            for(int i=0; i<minInterval; i++)
                minResultRows.Add(allRows[i]);
            for (int i = maxInterval; i < allRows.Count; i++)
                maxResultRows.Add(allRows[i]);

            resultRows.Add(minResultRows);
            resultRows.Add(maxResultRows);
            return resultRows;
        }
    }
}
