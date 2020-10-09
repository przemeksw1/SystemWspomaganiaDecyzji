using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SystemWspomaganiaDecyzji.Models;
using SystemWspomaganiaDecyzji.Services.Implementation;

namespace SystemWspomaganiaDecyzji
{
    /// <summary>
    /// Logika interakcji dla klasy PercentWindow.xaml
    /// </summary>
    public partial class PercentWindow : Window
    {
        private double MinInterval;
        private double MaxInterval;
        private int ColumnNumber;
        public PercentWindow(double min, double max, int column)
        {
            InitializeComponent();
            MinInterval = min;
            MaxInterval = max;
            ColumnNumber = column;
            HeavyMethod();
        }

        private void HeavyMethod()
        {
            AllRows allColumns = AllRows.GetInstance();
            List<List<RowView>> MinMaxRows = PercentMinMax.DoPercentMinMaxCut(MinInterval, MaxInterval, ColumnNumber);

            for (int i = 0; i < allColumns.FullFile[0].Value.Count; i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i));
                DataGridTextColumn column = new DataGridTextColumn();

                //binding.Converter = new PricesConverter();
                binding.Mode = BindingMode.OneWay;
                binding.ValidatesOnDataErrors = true;
                column.Binding = binding;
                column.CanUserSort = false;
                column.Header = allColumns.HeaderName[i];
                DataGrid_Min.Columns.Add(column);
            }
            DataGrid_Min.ItemsSource = MinMaxRows[0];
            for (int i = 0; i < allColumns.FullFile[0].Value.Count; i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i));
                DataGridTextColumn column = new DataGridTextColumn();

                //binding.Converter = new PricesConverter();
                binding.Mode = BindingMode.OneWay;
                binding.ValidatesOnDataErrors = true;
                column.Binding = binding;
                column.CanUserSort = false;
                column.Header = allColumns.HeaderName[i];
                DataGrid_Max.Columns.Add(column);
            }
            DataGrid_Max.ItemsSource = MinMaxRows[1];

        }
    }
}
