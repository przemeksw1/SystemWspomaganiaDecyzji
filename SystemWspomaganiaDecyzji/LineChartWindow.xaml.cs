using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji
{
    /// <summary>
    /// Logika interakcji dla klasy LineChartWindow.xaml
    /// </summary>
    public partial class LineChartWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string AxisXName { get; set; }
        public string AxisYName { get; set; }

        public LineChartWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                }
            };
            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            DataContext = this;
        }

        public LineChartWindow(int axisX, int axisY, int decisionClass)
        {
            InitializeComponent();
            AllRows allRows = AllRows.GetInstance();
            if (decisionClass == -1)
            {
                ChartValues<ObservablePoint> ValuesA = new ChartValues<ObservablePoint>();
                ScatterSeries scatter1 = new ScatterSeries();
                scatter1.Title = "No class";

                Wykres.Series.Add(scatter1);
                
                for (int i = 0; i < allRows.FullFile.Count; i++)
                {

                    ValuesA.Add(new ObservablePoint(Convert.ToDouble(allRows.FullFile[i].Value[axisX]), Convert.ToDouble(allRows.FullFile[i].Value[axisY])));
                }
                scatter1.Values = ValuesA;
               
            }
            else
            {
                List<string> classDecisionList = new List<string>();
                List<ChartValues<ObservablePoint>> observablePoints = new List<ChartValues<ObservablePoint>>();
                List<ScatterSeries> scatterPoints = new List<ScatterSeries>();
                for (int i = 0; i < allRows.FullFile.Count(); i++)
                {
                    var text = classDecisionList.SingleOrDefault(s => s.Equals(allRows.FullFile[i].Value[decisionClass]));
                    if (text == null)
                    {
                        classDecisionList.Add(allRows.FullFile[i].Value[decisionClass]);
                        ScatterSeries sc = new ScatterSeries();
                        sc.Title = allRows.FullFile[i].Value[decisionClass];
                        scatterPoints.Add(sc);
                        ChartValues<ObservablePoint> ob = new ChartValues<ObservablePoint>();
                        observablePoints.Add(ob);
                        ob.Add(new ObservablePoint(Convert.ToDouble(allRows.FullFile[i].Value[axisX]), Convert.ToDouble(allRows.FullFile[i].Value[axisY])));
                    }
                    else
                    {
                        var textToSearch = allRows.FullFile[i].Value[decisionClass];
                        int index = classDecisionList.FindIndex(c => c == textToSearch);
                        observablePoints[index].Add(new ObservablePoint(Convert.ToDouble(allRows.FullFile[i].Value[axisX]), Convert.ToDouble(allRows.FullFile[i].Value[axisY])));
                    }
                }
                
                for(int i=0; i< classDecisionList.Count(); i++)
                {
                    Wykres.Series.Add(scatterPoints[i]);
                    scatterPoints[i].Values = observablePoints[i];
                }


            }

          

            /*
            AllRows allRows = AllRows.GetInstance();
            ChartValues<double> chartY = new ChartValues<double>();
            AxisXName = allRows.HeaderName[axisX];
            AxisYName = allRows.HeaderName[axisY];
            string[] label = new string[allRows.FullFile.Count];
            for(int i=0; i< allRows.FullFile.Count; i++)
            {
                chartY.Add(Convert.ToDouble(allRows.FullFile[i].Value[axisY]));
                label[i] = allRows.FullFile[i].Value[axisX];
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = AxisXName + " " + AxisYName,
                    Values = chartY
                }
            };
            Labels = label;
            YFormatter = value => value.ToString("C");
            */
            DataContext = this;
        }
    }
}
