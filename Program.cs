using System;

namespace DataBase
{
    class Program
    {
        private static DataBase database;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, it is the biggest database creater!");
            MainMenu();
            database.Save();
        }

        private static void MainMenu()
        {
            int action = -1;
            Console.WriteLine("Do you want to create (input 1) or open (input 2) database?");
            while (action == -1)
            {
                Console.Write("Take an action: ");
                action = Convert.ToInt32(Console.ReadLine());

                switch (action)
                {
                    case 1:
                        CreateNewBase();
                        break;
                    case 2:
                        OpenBase();
                        break;
                    default:
                        Console.WriteLine("ERROR: Incorrent action! Try Again!");
                        action = -1;
                        break;
                }
            }
        }

        private static void CreateNewBase()
        {
            Console.Write("Input name of your base: ");
            MyString name = new MyString(Console.ReadLine().ToCharArray());
            Console.Write("Input count of tables: ");
            int countOfTables = Convert.ToInt32(Console.ReadLine());
            database = new DataBase(name, countOfTables);

            SetNamesForTables();

            AddFields(countOfTables, database);

            if (countOfTables > 1)
            {
                MyString y = new MyString('y');
                MyString answer = y;
                while (answer == y)
                {
                    Console.WriteLine("Do you want to add connection between tables?(y/n)");
                    answer = new MyString(Console.ReadLine().ToCharArray());
                    if (answer == y)
                        AddConnection(database);
                }
            }

            AddNotes(database);
        }

        private static void SetNamesForTables()
        {
            for (int i = 0; i < database.CountOfTables; i++)
            {
                Console.Write($"Input name of table {i + 1}: ");
                database.SetNameForTable(i, new MyString(Console.ReadLine().ToCharArray()));
            }
        }

        private static void AddConnection( DataBase database)
        {
            Console.Write("Input name of first table you want to connect: ");
            MyString firstConnect = new MyString(Console.ReadLine().ToCharArray());
            Console.Write("Input number of field you want to connect: ");
            int numberOfField = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.Write("Input name of second table you want to connect: ");
            MyString secondConnect = new MyString(Console.ReadLine().ToCharArray());
            Console.Write("Input number of field you want to connect: ");
            int secondNumberOfField = Convert.ToInt32(Console.ReadLine()) - 1;

            database.AddConnection(firstConnect, numberOfField, secondConnect, secondNumberOfField);

        }

        private static void AddFields(int countOfTables,DataBase database)
        {
            int countOfFields;
            for (int j = 0; j < countOfTables; j++)
            {
                countOfFields = 1;
                Console.WriteLine($"Add fields for {database.GetNameOfTable(j).GetString()} table");
                MyString name = new MyString('0');
                while (name != new MyString(new char[2] { '-', '1' }))
                {
                    Console.Write($"Input name of field {countOfFields} or input -1 to end: ");
                    name = new MyString(Console.ReadLine().ToCharArray());
                    if (name != new MyString(new char[2] { '-', '1' }))
                    {
                        database.AddField(database.GetNameOfTable(j), name);
                        countOfFields++;
                    }
                }
            }
        }

        private static void OpenBase()
        {
            Console.Write("Input name of your database: ");
            MyString name = new MyString(Console.ReadLine().ToCharArray());
            Console.Write("Input count of tables: ");
            int countOFTables = Convert.ToInt32(Console.ReadLine());
            database = new DataBase(name, countOFTables);
            database.Open(name);

            WorkWithDatabase(database);
        }

        private static void WorkWithDatabase(DataBase database)
        {
            bool exit = false;
            while (!exit)
            {
                int action = -1;
                Console.WriteLine("1. Show all data\n2. Find a note\n3. Edit data\n4. Sort data\n5. Add note\n6. Exit");
                while (action == -1)
                {
                    Console.Write("Take an action: ");
                    action = Convert.ToInt32(Console.ReadLine());
                    switch (action)
                    {
                        case 1:
                            ShowData(database);
                            break;
                        case 2:
                            FindData(database);
                            break;
                        case 3:
                            EditData(database);
                            break;
                        case 4:
                            SortData(database);
                            break;
                        case 5:
                            AddData(database);
                            break;
                        case 6:
                            exit = true;
                            break;
                        default:
                            {
                                Console.WriteLine("ERROR: Incorrent action! Try Again!");
                                action = -1;
                            }
                            break;
                    }
                }
            }
            Console.WriteLine("Bye...");
        }

