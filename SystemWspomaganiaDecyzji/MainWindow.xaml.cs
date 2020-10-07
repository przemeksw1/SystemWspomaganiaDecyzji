using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemWspomaganiaDecyzji.Helper;
using SystemWspomaganiaDecyzji.Models;
using SystemWspomaganiaDecyzji.Services;
using SystemWspomaganiaDecyzji.Services.Implementation;

namespace SystemWspomaganiaDecyzji
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool firstRowHaveHeader = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }
     
        // Zaznaczanie całej kolumny
        private void columnHeaderClick(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as DataGridColumnHeader;
            if (columnHeader != null)
            {
                dataGrid.SelectedCells.Clear();
                foreach (var item in dataGrid.Items)
                {
                    dataGrid.SelectedCells.Add(new DataGridCellInfo(item, columnHeader.Column));
                }
            }
        }



        // To jest wybieranie i wczytywanie pliku.     
        private async void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName.ToString();
             
                var result = MessageBox.Show("Czy pierwsza linia zawiera nagłowki", "Pytanie", MessageBoxButton.YesNo);   

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        firstRowHaveHeader = true;
                        break;
                    case MessageBoxResult.No:
                        firstRowHaveHeader = false;
                        break;
                }                 

                await HeavyMethod(filePath);
            }                       
        }
        private async Task HeavyMethod(string filePath)
        {
            FileReadWrite fileReadWrite = new FileReadWrite();
            fileReadWrite.ReadFileFromPath(filePath, firstRowHaveHeader);
            AllRows allColumns = AllRows.GetInstance();
           
            for (int i = 0; i < allColumns.FullFile[0].Value.Count(); i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i)); /// Prices[{0}].Price1 is path to Product -> Price -> Price ordered according to price names.
                DataGridTextColumn column = new DataGridTextColumn();

                //binding.Converter = new PricesConverter();
                binding.Mode = BindingMode.TwoWay;
                binding.ValidatesOnDataErrors = true;
                column.Binding = binding;
                column.CanUserSort = false;
                column.Header = allColumns.HeaderName[i];                
                dataGrid.Columns.Add(column);
            }
            dataGrid.ItemsSource = allColumns.FullFile;
            ComboBoxFill();
            await Task.Delay(50);

        }

        private void DisplayNewDataInMenu(int columnNumber)
        {
            AllRows allColumns = AllRows.GetInstance();
            Binding binding = new Binding(String.Format("Value[{0}]", columnNumber)); /// Prices[{0}].Price1 is path to Product -> Price -> Price ordered according to price names.
            DataGridTextColumn column = new DataGridTextColumn();

            //binding.Converter = new PricesConverter();
            binding.Mode = BindingMode.TwoWay;
            binding.ValidatesOnDataErrors = true;
            column.Binding = binding;
            column.CanUserSort = false;
            column.Header = allColumns.HeaderName[columnNumber];
            dataGrid.Columns.Insert(columnNumber,column);            
            ComboBoxFill();
        }

        private void ComboBoxFill()
        {
            ColumnCombo_Numeric.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Discret.ItemsSource = new List<string>();
            ColumnCombo_Discret.Text = "-- Kolumna --";
            ColumnCombo_Discret.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Normal.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Intervals.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Percent.ItemsSource = AllRows.GetInstance().HeaderName;
            Column1Combo_2D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column2Combo_2D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column1Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column2Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column3Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Histogram.ItemsSource = AllRows.GetInstance().HeaderName;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.Visibility == Visibility.Visible)
                MenuGrid.Visibility = Visibility.Collapsed;
            else MenuGrid.Visibility = Visibility.Visible;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SaveButton_Discret_Click(object sender, RoutedEventArgs e)
        {
            int countOfIntervals = Convert.ToInt32(IntervalsText_Discret.Text);
            int numOfColumn = ColumnCombo_Discret.SelectedIndex;
            if (numOfColumn < 0) MessageBox.Show("Wybierz kolumnę");
            else
            {
                Discretization.DoDiscretization(numOfColumn, countOfIntervals);
                //odświeżenie widoku - wyświetlenie zmian
                DisplayNewDataInMenu(numOfColumn + 1);
            }

        }

        private void SaveButton_Numeric_Click(object sender, RoutedEventArgs e)
        {
           // int countOfIntervals = Convert.ToInt32(ColumnCombo_Numeric.Text);
            int numOfColumn = ColumnCombo_Numeric.SelectedIndex;
            int numTypeOfSort = TypeCombo_Numeric.SelectedIndex;
            if (numOfColumn < 0) MessageBox.Show("Wybierz kolumnę");
            else
            {
                switch(numTypeOfSort)
                {
                    case 0:
                        {
                            TextToNumeric.AlfabeticTextToNumber(numOfColumn);
                        }
                        break;
                    case 1:
                        {
                            TextToNumeric.OrderTextToNumber(numOfColumn);
                        }
                        break;
                    default:
                        {
                            MessageBox.Show("Coś poszło nie tak");
                        }
                        break;
                }              
                //odświeżenie widoku - wyświetlenie zmian
                DisplayNewDataInMenu(numOfColumn + 1);
            }
        }
    }
}
