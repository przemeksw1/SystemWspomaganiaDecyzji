using SciChart.Charting3D.Model;
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
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji
{
    /// <summary>
    /// Logika interakcji dla klasy Chart3DWindow.xaml
    /// </summary>
    public partial class Chart3DWindow : Window
    {
        public Chart3DWindow(int x, int y, int z)
        {

            InitializeComponent();
            //var meshDataSeries = FillSeries(0, 200, 200);
            var meshDataSeries = FillSeriesByOwn(x, y, z);
            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
           // contourComboBox.ItemsSource = typeof(Colors).GetProperties().Select(x => new ColorModel { ColorName = x.Name, Color = (Color)x.GetValue(null, null) }).ToList();
        }

        private void CheckDrawSkirtChanged(object sender, RoutedEventArgs e)
        {
            if (surfaceMeshRenderableSeries != null)
            {
              //  surfaceMeshRenderableSeries.DrawSkirt = CheckDrawSkirt.IsChecked == true;
            }
        }
        private IDataSeries3D FillSeriesByOwn(int index, int width, int height)
        {

            AllRows allRows = AllRows.GetInstance();
            int xSize = allRows.FullFile.Count();
            int ySize = xSize;
            var xSteppings = new double[ySize];
            var ySteppings = new double[ySize];
            var zValue = new double[ySize];
            for (int i=0; i<allRows.FullFile.Count(); i++)
            {
                xSteppings[i] = Convert.ToDouble(allRows.FullFile[i].Value[width]);
                ySteppings[i] = Convert.ToDouble(allRows.FullFile[i].Value[height]);
                zValue[i] = Convert.ToDouble(allRows.FullFile[i].Value[index]);
            }

            var dataSeries = new NonUniformGridDataSeries3D<double>(xSize, ySize, xIndex => xSteppings[xIndex], yIndex => ySteppings[yIndex])
            {
                SeriesName= "3D wykres"             
            };
            for (int i = 0; i < allRows.FullFile.Count(); i++)
            {

                dataSeries[i, i] = zValue[i];
            }


            return dataSeries;
        }

        private IDataSeries3D FillSeries(int index, int width, int height)
        {
            double angle = Math.PI * 2 * index / 30;

            int w = width, h = height;

            var dataSeries = new UniformGridDataSeries3D<double>(w, h)
            {
                StepX = 0.01,
                StepZ = 0.11,
            };

            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    var v = (1 + Math.Sin(x * 0.04 + angle)) * 50 + (1 + Math.Sin(y * 0.1 + angle)) * 50 * (1 + Math.Sin(angle * 2));
                    var cx = w / 2;
                    var cy = h / 2;
                    var r = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    var zValue = v * exp;

                    dataSeries[y, x] = zValue;
                }
            }

            return dataSeries;
        }

        private void ContourColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  surfaceMeshRenderableSeries.ContourStroke = ((ColorModel)contourComboBox.SelectedItem).Color;
        }
    }

    public class ColorModel
    {
        public Color Color { get; set; }

        public Brush Brush
        {
            get { return new SolidColorBrush(Color); }
        }

        public string ColorName { get; set; }
    }
}
