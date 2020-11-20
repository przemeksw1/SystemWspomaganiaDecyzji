using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services.Implementation
{
    class FileReadWrite : IFileReadWrite
    {
        public void ReadFileFromPath(string path, bool firstRowHeader)
        {
            AllRows.ClearFullFile();
            AllRows allColumns = AllRows.GetInstance();

            Regex regex = new Regex(@"^-?[0-9]*.[0-9]+$");

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
                                if (regex.IsMatch(splitLine[i])) splitLine[i] = CovertDotToComma(splitLine[i]);
                                column.Value.Add(splitLine[i]);
                                allColumns.HeaderName.Add("Kolumna__" + (i+1));
                            }
                            allColumns.FullFile.Add(column);
                        }
                        else
                        {
                            RowView column = new RowView();
                            for (int i = 0; i < splitLine.Length; i++)
                            {
                                //List<string> column = new List<string>();
                                if (regex.IsMatch(splitLine[i])) splitLine[i] = CovertDotToComma(splitLine[i]);
                                column.Value.Add(splitLine[i]);

                            }
                            allColumns.FullFile.Add(column);
                        }
                }
               
            }          
        }
        // Jeszcze trzeba dokończyć albo inaczej zacząć na dobre
        public static void WriteToFile(string fileName, bool firstRowAsName)
        {
            AllRows allColumns = AllRows.GetInstance();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            string line = "";
           
            if (firstRowAsName)
            {
             
                foreach (string header in allColumns.HeaderName)
                {
                    if(line == "")
                    {
                        line = header;
                    }
                    else
                    {
                        line = line + ";" + header;
                    }
                    
                }
                sw.WriteLine(line);
                line = "";
            }
                foreach (RowView rowView in allColumns.FullFile)
                {
                    foreach(string str in rowView.Value)
                    {
                    if (line == "")
                    {
                        line = str;
                    }
                    else
                    {
                        line = line + ";" + str;
                    }
                }
                    sw.WriteLine(line);
                    line = "";
                }
            sw.Close();
        }

        //zapisywanie jednokolumnowej listy decimal i indexami
        public static void WriteToFileDecimalList(string fileName, List<decimal> list)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            string line = "";

            for (int k=0; k<list.Count; k++)
            {
                line = k+1 + ";" + list[k];
                sw.WriteLine(line);
                line = "";
            }
            sw.Close();
        }

        public static string CovertDotToComma(string word)
        {
            string result = "";
            for(int i=0; i<word.Length; i++)
            {
                if (word[i] == '.') result += ',';
                else result += word[i];
            }
            return result;
        }
    }
}
