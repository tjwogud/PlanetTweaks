using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlanetTweaks.Utils
{
    public class IndexedDictionary<K, V> : IDictionary<K, V>
    {
        public V this[K key]
        {
            get
            {
                int index = keys.IndexOf(key);
                return index == -1 ? default : values[index];
            }
            set
            {
                int index = keys.IndexOf(key);
                if (index != -1)
                    values[index] = value;
            }
        }
        public ICollection<K> Keys => keys;
        public ICollection<V> Values => values;

        private List<K> keys = new List<K>();
        private List<V> values = new List<V>();

        public int Count => keys.Count;

        public bool IsReadOnly => false;

        public void Add(K key, V value)
        {
            if (ContainsKey(key))
                throw new ArgumentException("item already exists!");
            keys.Add(key);
            values.Add(value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public void Insert(int index, K key, V value)
        {
            if (ContainsKey(key))
                throw new ArgumentException("item already exists!");
            keys.Insert(index, key);
            values.Insert(index, value);
        }

        public void Insert(int index, KeyValuePair<K,V> item)
        {
            Insert(index, item.Key, item.Value);
        }

        public void Replace(int index, K key, V value)
        {
            int i = keys.IndexOf(key);
            if (i != -1 && i != index)
                throw new ArgumentException("item already exists!");
            keys[index] = key;
            values[index] = value;
        }

        public void Replace(int index, KeyValuePair<K, V> item)
        {
            Replace(index, item.Key, item.Value);
        }

        public void Replace(K prevKey, K key, V value)
        {
            int index = keys.IndexOf(prevKey);
            if (index != -1)
                Replace(index, key, value);
        }

        public void Replace(K prevKey, KeyValuePair<K, V> item)
        {
            Replace(prevKey, item.Key, item.Value);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return this[item.Key]?.Equals(item.Value) ?? false;
        }

        public bool ContainsKey(K key)
        {
            return keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            int i = 0;
            return keys.Select(k => new KeyValuePair<K, V>(k, values[i++])).GetEnumerator();
        }

        public bool Remove(K key)
        {
            int index = keys.IndexOf(key);
            if (index == -1)
                return false;
            keys.RemoveAt(index);
            values.RemoveAt(index);
            return true;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(K key, out V value)
        {
            int index = keys.IndexOf(key);
            if (index == -1)
            {
                value = default;
                return false;
            }
            value = values[index];
            return true;
        }

        public KeyValuePair<K, V> ElementAt(int index)
        {
            return new KeyValuePair<K, V>(keys[index], values[index]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
