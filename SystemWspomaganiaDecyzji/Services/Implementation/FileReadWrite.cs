using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    class FileReadWrite : IFileReadWrite
    {
        public void ReadFileFromPath(string path, bool firstRowHeader)
        {
            AllRows allColumns = AllRows.GetInstance();

            char[] delimiters = new[] { ' ', ';', '\t' };
            string line;
            string[] splitLine;
            bool firstRow = true;
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                splitLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if(splitLine.Length >0 )
                if (splitLine[0].FirstOrDefault() != '#')
                {
                        if (firstRowHeader && firstRow)
                        {
                            allColumns.HeaderName = new List<string>();
                            for (int i = 0; i < splitLine.Length; i++)
                            {
                                allColumns.HeaderName.Add(splitLine[i]);
                            }                            
                            firstRow = false;
                        }
                        else if(!firstRowHeader && firstRow)
                        {
                            RowView column = new RowView();
                            allColumns.HeaderName = new List<string>();
                            for (int i = 0; i < splitLine.Length; i++)
                            {                                
                                column.Value.Add(splitLine[i]);
                                allColumns.HeaderName.Add("Kolumna " + "1");
                            }
                            allColumns.FullFile.Add(column);
                        }
                        else
                        {
                            RowView column = new RowView();
                            for (int i = 0; i < splitLine.Length; i++)
                            {
                                //List<string> column = new List<string>();
                                column.Value.Add(splitLine[i]);

                            }
                            allColumns.FullFile.Add(column);
                        }
                }
               
            }



            // działa jakoś trzeba poprawić aby ominąć dodanie początkowego wiersza albo coś takiego
           
        }
    }
}
