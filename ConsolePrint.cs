using System;

namespace DataBase
{
    public static class ConsolePrint
    {
        public static void PrintLine(int size)
        {
            Console.WriteLine();
            for (int z = 0; z < size * 16; z++)
                Console.Write("-");
            Console.WriteLine();
        }

        public static void PrintTable(MyList<Row> rows)
        {
            for (int i = 0; i < rows.Size; i++)
                Console.Write(" {0,12} | ", rows[i].Data[0].GetString());
            PrintLine(rows.Size);
            for (int i = 1; i < rows[0].Data.Size; i++)
            {
                for (int j = 0; j < rows.Size; j++)
                    Console.Write(" {0,12} | ", rows[j].Data[i].GetString());
                PrintLine(rows.Size);
            }
        }
    }
}
