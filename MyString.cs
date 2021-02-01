using System;

namespace DataBase
{
    public class MyString
    {
        public MyList<char> Data { get; private set; }
        public int Length { get; private set; }
        public MyString()
        {
            Data = new MyList<char>();
            Length = Data.Size;
        }

        public MyString(char[] data)
        {
            Data = new MyList<char>();
            for (int i = 0; i < data.Length; i++)
                Data.Add(data[i]);
            Length = Data.Size;
        }

        public MyString(char data)
        {
            Data = new MyList<char>();
            Data.Add(data);
            Length = Data.Size;
        }

        public void Print(MyString myString = null, MyString myString2 = null)
        {
            for (int i = 0; i < myString.Length; i++)
                Console.Write(myString.Data[i]);
            for (int i = 0; i < Length; i++)
                Console.Write(Data[i]);
            for (int i = 0; i < myString2.Length; i++)
                Console.Write(myString2.Data[i]);
        }

        public MyString(int data)
        {
            Data = new MyList<char>();
            char[] integer = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (data == 0)
                Data.Add('0');
            else
            {
                while (data % 10 > 0)
                {
                    int last = data % 10;
                    for (int i = 0; i < integer.Length; i++)
                        if (last == i)
                            Data.Add(integer[i]);
                    data /= 10;
                }
            }
            Length = Data.Size;
        }

        public int Compare(MyString first, MyString second)
        {
            int length = first.Length < second.Length ? first.Length : second.Length;
            for (int i = 0; i < length; i++)
            {
                if (first[i] > second[i])
                    return -1;
            }
            for (int i = 0; i < length; i++)
            {
                if (first[i] < second[i])
                    return 1;
            }
            if (first.Length == second.Length)
                return 0;
            return 0;
        }

        public string GetString()
        {
            string str = "";
            for (int i = 0; i < Data.Size; i++)
                str += Data[i];
            return str;
        }

        public MyString Substring(int start, int length)
        {
            MyString result = new MyString();

            for (int i = start; i < (start + length); i++)
            {
                result.Data.Add(Data[i]);
                result.Length++;
            }

            return result;
        }

        public int IndexOf(MyString substring)
        {
            int index;
            MyString temp = new MyString();
            for (int i = 0; i < Length; i++)
            {
                if (Data[i] == substring[0])
                {
                    index = i;
                    temp.Data.Add(Data[i]);
                    temp.Length++;
                    for (int j = i + 1; j < substring.Length; j++)
                    {
                        temp.Data.Add(Data[j]);
                        temp.Length++;
                        if (temp == substring)
                            return index;
                        if (temp[temp.Length - 1] != substring[j])
                            break;
                    }
                    if (temp == substring)
                        return index;
                    temp.Data.Clear();
                }
            }
            return -1;
        }

        public char this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }

        public static bool operator !=(MyString first, MyString second)
        {
            if (first.Length == second.Length)
            {
                for (int i = 0; i < first.Length; i++)
                    if (first[i] != second[i])
                        return true;
                return false;
            }
            return true;
        }

        public static bool operator ==(MyString first, MyString second)
        {
            if (first.Length == second.Length)
            {
                for (int i = 0; i < first.Length; i++)
                    if (first[i] != second[i])
                        return false;
                return true;
            }
            return false;
        }

        public static MyString operator +(MyString first, MyString second)
        {
            for (int i = 0; i < second.Length; i++)
            {
                first.Data.Add(second.Data[i]);
                first.Length++;
            }
            return first;
        }

        public MyString[] Split(char substring)
        {
            int newWordIndex = 0; 
            int size = 1; int j = 0; 

            for (int i = 0; i < Length; i++) 
                if (Data[i] == substring)
                    size++;
            MyString[] result = new MyString[size];

            for (int i = 0; i < Length; i++)
                if (Data[i] == substring)
                {
                    result[j] = Substring(newWordIndex, i - newWordIndex);
                    newWordIndex = i + 1;
                    j++;
                }
            result[j] = Substring(newWordIndex, Length - newWordIndex); 
            return result;
        }

        public MyString Replace(MyString past, MyString next) 
        {
            int indexPastString = IndexOf(past);
            MyString newstr = Substring(0, indexPastString);
            if (next != new MyString('/'))
            {
                   newstr += next;
                   newstr.Length += next.Length;
            }
            newstr += Substring(indexPastString + past.Length, Length - indexPastString - past.Length);
            return newstr;
        }

        public int GetInt()
        {
            int result = 0;
            int size = 0;
            char[] integer = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            for (int i = Data.Size - 1; i >= 0; i--)
            {
                for (int j = 0; j < integer.Length; j++)
                    if (Data[i] == integer[j])
                    {
                        result += j * Pow(10, size);
                        size++;
                        break;
                    }
            }
            return result;
        }

        private int Pow(int a, int b)
        {
            if (b == 0) return 1;
            for (int i = 1; i <= b; i++)
                a *= b;
            return a;
        }
    }
}
