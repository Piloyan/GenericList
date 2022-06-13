using System.Collections;

namespace GenericListConsoleApp
{
    public class GenericList<T> : IEnumerable<T>
    {
        private const int _defaultCapacity = 4;
        private T[] _items;
        static readonly T[] _emptyArray = new T[0];
        private int _size;

        public GenericList()
        {
            _items = _emptyArray;
        }

        public int Count
        {
            get { return _size; }
        }

        public int Capacity
        {
            get { return _items.Length; }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }

                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public void Add(T item)
        {
            if (_size == _items.Length)
                EnsureCapacity(_size + 1);
            _items[_size++] = item;
        }

        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_size)
            {
                throw new IndexOutOfRangeException();
            }

            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }

            _items[index] = item;
            _size++;
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > 0X7FEFFFFF) newCapacity = 0X7FEFFFFF;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Clear()
        {
            _items = _emptyArray;
        }

        public override string ToString()
        {
            return $"Count = {Count}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new GenericListEnumerator<T>(_items, _size);
        }

        public bool Contains(T element)
        {
            foreach (var item in _items)
            {
                if (object.Equals(item, element)) return true;
            }

            return false;
        }

        public void CopyTo(T[] targetArray, int insertionIndex)
        {
            if (insertionIndex >= targetArray.Length || insertionIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (insertionIndex + _items.Length > targetArray.Length)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < _items.Length; i++)
            {
                targetArray[insertionIndex + i] = _items[i];
            }
        }

        public void CopyTo(int insertionIndex, T[] targetArray, int rangeStartIndex, int rangeEndIndex)
        {
            if (rangeStartIndex > rangeEndIndex)
                throw new ArgumentOutOfRangeException();
            if (rangeStartIndex < 0 || rangeStartIndex > _items.Length - 1 || rangeEndIndex < 0 || rangeEndIndex > _items.Length - 1)
                throw new ArgumentOutOfRangeException();
            if (insertionIndex < 0 || insertionIndex > targetArray.Length)
                throw new ArgumentOutOfRangeException();
            if (rangeEndIndex - rangeStartIndex > targetArray.Length)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < rangeStartIndex - rangeEndIndex; i++)
            {
                targetArray[insertionIndex + i] = _items[rangeStartIndex + i];
            }
        }

        public void CopyTo(T[] targetArray)
        {
            if (targetArray.Length < _items.Length)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < _items.Length; i++)
            {
                targetArray[i] = _items[i];
            }
        }

        public int IndexOf(T element)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (object.Equals(element, _items[i]))
                    return i;
            }

            return -1;
        }

        public void InsertRange(int insertionIndex, T[] arrayToInsert)
        {
            if (0 < insertionIndex || insertionIndex > _items.Length)
                throw new ArgumentOutOfRangeException();
            if (_items.Length + arrayToInsert.Length > _size)
            {
                EnsureCapacity(_items.Length + arrayToInsert.Length);
            }

            for (int i = 0; i < arrayToInsert.Length; i++)
            {
                _items[insertionIndex + arrayToInsert.Length + i] = _items[insertionIndex + i];
                _items[insertionIndex + i] = arrayToInsert[i];
            }

            _size += arrayToInsert.Length;
        }

        public int LastIndexOf(T element)
        {
            for (int i = _items.Length - 1; i >= 0; i--)
            {
                if (object.Equals(element, _items[i]))
                    return i;
            }

            return -1;
        }

        public bool Remove(T element)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (object.Equals(element, _items[i]))
                {
                    for (int j = i + 1; j < _items.Length - 1; j++)
                    {
                        _items[j - 1] = _items[j];
                    }

                    _items[_items.Length - 1] = default;
                    _size--;
                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (0 < index || index > _items.Length - 1)
                throw new IndexOutOfRangeException();

            for (int j = index + 1; j < _items.Length - 1; j++)
            {
                _items[j - 1] = _items[j];
            }

            _items[^1] = default;
            _size--;
        }

        public void RemoveRange(int index, int count)
        {
            if (0 < index || index > _items.Length - 1)
                throw new IndexOutOfRangeException();
            if (index + count > _items.Length)
                throw new IndexOutOfRangeException();

            for (int i = 0; i < _items.Length - index; i++)
            {
                _items[index + i] = _items[index + count + i];
                _items[index + count + i] = default;
            }

            _size -= count;
        }

        public void Reverse()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                T tmp = _items[i];
                _items[i] = _items[_items.Length - 1 - i];
                _items[_items.Length - 1 - i] = tmp;
            }
        }

        public T[] ToArray()
        {
            return _items;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
