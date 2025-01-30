public abstract class Heap<T> where T : IComparable<T>
{
    protected readonly List<T> _elements;

    public Heap()
    {
        _elements = new List<T>();
    }

    public int Count => _elements.Count;

    public void Insert(T item)
    {
        _elements.Add(item);
        HeapifyUp(_elements.Count - 1);
    }

    public T RemoveTop()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Heap is empty.");

        T top = _elements[0];
        _elements[0] = _elements[_elements.Count - 1];
        _elements.RemoveAt(_elements.Count - 1);
        HeapifyDown(0);
        return top;
    }

    public T Peek()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Heap is empty.");

        return _elements[0];
    }

    protected void Swap(int indexA, int indexB)
    {
        T temp = _elements[indexA];
        _elements[indexA] = _elements[indexB];
        _elements[indexB] = temp;
    }

    protected abstract bool Compare(T a, T b);

    private void HeapifyUp(int index)
    {
        int current = index;
        while (current > 0)
        {
            int parent = (current - 1) / 2;
            if (Compare(_elements[parent], _elements[current]))
                break;
            Swap(parent, current);
            current = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int current = index;
        while (true)
        {
            int leftChild = 2 * current + 1;
            int rightChild = 2 * current + 2;
            int dominantChild = leftChild;

            if (leftChild >= _elements.Count)
                break;

            if (rightChild < _elements.Count && Compare(_elements[rightChild], _elements[leftChild]))
                dominantChild = rightChild;

            if (Compare(_elements[current], _elements[dominantChild]))
                break;

            Swap(current, dominantChild);
            current = dominantChild;
        }
    }
}

public class MaxHeap<T> : Heap<T> where T : IComparable<T>
{
    protected override bool Compare(T a, T b)
    {
        return a.CompareTo(b) >= 0;
    }
}

public class MinHeap<T> : Heap<T> where T : IComparable<T>
{
    protected override bool Compare(T a, T b)
    {
        return a.CompareTo(b) <= 0;
    }
}

class Program
{
    static void Main()
    {
        // Max Heap Example
        var maxHeap = new MaxHeap<int>();
        
        maxHeap.Insert(3);
        maxHeap.Insert(1);
        maxHeap.Insert(4);
        maxHeap.Insert(2);
        maxHeap.Insert(5);

        Console.WriteLine("Max Heap Elements:");
        while (maxHeap.Count > 0)
            Console.WriteLine(maxHeap.RemoveTop());

        // Min Heap Example
        var minHeap = new MinHeap<int>();

        minHeap.Insert(3);
        minHeap.Insert(1);
        minHeap.Insert(4);
        minHeap.Insert(2);
        minHeap.Insert(5);

        minHeap.RemoveTop();

        Console.WriteLine("\nMin Heap Elements:");
        while (minHeap.Count > 0)
            Console.WriteLine(minHeap.RemoveTop());
    }
}