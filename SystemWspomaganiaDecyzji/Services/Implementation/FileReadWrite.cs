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
        public void ReadFileFromPath(string path)
        {
            AllColumns allColumns = AllColumns.GetInstance();

            char[] delimiters = new[] { ' ', ';', '\t' };
            string line;
            string[] splitLine;

            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                splitLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if(splitLine.Length >0 )
                if (splitLine[0].FirstOrDefault() != '#')
                {
                        //if (allColumns.FullFile.Count() == 0)
                        //{
                        ColumnView column = new ColumnView();
                        for (int i = 0; i < splitLine.Length; i++)
                        {
                               
                            //List<string> column = new List<string>();
                            column.Value.Add(splitLine[i]);
                          
                        }
                        allColumns.FullFile.Add(column);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < allColumns.FullFile.Count(); i++)
                        //    {
                        //        allColumns.FullFile[i].Value.Add(splitLine[i]);
                        //    }
                        //}
                    }
               
            }

            // działa jakoś trzeba poprawić aby ominąć dodanie początkowego wiersza albo coś takiego
           
        }
    }
}
