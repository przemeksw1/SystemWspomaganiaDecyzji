using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
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
    /// Logika interakcji dla klasy ClasifyNewWindow.xaml
    /// </summary>
    public partial class ClasifyNewWindow : Window
    {
        private List<RowView> NewObjects = new List<RowView>();
        private int DecisionColumn;
        private MetricName Metric;
        private int k=1;
        public ClasifyNewWindow()
        {
            InitializeComponent();
            ComboBoxFill();
            HeavyMethod();
        }

        public void HeavyMethod()
        {
            AllRows allColumns = AllRows.GetInstance();
            RowView row = new RowView();
            for (int i = 0; i < allColumns.FullFile[0].Value.Count; i++)
            {
                Binding binding = new Binding(String.Format("Value[{0}]", i));
                DataGridTextColumn column = new DataGridTextColumn();
                binding.Mode = BindingMode.TwoWay;
                binding.ValidatesOnDataErrors = true;
                column.Binding = binding;
                column.CanUserSort = false;
                column.Header = allColumns.HeaderName[i];
                dataGrid.Columns.Add(column);
                row.Value.Add("");
            }
            NewObjects.Add(row);
            dataGrid.ItemsSource = NewObjects;
            
            //dataGrid.ItemsSource = allColumns.FullFile;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void ComboBoxFill()
        {
            ClassifyColumnCombo.ItemsSource = new List<string>();
            ClassifyColumnCombo.Text = "-- K.Decyzyjna --";
            ClassifyColumnCombo.ItemsSource = AllRows.GetInstance().HeaderName;
            MetricCombo.Text = "-- Metryka --";
            MetricCombo.Items.Add(MetricName.Euklides);
            MetricCombo.Items.Add(MetricName.Manhattan);
            MetricCombo.Items.Add(MetricName.Czebyszew);
            MetricCombo.Items.Add(MetricName.Mahalanobis);
        }


        private void ClassifyButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsError = false;

            //jeżeli nie wybrano klasy decyzyjnej program domyślnie wybierze ostatnią kolumne
            if (ClassifyColumnCombo.SelectedItem == null)
                DecisionColumn = AllRows.GetInstance().HeaderName.ToList().Count - 1;
            else DecisionColumn = ClassifyColumnCombo.SelectedIndex;

            if (MetricCombo.SelectedItem == null)
                Metric = MetricName.Euklides;
            else Metric = (MetricName)MetricCombo.SelectedItem;
            //MessageBox.Show(Metric.ToString());

            k = Convert.ToInt32(NeighboursText.Text);
            if (k > AllRows.GetInstance().FullFile.Count) 
            {
                MessageBox.Show("k nie moze byc wieksze od ilosci obiektow");
                IsError = true;

            }

            for(int i=0; i<NewObjects[0].Value.Count-1; i++)
            {
                if(NewObjects[0].Value[i]=="")
                {
                    MessageBox.Show("Uzupelnij wszystkie pola aby kontynuowac");
                    IsError = true;
                    break;
                }
            }

            if(!IsError)
            {
                Classification classificator = new Classification(k, Metric, DecisionColumn);
                string decision = classificator.Classify(NewObjects[0]);
                //MessageBox.Show(decision);
                NewObjects[0].Value[NewObjects[0].Value.Count - 1] = decision;
                dataGrid.ItemsSource = null;                
                dataGrid.ItemsSource = NewObjects;
  
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AllRows.GetInstance().FullFile.Add(NewObjects[0]);
            this.Close();
        }
    }
}