        private static void AddData(DataBase database)
        {
            Console.Write("Input name of table for add new note: ");
            MyString name = new MyString(Console.ReadLine().ToCharArray());
            MyList<Row> rows = database.GetRows(name);
            for (int i = 0; i < rows.Size; i++)
            {
                Console.Write($"Input new data for {rows[i].Data[0].GetString()}: ");
                MyString data = new MyString(Console.ReadLine().ToCharArray());
                database.AddNote(name, data, rows[i].Data[0]);
            }
        }

        private static void SortData(DataBase database)
        {
            Console.Write("Input name of table for sort: ");
            MyString nameOfTable = new MyString(Console.ReadLine().ToCharArray());
            Console.Write("Input name of field for sort: ");
            MyString nameOfField = new MyString(Console.ReadLine().ToCharArray());

            database.Sort(nameOfTable, nameOfField);
        }

        private static void EditData(DataBase database)
        {
            Console.Write("Input name of table for edit: ");
            MyString nameOfTable = new MyString(Console.ReadLine().ToCharArray());
            MyList<Row> rows = database.GetRows(nameOfTable);
            Console.Write($"Input number of {rows[0].Data[0].GetString()} for edit: ");
            int idNumber = Convert.ToInt32(Console.ReadLine());

            database.EditNote(nameOfTable, idNumber);
        }

        private static void FindData(DataBase database)
        {
            Console.Write("Input name of table for search: ");
            MyString nameOfTable = new MyString(Console.ReadLine().ToCharArray());
            MyList<Row> rows = database.GetRows(nameOfTable);
            int action = -1;
            while (action < 1 || action > rows.Size)
            {
                for (int i = 1; i <= rows.Size; i++)
                    Console.WriteLine($"{i}. Find with field {rows[i - 1].Data[0].GetString()}");
                Console.Write("Action: ");
                action = Convert.ToInt32(Console.ReadLine());
                if (action < 1 || action > rows.Size)
                    Console.WriteLine("ERROR! Wrong action!");
            }
            Console.Write("Input data: ");
            MyString data = new MyString(Console.ReadLine().ToCharArray());

            MyList<Row> result = database.SearchWithField(nameOfTable, action - 1, data);
            ConsolePrint.PrintTable(result);
        }

        private static void ShowData(DataBase database)
        {
            for (int i = 0; i < database.CountOfTables; i++)
                database.ShowTable(i);
        }

        private static void AddNotes(DataBase database)
        {
            for (int i = 0; i < database.CountOfTables; i++)
            {
                Console.WriteLine($"Add notes for {database.GetNameOfTable(i).GetString()} table");
                MyString answer = new MyString('y');
                int countOfFields = database.GetCountOfFields(i);
                MyList<Row> rows = database.GetRows(database.GetNameOfTable(i));
                int j = 0;
                while (answer == new MyString('y'))
                {
                    for (int k = 0; k < countOfFields; k++)
                    {
                        MyString currentField = database.GetNameOfField(i, k);
                        if (!rows[k].IsConnected)
                        {
                            Console.Write($"Input name of note {j + 1} for {rows[k].Data[0].GetString()} field: ");
                        }
                        else
                        {
                            database.PrintNotesWithConnect(rows[k].NumberTableToConnect, rows[k].NumberFieldToConnect);
                        }
                        MyString data = new MyString(Console.ReadLine().ToCharArray());
                        database.AddNote(database.GetNameOfTable(i), data, currentField);
                    }
                    j++;
                    Console.WriteLine("Do you want to add more?(y/n)");
                    answer = new MyString(Console.ReadLine().ToCharArray());
                }
            }
        }
    }
}
