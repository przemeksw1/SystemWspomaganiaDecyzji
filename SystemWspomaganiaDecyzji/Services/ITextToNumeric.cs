using System;
using System.Collections.Generic;
using System.Text;

namespace SystemWspomaganiaDecyzji.Services
{
    public interface ITextToNumeric
    {
        void AlfabeticTextToNumber(int collumn, bool firstRowHaveHeader);

        void OrderTextToNumber(int collumn, bool firstRowHaveHeader);
    }
}


//TextToNumeric textToNumeric = new TextToNumeric();
//textToNumeric.AlfabeticTextToNumber(4, firstRowHaveHeader);
//AllRows allColumns = AllRows.GetInstance();
//Binding binding = new Binding(String.Format("Value[{0}]", 5)); /// Prices[{0}].Price1 is path to Product -> Price -> Price ordered according to price names.
//DataGridTextColumn column = new DataGridTextColumn();

////binding.Converter = new PricesConverter();
//binding.Mode = BindingMode.TwoWay;
//binding.ValidatesOnDataErrors = true;
//column.Binding = binding;
//column.CanUserSort = false;
//column.Header = allColumns.HeaderName[5];
//dataGrid.Columns.Add(column);