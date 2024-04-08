using System.Collections;

namespace DesignPatterns;


public class LinkedListClient
{
    public static void Main()
    {
        var linkedList = new LinkedList();
        for (int i = 1; i <= 10; i++)
        {
            linkedList.Add(i);
        }

        Console.WriteLine("------");

        //GetEnumerator (is in IEnumerable<T> interface)
        using (var iterator = linkedList.GetEnumerator())
        {
            while (iterator.MoveNext())
            {
                Console.WriteLine(iterator.Current);
            }    
        }
        
        
        foreach (var item in linkedList)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine(linkedList.Average());
    }
}

public class LinkedList : IEnumerable<int>
{
    public class Node
    {
        public int Value { get; set; }
        public Node? Next { get; set; }
    }

    private Node? _head;
    private Node? _last;

    public void Add(int value)
    {
        if (_head == null)
        {
            _head = new Node { Value = value };
            _last = _head;
        }
        else
        {
            _last!.Next = new Node { Value = value };
            _last = _last.Next;
        }
    }

    //print, min, max, sum, average
    public int? Min()
    {
        if (_head == null)
        {
            return null;
        }

        int min = int.MaxValue;
        var current = _head;
        while (current != null)
        {
            if (current.Value < min)
            {
                min = current.Value;
            }
            current = current.Next;
        }

        return min;
    }


    // public IEnumerator<int> GetEnumerator()
    // {
    //     var current = _head;
    //     while (current != null)
    //     {
    //         yield return current.Value;
    //         current = current.Next;
    //     }
    // }

    public IEnumerable<int> GetEnumerable()
    {
        return (IEnumerable<int>)GetEnumerator();
    }

    public IEnumerator<int> GetEnumerator()
    {
        return new LinkedListIterator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class LinkedListIterator : IEnumerator<int>
    {
        private readonly LinkedList _linkedList;

        private LinkedList.Node CurrentNode { get; set; }

        public LinkedListIterator(LinkedList linkedList)
        {
            _linkedList = linkedList;
        }

        public bool MoveNext()
        {
            if (CurrentNode == null)
            {
                CurrentNode = _linkedList._head;
                return CurrentNode != null;
            }

            CurrentNode = CurrentNode.Next;

            return CurrentNode != null;
        }

        public void Reset()
        {
            CurrentNode = _linkedList._head;
        }

        public int Current => CurrentNode.Value;

        object IEnumerator.Current => Current;

        public void Dispose()
        {

        }
    }
}

