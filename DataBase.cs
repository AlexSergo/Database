using System;
using System.IO;

namespace DataBase
{
    public class DataBase
    {
        public MyString Name { get; private set; }
        public int CountOfTables { get; private set; }
        private Table[] tables;

        public DataBase(MyString name, int countOfTables)
        {
            Name = name;
            CountOfTables = countOfTables;
            tables = new Table[countOfTables];
        }

        public void AddField(MyString nameOfTable, MyString name)
        {
            tables[GetNumberOfTable(nameOfTable)].AddField(name);
        }

        public void AddNote(MyString nameOfTable, MyString data, MyString nameOfField)
        {
            tables[GetNumberOfTable(nameOfTable)].AddNote(data, nameOfField);
        }

        public MyString GetNameOfField(int numberOfTable, int numberOfField)
        {
            return tables[numberOfTable].GetNameOfField(numberOfField);
        }

         public void AddConnection(MyString firstConnect, int numberOfField, MyString secondConnect, int secondNumberOfField)
         {
            int firstIndex = -1, secondIndex = -1;
            for (int i = 0; i < tables.Length; i++)
                if (secondIndex == -1 || firstIndex == -1)
                {
                    if (tables[i].Name == firstConnect)
                        firstIndex = i;
                    else if (tables[i].Name == secondConnect)
                        secondIndex = i;
                }
                else
                    break;
            tables[firstIndex].AddConnectByNumberFieldWithAnotherTable(numberOfField, secondIndex, secondNumberOfField);   
         }

        public void ShowTable(int numberOfTable)
        {
            tables[numberOfTable].Show();
        }

        public void Save()
        {
            Console.WriteLine("Saving the base...");
            for (int j = 0; j < CountOfTables; j++)
            {
                using (FileStream fstream = new FileStream($"{Name.GetString()} {tables[j].Name.GetString()}.txt", FileMode.OpenOrCreate))
                {
                    MyString str = new MyString();
                    MyList<Row> rows = GetRows(tables[j].Name);
                    for (int i = 0; i < rows.Size; i++)
                    {
                        if (!rows[i].IsConnected)
                            str += rows[i].Data[0] + new MyString(' ');
                        else
                            str += rows[i].Data[0] + new MyString('[') + 
                                GetNameOfTable(rows[i].NumberTableToConnect) + new MyString(',') + 
                                new MyString(rows[i].NumberFieldToConnect) + new MyString(']') + new MyString(' ');
                    }
                    str += new MyString('\n');

                    for (int k = 1; k < rows[0].Data.Size; k++)
                    {
                        for (int i = 0; i < rows.Size; i++)
                            str += rows[i].Data[k] + new MyString(' ');

                        str += new MyString('\n');
                    }
                    str = str.Substring(0, str.Length - 2);
                    byte[] array = System.Text.Encoding.Default.GetBytes(str.GetString());
                    fstream.Write(array, 0, array.Length);
                }
            }
            Console.WriteLine("Done.");
        }

        public void SetNameForTable(int i, MyString name)
        {
            tables[i] = new Table(name);
        }

        public int GetCountOfFields(int numberOfTable)
        {
            return tables[numberOfTable].Rows.Size;
        }

        public MyList<Row> GetRows(MyString nameOfTable)
        {
            return tables[FindNumberByName(nameOfTable)].Rows;
        }

        private int FindNumberByName(MyString nameOfTable)
        {
            for (int i = 0; i < tables.Length; i++)
                if (tables[i].Name == nameOfTable)
                    return i;
            return -1;
        }

        public Row GetRowInTable(int numberTableToConnect, int numberFieldToConnect)
        {
            return tables[numberTableToConnect].Rows[numberFieldToConnect];
        }

        public void Open(MyString name)
        {
            for (int i = 1; i <= CountOfTables; i++)
            {
                int countOfFields = 0;
                Console.Write($"Input name of {i} table: ");
                MyString nameTable = new MyString(Console.ReadLine().ToCharArray());
                SetNameForTable(i - 1, nameTable);
                using (FileStream fstream = File.OpenRead($"{name.GetString()} {nameTable.GetString()}.txt"))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    char[] textFromFile = System.Text.Encoding.Default.GetChars(array);
                    MyString text = new MyString(textFromFile);

                    MyString[] parsingTable = text.Split(' ');
                    for (int j = 0; j < parsingTable.Length; j++)
                    {
                        int index = parsingTable[j].IndexOf(new MyString('\n'));
                        if (index >= 0)
                        {
                            if (countOfFields == 0)
                            {
                                countOfFields = j;
                                for (int l = 0; l < countOfFields; l++)
                                      AddField(nameTable, parsingTable[l]);
                            }
                            parsingTable[j] = parsingTable[j].Replace(new MyString('\n'), new MyString('/'));
                        }
                    }
                    ReadNotes(nameTable, countOfFields, parsingTable);
                }
            }
            ReadConnections();
        }

