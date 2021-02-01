namespace DataBase
{
    public class MyList<T>
    {
        private Node<T> head;
        public int Size { get; private set; }

        public MyList()
        {
            Size = 0;
            head = null;
        }

        public void Add(T data)
        {
            if (head == null)
            {
                head = new Node<T>(data);
                Size = 1;
            }
            else
            {
                Node<T> current = head;
                while (current.next != null)
                    current = current.next;
                current.next = new Node<T>(data);
                Size++;
            }
        }

        public void Clear()
        {
            head = null;
            Size = 0;
        }

        public T this[int i]
        {
            get { return GetItem(i); }
            set { SetItem(i, value); }
        }

        private void SetItem(int i, T value)
        {
            Node<T> current = head;
            for (int j = 0; j < i; j++)
                if (current.next != null)
                    current = current.next;
            current.data = value;
        }

        public T GetItem(int i)
        {
            Node<T> current = head;
            for (int j = 0; j < i; j++)
                if (current.next != null)
                    current = current.next;
            return current.data;
        }

        public T[] ToArray()
        {
            T[] result = new T[Size];

            for (int i = 0; i < Size; i++)
                result[i] = GetItem(i);

            return result;
        }
    }
}