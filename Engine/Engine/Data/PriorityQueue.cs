using System;

namespace Engine.Data
{
  public class PriorityQueue<T>
  {
    private int _count;
    private Tuple<int, T>[] _queueArray;
    private HashTable<T, int> _indexingReference;

    public int Count { get { return _count; } }

    public int TopPriority
    {
      get
      {
        if (_count > 0)
          return _queueArray[1].Left;
        else throw new InvalidOperationException();
      }
    }

    public PriorityQueue(int max)
    {
      _indexingReference = new HashTable<T, int>();
      _queueArray = new Tuple<int, T>[max + 1];
      _queueArray[0] = new Tuple<int, T>(int.MaxValue, default(T));
      _count = 0;
    }

    public void Add(T addition, int priority)
    {
      if (_count < _queueArray.Length - 1)
      {
        _queueArray[_count + 1] = new Tuple<int, T>(priority, addition);
        _indexingReference.Add(addition, _count + 1);
        ShiftPositive(_count + 1);
        _count++;
      }
      else throw new InvalidOperationException();
    }

    public T RemoveTop()
    {
      if (_count > 0)
      {
        T removal = _queueArray[1].Right;
        ArraySwap(1, _count);
        _count--;
        _indexingReference.Remove(removal);
        ShiftNegative(1);
        return removal;
      }
      else throw new InvalidOperationException();
    }

    public void IncreasePriority(T item)
    {
      int index = _indexingReference[item];
      _queueArray[index] = new Tuple<int, T>(_queueArray[index].Left + 1, item);
      ShiftPositive(index);
    }

    public void DecreasePriority(T item)
    {
      int index = _indexingReference[item];
      _queueArray[index] = new Tuple<int, T>(_queueArray[index].Left - 1, item);
      ShiftNegative(index);
    }

    private void ShiftPositive(int index)
    {
      while (_queueArray[index].Left > _queueArray[index / 2].Left)
      //NOTE: "index / 2" is the index of the parent of the item at location "index"
      {
        ArraySwap(index, index / 2);
        index = index / 2;
      }
    }

    private void ShiftNegative(int index)
    {
      while ((index * 2) <= _count)
      //NOTE: "index * 2" is the index of the leftchild of the item at location "index"
      {
        int index2 = index * 2;
        if (((index * 2) + 1) <= _count && _queueArray[(index * 2) + 1].Left < _queueArray[index].Left) index2++;
        //NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
        if (_queueArray[index].Left >= _queueArray[index2].Left) break;
        ArraySwap(index, index2);
        index = index2;
      }
    }

    private void ArraySwap(int indexOne, int indexTwo)
    {
      Tuple<int, T> swapStorage = _queueArray[indexTwo];
      _queueArray[indexTwo] = _queueArray[indexOne];
      _queueArray[indexOne] = swapStorage;
      _indexingReference[_queueArray[indexOne].Right] = indexOne;
      _indexingReference[_queueArray[indexTwo].Right] = indexTwo;
    }
  }

  class PriorityQueueException : Exception
  {
    public PriorityQueueException(string message) : base(message) { }
  }
}