        private void ReadNotes(MyString nameTable, int countOfFields, MyString[] data)
        {
            int k = 0;
            for (int i = countOfFields; i < data.Length; i++)
            {
                AddNote(nameTable, data[i], tables[GetNumberOfTable(nameTable)].Rows[k].Data[0]);
                if (k + 1 < countOfFields)
                    k++;
                else
                    k = 0;
            }
        }

        private void ReadConnections()
        {
            int firstNumberOfFieldToconnect = -1;
            int secondNumberOfFieldToConnect = -1;
            MyString  secondTableToConnect = new MyString();

            for (int i = 0; i < CountOfTables; i++)
            {
                for (int j = 0; j < tables[i].Rows.Size; j++)
                {
                    MyString field = tables[i].Rows[j].Data[0];
                    int startConnectIndex = field.IndexOf(new MyString('['));
                    int endConnectIndex = field.IndexOf(new MyString(']'));
                    int point = field.IndexOf(new MyString(','));

                    if (startConnectIndex > 0)
                    {
                        firstNumberOfFieldToconnect = j;
                        secondNumberOfFieldToConnect = field.Substring(point + 1, endConnectIndex - (point + 1)).GetInt();
                        secondTableToConnect = field.Substring(startConnectIndex + 1, point - (startConnectIndex + 1));

                        tables[i].Rows[firstNumberOfFieldToconnect].SetConnection(GetNumberOfTable(secondTableToConnect), secondNumberOfFieldToConnect);

                        tables[i].RemoveBreakets(firstNumberOfFieldToconnect);
                    }
                }
            }
        }

        private int GetNumberOfTable(MyString name)
        {
            for (int i = 0; i < CountOfTables; i++)
                if (tables[i].Name == name)
                    return i;
            return -1;
        }

        public MyString GetNameOfTable(int numberOfTable)
        {
            return tables[numberOfTable].Name;
        }

        public void Sort(MyString nameOfTable, MyString nameOfField)
        {
            tables[GetNumberOfTable(nameOfTable)].Sort(nameOfField);
        }

        public void EditNote(MyString nameOfTable, int idNumber)
        {
            int numberOfTable = FindNumberByName(nameOfTable);
            int indexOFNote = -1;
            for (int i = 1; i < tables[numberOfTable].Rows[0].Data.Size; i++)
                if (tables[numberOfTable].Rows[0].Data[i] == new MyString(idNumber))
                    indexOFNote = i;

            if (indexOFNote == -1)
            {
                Console.WriteLine("Not found!");
                return;
            }
            else
            {
                for (int i = 0; i < tables[numberOfTable].Rows.Size; i++)
                {
                    if (tables[numberOfTable].Rows[i].IsConnected)
                    {
                        PrintNotesWithConnect(tables[numberOfTable].Rows[i].NumberTableToConnect, tables[numberOfTable].Rows[i].NumberFieldToConnect);
                        char[] message = { 'I', 'n', 'p', 'u', 't', ' ', 'n', 'e', 'w', ' ', 'v', 'a', 'l', 'u', 'e', ' ', 'f', 'o', 'r', ' ' };
                        tables[numberOfTable].Rows[i].Data[0].Print(new MyString(message), new MyString(new char[] {':', ' ' }));
                        MyString value = new MyString(Console.ReadLine().ToCharArray());
                        tables[numberOfTable].Rows[i].Data[indexOFNote] = value;
                    }
                    else
                        tables[FindNumberByName(nameOfTable)].EditNoteByIndexOfNote(i, indexOFNote);
                }
            }
            Console.WriteLine("Success!");
        }

        public void PrintNotesWithConnect(int numberOfConnectionTable, int numberOfConnectionField)
        { 
            Console.WriteLine($"Choose one from the table {GetNameOfTable(numberOfConnectionTable).GetString()}: ");
            Row rowConnect = GetRowInTable(numberOfConnectionTable, numberOfConnectionField);
            for (int z = 1; z < rowConnect.Data.Size; z++)
            {
                for (int k = 0; k < tables[numberOfConnectionTable].Rows.Size; k++)
                    Console.Write($"{tables[numberOfConnectionTable].Rows[k].Data[z].GetString()} | ");
                Console.WriteLine();
            }
        }

        public MyList<Row> SearchWithField(MyString nameOfTable, int numberOfField, MyString data)
        {
            return tables[FindNumberByName(nameOfTable)].SearchWithField(numberOfField, data);
        }
    }
}
