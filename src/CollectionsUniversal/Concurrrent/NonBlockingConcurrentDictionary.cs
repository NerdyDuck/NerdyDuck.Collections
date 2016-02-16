#region Copyright
/*******************************************************************************
 * <copyright file="NonBlockingConcurrentDictionary.cs" owner="Daniel Kopp">
 * Copyright 2015-2016 Daniel Kopp
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * </copyright>
 * <author name="Daniel Kopp" email="dak@nerdyduck.de" />
 * <assembly name="NerdyDuck.Collections">
 * Specialized collections for .NET
 * </assembly>
 * <file name="NonBlockingConcurrentDictionary.cs" date="2016-02-15">
 * Represents a thread-safe collection of keys and values.
 * </file>
 ******************************************************************************/
#endregion

using NerdyDuck.CodedExceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NerdyDuck.Collections.Concurrent
{
	/// <summary>
	/// Represents a thread-safe collection of keys and values.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	/// <remarks><para>The <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> generic class provides a mapping from a set of keys to a set of values.
	/// Each addition to the dictionary consists of a value and its associated key.
	/// Retrieving a value by using its key is very fast, close to O(1), because the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> class is implemented as a hash table.</para>
	/// <para>This dictionary is thread-safe, however the methods used to access the dictionary and the enumerator do not take synchronization locks.
	/// Instead, whenever the dictionary is modified, the dictionary is copied, the copy is modified, and a reference is set to the copy of the dictionary.
	/// Due to this mechanic, the dictionary is best suited where modifications happen infrequently, while the dictionary is read very frequently.
	/// Methods like <see cref="o:Add"/>, <see cref="o:Remove"/>, and <see cref="Clear"/> modify the elements in the dictionary, and are synchronized with a simple lock statement.</para></remarks>
	[System.Diagnostics.DebuggerDisplay("Count = {Count}")]
	public sealed class NonBlockingConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>,
		IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, IReadOnlyDictionary<TKey, TValue>,
		IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		#region Private fields
		private Dictionary<TKey, TValue> InternalDictionary;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> class that is empty and has the default initial capacity.
		/// </summary>
		public NonBlockingConcurrentDictionary()
		{
			InternalDictionary = new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> can initially store.</param>
		public NonBlockingConcurrentDictionary(int capacity)
		{
			InternalDictionary = new Dictionary<TKey, TValue>(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> class that contains elements copied from the specified <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		public NonBlockingConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
		{
			InternalDictionary = new Dictionary<TKey, TValue>(dictionary);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Gets a collection containing the keys in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.KeyCollection"/> containing the keys in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get { return InternalDictionary.Keys; }
		}

		/// <summary>
		/// Gets a collection containing the values in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.ValueCollection"/> containing the values in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get { return InternalDictionary.Values; }
		}
		#endregion

		#region IDictionary<TKey, TValue> implementation
		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be <see langword="null"/> for reference types.</param>
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x29), nameof(key));
			}
			lock (this)
			{
				Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
				NewDictionary.Add(key, value);
				InternalDictionary = NewDictionary;
			}
		}

		/// <summary>
		/// Determines whether the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		/// <returns><see langword="true"/> if the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool ContainsKey(TKey key)
		{
			return InternalDictionary.ContainsKey(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.KeyCollection"/> containing the keys in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return InternalDictionary.Keys; }
		}

		/// <summary>
		/// Removes the value with the specified key from the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns><see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="key"/> is not found in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x2a), nameof(key));
			}
			lock (this)
			{
				Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
				bool ReturnValue = NewDictionary.Remove(key);
				InternalDictionary = NewDictionary;

				return ReturnValue;
			}
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the <paramref name="key"/> is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return InternalDictionary.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets a collection containing the values in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.ValueCollection"/> containing the values in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return InternalDictionary.Values; }
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</returns>
		public TValue this[TKey key]
		{
			get { return InternalDictionary[key]; }
			set
			{
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(0x2b), nameof(key));
				}
				lock (this)
				{
					Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
					NewDictionary[key] = value;
					InternalDictionary = NewDictionary;
				}
			}
		}
		#endregion

		#region ICollection<KeyValuePair<TKey, TValue>>
		/// <summary>
		/// Adds the specified value to the <see cref="ICollection{T}"/> with the specified key.
		/// </summary>
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure representing the key and value to add to the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			if (item.Key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x2c), "item.Key");
			}
			lock (this)
			{
				if (InternalDictionary.ContainsKey(item.Key))
				{
					throw new CodedArgumentException(Errors.CreateHResult(0x2d), Properties.Resources.Global_Add_Duplicate, nameof(item));
				}
				Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
				NewDictionary.Add(item.Key, item.Value);
				InternalDictionary = NewDictionary;
			}
		}

		/// <summary>
		/// Determines whether the <see cref="ICollection{T}"/> contains a specific key and value.
		/// </summary>
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure to locate in the <see cref="ICollection{T}"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="ICollection{T}"/>; otherwise, <see langword="false"/>.</returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).Contains(item);
		}

		/// <summary>
		/// Copies the elements of the <see cref="ICollection{T}"/> to an array of type <see cref="KeyValuePair{TKey, TValue}"/>, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional array of type <see cref="KeyValuePair{TKey, TValue}"/> that is the destination of the <see cref="KeyValuePair{TKey, TValue}"/> elements copied from the <see cref="ICollection{T}"/>. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets a value indicating whether the dictionary is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="ICollection{T}"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).IsReadOnly; }
		}

		/// <summary>
		/// Removes a key and value from the dictionary.
		/// </summary>
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure representing the key and value to remove from the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		/// <returns><see langword="true"/> if the key and value represented by <paramref name="item"/> is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="item"/> is not found in the <see cref="ICollection{T}"/>.</returns>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			lock (this)
			{
				Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
				bool ReturnValue = ((ICollection<KeyValuePair<TKey, TValue>>)NewDictionary).Remove(item);
				if (ReturnValue)
				{
					InternalDictionary = NewDictionary;
				}

				return ReturnValue;
			}
		}
		#endregion

		#region IEnumerable<KeyValuePair<TKey, TValue>>
		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator"/> structure for the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return InternalDictionary.GetEnumerator();
		}
		#endregion

		#region IDictionary implementation
		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The object to use as the key.</param>
		/// <param name="value">The object to use as the value.</param>
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x2e), nameof(key));
			}
			if (value == null && default(TValue) != null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x2f), nameof(value));
			}
			try
			{
				TKey Key = (TKey)key;
				try
				{
					TValue Value = (TValue)value;
					lock (this)
					{
						if (InternalDictionary.ContainsKey(Key))
						{
							throw new CodedArgumentException(Errors.CreateHResult(0x30), Properties.Resources.Global_Add_Duplicate, nameof(key));
						}
						Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
						NewDictionary.Add(Key, Value);
						InternalDictionary = NewDictionary;
					}
				}
				catch (InvalidCastException)
				{
					throw new CodedArgumentException(Errors.CreateHResult(0x32), string.Format(Properties.Resources.Global_AddDictionary_InvalidCast, value, typeof(TValue)), nameof(value));
				}
			}
			catch (InvalidCastException)
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x31), string.Format(Properties.Resources.Global_AddDictionary_InvalidCastKey, key, typeof(TKey)), nameof(key));
			}
		}

		/// <summary>
		/// Removes all keys and values from the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		public void Clear()
		{
			lock (this)
			{
				InternalDictionary = new Dictionary<TKey, TValue>();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="IDictionary"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="IDictionary"/>.</param>
		/// <returns><see langword="true"/> if the <see cref="IDictionary"/> contains an element with the specified key; otherwise, <see langword="false"/>. </returns>
		bool IDictionary.Contains(object key)
		{
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x33), "key");
			}

			return ((IDictionary)InternalDictionary).Contains(key);
		}

		/// <summary>
		/// Returns an <see cref="IDictionaryEnumerator"/> for the <see cref="IDictionary"/>.
		/// </summary>
		/// <returns>An <see cref="IDictionaryEnumerator"/> for the <see cref="IDictionary"/>.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)InternalDictionary).GetEnumerator();
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IDictionary"/> has a fixed size.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IDictionary"/> has a fixed size; otherwise, <see langword="false"/>. In the default implementation of <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool IDictionary.IsFixedSize
		{
			get { return ((IDictionary)InternalDictionary).IsFixedSize; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IDictionary"/> is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IDictionary"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool IDictionary.IsReadOnly
		{
			get { return ((IDictionary)InternalDictionary).IsReadOnly; }
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> containing the keys of the <see cref="IDictionary"/>.
		/// </summary>
		/// <value>An <see cref="ICollection"/> containing the keys of the <see cref="IDictionary"/>.</value>
		ICollection IDictionary.Keys
		{
			get { return ((IDictionary)InternalDictionary).Keys; }
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="IDictionary"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x34), nameof(key));
			}
			lock (this)
			{
				Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
				((IDictionary)NewDictionary).Remove(key);
				InternalDictionary = NewDictionary;
			}
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> containing the values of the <see cref="IDictionary"/>.
		/// </summary>
		/// <value>An <see cref="ICollection"/> containing the values of the <see cref="IDictionary"/>.</value>
		ICollection IDictionary.Values
		{
			get { return ((IDictionary)InternalDictionary).Values; }
		}

		/// <summary>
		/// Gets or sets the value with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key, or <see langword="null"/> if <paramref name="key"/> is not in the dictionary or <paramref name="key"/> is of a type that is not assignable to the key type TKey of the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		object IDictionary.this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(0x35), nameof(key));
				}
				return ((IDictionary)InternalDictionary)[key];
			}
			set
			{
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(0x36), nameof(key));
				}
				if (value == null && default(TValue) != null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(0x37), nameof(value));
				}
				try
				{
					TKey Key = (TKey)key;
					try
					{
						TValue Value = (TValue)value;
						lock (this)
						{
							Dictionary<TKey, TValue> NewDictionary = new Dictionary<TKey, TValue>(InternalDictionary);
							NewDictionary[Key] = Value;
							InternalDictionary = NewDictionary;
						}
					}
					catch (InvalidCastException)
					{
						throw new CodedArgumentException(Errors.CreateHResult(0x39), string.Format(Properties.Resources.Global_AddDictionary_InvalidCast, value, typeof(TValue)), nameof(value));
					}
				}
				catch (InvalidCastException)
				{
					throw new CodedArgumentException(Errors.CreateHResult(0x38), string.Format(Properties.Resources.Global_AddDictionary_InvalidCastKey, key, typeof(TKey)), nameof(key));
				}
			}
		}
		#endregion

		#region ICollection implementation
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to an array, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="ICollection"/>. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)InternalDictionary).CopyTo(array, index);
		}

		/// <summary>
		/// Gets the number of key/value pairs contained in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>The number of key/value pairs contained in the <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public int Count
		{
			get { return InternalDictionary.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value><see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread safe); otherwise, <see langword="false"/>. In the default implementation of <see cref="NonBlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="true"/>. </value>
		bool ICollection.IsSynchronized
		{
			get { return true; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
		/// </summary>
		/// <value>An object that can be used to synchronize access to the <see cref="ICollection"/>. </value>
		object ICollection.SyncRoot
		{
			get { return this; }
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)InternalDictionary).GetEnumerator();
		}
		#endregion

		#region IReadOnlyDictionary<TKey, TValue> implementation
		/// <summary>
		/// Gets a collection containing the keys of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A collection containing the keys of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</value>
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get { return InternalDictionary.Keys; }
		}

		/// <summary>
		/// Gets a collection containing the values of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A collection containing the values of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</value>
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get { return InternalDictionary.Values; }
		}
		#endregion
	}
}
