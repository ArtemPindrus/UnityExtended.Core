using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace UnityExtended.Core.Types {
	public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
		private readonly Dictionary<TKey, TValue> dictionary = new();

		public event Action? CollectionChanged;

		// IDictionary implementation
		public TValue this[TKey key] {
			get => dictionary[key];
			set => dictionary[key] = value;
		}

		public ICollection<TKey> Keys => dictionary.Keys;

		public ICollection<TValue> Values => dictionary.Values;

		public int Count => dictionary.Count;

		public bool IsReadOnly => true;

		public void Add(TKey key, TValue value) {
			dictionary.Add(key, value);

			CollectionChanged?.Invoke();
		}

		public void Add(KeyValuePair<TKey, TValue> item) {
			dictionary.Add(item.Key, item.Value);

			CollectionChanged?.Invoke();
		}

		public bool Remove(TKey key) {
			if (dictionary.Remove(key)) {
				CollectionChanged?.Invoke();

				return true;
			} else return false;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item) {
			if (Contains(item) && dictionary.Remove(item.Key)) {
				CollectionChanged?.Invoke();

				return true;
			} else return false;
		}

		public void Clear() {
			dictionary.Clear();

			CollectionChanged?.Invoke();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.TryGetValue(item.Key, out TValue value) && value != null && value.Equals(item.Value);

		public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);



		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();
	}
}
