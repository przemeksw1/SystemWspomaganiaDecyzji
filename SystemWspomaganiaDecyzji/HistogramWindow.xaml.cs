using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji
{
    /// <summary>
    /// Logika interakcji dla klasy HistogramWindow.xaml
    /// </summary>
    public partial class HistogramWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection Series { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private double _from;
        private double _to;
        private readonly ChartValues<GanttPoint> _values;
        public HistogramWindow()
        {
            InitializeComponent();

            var now = DateTime.Now;

            _values = new ChartValues<GanttPoint>
            {
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks)
            };

            Series = new SeriesCollection
            {
                new RowSeries
                {
                    Values = _values,
                    DataLabels = true
                }
            };
            Formatter = value => new DateTime((long)value).ToString("dd MMM");

            var labels = new List<string>();
            for (var i = 0; i < 12; i++)
                labels.Add("Task " + i);
            Labels = labels.ToArray();

            ResetZoomOnClick(null, null);


            DataContext = this;
        }

        public double From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            From = _values.First().StartPoint;
            To = _values.Last().EndPoint;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public HistogramWindow(int columnNumber, int countOfIntervals)
        {
            InitializeComponent();

            AllRows allRows = AllRows.GetInstance();
            bool rowIsString = false;
            try
            {
                double tmp = Convert.ToDouble(allRows.FullFile[0].Value[columnNumber]);
            }
            catch
            {
                rowIsString = true;
            }

            if(rowIsString)
            {
                List<string> allString = new List<string>();
                ChartValues<double> rowCount= new ChartValues<double>();
                for (int i = 0; i < allRows.FullFile.Count(); i++)
                {
                    var text = allString.SingleOrDefault(s => s.Equals(allRows.FullFile[i].Value[columnNumber]));
                    if (text == null)
                    {
                        var counter = allRows.FullFile.Where(s => s.Value[columnNumber] == allRows.FullFile[i].Value[columnNumber]).Count();
                        allString.Add(allRows.FullFile[i].Value[columnNumber]);
                        rowCount.Add(counter);
                    }
                }

                string[] converter = allString.ToArray();

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {               
                        
                        Values = rowCount
                    }
                };

                Labels = converter;
                Formatter = value => value.ToString("N");
            }
            else
            {
                List<double> allValue = new List<double>();
                ChartValues<double> rowCount = new ChartValues<double>();

                List<decimal> intervals = new List<decimal>();
                decimal[] minmax = MathHelper.FindMinMax(AllRows.GetInstance().FullFile, columnNumber);
                decimal intervalSize = (minmax[1] - minmax[0]) / countOfIntervals;
                decimal leftLimit = minmax[0];
                while (leftLimit < minmax[1])
                {
                    intervals.Add(leftLimit);
                    leftLimit += intervalSize;
                }
                double cell;
                for (int i = 0; i < AllRows.GetInstance().FullFile.Count; i++)
                {
                    for (int j = 0; j < intervals.Count; j++)
                    {
                        if (j == intervals.Count - 1)
                        {
                            cell = j + 1;
                            allValue.Add(cell);
                            break;
                        }
                        else if (Convert.ToDecimal(AllRows.GetInstance().FullFile[i].Value[columnNumber]) < intervals[j + 1])
                        {
                            cell = j + 1;
                            allValue.Add(cell);
                            break;
                        }
                    }
                }

                string[] label = new string[intervals.Count()];

                for (int i=0; i<intervals.Count(); i++ )
                {
                    if (i == intervals.Count() - 1)
                    {
                        label[i] = "<" + Math.Round(intervals[i],3).ToString() + " : " + minmax[1].ToString() + ">";
                    }
                    else
                    {
                        label[i] = "<"+ Math.Round(intervals[i],3).ToString() + " : " + Math.Round(intervals[i+1],3).ToString() + ")";
                    }
                    var counter = allValue.Where(c => c == i + 1).Count();
                    rowCount.Add(counter);
                }

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                         Values = rowCount
                    }
                };
               
                
                Labels = label;
                Formatter = value => value.ToString("N");
            }
       

            ////adding series will update and animate the chart automatically
            //SeriesCollection.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            ////also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);
           
           

            DataContext = this;
        }
    }
}
