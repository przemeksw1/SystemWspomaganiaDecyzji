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
        //private DataGrid dataGrid;
        //private ProgressBar progressBar;

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

      
        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
           // AllColumns allColumns = AllColumns.GetInstance();
            //allColumns.FullFile[1].Value[1]  = "fdf";
          //  dataGrid.SelectedCells.
        }

        // To jest wybieranie i wczytywanie pliku.     
        private async void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName.ToString();

                // 
                // Dodać sprawdzenie czy posiada nagłówki już czy mamy ręcznie podać nagłówki i je dodać.
                //

                await HeavyMethod(filePath);
            }                       
        }
        private async Task HeavyMethod(string filePath)
        {
            FileReadWrite fileReadWrite = new FileReadWrite();
            fileReadWrite.ReadFileFromPath(filePath);
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
                column.Header = "dupa" + i;                
                dataGrid.Columns.Add(column);
            }
            dataGrid.ItemsSource = allColumns.FullFile;
            await Task.Delay(50);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.Visibility == Visibility.Visible)
                MenuGrid.Visibility = Visibility.Collapsed;
            else MenuGrid.Visibility = Visibility.Visible;
        }
    }
}
