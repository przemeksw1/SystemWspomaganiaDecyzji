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
            MetricCombo_Quality.Items.Add(MetricName.Euklides);
            MetricCombo_Quality.Items.Add(MetricName.Manhattan);
            MetricCombo_Quality.Items.Add(MetricName.Czebyszew);
            MetricCombo_Quality.Items.Add(MetricName.Mahalanobis);
            ButtonsEnabledChange(false);
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
                ButtonsEnabledChange(true);
            }                       
        }
        private async Task HeavyMethod(string filePath)
        {
            FileReadWrite fileReadWrite = new FileReadWrite();
            fileReadWrite.ReadFileFromPath(filePath, firstRowHaveHeader);
            AllRows allColumns = AllRows.GetInstance();
            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;

            for (int i = 0; i < allColumns.FullFile[0].Value.Count; i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i)); 
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
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DisplayNewDataInMenu(int columnNumber)
        {
            AllRows allColumns = AllRows.GetInstance();
            dataGrid.Columns.Clear();

            for (int i = 0; i < allColumns.FullFile[0].Value.Count; i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i));
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



            //AllRows allColumns = AllRows.GetInstance();
            //Binding binding = new Binding(String.Format("Value[{0}]", columnNumber)); 
            //DataGridTextColumn column = new DataGridTextColumn();

            ////binding.Converter = new PricesConverter();
            //binding.Mode = BindingMode.TwoWay;
            //binding.ValidatesOnDataErrors = true;
            //column.Binding = binding;
            //column.CanUserSort = false;
            //column.Header = allColumns.HeaderName[columnNumber];
            //dataGrid.Columns.Insert(columnNumber,column);
            //dataGrid.ItemsSource = null;
            //dataGrid.ItemsSource = AllRows.GetInstance().FullFile;
            //ComboBoxFill();
            //dataGrid.Items.Refresh();
        }

        private void ButtonsEnabledChange(bool type)
        {
            SaveFileButton.IsEnabled = type;
            ClasifyButton.IsEnabled = type;
            SaveButton_Numeric.IsEnabled = type;
            SaveButton_Normal.IsEnabled = type;
            SaveButton_Discret.IsEnabled = type;
            SaveButton_Intervals.IsEnabled = type;
            SaveButton_Percent.IsEnabled = type;
            SaveButton_2D.IsEnabled = type;
            SaveButton_3D.IsEnabled = type;
            SaveButton_Histogram.IsEnabled = type;
            SaveButton_Quality.IsEnabled = type;
        }

        private void ComboBoxFill()
        {
            ColumnCombo_Numeric.ItemsSource = new List<string>();
            ColumnCombo_Numeric.Text = "-- Kolumna --";
            ColumnCombo_Numeric.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Discret.ItemsSource = new List<string>();
            ColumnCombo_Discret.Text = "-- Kolumna --";
            ColumnCombo_Discret.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Normal.ItemsSource = new List<string>();
            ColumnCombo_Normal.Text = "-- Kolumna --";
            ColumnCombo_Normal.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Intervals.ItemsSource = new List<string>();
            ColumnCombo_Intervals.Text = "-- Kolumna --";
            ColumnCombo_Intervals.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Percent.ItemsSource = new List<string>();
            ColumnCombo_Percent.Text = "-- Kolumna --";
            ColumnCombo_Percent.ItemsSource = AllRows.GetInstance().HeaderName;
            Column1Combo_2D.ItemsSource = new List<string>();
            Column1Combo_2D.Text = "-- Kolumna --";
            Column1Combo_2D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column2Combo_2D.ItemsSource = new List<string>();
            Column2Combo_2D.Text = "-- Kolumna --";
            Column2Combo_2D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column1Combo_3D.ItemsSource = new List<string>();
            Column1Combo_3D.Text = "-- Kolumna --";
            Column1Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column2Combo_3D.ItemsSource = new List<string>();
            Column2Combo_3D.Text = "-- Kolumna --";
            Column2Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            Column3Combo_3D.ItemsSource = new List<string>();
            Column3Combo_3D.Text = "-- Kolumna --";
            Column3Combo_3D.ItemsSource = AllRows.GetInstance().HeaderName;
            ColumnCombo_Histogram.ItemsSource = new List<string>();
            ColumnCombo_Histogram.Text = "-- Kolumna --";
            ColumnCombo_Histogram.ItemsSource = AllRows.GetInstance().HeaderName;
            Column3Combo_2D.ItemsSource = new List<string>();
            Column3Combo_2D.Text = "-- K.Decyzyjna --";
            Column3Combo_2D.ItemsSource = AllRows.GetInstance().HeaderName;
            ClassifyColumnCombo_Quality.ItemsSource = new List<string>();
            ClassifyColumnCombo_Quality.Text = "-- K.Decyzyjna --";
            ClassifyColumnCombo_Quality.ItemsSource = AllRows.GetInstance().HeaderName;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.Visibility == Visibility.Visible)
                MenuGrid.Visibility = Visibility.Collapsed;
            else MenuGrid.Visibility = Visibility.Visible;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialogs dialogs = new SaveFileDialogs("Podaj nazwe pliku", "");
            //if (dialogs.ShowDialog() == true)
            //{

            //    string name = dialogs.Answer;
            //    if (name == "")
            //        name = "tmp.txt";
            //    else
            //        name = name + ".txt";
            //    FileReadWrite.WriteToFile(name, firstRowHaveHeader);
            //}

            string filePath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName.ToString();
                FileReadWrite.WriteToFile(filePath, firstRowHaveHeader);
            }
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
                            TextToNumeric.OrderTextToNumber(numOfColumn);
                        }
                        break;
                    case 1:
                        {
                            TextToNumeric.AlfabeticTextToNumber(numOfColumn);
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

        private void SaveButton_Normal_Click(object sender, RoutedEventArgs e)
        {
            if (ColumnCheck_Normal.IsChecked == false)
            {
                int numOfColumn = ColumnCombo_Normal.SelectedIndex;
                if (numOfColumn < 0) MessageBox.Show("Wybierz kolumnę");
                else
                {
                    Normalization.DoNormalization(numOfColumn);
                    //odświeżenie widoku - wyświetlenie zmian
                    DisplayNewDataInMenu(numOfColumn + 1);
                }
            }
            else
            {
                for (int i = 0; i < AllRows.GetInstance().HeaderName.Count-1; i+=2)
                {
                    Normalization.DoNormalization(i);
                    //odświeżenie widoku - wyświetlenie zmian
                    DisplayNewDataInMenu(i + 1);
                }
            }
        }
        private void ColumnCheck_Normal_Clicked(object sender, RoutedEventArgs e)
        {
            if (ColumnCheck_Normal.IsChecked == true) ColumnCombo_Normal.IsEnabled = false;
            else ColumnCombo_Normal.IsEnabled = true;
        }

        private void SaveButton_Intervals_Click(object sender, RoutedEventArgs e)
        {
            decimal min = Convert.ToDecimal(MinText_Intervals.Text);
            decimal max = Convert.ToDecimal(MaxText_Intervals.Text);
            int numOfColumn = ColumnCombo_Intervals.SelectedIndex;
            if (numOfColumn < 0) MessageBox.Show("Wybierz kolumnę");
            else
            {
                ScalingIntervals.DoScalingIntervals(numOfColumn, min, max);
                //odświeżenie widoku - wyświetlenie zmian
                DisplayNewDataInMenu(numOfColumn + 1);
            }
        }

        private void SaveButton_Percent_Click(object sender, RoutedEventArgs e)
        {
            double min = Convert.ToDouble(MinText_Percent.Text);
            double max = Convert.ToDouble(MaxText_Percent.Text);
            int numOfColumn = ColumnCombo_Percent.SelectedIndex;
            if (numOfColumn < 0) MessageBox.Show("Wybierz kolumnę");
            else
            {
                PercentWindow percentWindow = new PercentWindow(min, max, numOfColumn);
                percentWindow.Show();
            }
        }

        private void SaveButton_Histogram_Click(object sender, RoutedEventArgs e)
        {
            int numOfColumn = ColumnCombo_Histogram.SelectedIndex;
            int intervals = Convert.ToInt32(IntervalsText_Histogram.Text);
            HistogramWindow histogramWindow = new HistogramWindow(numOfColumn,intervals);
            histogramWindow.Show();
        }

        private void SaveButton_2D_Click(object sender, RoutedEventArgs e)
        {
            int axisX = Column1Combo_2D.SelectedIndex;
            int axisY = Column2Combo_2D.SelectedIndex;
            int decisionColumn = - 1; //jeżeli nie wybrano klasy decyzyjnej program domyślnie wyswietli wykres bez grupowania
            if (Column3Combo_2D.SelectedItem != null)
                decisionColumn = Column3Combo_2D.SelectedIndex;
            LineChartWindow lineChartWindow = new LineChartWindow(axisX, axisY, decisionColumn);
            lineChartWindow.Show();
        }

        private void ClasifyButton_Click(object sender, RoutedEventArgs e)
        {
            ClasifyNewWindow win = new ClasifyNewWindow();
            win.ShowDialog();
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = AllRows.GetInstance().FullFile;
        }

        private void SaveButton_3D_Click(object sender, RoutedEventArgs e)
        {
            int axisX = Column1Combo_3D.SelectedIndex;
            int axisY = Column2Combo_3D.SelectedIndex;
            int axisZ = Column3Combo_3D.SelectedIndex;
            Chart3DWindow win = new Chart3DWindow(axisX, axisY, axisZ);
            win.Show();
        }

        private void SaveButton_Quality_Click(object sender, RoutedEventArgs e)
        {
            bool IsError = false;
            int decisionColumn = AllRows.GetInstance().HeaderName.ToList().Count - 1; //jeżeli nie wybrano klasy decyzyjnej program domyślnie wybierze ostatnią kolumne
            MetricName metric = MetricName.Euklides;
            int k;
            decimal quality=0;
            
            if (ClassifyColumnCombo_Quality.SelectedItem != null)
                decisionColumn = ClassifyColumnCombo_Quality.SelectedIndex;
            if (MetricCombo_Quality.SelectedItem != null)
                metric = (MetricName)MetricCombo_Quality.SelectedItem;

            //wywołanie dla jednej okreslonej liczby sasiadow
            if (AutomaticCheckBox_Quality.IsChecked == false)
            {
                k = Convert.ToInt32(NeighboursText_Quality.Text);
                if (k > AllRows.GetInstance().FullFile.Count-1)
                {
                    MessageBox.Show("Liczba sasiadow nie moze byc wieksza od ilosci obiektow w zbiorze - 1");
                    IsError = true;
                }


                if (!IsError)
                {
                    Classification classificator = new Classification(k, metric, decisionColumn);
                    quality = classificator.GetClassificationQuality();
                    MessageBox.Show("Jakosc klasyfikatora k-nn dla metryki '" + metric + "' i k=" + k + " wynosi:\nQUALITY=" + quality);
                }
            }

            //wywolanie dla automatycznie zmiennej liczny sasiadow od 1 do n-1
            else
            {
                if (!IsError)
                {
                    Classification classificator = new Classification(0, metric, decisionColumn);
                    List<decimal> qualities = classificator.GetClassificationQualityForAllNeighbours();

                    MessageBoxResult result = MessageBox.Show("Jakosc klasyfikacji dla" + metric + "\nnajmniejsza: " + qualities.Min() + "\nnajwieksza: "
                        + qualities.Max() + "\n\nCzy chcesz zapisać wszystkie wyniki do pliku?","Jakosc klasyfikacji k-nn",MessageBoxButton.YesNo);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            string filePath;
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                filePath = saveFileDialog.FileName.ToString();
                                FileReadWrite.WriteToFileDecimalList(filePath, qualities);
                            }
                            break;
                    }
                }
            }
            
        }

        private void AutomaticCheckBox_Quality_Click(object sender, RoutedEventArgs e)
        {
            if (AutomaticCheckBox_Quality.IsChecked == true) NeighboursText_Quality.IsEnabled = false;
            else NeighboursText_Quality.IsEnabled = true;
        }

        
    }
}
