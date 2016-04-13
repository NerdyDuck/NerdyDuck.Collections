#region Copyright
/*******************************************************************************
 * <copyright file="BlockingConcurrentDictionary.cs" owner="Daniel Kopp">
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
 * <file name="BlockingConcurrentDictionary.cs" date="2016-02-15">
 * Represents a thread-safe collection of keys and values.
 * </file>
 ******************************************************************************/
#endregion

using NerdyDuck.CodedExceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NerdyDuck.Collections.Concurrent
{
	/// <summary>
	/// Represents a thread-safe collection of keys and values.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	/// <remarks><para>The <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> generic class provides a mapping from a set of keys to a set of values.
	/// Each addition to the dictionary consists of a value and its associated key.
	/// Retrieving a value by using its key is very fast, close to O(1), because the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> class is implemented as a hash table.</para>
	/// <para>This dictionary is thread-safe, using a <see cref="ReaderWriterLockSlim"/> to synchronize all accesses. Enumerating the dictionary is also synchronized.
	/// Due to this mechanic, the dictionary is suitable for scenarios where multiple threads read and write the list frequently.
	/// Methods like <see cref="o:Add"/>, <see cref="o:Remove"/>, and <see cref="Clear"/> modify the elements in the collection.</para></remarks>
	[System.Diagnostics.DebuggerDisplay("Count = {Count}")]
	public sealed partial class BlockingConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>,
		IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, IReadOnlyDictionary<TKey, TValue>,
		IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IDisposable
	{
		#region Private fields
		private Dictionary<TKey, TValue> InternalDictionary;
		private ReaderWriterLockSlim Lock;
		private bool IsDisposed;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> class that is empty and has the default initial capacity.
		/// </summary>
		public BlockingConcurrentDictionary()
		{
			InternalDictionary = new Dictionary<TKey, TValue>();
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			IsDisposed = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> can initially store.</param>
		public BlockingConcurrentDictionary(int capacity)
		{
			InternalDictionary = new Dictionary<TKey, TValue>(capacity);
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			IsDisposed = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> class that contains elements copied from the specified <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		public BlockingConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
		{
			InternalDictionary = new Dictionary<TKey, TValue>(dictionary);
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			IsDisposed = false;
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Destructor.
		/// </summary>
		~BlockingConcurrentDictionary()
		{
			Dispose(false);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Gets a collection containing the keys in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.KeyCollection"/> containing the keys in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Keys;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets a collection containing the values in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.ValueCollection"/> containing the values in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Values;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
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
			AssertDisposed();
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_KeyNull), nameof(key));
			}

			try
			{
				Lock.EnterWriteLock();
				InternalDictionary.Add(key, value);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		/// <returns><see langword="true"/> if the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool ContainsKey(TKey key)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return InternalDictionary.ContainsKey(key);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Gets a collection containing the keys in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.KeyCollection"/> containing the keys in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Keys;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Removes the value with the specified key from the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns><see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="key"/> is not found in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		public bool Remove(TKey key)
		{
			AssertDisposed();
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Remove_KeyNull), nameof(key));
			}
			try
			{
				Lock.EnterWriteLock();
				return InternalDictionary.Remove(key);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the <paramref name="key"/> is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return InternalDictionary.TryGetValue(key, out value);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Gets a collection containing the values in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey, TValue}.ValueCollection"/> containing the values in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Values;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</returns>
		public TValue this[TKey key]
		{
			get
			{
				AssertDisposed();
				return InternalDictionary[key];
			}
			set
			{
				AssertDisposed();
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_Set_KeyNull), nameof(key));
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
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure representing the key and value to add to the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			AssertDisposed();
			if (item.Key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_ItemKeyNull), "item.Key");
			}
			try
			{
				Lock.EnterWriteLock();
				if (InternalDictionary.ContainsKey(item.Key))
				{
					throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_KeyExists), Properties.Resources.Global_Add_Duplicate, nameof(item));
				}
				InternalDictionary.Add(item.Key, item.Value);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="ICollection{T}"/> contains a specific key and value.
		/// </summary>
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure to locate in the <see cref="ICollection{T}"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="ICollection{T}"/>; otherwise, <see langword="false"/>.</returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return ((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).Contains(item);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Copies the elements of the <see cref="ICollection{T}"/> to an array of type <see cref="KeyValuePair{TKey, TValue}"/>, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional array of type <see cref="KeyValuePair{TKey, TValue}"/> that is the destination of the <see cref="KeyValuePair{TKey, TValue}"/> elements copied from the <see cref="ICollection{T}"/>. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).CopyTo(array, arrayIndex);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Gets a value indicating whether the dictionary is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="ICollection{T}"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				AssertDisposed();
				return ((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).IsReadOnly;
			}
		}

		/// <summary>
		/// Removes a key and value from the dictionary.
		/// </summary>
		/// <param name="item">The <see cref="KeyValuePair{TKey, TValue}"/> structure representing the key and value to remove from the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</param>
		/// <returns><see langword="true"/> if the key and value represented by <paramref name="item"/> is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="item"/> is not found in the <see cref="ICollection{T}"/>.</returns>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				return ((ICollection<KeyValuePair<TKey, TValue>>)InternalDictionary).Remove(item);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion

		#region IEnumerable<KeyValuePair<TKey, TValue>>
		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator"/> structure for the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			AssertDisposed();
			return new Enumerator<TKey, TValue>(InternalDictionary, Lock);
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
			AssertDisposed();
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_ObjectKeyNull), nameof(key));
			}
			if (value == null && default(TValue) != null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_ObjectValueNull), nameof(value));
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
							throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_ObjectKeyExists), Properties.Resources.Global_Add_Duplicate, nameof(key));
						}
						try
						{
							Lock.EnterWriteLock();
							InternalDictionary.Add(Key, Value);
						}
						finally
						{
							Lock.ExitWriteLock();
						}
					}
				}
				catch (InvalidCastException)
				{
					throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_ValueInvalidType), string.Format(Properties.Resources.Global_AddDictionary_InvalidCast, value, typeof(TValue)), nameof(value));
				}
			}
			catch (InvalidCastException)
			{
				throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Add_KeyInvalidType), string.Format(Properties.Resources.Global_AddDictionary_InvalidCastKey, key, typeof(TKey)), nameof(key));
			}
		}

		/// <summary>
		/// Removes all keys and values from the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		public void Clear()
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				InternalDictionary.Clear();
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="IDictionary"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="IDictionary"/>.</param>
		/// <returns><see langword="true"/> if the <see cref="IDictionary"/> contains an element with the specified key; otherwise, <see langword="false"/>. </returns>
		bool IDictionary.Contains(object key)
		{
			AssertDisposed();
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Contains_KeyNull), nameof(key));
			}

			try
			{
				Lock.EnterReadLock();
				return ((IDictionary)InternalDictionary).Contains(key);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Returns an <see cref="IDictionaryEnumerator"/> for the <see cref="IDictionary"/>.
		/// </summary>
		/// <returns>An <see cref="IDictionaryEnumerator"/> for the <see cref="IDictionary"/>.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			AssertDisposed();
			return new Enumerator<TKey, TValue>(InternalDictionary, Lock);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IDictionary"/> has a fixed size.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IDictionary"/> has a fixed size; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool IDictionary.IsFixedSize
		{
			get
			{
				AssertDisposed();
				return ((IDictionary)InternalDictionary).IsFixedSize;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IDictionary"/> is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IDictionary"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="false"/>.</value>
		bool IDictionary.IsReadOnly
		{
			get
			{
				AssertDisposed();
				return ((IDictionary)InternalDictionary).IsReadOnly;
			}
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> containing the keys of the <see cref="IDictionary"/>.
		/// </summary>
		/// <value>An <see cref="ICollection"/> containing the keys of the <see cref="IDictionary"/>.</value>
		ICollection IDictionary.Keys
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return ((IDictionary)InternalDictionary).Keys;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="IDictionary"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		void IDictionary.Remove(object key)
		{
			AssertDisposed();
			if (key == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Remove_ObjectKeyNull), nameof(key));
			}
			try
			{
				Lock.EnterWriteLock();
				((IDictionary)InternalDictionary).Remove(key);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> containing the values of the <see cref="IDictionary"/>.
		/// </summary>
		/// <value>An <see cref="ICollection"/> containing the values of the <see cref="IDictionary"/>.</value>
		ICollection IDictionary.Values
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return ((IDictionary)InternalDictionary).Values;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets or sets the value with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key, or <see langword="null"/> if <paramref name="key"/> is not in the dictionary or <paramref name="key"/> is of a type that is not assignable to the key type TKey of the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</returns>
		object IDictionary.this[object key]
		{
			get
			{
				AssertDisposed();
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_get_KeyNull), nameof(key));
				}
				try
				{
					Lock.EnterReadLock();
					return ((IDictionary)InternalDictionary)[key];
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
			set
			{
				AssertDisposed();
				if (key == null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_set_KeyNull), nameof(key));
				}
				if (value == null && default(TValue) != null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_set_ValueNull), nameof(value));
				}
				try
				{
					TKey Key = (TKey)key;
					try
					{
						TValue Value = (TValue)value;
						try
						{
							Lock.EnterWriteLock();
							InternalDictionary[Key] = Value;
						}
						finally
						{
							Lock.ExitWriteLock();
						}
					}
					catch (InvalidCastException)
					{
						throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_set_ValueInvalidType), string.Format(Properties.Resources.Global_AddDictionary_InvalidCast, value, typeof(TValue)), nameof(value));
					}
				}
				catch (InvalidCastException)
				{
					throw new CodedArgumentException(Errors.CreateHResult(ErrorCodes.BlockingConcurrentDictionary_Item_set_KeyInvalidType), string.Format(Properties.Resources.Global_AddDictionary_InvalidCastKey, key, typeof(TKey)), nameof(key));
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
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				((ICollection)InternalDictionary).CopyTo(array, index);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Gets the number of key/value pairs contained in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>The number of key/value pairs contained in the <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>.</value>
		public int Count
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Count;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value><see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread safe); otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentDictionary{TKey, TValue}"/>, this property always returns <see langword="true"/>. </value>
		bool ICollection.IsSynchronized
		{
			get { return true; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
		/// </summary>
		/// <value>An object that can be used to synchronize access to the <see cref="ICollection"/>.</value>
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
			AssertDisposed();
			return new Enumerator<TKey, TValue>(InternalDictionary, Lock);
		}
		#endregion

		#region IReadOnlyDictionary<TKey, TValue> implementation
		/// <summary>
		/// Gets a collection containing the keys of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A collection containing the keys of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</value>
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Keys;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets a collection containing the values of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <value>A collection containing the values of the <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</value>
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalDictionary.Values;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}
		#endregion

		#region IDisposable implementation
		/// <summary>
		/// Releases allocated resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases allocated resources.
		/// </summary>
		/// <param name="disposing">A value indicating if the method was called by user code. If <see langword="false"/>, the method was called by the runtime in the finalizer.</param>
		/// <remarks>If <paramref name="disposing"/> is <see langword="false"/>, no other objects should be referenced.</remarks>
		private void Dispose(bool disposing)
		{
			if (IsDisposed)
				return;

			IsDisposed = true;
			if (disposing)
			{
				if (Lock != null)
				{
					Lock.Dispose();
				}
			}
			Lock = null;
			InternalDictionary = null;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Checks if the objects was previously exposed.
		/// </summary>
		private void AssertDisposed()
		{
			if (IsDisposed)
				throw new ObjectDisposedException(this.ToString());
		}
		#endregion
	}
}
