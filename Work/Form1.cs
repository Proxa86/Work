using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Work
{
    public partial class Form1 : Form 
    {
        List<string> lCreatedFiles; // для хранения данных из файла create, созданного maven. Расширение файла (.class)
        List<string> lInputFiles; // для хранения данных из файла input, созданного maven. Расширение файла (.java)

        // поиск nop файлов
        List<string> lTmpNumberSrc; //Лист для хранение tmp значений из исходников
        List<string> lTmpPathFile; //Лист для хранение путей исходников

        int count = 0; // кол-во tmp номеров

        StatusBar sb;

        string saveNameFile = null;
        static string pathFolder = @"E:\Work\W_INS_KMO\Tirs(АДЛБ.181)_v3\srcCB_tirs_v3\EFSK.5005X.12\build\src";

        FolderBrowserDialog fbd;
        SaveFileDialog saveFile;

        bool flagStartSrcClassJar;
        bool flagStartSrcJar;

        public Form1()
        {
            InitializeComponent();

            sb = new StatusBar();
            sb.ShowPanels = true;
            this.Controls.Add(sb);

            textBoxFilter.Text = "c,cc,cpp,h,hh,hpp";
        }

        // считываем файл и сохраняем его в лист

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            lCreatedFiles = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"E:\Work\W_INS_KMO\Tirs(АДЛБ.181)_v3\srcCB_tirs_v3\EFSK.5005X.12\build\src";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                { // открытие файла
                    if((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        { // считываем выбранный файл
                            StreamReader readFile = new StreamReader(myStream);
                            string line = "";
                            int countLine = 0;
                            // считываем по строке файл
                            while((line = readFile.ReadLine()) != null)
                            {
                                // сохраняем в лист строки
                                lCreatedFiles.Add(line);
                                countLine++;
                            }
                            readFile.Close();
                            Console.WriteLine(countLine);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Origial error:" + ex.Message);
                }
            }
        }

        // считываем файл и сохраняем его в лист
        private void buttonOpenFileInput_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            lInputFiles = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E:\\Work\\W_INS_KMO\\objsrc_INS_KMO\\vbma_644\\extlibs";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader readFile = new StreamReader(myStream);
                            string line = "";
                            int countLine = 0;
                            while ((line = readFile.ReadLine()) != null)
                            {
                                lInputFiles.Add(line);
                                countLine++;
                            }
                            readFile.Close();
                            Console.WriteLine(countLine);
                        }
                    }
                    // сравниваем два файла
                    compareFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Origial error:" + ex.Message);
                }
            }
        }

        // Функция для сравнения двух файлов 
        private void compareFile()
        {
            if((lCreatedFiles.Count != 0) & (lInputFiles.Count != 0) & (lCreatedFiles.Count >= lInputFiles.Count))
            {
                int count = 0;
                string paternForInputFiles = "\\/\\w+\\.{1}java";
                string paternForCreateFiles = "\\/\\w+\\.{1}class";
                List<string> lIFiles = new List<string>();
                List<string> lCFiles = new List<string>();
                Regex regex1 = new Regex(paternForInputFiles);
                Regex regex2 = new Regex(paternForCreateFiles);
                foreach (var v in lInputFiles)
                {
                    Match match = regex1.Match(v);
                    if(match.Success)
                    {
                        lIFiles.Add(match.Value);
                    }
                }
                foreach (var v in lCreatedFiles)
                {
                    Match match = regex2.Match(v);
                    if (match.Success)
                    {
                        lCFiles.Add(match.Value);
                    }
                }

                foreach(var i in lIFiles)
                {
                    string paternFiles = "\\w+";
                    Regex regex = new Regex(paternFiles);

                    Match match1 = regex.Match(i);
                    
                    foreach (var c in lCFiles)
                    {
                        Match match2 = regex.Match(c);
                        if (match1.Value.Equals(match2.Value))
                            count++;
                    }
                }
                MessageBox.Show("Количество файлов .java: "+lIFiles.Count+"\nКоличество файлов .class: "+lCFiles.Count+
                    "\nКоличество совпадений: "+ count);
            }
        }

        // =====================================================
        private List<string> searchFileJava(string [] getFilesJava, ref int countRepeatNameFileJava)
        {

            List<string> lFilesJava = new List<string>();
            string paternFilesJava = @"[\w-]+\.{1}java";
            Regex regexJava = new Regex(paternFilesJava);

            int index = 1;

            foreach (var v in getFilesJava)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / getFilesJava.Length);
                //, string.Format("Java: {0}", processing)
                // применяем шаблон
                Match match = regexJava.Match(v);
                if (match.Success)
                {
                    // если list пустой
                    if (lFilesJava.Count == 0)
                    {
                        lFilesJava.Add(match.Value);
                    }
                    else
                    {
                        // проверяем присутствует ли в list такое же название, если нет то добавляем
                        if (!lFilesJava.Exists(x => x.Equals(match.Value)))
                        {
                            lFilesJava.Add(match.Value);
                        }
                        else
                        {
                            ++countRepeatNameFileJava;
                            //Console.WriteLine("Имена совпадающих файлов java: " + match.Value);
                        }
                    }
                }
            }
            return lFilesJava;
        }

        private List<string> searchFileScala(string[] getFilesScala, ref int countRepeatNameFileScala)
        {

            List<string> lFilesScala = new List<string>();
            string paternFilesScala = @"[\w-]+\.{1}scala";
            Regex regexJava = new Regex(paternFilesScala);

            int index = 1;

            foreach (var v in getFilesScala)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / getFilesScala.Length);
                //, string.Format("Java: {0}", processing)
                // применяем шаблон
                Match match = regexJava.Match(v);
                if (match.Success)
                {
                    // если list пустой
                    if (lFilesScala.Count == 0)
                    {
                        lFilesScala.Add(match.Value);
                    }
                    else
                    {
                        // проверяем присутствует ли в list такое же название, если нет то добавляем
                        if (!lFilesScala.Exists(x => x.Equals(match.Value)))
                        {
                            lFilesScala.Add(match.Value);
                        }
                        else
                        {
                            ++countRepeatNameFileScala;
                            //Console.WriteLine("Имена совпадающих файлов java: " + match.Value);
                        }
                    }
                }
            }
            return lFilesScala;
        }

        private List<string> searchFileClass(string[] getFilesClass, ref int countRepeatNameFileClass, ref int countInternalFileClass)
        {
            List<string> lFilesClassFromStr = new List<string>();
            string paternFilesClass = @"[\w$-]+\.{1}class";
            Regex regexClass = new Regex(paternFilesClass);
            int index = 1;

            foreach (var v in getFilesClass)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / getFilesClass.Length);

                Match match = regexClass.Match(v);

                if (match.Success)
                {
                    // если класс не анонимный
                    if ((v.IndexOf("$") == -1))
                    {
                        if (lFilesClassFromStr.Count == 0)
                        {
                            lFilesClassFromStr.Add(match.Value);
                        }
                        else
                        {
                            // если нет такого имени файла в листе
                            if (!lFilesClassFromStr.Exists(x => x.Equals(match.Value)))
                            {
                                lFilesClassFromStr.Add(match.Value);
                            }
                            else
                            {
                                ++countRepeatNameFileClass; // подсчет повторяющихся имен файлов
                            }
                        }
                    }
                    else ++countInternalFileClass; // подстчет внутренних классов
                }
            }
            return lFilesClassFromStr;
        }

        private void compareFilesJavaAndClass(List<string> lFilesJava, List<string> lFilesClassFromStr, ref List<string> lFindFreeFileJava, ref List<string> lFindFreeFileClass, 
            ref int count)
        {
            int index = 1;
            lFilesJava.Sort();
            lFilesClassFromStr.Sort();
            foreach (var i in lFilesJava)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / lFilesJava.Count);

                string paternFiles = "\\w+"; // шаблон для отбора только имени без расширения
                Regex regex = new Regex(paternFiles);
                Match match1 = regex.Match(i);
                bool findComparer = false; // флаг совпадения


                foreach (var c in lFilesClassFromStr)
                {
                    Match match2 = regex.Match(c);
                    if (match1.Value.Equals(match2.Value)) // если имена совпадают
                    {
                        ++count; // подсчитываем кол-во совпадений
                        findComparer = true;
                    }
                }
                if (!findComparer) //если не совпали
                {
                    lFindFreeFileJava.Add(i); // сохраняем файлы java у которых нет файлов class
                }
            }

            index = 1;
            foreach (var i in lFilesClassFromStr)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / lFilesClassFromStr.Count);

                string paternFiles = "\\w+";
                Regex regex = new Regex(paternFiles);
                Match match1 = regex.Match(i);
                bool findComparer = false;

                foreach (var c in lFilesJava)
                {
                    Match match2 = regex.Match(c);
                    if (match1.Value.Equals(match2.Value))
                    {
                        //++count;
                        findComparer = true;
                    }
                }
                if (!findComparer)
                {
                    lFindFreeFileClass.Add(i); // сохраняем файлы class у которых не нашлось файлов java
                }
            }

            //if (lFilesJava.Count >= lFilesClassFromStr.Count)
            //{

            //    foreach (var i in lFilesJava)
            //    {
            //        //Thread.Sleep(5);
            //        backgroundWorker.ReportProgress(index++ * 100 / lFilesJava.Count);

            //        string paternFiles = "\\w+"; // шаблон для отбора только имени без расширения
            //        Regex regex = new Regex(paternFiles);
            //        Match match1 = regex.Match(i);
            //        bool findComparer = false; // флаг совпадения

            //        foreach (var c in lFilesClassFromStr)
            //        {
            //            Match match2 = regex.Match(c);
            //            if (match1.Value.Equals(match2.Value)) // если имена совпадают
            //            {
            //                ++count; // подсчитываем кол-во совпадений
            //                findComparer = true;
            //            }
            //        }
            //        if (!findComparer) //если не совпали
            //        {
            //            lFindFreeFileJava.Add(i); // сохраняем файлы java у которых нет файлов class
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var i in lFilesClassFromStr)
            //    {
            //        //Thread.Sleep(5);
            //        backgroundWorker.ReportProgress(index++ * 100 / lFilesClassFromStr.Count);

            //        string paternFiles = "\\w+";
            //        Regex regex = new Regex(paternFiles);
            //        Match match1 = regex.Match(i);
            //        bool findComparer = false;

            //        foreach (var c in lFilesJava)
            //        {
            //            Match match2 = regex.Match(c);
            //            if (match1.Value.Equals(match2.Value))
            //            {
            //                ++count;
            //                findComparer = true;
            //            }
            //        }
            //        if (!findComparer)
            //        {
            //            lFindFreeFileClass.Add(i); // сохраняем файлы class у которых не нашлось файлов java
            //        }
            //    }
            //}
        }

        private void writeTemplateFormFilesResults(SaveFileDialog saveFile, FolderBrowserDialog fbd, params int[] count)
        {
            string s = String.Format(@"
//////////////////////////////////////////////////////////////////////////
//  Анализ файлов java и class в выбранной дирректории                  //
//////////////////////////////////////////////////////////////////////////
// Общее количество файлов .java: ..................................{0} //
// Общее количество файлов .scala ..................................{1} //
// Количество повторяемых имен .java: ..............................{2} //
// Количество повторяемых имен .scala: .............................{3} //
// Количество оригинальных файлов .java/.scala: ....................{4} //
//======================================================================//
// Общее количество файлов .class: .................................{5} //
// Количество повторяемых имен .class: .............................{6} //
// Количество анонимных классов: ...................................{7} //
// Количество оригинальных файлов .class: ..........................{8} //
//======================================================================//
// Количество совпавших файлов .java/scala/class по именам: ........{9} //
// Количество файлов .java/scala без файлов .class ................{10} // 
// Количество файлов .class не совпавших по именам с .java/scala ..{11} //
//======================================================================//
// Всего jar архивов в папке .......................................{12}//
//////////////////////////////////////////////////////////////////////////                                           

", count[0], count[1], count[2], count[3], count[4],count[5], count[6], count[7], count[8], count[9],count[10], count[11], count[12]);

            File.WriteAllText(saveFile.FileName, fbd.SelectedPath + "\n" + s, Encoding.UTF8);
        }

        private void writeFilesNoHavePars(SaveFileDialog saveFile, FolderBrowserDialog fbd, string[] getFilesJava, string[] getFilesScala, List<string> lFilesJava, 
            List<string> lFilesClassFromStr, List<string> lFindFreeFileJava, List<string> lFindFreeFileClass)
        {
            int index = 1;
            string[] filesJavaAndScala = new string[getFilesJava.Length + getFilesScala.Length];
            //Dictionary<string, string> dFilesClassNoFindInJava = new Dictionary<string, string>();
            try
            {
                
                Array.Copy(getFilesJava, 0, filesJavaAndScala, 0, getFilesJava.Length);
                Array.Copy(getFilesScala, 0, filesJavaAndScala, getFilesJava.Length, getFilesScala.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (lFindFreeFileJava.Count != 0 && lFindFreeFileClass.Count != 0)
            {
                File.AppendAllText(saveFile.FileName, "\nДанные файлы java/scala не совпали по имени ни с одним файлом class:\n", Encoding.UTF8);
                int countNoPars = 0;

                foreach (var path in getFilesJava)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / getFilesJava.Length);

                    foreach (var cl in lFindFreeFileJava)
                    {
                        string str = "\\" + cl;
                        if (path.Contains(str))
                        {
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                            ++countNoPars;
                        }
                    }
                }
                index = 1;
                foreach (var path in getFilesScala)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / getFilesScala.Length);

                    foreach (var cl in lFindFreeFileJava)
                    {
                        string str = "\\" + cl;
                        if (path.Contains(str))
                        {
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                            ++countNoPars;
                        }
                    }
                }

                if(getFilesJava.Length != 0 & getFilesScala.Length != 0)
                {
                    File.AppendAllText(saveFile.FileName, "_____ВСЕГО ФАЙЛОВ: "+ countNoPars +"\n", Encoding.UTF8);
                }

                index = 1;
                bool flag = true;
                List<string> copyFindFreeFileClass = lFindFreeFileClass.GetRange(0, lFindFreeFileClass.Count);

                int countFindParsInFile = 0;
                foreach (var path in filesJavaAndScala)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / filesJavaAndScala.Length);

                    string text = File.ReadAllText(path);
                    foreach (var cl in lFindFreeFileClass)
                    {
                        string paternFiles = "\\w+";
                        Regex regex = new Regex(paternFiles);
                        Match match = regex.Match(cl);
                        string str = "class " + match.Value + " "; //!!!
                        if (text.Contains(str))
                        {
                            if (flag)
                            {
                                File.AppendAllText(saveFile.FileName, "\nДанные файлы class не совпали по имени ни с одним файлом java.\n" +
                     "Но они были найденны внутри следующих файлов java:\n", Encoding.UTF8);
                            }
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                            flag = false;
                            copyFindFreeFileClass.Remove(cl);
                            ++countFindParsInFile;
                        }
                    }
                }

                if(copyFindFreeFileClass.Count != 0)
                    File.AppendAllText(saveFile.FileName, "_____ВСЕГО ФАЙЛОВ: "+ countFindParsInFile + "\n", Encoding.UTF8);

                if (copyFindFreeFileClass.Count != 0)
                {
                    File.AppendAllText(saveFile.FileName, "\nДанные файлы class не совпали по имени ни с одним файлом java.\n" +
                     "И не были найденны внутри файлов java.\n", Encoding.UTF8);
                    foreach (var item in copyFindFreeFileClass)
                    {
                        File.AppendAllText(saveFile.FileName, item + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "_____ВСЕГО ФАЙЛОВ: "+copyFindFreeFileClass.Count + "\n", Encoding.UTF8);
                }

            }
            else if (lFilesJava.Count > lFilesClassFromStr.Count)
            {
                File.AppendAllText(saveFile.FileName, "\nДанные файлы java/scala не совпали по имени ни с одним файлом class:\n", Encoding.UTF8);

                foreach (var path in getFilesJava)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / getFilesJava.Length);

                    foreach (var cl in lFindFreeFileJava)
                    {
                        string str = "\\" + cl;
                        if (path.Contains(str))
                        {
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                        }
                    }
                }
                index = 1;
                foreach (var path in getFilesScala)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / getFilesScala.Length);

                    foreach (var cl in lFindFreeFileJava)
                    {
                        string str = "\\" + cl;
                        if (path.Contains(str))
                        {
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                        }
                    }
                }
            }
            else if(lFilesJava.Count < lFilesClassFromStr.Count)
            {
                
                bool flag = true;
                List<string> copyFindFreeFileClass = lFindFreeFileClass.GetRange(0, lFindFreeFileClass.Count);

                foreach (var path in filesJavaAndScala)
                {
                    Thread.Sleep(5);
                    backgroundWorker.ReportProgress(index++ * 100 / filesJavaAndScala.Length);

                    string text = File.ReadAllText(path);
                    foreach (var cl in lFindFreeFileClass)
                    {
                        string paternFiles = "\\w+";
                        Regex regex = new Regex(paternFiles);
                        Match match = regex.Match(cl);
                        string str = "class " + match.Value + " "; //!!!

                        if (text.Contains(str))
                        {
                            if (flag)
                            {
                                File.AppendAllText(saveFile.FileName, "\nДанные файлы class не совпали по имени ни с одним файлом java/scala.\n" +
                     "Но они были найденны внутри следующих файлов java/scala:\n", Encoding.UTF8);
                            }
                            File.AppendAllText(saveFile.FileName, cl + "\n" + path + "\n", Encoding.UTF8);
                            flag = false;
                            copyFindFreeFileClass.Remove(cl);
                        }
                    }
                }
                if(copyFindFreeFileClass.Count != 0)
                {
                    File.AppendAllText(saveFile.FileName, "\nДанные файлы class не совпали по имени ни с одним файлом java/scala.\n" +
                     "И не были найденны внутри файлов java/scala.\n", Encoding.UTF8);
                    foreach (var item in copyFindFreeFileClass)
                    {
                        File.AppendAllText(saveFile.FileName, item + "\n", Encoding.UTF8);
                    }
                }
                
            }
        }

        private void searchFileClassFromReadJar(string[] allLines, ref List<string> lFileClassFromBin, 
            ref List<string> lAllClassFromBin, ref int countInnerClass, ref int countRepeadClass)
        {
            int n = 1;
            int index = 1;
            Dictionary<string, int> dRepeatFileClass = new Dictionary<string, int>();

            foreach (var v in allLines)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / allLines.Length);

                string paternTmpNumber = @"[\w$-]+\.{1}class";
                Match match = Regex.Match(v, paternTmpNumber);

                while (match.Success)
                {
                    lAllClassFromBin.Add(match.Value);

                    if ((match.Value.IndexOf("$") == -1))
                    {
                        if (lFileClassFromBin.Count == 0)
                        {
                            lFileClassFromBin.Add(match.Value);
                        }
                        else if (!lFileClassFromBin.Exists(x => x.Equals(match.Value)))
                        {
                            lFileClassFromBin.Add(match.Value);
                        }
                        else
                        {
                            if (dRepeatFileClass.Count == 0)
                            {
                                dRepeatFileClass.Add(match.Value, n);
                            }
                            else if (!dRepeatFileClass.ContainsKey(match.Value))
                            {
                                dRepeatFileClass.Add(match.Value, n);
                            }
                            else dRepeatFileClass[match.Value]++;
                        }

                    }
                    else ++countInnerClass;

                    match = match.NextMatch();
                }
            }
  
            foreach (var v in dRepeatFileClass)
            {
                index = 1;
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / allLines.Length);
                countRepeadClass += v.Value;
            }
        }

        private void compareFilesClassFromStrAndFromBin(List<string> lFilesClassFromStr, ref List <string> copyFilesClassFromBin,
            ref List<string> copyFilesClassFromStc, ref int countIn, ref int countOut)
        {
            int index = 1;

            foreach (var cl in lFilesClassFromStr)
            {
                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / lFilesClassFromStr.Count);

                if (copyFilesClassFromBin.Exists(x => x.Equals(cl)))
                {
                    ++countIn;
                    //File.AppendAllText(saveFile.FileName, cl + "\tOK\n", Encoding.UTF8);
                    copyFilesClassFromBin.Remove(cl);
                    copyFilesClassFromStc.Remove(cl);
                }
                else
                {
                    ++countOut;
                    //File.AppendAllText(saveFile.FileName, cl + "\tNO\n", Encoding.UTF8);
                    //copyFilesClassFromBin.Remove(cl);
                    //copyFilesClassFromStc.Remove(cl);
                }
            }
        }

        private string writeTemplateFormJarResults(params int[] count)
        {
            return String.Format(@"
////////////////////////////////////////////////////////////////
//      Анализ файлов jar на содержание файлов class          //
////////////////////////////////////////////////////////////////
// Общее количество файлов .class в архиве .jar: ...........{0} //
// Количество повторяемых имен .class в архиве .jar: .......{1} //
// Количество анонимных классов в .jar: ...................{2} //
// Количество оригинальных файлов .class в .jar: ...........{3} //
////////////////////////////////////////////////////////////////                                           

", count[0], count[1], count[2], count[3]);
        }

        private void writeResultsJar(SaveFileDialog saveFile, string pathJar, List <string> lFilesClassFromStr, 
            List<string> copyFilesClassFromStc, ref List<string> lInBinNoFileClass, ref List<string> lNoMatches, ref int countIn, int index)
        {
            int countRepeadClass = 0;
            int countInnerClass = 0;
            List<string> lFileClassFromBin = new List<string>();
            List<string> lAllClassFromBin = new List<string>();

            string[] allLines = File.ReadAllLines(pathJar);

            searchFileClassFromReadJar(allLines, ref lFileClassFromBin, ref lAllClassFromBin, ref countInnerClass, ref countRepeadClass);

            if (lFileClassFromBin.Count == 0)
            {
                lInBinNoFileClass.Add(pathJar);
            }
            else
            {
                File.AppendAllText(saveFile.FileName, "\n"+index +"________________________________________________________________________\n" +
                pathJar + "\n", Encoding.UTF8);
            }
            string j = writeTemplateFormJarResults(lAllClassFromBin.Count, countRepeadClass, countInnerClass, lFileClassFromBin.Count);

            // Копируем List
            List<string> copyFilesClassFromBin = lFileClassFromBin.GetRange(0, lFileClassFromBin.Count);
            copyFilesClassFromBin.Sort();

            int countOut = 0;
            countIn = 0;

            compareFilesClassFromStrAndFromBin(lFilesClassFromStr, ref copyFilesClassFromBin, ref copyFilesClassFromStc, ref countIn, ref countOut);

            if (countIn == 0 && lFileClassFromBin.Count != 0)
            {
                lNoMatches.Add(pathJar);
                File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло " + countIn + " файлов .class\n" + "В jar архив НЕ вошло " + countOut + " файлов .class\n", Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromStc.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
                if (lFileClassFromBin.Count != countIn)
                {
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
                    foreach (var unknownClass in copyFilesClassFromBin)
                    {
                        //File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
                }
            }
            else if(countIn == 0 && lFileClassFromBin.Count == 0)
            {
                lNoMatches.Add(pathJar);
            }
            else
            {
                File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло " + countIn + " файлов .class\n" + "В jar архив НЕ вошло " + countOut + " файлов .class\n", Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromStc.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
                if (lFileClassFromBin.Count != countIn)
                {
                    File.AppendAllText(saveFile.FileName, "В jar архиве находятся неизвестные классы:\n", Encoding.UTF8);

                    foreach (var unknownClass in copyFilesClassFromBin)
                    {
                        //File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(saveFile.FileName, "Файлы .class полученные из str которые не найдены в bin:\n", Encoding.UTF8);
                    foreach (var unknownClass in copyFilesClassFromStc)
                    {
                        File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов полученных из str и не найденных в jar: " + copyFilesClassFromStc.Count + "\n", Encoding.UTF8);
                }

            }
        }

        //================================================================
        // Кнопка для поиска и сравнения java и class файлов
        //================================================================
        private void buttonFindFiles_Click(object sender, EventArgs e)
        {
            flagStartSrcClassJar = true;
            buttonOpenReport.Enabled = false;
            fbd = new FolderBrowserDialog();
            fbd.SelectedPath = pathFolder;
            fbd.ShowNewFolderButton = false;

            // Для сохранения  файла срезультатами
            saveFile = new SaveFileDialog();
            saveFile.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pathFolder = fbd.SelectedPath;

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        saveNameFile = saveFile.FileName;

                        if (!backgroundWorker.IsBusy)
                        {
                            backgroundWorker.RunWorkerAsync();
                        }
                    }
                    else buttonOpenReport.Enabled = true;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
            }
            else
            {
                buttonOpenReport.Enabled = true;
                return;
            }
        }

        private void workWithFilesJavaAndClass(FolderBrowserDialog fbd, SaveFileDialog saveFile)
        {
                // получаем файлы по расширениям
                string [] getFilesJava = Directory.GetFiles(fbd.SelectedPath, "*.java", SearchOption.AllDirectories);
                string [] getFilesScala = Directory.GetFiles(fbd.SelectedPath, "*.scala", SearchOption.AllDirectories);
                string [] getFilesClass = Directory.GetFiles(fbd.SelectedPath, "*.class", SearchOption.AllDirectories);
                string [] getJar = Directory.GetFiles(fbd.SelectedPath, "*.jar", SearchOption.AllDirectories);

                int countRepeatNameFileJava = 0; // подсчет количества повторяющихся имен java
                int countRepeatNameFileScala = 0;

                List<string> lFilesJava = new List<string>();
                List<string> lFilesScala = new List<string>();

                lFilesJava = searchFileJava(getFilesJava, ref countRepeatNameFileJava);
                lFilesScala = searchFileScala(getFilesScala, ref countRepeatNameFileScala);

                lFilesJava.AddRange(lFilesScala.GetRange(0, lFilesScala.Count));

                int countInternalFileClass = 0; // подсчет внутренних (анонимных) классов 
                int countRepeatNameFileClass = 0; // подстчет повторяющихся имен файлов class

                List<string> lFilesClassFromStr = new List<string>();

                lFilesClassFromStr = searchFileClass(getFilesClass, ref countRepeatNameFileClass, ref countInternalFileClass);

                // lists для хранения не нашедших пар файлов java и class
                List<string> lFindFreeFileJava = new List<string>();
                List<string> lFindFreeFileClass = new List<string>();

                compareFilesJavaAndClass(lFilesJava, lFilesClassFromStr, ref lFindFreeFileJava,
                    ref lFindFreeFileClass, ref count);


            writeTemplateFormFilesResults(saveFile, fbd, getFilesJava.Length, getFilesScala.Length, countRepeatNameFileJava, countRepeatNameFileScala,
                    lFilesJava.Count, getFilesClass.Length, countRepeatNameFileClass, countInternalFileClass,
                    lFilesClassFromStr.Count, count, lFindFreeFileJava.Count, lFindFreeFileClass.Count,
                    getJar.Length);

                count = 0;

                writeFilesNoHavePars(saveFile, fbd, getFilesJava, getFilesScala, lFilesJava, lFilesClassFromStr,
                    lFindFreeFileJava, lFindFreeFileClass);

                // Копируем List
                List<string> copyFilesClassFromStc = lFilesClassFromStr.GetRange(0, lFilesClassFromStr.Count);
                List<string> lNoMatches = new List<string>();
                List<string> lInBinNoFileClass = new List<string>();
                int countIn = 0;
                int index = 0;

                foreach (var pathJar in getJar)
                {
                    Thread.Sleep(3);
                    backgroundWorker.ReportProgress(index++ * 100 / getJar.Length);
                              
                    writeResultsJar(saveFile, pathJar, lFilesClassFromStr, copyFilesClassFromStc, ref lInBinNoFileClass, ref lNoMatches, ref countIn, index);
                }

                if (lInBinNoFileClass.Count != 0)
                {
                    index = 1; 
                    File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не содержащих файлов .class\n", Encoding.UTF8);
                    foreach (var noFileClass in lInBinNoFileClass)
                    {
                        Thread.Sleep(3);
                        backgroundWorker.ReportProgress(index++ * 100 / lInBinNoFileClass.Count);
                        File.AppendAllText(saveFile.FileName, noFileClass + "\n", Encoding.UTF8);
                    }
                }

                if (lNoMatches.Count != 0)
                {
                    index = 1;
                    File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не имеющих совпанений с файлами .class полученных от src:\n", Encoding.UTF8);
                    foreach (var noMatches in lNoMatches)
                    {
                        Thread.Sleep(3);
                        backgroundWorker.ReportProgress(index++ * 100 / lNoMatches.Count);
                        File.AppendAllText(saveFile.FileName, noMatches + "\n", Encoding.UTF8);
                    }
                }
                backgroundWorker.CancelAsync();

        }


        //================================================================
        // Кнопки для выбора tmp номеров из исходников и бинарников 
        // и их сравнения
        //================================================================
        private void buttonOpenFileSrc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"E:\Work\W_Kanva\ARM_NBR(215)\nop_ARM_NBR\Lab";
            fbd.ShowNewFolderButton = false;


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // list хранения шаблонов языка C/C++
                    List<string[]> paternFilters = new List<string[]>();

                    // list для хранения tmp.. номеров
                    lTmpNumberSrc = new List<string>();
                    // list для хранения путей файлов c tmp..
                    lTmpPathFile = new List<string>();

                    // выбор файлов с расширениями
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.c", SearchOption.AllDirectories));
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cpp", SearchOption.AllDirectories));
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cc", SearchOption.AllDirectories));
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.h", SearchOption.AllDirectories));
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hh", SearchOption.AllDirectories));
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hpp", SearchOption.AllDirectories));

                    count = 0; // кол-во tmp номеров
                    try
                    {
                        foreach (var filter in paternFilters)
                        {
                            foreach (var path in filter)
                            {
                                string[] allLines = File.ReadAllLines(path);
                                foreach (var v in allLines)
                                {
                                    // шаблон для выбора номера tmp00000000
                                    string paternTmpNumber = "tmp[0-9]{8}";

                                    Regex regex = new Regex(paternTmpNumber);
                                    Match match = regex.Match(v);
                                    if (match.Success)
                                    {
                                        lTmpNumberSrc.Add(match.Value);
                                        ++count;
                                        lTmpPathFile.Add(path);
                                    }

                                }
                            }
                        }
                        Console.WriteLine("!!! src " + count);
                        lbPersent.Text = "Сканирование Src завершено!";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Origial error:" + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }

            }
            else return;
        }

        // кнопка для выбора tmp номеров из бинарников
        private void buttonOpenFileBin_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"E:\\Work\\W_Kanva\\ARM_NBR(215)\\nop_ARM_NBR";
            fbd.ShowNewFolderButton = false;
            // словарь для хранения найденного tmp номера и его пути
            Dictionary<string, string> dFoundTmpNumber = new Dictionary<string, string>();
            // словарь для хранения не найденного tmp номера и его пути
            Dictionary<string, string> dNotFoundTmpSrcInBin = new Dictionary<string, string>();
            // словарь для хранения найденного tmp номера и его количества раз встреченного в bin файле
            Dictionary<string, int> dRepeadCountTmpNumber = new Dictionary<string, int>();
            List<string[]> paternFilters = new List<string[]>();
            // list для хранения tmp номеров из бинарника
            List<string> lTmpNumberBin = new List<string>();
            List<string> lNoFoundTmpInSrc = new List<string>();
            int countTmpInBin = 0;
            // Словарь, содержит путь и список tmp номеров и перечень файлов
            Dictionary<string, List<string>> dPathBinAndNumberTmp = new Dictionary<string, List<string>>();
            
             
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    // list массивов для работы с бинарниками
                    
                    paternFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories));
                    int n = 1;
                    try
                    {
                        
                        foreach (var filter in paternFilters)
                        {
                            foreach (var path in filter)
                            {
                                string[] allLines = File.ReadAllLines(path);
                                List<string> lTmpNumberInOneBin = new List<string>();


                                foreach (var v in allLines)
                                {
                                    string paternTmpNumber = "tmp[0-9]{8}";
                                    Match match = Regex.Match(v, paternTmpNumber);
                                    
                                    while (match.Success)
                                    {
                                        ++countTmpInBin;
                                        if (lTmpNumberBin.Count == 0)
                                        {
                                            lTmpNumberBin.Add(match.Value); // Сохраняет все tmp_bin со всех bin
                                        }
                                        else if (!lTmpNumberBin.Exists(x => x.Equals(match.Value)))
                                        {
                                            lTmpNumberBin.Add(match.Value);
                                        }
                                        else
                                        {
                                            if (dRepeadCountTmpNumber.Count == 0) // если tmp_bin  повторяются
                                            {
                                                dRepeadCountTmpNumber.Add(match.Value, n);
                                            }
                                            else if(!dRepeadCountTmpNumber.ContainsKey(match.Value))
                                            {
                                                dRepeadCountTmpNumber.Add(match.Value, n);
                                            }
                                            else dRepeadCountTmpNumber[match.Value]++;
                                        }
                                        // Заносим в список bin_tmp по каждому бинарнику. Повторы bin_tmp удалены.
                                        if (lTmpNumberInOneBin.Count == 0)
                                        {
                                            lTmpNumberInOneBin.Add(match.Value);
                                        }
                                        else if (!lTmpNumberInOneBin.Exists(x => x.Equals(match.Value)))
                                        {
                                            lTmpNumberInOneBin.Add(match.Value);
                                        }
                                        match = match.NextMatch(); // ищем следующее совпадение в текущей строке 
                                    }
                                }
                                // сохраняем путь бинарника и кол-во tmp_bin
                                dPathBinAndNumberTmp.Add(path, lTmpNumberInOneBin);
                            }
                        }
                        
                        Console.WriteLine("!!!bin " + lTmpNumberBin.Count);
                        if (lTmpNumberSrc == null)
                        {
                            MessageBox.Show("Выберите сначала папку с исходными текстами с датчиками!");
                            return;
                        }
                        if (lTmpNumberBin == null)
                        {
                            MessageBox.Show("Не выбраны бинарники с датчиками!");
                            return;
                        }

                        // сравниваем tpm номера найденные в src и bin
                        if (lTmpNumberBin.Count < lTmpNumberSrc.Count)
                        {
                            foreach (var tmpSrc in lTmpNumberSrc)
                            {
                                if (lTmpNumberBin.Exists(x => x.Equals(tmpSrc)))
                                {
                                    dFoundTmpNumber.Add(tmpSrc, lTmpPathFile[lTmpNumberSrc.IndexOf(tmpSrc)]);
                                }
                                else
                                {
                                    dNotFoundTmpSrcInBin.Add(tmpSrc, lTmpPathFile[lTmpNumberSrc.IndexOf(tmpSrc)]);
                                }
                            }

                        }
                        else
                        {
                            foreach (var tmpBin in lTmpNumberBin)
                            {
                                if (lTmpNumberSrc.Exists(x => x.Equals(tmpBin)))
                                {
                                    dFoundTmpNumber.Add(tmpBin, lTmpPathFile[lTmpNumberSrc.IndexOf(tmpBin)]);
                                }
                                else
                                {
                                    lNoFoundTmpInSrc.Add(tmpBin);
                                }
                            }
                        }
                          
                        Console.WriteLine("Найденных tmp_src в bin: " + dFoundTmpNumber.Count);
                        Console.WriteLine("Не найденных tmp_src в bin: " + dNotFoundTmpSrcInBin.Count);
                        Console.WriteLine("Лишние tmp_bin без src: "+ lNoFoundTmpInSrc);
                        Console.WriteLine("Подсчет одинаковых tmp: " + dRepeadCountTmpNumber.Count);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Origial error:" + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }

            }
            else return;

            lbPersent.Text = "Сканирование Bin завершено!";

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;
            try
            {
                
                
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string namePath = saveFile.FileName;
                    
                    string paternNameFile = "\\w+\\.{1}txt"; // шаблон для выбора имени заданного пользователем
                    Match matchNameFile = Regex.Match(namePath, paternNameFile);
                    string[] temp = matchNameFile.Value.Split(new char [] { '.' }); // выбираем только имя без расширени

                    // записываем найденные tmp                

                    File.WriteAllText(namePath, "СЛЕДУЮЩИЕ МАРКЕРЫ ИСХОДНЫХ ТЕКСТОВ НАЙДЕНЫ В БИНАРНИКАХ\n\n", Encoding.UTF8);

                    // Сортируем
                    List<KeyValuePair<string, string>> lDicFoundTmpNumberSort = dFoundTmpNumber.ToList();
                    lDicFoundTmpNumberSort.Sort(delegate (KeyValuePair<string, string> firstPair, KeyValuePair<string, string> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    }
                    );

                    foreach (var foundTmp in lDicFoundTmpNumberSort)
                    {
                        File.AppendAllText(namePath, foundTmp.Key + "\t" + foundTmp.Value + "\n", Encoding.UTF8);
                    }

                    File.AppendAllText(namePath, "\nВсего: "+dFoundTmpNumber.Count+"\n\nСЛЕДУЮЩИЕ МАРКЕРЫ ИСХОДНЫХ ТЕКСТОВ НЕ НАЙДЕНЫ В БИНАРНИКАХ\n\n", Encoding.UTF8);

                    // Сортируем
                    List<KeyValuePair<string, string>> lDicNotFoundTmpSrcInBinSort = dNotFoundTmpSrcInBin.ToList();
                    lDicNotFoundTmpSrcInBinSort.Sort(delegate (KeyValuePair<string, string> firstPair, KeyValuePair<string, string> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    }
                    );

                    foreach (var noFoundTmp in lDicNotFoundTmpSrcInBinSort)
                    {
                        File.AppendAllText(namePath, noFoundTmp.Key + "\t" + noFoundTmp.Value + "\n", Encoding.UTF8);
                    }

                    File.AppendAllText(namePath, "\nВсего: "+ dNotFoundTmpSrcInBin.Count+ "\n\nСПИСОК МАРКЕРОВ В БИНАРНИКАХ НЕ НАЙДЕННЫЕ В ИСХОДНИКАХ\n\n", Encoding.UTF8);

                    foreach(var noFoundTmp in lNoFoundTmpInSrc)
                    {
                        File.AppendAllText(namePath, noFoundTmp + "\n", Encoding.UTF8);
                    }

                    File.AppendAllText(namePath, "\nВсего: " + lNoFoundTmpInSrc.Count+ "\n\n", Encoding.UTF8);

                    int repeadTmp = 0;
                    foreach(var v in dRepeadCountTmpNumber)
                    {
                        repeadTmp += v.Value;
                    }

                    string findTmp = String.Format(@"
=====================================================
= Всего маркеров tmp_src: {0}                       =
= Всего маркеров tmp_bin: {1}                       =
= Всего не повторяющихся маркеров tmp_bin: {2}      =
= Кол-во повторяющихся маркеров tmp_bin: {3}        =
= Найденных макреров tmp_src в bin: {4}             =
= Не найденных макреров tmp_src в bin: {5}          =
= Кол-во маркеров tmp_bin без пары tmp_src: {6}     =
= Кол-во повторяющихся маркеров tmp_src в bin: {7}  =
=====================================================
", lTmpNumberSrc.Count, countTmpInBin, lTmpNumberBin.Count, repeadTmp,
dFoundTmpNumber.Count, dNotFoundTmpSrcInBin.Count, lNoFoundTmpInSrc.Count, dRepeadCountTmpNumber.Count);
                    File.AppendAllText(namePath, findTmp + "\n", Encoding.UTF8);
                    // записываем не найденные tmp

                    int n = 1;
                    foreach (var binPathNumberTmp in dPathBinAndNumberTmp)
                    {
                        File.AppendAllText(namePath, "\n"+"("+n+")"+ binPathNumberTmp.Key+"\n\n", Encoding.UTF8);

                        foreach (var numberTmpPathFile in lDicFoundTmpNumberSort)
                        {
                            if (binPathNumberTmp.Value.Exists(x => x.Equals(numberTmpPathFile.Key)))
                            {
                                File.AppendAllText(namePath, numberTmpPathFile.Key +"\t"+ numberTmpPathFile.Value+"\n", Encoding.UTF8);
                            }
                        }
                        ++n;

                        File.AppendAllText(namePath, "\nВсего: " + binPathNumberTmp.Value.Count + "\n\n", Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Запись в файл: " + ex.ToString());
            }

            MessageBox.Show("Запись завершена!");
        }


        private void buttonDeleteFileFilter_Click(object sender, EventArgs e)
        {
            //flagStartSrcClassJar = true;
            //buttonOpenReport.Enabled = false;
            fbd = new FolderBrowserDialog();
            fbd.SelectedPath = pathFolder;
            fbd.ShowNewFolderButton = false;

            // Для сохранения  файла срезультатами
            saveFile = new SaveFileDialog();
            saveFile.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;

            // Текст бокс
            string templateFilters = textBoxFilter.Text;
            if(templateFilters == "")
            {
                MessageBox.Show("Вы не ввели шаблоны расширений.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        pathFolder = fbd.SelectedPath;
                        string[] getFilesAll = Directory.GetFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories);
                        List<string> lFilters = templateFilters.Split(',').ToList();
                        Dictionary<string, int> dCountFilesStay = new Dictionary<string, int>();
                        foreach (var item in lFilters)
                        {
                            dCountFilesStay.Add("." + item, 0);
                        }

                        //Dictionary<string, int> dCountDeleteFiles = dCountFilesStay.ToDictionary(x => x.Key, x => x.Value);
                        Dictionary<string, int> dCountDeleteFiles = new Dictionary<string, int>();
                        int countFilesNoFilter = 0;

                        foreach (var item in getFilesAll)
                        {
                            string filter = Path.GetExtension(item);
                            ////string patern = @"[\w$-]+\.{1}class";
                            //string patern = @".{1}\w+$";
                            //Match match = Regex.Match(item, patern);

                            if(templateFilters != "" & getFilesAll.Length != 0 & filter != "")
                            {
                                if(dCountFilesStay.ContainsKey(filter))
                                {
                                    ++dCountFilesStay[filter];
                                }
                                else
                                {
                                    File.Delete(item);
                                    //if (dCountDeleteFiles.Count == 0)
                                    //    dCountDeleteFiles.Add(filter, 0);
                                    if (!dCountDeleteFiles.ContainsKey(filter))
                                    {
                                        dCountDeleteFiles.Add(filter, 1);
                                    }
                                    else ++dCountDeleteFiles[filter];
                                }
                            }
                            else if(filter == "")
                            {
                                ++countFilesNoFilter;
                            }
                        }

                        int sumAllFileStay = 0, sumAllFilesDelete = 0;

                        foreach (var item in dCountFilesStay)
                        {
                            sumAllFileStay += item.Value;
                        }

                        foreach (var item in dCountDeleteFiles)
                        {
                            sumAllFilesDelete += item.Value;
                        }

                        File.WriteAllText(saveFile.FileName, "\nВсего файлов: " + getFilesAll.Length + "\n", Encoding.UTF8);
                        File.AppendAllText(saveFile.FileName, "\nКоличество файлов без расширений: "+ countFilesNoFilter + "\n", Encoding.UTF8);


                        if(dCountFilesStay.Count != 0 | dCountDeleteFiles.Count != 0)
                        {
                            if(dCountFilesStay.Count != 0)
                            {
                                File.AppendAllText(saveFile.FileName, "\nСписок файлов согласно фильтру расширений:\n", Encoding.UTF8);
                                foreach (var item in dCountFilesStay)
                                {
                                    //File.AppendAllText(saveFile.FileName, item.Key+"\t"+item.Value+"\n", Encoding.UTF8);
                                    File.AppendAllText(saveFile.FileName, String.Format("{0,10:0}{1,10:0\n}", item.Key, item.Value), Encoding.UTF8);
                                }
                                File.AppendAllText(saveFile.FileName, "\nВсего файлов ______: " + sumAllFileStay+"\n", Encoding.UTF8);
                            }
                            if (dCountDeleteFiles.Count != 0)
                            {
                                File.AppendAllText(saveFile.FileName, "\nСписок удаленных файлов:\n", Encoding.UTF8);
                                foreach (var item in dCountDeleteFiles)
                                {
                                    //File.AppendAllText(saveFile.FileName, item.Key + "\t" + item.Value + "\n", Encoding.UTF8);
                                    File.AppendAllText(saveFile.FileName, String.Format("{0,10:0}{1,10:0\n}", item.Key, item.Value), Encoding.UTF8);
                                }
                                File.AppendAllText(saveFile.FileName, "\nВсего файлов ______: " + sumAllFilesDelete, Encoding.UTF8);
                            }

                        }

                        //if (saveFile.ShowDialog() == DialogResult.OK)
                        //{
                        //    saveNameFile = saveFile.FileName;

                        //    if (!backgroundWorker.IsBusy)
                        //    {
                        //        backgroundWorker.RunWorkerAsync();
                        //    }
                        //}
                        //else buttonOpenReport.Enabled = true;
                        MessageBox.Show("Запись файла завершена!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
            }
            else
            {
                return;
            }


























            //==================================================================================
            // Для циклического переноса файлос с одной папки на другую, с проверкой контрольных сумм
            //==================================================================================
            //string pathSourse = @"E:/qwe";
            //DirectoryInfo sourse = new DirectoryInfo(pathSourse);
            //DirectoryInfo destin = new DirectoryInfo(@"E:/zxc");

            //int hashFileSourse = 0;
            //int hashFileDestin = 0;
            //DialogResult dr = MessageBox.Show("Начать копирование?", "", MessageBoxButtons.OKCancel);
            //if (dr == DialogResult.OK)
            //{
            //    try
            //    {
            //        while (true)
            //        {
            //            Thread.Sleep(1000);
            //            string[] str = Directory.GetFiles(pathSourse);

            //            if (str.Length != 0)
            //            {
            //                foreach (var s in sourse.GetFiles())
            //                {
            //                    hashFileSourse = s.Name.GetHashCode();
            //                    s.CopyTo(destin + "/" + s.Name, true);

            //                    foreach (var d in destin.GetFiles(s.Name))
            //                    {
            //                        hashFileDestin = d.Name.GetHashCode();
            //                    }
            //                    if (hashFileSourse != hashFileDestin)
            //                    {
            //                        Task.Factory.StartNew(() => MessageBox.Show(s.Name + " скопированно не корректно"));
            //                    }
            //                    s.Delete();
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //    }
            //}
            //else if (dr == DialogResult.Cancel)
            //    return;


        }

        private void buttonOpenReport_Click(object sender, EventArgs e)
        {
            Process.Start("Notepad++.exe", saveNameFile);
        }

        

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lbPersent.Text = string.Format("Processing ... {0}%", e.ProgressPercentage);
            //lbPersent.Update();
            progressBar1.Update();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!backgroundWorker.CancellationPending)
                {
                    if(flagStartSrcClassJar)
                        workWithFilesJavaAndClass(fbd, saveFile);
                    else if(flagStartSrcJar)  
                        workWithFilesJavaAndJar(fbd, saveFile);
                }
            }
            catch (Exception ex)
            {
                backgroundWorker.CancelAsync();
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbPersent.Text = string.Format("Processing END");
            MessageBox.Show("Файл записан");
            buttonOpenReport.Enabled = true;
            buttonOpenReport2.Enabled = true;
            flagStartSrcClassJar = false;
            flagStartSrcJar = false;
        }

        private void buttonFindFilesJava2_Click(object sender, EventArgs e)
        {
            flagStartSrcJar = true;
            buttonOpenReport2.Enabled = false;
            fbd = new FolderBrowserDialog();
            fbd.SelectedPath = pathFolder;
            fbd.ShowNewFolderButton = false;

            // Для сохранения  файла срезультатами
            saveFile = new SaveFileDialog();
            saveFile.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pathFolder = fbd.SelectedPath;

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        saveNameFile = saveFile.FileName;

                        if (!backgroundWorker.IsBusy)
                        {
                            backgroundWorker.RunWorkerAsync();
                        }
                    }
                    else buttonOpenReport.Enabled = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
            }
            else
            {
                buttonOpenReport.Enabled = true;
                buttonOpenReport2.Enabled = true;
                return;
            }
        }

        private void workWithFilesJavaAndJar(FolderBrowserDialog fbd, SaveFileDialog saveFile)
        {
            string[] getFilesJava = Directory.GetFiles(fbd.SelectedPath, "*.java", SearchOption.AllDirectories);
            string[] getFilesScala = Directory.GetFiles(fbd.SelectedPath, "*.scala", SearchOption.AllDirectories);
            //string[] getFilesClass = Directory.GetFiles(fbd.SelectedPath, "*.class", SearchOption.AllDirectories);
            string[] getJar = Directory.GetFiles(fbd.SelectedPath, "*.jar", SearchOption.AllDirectories);

            int countRepeatNameFileJava = 0; // подсчет количества повторяющихся имен java
            int countRepeatNameFileScala = 0;


            List<string> lFilesJava = new List<string>();
            List<string> lFilesScala = new List<string>();

            lFilesJava = searchFileJava(getFilesJava, ref countRepeatNameFileJava);
            lFilesScala = searchFileScala(getFilesScala, ref countRepeatNameFileScala);

            lFilesJava.AddRange(lFilesScala.GetRange(0, lFilesScala.Count));

            //int countInternalFileClass = 0; // подсчет внутренних (анонимных) классов 
            //int countRepeatNameFileClass = 0; // подстчет повторяющихся имен файлов class

            //List<string> lFilesClassFromStr = new List<string>();

            //lFilesClassFromStr = searchFileClass(getFilesClass, ref sumPersent, ref countRepeatNameFileClass, ref countInternalFileClass);

            int countRepeadClass = 0;
            int countInnerClass = 0;
            int count = 0;
            List<string> lFileClassFromBin = new List<string>();
            List<string> lAllClassFromBin = new List<string>();

            searchFileClassInJar(getJar, ref lFileClassFromBin, ref lAllClassFromBin, ref countRepeadClass, ref countInnerClass);
            // lists для хранения не нашедших пар файлов java и class
            List<string> lFindFreeFileJava = new List<string>();
            List<string> lFindFreeFileClass = new List<string>();

            compareFilesJavaAndClass(lFilesJava, lFileClassFromBin, ref lFindFreeFileJava, ref lFindFreeFileClass, ref count);

            //compareFilesJavaAndClass(lFilesJava, lFilesClassFromStr, ref lFindFreeFileJava,
            //    ref lFindFreeFileClass, ref sumPersent, ref count);

            writeTemplateFormFilesResults(saveFile, fbd, getFilesJava.Length, getFilesScala.Length, countRepeatNameFileJava, countRepeatNameFileScala,
                lFilesJava.Count, lAllClassFromBin.Count, countRepeadClass, countInnerClass,
                lFileClassFromBin.Count, count, lFindFreeFileJava.Count, lFindFreeFileClass.Count,
                getJar.Length);

            //writeTemplateFormFilesResults(saveFile, fbd, getFilesJava.Length, countRepeatNameFileJava,
            //    lFilesJava.Count, getFilesClass.Length, countRepeatNameFileClass, countInternalFileClass,
            //    lFilesClassFromStr.Count, count, lFindFreeFileJava.Count, lFindFreeFileClass.Count,
            //    getJar.Length);  

            count = 0;

            writeFilesNoHavePars(saveFile, fbd, getFilesJava, getFilesScala, lFilesJava, lFileClassFromBin,
                lFindFreeFileJava, lFindFreeFileClass);

            //writeFilesNoHavePars(saveFile, fbd, getFilesJava, lFilesJava, lFilesClassFromStr,
            //    lFindFreeFileJava, lFindFreeFileClass, ref sumPersent);

            //List<string> copyFilesClassFromBin = lFileClassFromBin.GetRange(0, lFileClassFromBin.Count);
            List<string> lNoMatches = new List<string>();
            List<string> lInBinNoFileClass = new List<string>();
            int countIn = 0;
            int index = 0;

            foreach (var pathJar in getJar)
            {
                Thread.Sleep(3);
                backgroundWorker.ReportProgress(index++ * 100 / getJar.Length);

                writeResultsJavaAndJar(saveFile, pathJar, lFilesJava, lFileClassFromBin, ref lInBinNoFileClass, ref lNoMatches, ref countIn, index);
            }

            if (lInBinNoFileClass.Count != 0)
            {
                index = 1;
                File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не содержащих файлов .class\n", Encoding.UTF8);
                foreach (var noFileClass in lInBinNoFileClass)
                {
                    Thread.Sleep(3);
                    backgroundWorker.ReportProgress(index++ * 100 / lInBinNoFileClass.Count);
                    File.AppendAllText(saveFile.FileName, noFileClass + "\n", Encoding.UTF8);
                }
            }

            if (lNoMatches.Count != 0)
            {
                index = 1;
                File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не имеющих совпанений с файлами .class полученных от src:\n", Encoding.UTF8);
                foreach (var noMatches in lNoMatches)
                {
                    Thread.Sleep(3);
                    backgroundWorker.ReportProgress(index++ * 100 / lNoMatches.Count);
                    File.AppendAllText(saveFile.FileName, noMatches + "\n", Encoding.UTF8);
                }
            }


            //// Копируем List
            //List<string> copyFilesClassFromStc = lFilesClassFromStr.GetRange(0, lFilesClassFromStr.Count);
            //List<string> lNoMatches = new List<string>();
            //List<string> lInBinNoFileClass = new List<string>();
            //int countIn = 0;
            //int index = 0;

            //foreach (var pathJar in getJar)
            //{
            //    Thread.Sleep(3);
            //    backgroundWorker.ReportProgress(index++ * 100 / getJar.Length);

            //    writeResultsJar(saveFile, pathJar, lFilesClassFromStr, copyFilesClassFromStc, sumPersent, ref lInBinNoFileClass, ref lNoMatches, ref countIn, index);
            //}

            //if (lInBinNoFileClass.Count != 0)
            //{
            //    index = 1;
            //    File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не содержащих файлов .class\n", Encoding.UTF8);
            //    foreach (var noFileClass in lInBinNoFileClass)
            //    {
            //        Thread.Sleep(3);
            //        backgroundWorker.ReportProgress(index++ * 100 / lInBinNoFileClass.Count);
            //        File.AppendAllText(saveFile.FileName, noFileClass + "\n", Encoding.UTF8);
            //    }
            //}

            //if (countIn == 0)
            //{
            //    index = 1;
            //    File.AppendAllText(saveFile.FileName, "\nПеречень jar архивов не имеющих совпанений с файлами .class полученных от src:\n", Encoding.UTF8);
            //    foreach (var noMatches in lNoMatches)
            //    {
            //        Thread.Sleep(3);
            //        backgroundWorker.ReportProgress(index++ * 100 / lNoMatches.Count);
            //        File.AppendAllText(saveFile.FileName, noMatches + "\n", Encoding.UTF8);
            //    }
            //}
            backgroundWorker.CancelAsync();
        }

        private void searchFileClassInJar(string[] getJar, ref List<string> lFileClassFromBin, ref List<string> lAllClassFromBin, ref int countRepeadClass, ref int countInnerClass)
        {
            int index = 1;
            foreach (var pathJar in getJar)
            {
                Thread.Sleep(3);
                backgroundWorker.ReportProgress(index++ * 100 / getJar.Length);

                //int countRepeadClass = 0;
                //int countInnerClass = 0;
                //List<string> lFileClassFromBin = new List<string>();
                //List<string> lAllClassFromBin = new List<string>();

                string[] allLines = File.ReadAllLines(pathJar);

                searchFileClassFromReadJar(allLines, ref lFileClassFromBin, ref lAllClassFromBin, ref countInnerClass, ref countRepeadClass);

            }
        }

        private void writeResultsJavaAndJar(SaveFileDialog saveFile, string pathJar, List<string> lFilesJava, List <string> lFilesClassFromBinAllProject, ref List<string> lInBinNoFileClass, 
            ref List<string> lNoMatches, ref int countIn, int index)
        {
            int countRepeadClass = 0;
            int countInnerClass = 0;
            List<string> lFileClassFromJar = new List<string>();
            List<string> lAllClassFromJar = new List<string>();

            string[] allLines = File.ReadAllLines(pathJar);

            searchFileClassFromReadJar(allLines, ref lFileClassFromJar, ref lAllClassFromJar, ref countInnerClass, ref countRepeadClass);

            if (lFileClassFromJar.Count == 0)
            {
                lInBinNoFileClass.Add(pathJar);
            }
            else
            {
                File.AppendAllText(saveFile.FileName, "\n"+index +"________________________________________________________________________\n" +
                pathJar + "\n", Encoding.UTF8);
            }
            string j = writeTemplateFormJarResults(lAllClassFromJar.Count, countRepeadClass, countInnerClass, lFileClassFromJar.Count);

            //// Копируем List
            List<string> copyFilesClassFromJar = lFileClassFromJar.GetRange(0, lFileClassFromJar.Count);
            List<string> copyFilesJava = lFilesJava.GetRange(0, lFilesJava.Count);
            copyFilesClassFromJar.Sort();

            int countOut = 0;
            countIn = 0;

            compareFilesJavaAndFilesFromBin(lFilesJava, ref copyFilesJava, lFileClassFromJar, ref copyFilesClassFromJar, ref countIn, ref countOut);

            //if (countIn != 0 & copyFilesClassFromJar.Count == 0)
            if (countIn == 0 & copyFilesClassFromJar.Count != 0)
            {
                lNoMatches.Add(pathJar);
                File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло (одноименных) " + countIn + " файлов .class(java)\n" + "В jar архив НЕ вошло (одноименных) " + countOut + " файлов .class(java)\n", Encoding.UTF8);
                //File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromBin.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
                if (lFileClassFromJar.Count != countIn)
                {
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных одноименных файлов java и class в jar файле: " + copyFilesClassFromJar.Count + "\n", Encoding.UTF8);
                    foreach (var unknownClass in copyFilesClassFromJar)
                    {
                        //File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromJar.Count + "\n", Encoding.UTF8);
                }
            }
            else if (countIn == 0 && lFileClassFromJar.Count == 0)
            {
                lNoMatches.Add(pathJar);
            }
            else
            {
                //lNoMatches.Add(pathJar);
                File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло (одноименных) " + countIn + " файлов .class(java)\n" + "В jar архив НЕ вошло (одноименных) " + countOut + " файлов .class(java)\n", Encoding.UTF8);
                //File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromBin.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
                File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
                if (lFileClassFromJar.Count != countIn)
                {
                    File.AppendAllText(saveFile.FileName, "В jar архиве находятся неизвестные классы не совпавшие по именам с java:\n", Encoding.UTF8);

                    foreach (var unknownClass in copyFilesClassFromJar)
                    {
                        //File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
                    }
                    File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromJar.Count + "\n", Encoding.UTF8);
                }

            }





            //if (countIn == 0 && lFileClassFromBin.Count != 0)
            //{
            //    lNoMatches.Add(pathJar);
            //    File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло " + countIn + " файлов .class\n" + "В jar архив НЕ вошло " + countOut + " файлов .class\n", Encoding.UTF8);
            //    File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromBin.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
            //    File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
            //    if (lFileClassFromBin.Count != countIn)
            //    {
            //        File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
            //        foreach (var unknownClass in copyFilesClassFromBin)
            //        {
            //            File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
            //        }
            //        File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
            //    }
            //}
            //else if(countIn == 0 && lFileClassFromBin.Count == 0)
            //{
            //    lNoMatches.Add(pathJar);
            //}
            //else
            //{
            //    File.AppendAllText(saveFile.FileName, "\nВ jar архив вошло " + countIn + " файлов .class\n" + "В jar архив НЕ вошло " + countOut + " файлов .class\n", Encoding.UTF8);
            //    File.AppendAllText(saveFile.FileName, "Все ли файлы .class из src используются: " + ((copyFilesClassFromBin.Count == 0) ? "YES\n\n" : "NO\n\n"), Encoding.UTF8);
            //    File.AppendAllText(saveFile.FileName, j + "\n", Encoding.UTF8);
            //    if (lFileClassFromBin.Count != countIn)
            //    {
            //        File.AppendAllText(saveFile.FileName, "В jar архиве находятся неизвестные классы:\n", Encoding.UTF8);

            //        foreach (var unknownClass in copyFilesClassFromBin)
            //        {
            //            File.AppendAllText(saveFile.FileName, unknownClass + "\n", Encoding.UTF8);
            //        }
            //        File.AppendAllText(saveFile.FileName, "\nВсего неизвестных классов в jar файле: " + copyFilesClassFromBin.Count + "\n", Encoding.UTF8);
            //    }

            //}
        }

        private void compareFilesJavaAndFilesFromBin(List<string> lFilesJava, ref List<string> copyFilesJava, List<string> lFileClassFromJar, ref List<string> copyFilesClassFromJar,
             ref int countIn, ref int countOut)
        {
            int index = 1;

            foreach (var cl in lFileClassFromJar)
            {

                //Thread.Sleep(5);
                backgroundWorker.ReportProgress(index++ * 100 / lFileClassFromJar.Count);

                string paternFiles = "\\w+";
                Regex regex = new Regex(paternFiles);
                Match match = regex.Match(cl);
                string strJava = match.Value + ".java"; //!!!
                string strScala = match.Value + ".scala";
                if (lFilesJava.Exists(x => x.Equals(strJava)))
                {
                    ++countIn;
                    //File.AppendAllText(saveFile.FileName, cl + "\tOK\n", Encoding.UTF8);
                    copyFilesClassFromJar.Remove(cl);
                    copyFilesJava.Remove(strJava);
                }
                else if(lFilesJava.Exists(x => x.Equals(strScala)))
                {
                    ++countIn;
                    //File.AppendAllText(saveFile.FileName, cl + "\tOK\n", Encoding.UTF8);
                    copyFilesClassFromJar.Remove(cl);
                    copyFilesJava.Remove(strScala);
                }
                else
                {
                    ++countOut;
                    //File.AppendAllText(saveFile.FileName, cl + "\tNO\n", Encoding.UTF8);
                    //copyFilesClassFromBin.Remove(cl);
                    //copyFilesClassFromStc.Remove(cl);
                }
            }
        }

        private void buttonOpenReport2_Click(object sender, EventArgs e)
        {
            buttonOpenReport_Click(sender, e);
        }

        //==================================================================
        // Для удаление файлов в директории по файлу путей
        //==================================================================

        private void buttonChoiceFileText_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            List <string> lStringsFile = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = @"E:\Work\W_INS_KMO\Tirs(АДЛБ.181)_v3\srcCB_tirs_v3\EFSK.5005X.12\build\src";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> lPath = new List<string>();

                try
                { // открытие файла

                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        { // считываем выбранный файл
                            StreamReader readFile = new StreamReader(myStream);
                            string line = "";
                            // считываем по строке файл
                            while ((line = readFile.ReadLine()) != null)
                            {
                                // сохраняем в лист строки
                                lStringsFile.Add(line);
                            }
                            readFile.Close();
                        }
                                              
                        foreach(var l in lStringsFile)
                        {
                            string [] tmp = l.Split(new char[] { '\t' } );
                            if (tmp.Length != 2)
                                continue;
                            else lPath.Add(tmp[1]);
                        }
                    }
                    foreach(var d in lPath)
                    {
                        File.Delete(d);
                    }
                    MessageBox.Show("Delete files complite");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Origial error:" + ex.Message);
                }
            }
        }
    }
}
