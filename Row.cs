namespace DataBase
{
    public class Row
    {
        public MyList<MyString> Data; 
        public bool IsConnected { get; private set; }
        public int NumberFieldToConnect { get; private set; }

        public int NumberTableToConnect { get; private set; }
        public MyString Type { get; private set; }

        public Row(MyString data)
        {
            Data = new MyList<MyString>();
            Data.Add(data);
            IsConnected = false;
            NumberTableToConnect = -1;
            NumberFieldToConnect = -1;
        }

        public void SetConnection(int numberOfTable, int numberOfField)
        {
            NumberFieldToConnect = numberOfField;
            NumberTableToConnect = numberOfTable;
            IsConnected = true;
        }
    }
}