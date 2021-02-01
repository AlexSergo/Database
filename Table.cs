using System;

namespace DataBase
{
    public class Table
    {
        public MyList<Row> Rows { get; private set; }
        public MyString Name { get; private set; }

        public Table(MyString name)
        {
            Name = name;
            Rows = new MyList<Row>();
        }

        public void AddField(MyString name)
        {
            Rows.Add(new Row(name));
        }

        public void AddNote(MyString data, MyString nameOfField)
        {
            for (int i = 0; i < Rows.Size; i++)
               if (Rows[i].Data[0] == nameOfField)
                {
                    Rows[i].Data.Add(data);
                    break;
                }
        }

        public MyString GetNameOfField(int numberOfField)
        {
            return Rows[numberOfField].Data[0];
        }

        public void AddConnectByNumberFieldWithAnotherTable(int numberOfField, int secondConnect, int secondNumberOfField)
        {
            Rows[numberOfField].SetConnection(secondConnect, secondNumberOfField);
        }

        public void Show()
        {
            Console.Write("| ");
            for (int i = 0; i < Rows.Size; i++)
                Console.Write(" {0,12} | ",Rows[i].Data[0].GetString());

            ConsolePrint.PrintLine(Rows.Size);
            Console.Write("| ");

            int j = 0;
            for (int i = 0; i < Rows[0].Data.Size - 1; i++)
            {
                Console.Write(" {0,12} | ", Rows[j].Data[i + 1].GetString());
                if (j + 1 < Rows.Size)
                {
                    j++;
                    i--;
                }
                else
                {
                    j = 0;
                    ConsolePrint.PrintLine(Rows.Size);
                    Console.Write("| ");
                }
            }
            Console.WriteLine();
        }

        public MyList<Row> SearchWithField(int numberOfField, MyString data)
        {
            MyList<Row> result = new MyList<Row>();
            int id = -1;
            for (int i = 1; i < Rows[numberOfField].Data.Size; i++)
                if (Rows[numberOfField].Data[i] == data)
                    id = i;

            if (id == -1)
                Console.WriteLine("NOT FOUND!");

            for (int i = 0; i < Rows.Size; i++)
                result.Add(new Row(Rows[i].Data[0]));

            for (int i = 0; i < Rows.Size; i++)
                    result[i].Data.Add(Rows[i].Data[id]);

            return result;
        }

        public void EditNoteByIndexOfNote(int rowIndex, int indexOfNote)
        {
                Console.Write($"Input new value for {Rows[rowIndex].Data[0].GetString()}: ");
                MyString value = new MyString(Console.ReadLine().ToCharArray());
                Rows[rowIndex].Data[indexOfNote] = value;
        }

        public void Sort(MyString nameOfField)
        {
            for (int i = 0; i < Rows.Size; i++)
                if (Rows[i].Data[0] == nameOfField)
                {
                    QuickSort(i, 1, Rows[i].Data.Size - 1);
                    break;
                }
        }

        private void QuickSort(int numberOfRow, int startIndex, int endIndex)
        {
            if (startIndex < endIndex)
            {
                int temp = Segmention(numberOfRow, startIndex, endIndex);

                QuickSort(numberOfRow, startIndex, temp - 1);
                QuickSort(numberOfRow, temp, endIndex);
            }
        }

        private int Segmention(int numberOfRow, int startIndex, int endIndex)
        {
            MyString mid = Rows[numberOfRow].Data[(startIndex + endIndex) / 2];
            int i = startIndex;
            int j = endIndex;

            while (i <= j)
            {
                while (Rows[numberOfRow].Data[i].Compare(Rows[numberOfRow].Data[i], mid) < 0)
                    i++;
                while (Rows[numberOfRow].Data[j].Compare(Rows[numberOfRow].Data[j], mid) > 0)
                    j--;
                if (i <= j)
                {
                    for (int k = 0; k < Rows.Size; k++)
                    {
                        MyString temp = Rows[k].Data[i];
                        Rows[k].Data[i] = Rows[k].Data[j];
                        Rows[k].Data[j] = temp;
                    }

                    i++;
                    j--;
                }
            }

            return i;
        }

        public void RemoveBreakets(int rowIndex)
        {
            int breacketPosition = Rows[rowIndex].Data[0].IndexOf(new MyString('['));
            if (breacketPosition > 0)
                 Rows[rowIndex].Data[0] = Rows[rowIndex].Data[0].Substring(0, breacketPosition);
        }
    }
}
