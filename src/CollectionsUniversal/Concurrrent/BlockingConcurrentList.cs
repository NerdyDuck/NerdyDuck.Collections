#region Copyright
/*******************************************************************************
 * <copyright file="BlockingConcurrentList.cs" owner="Daniel Kopp">
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
 * <file name="BlockingConcurrentList.cs" date="2016-02-15">
 * Represents a thread-safe, strongly typed list of objects that can be accessed
 * by index. Provides methods to search, sort, and manipulate lists.
 * </file>
 ******************************************************************************/
#endregion

using NerdyDuck.CodedExceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace NerdyDuck.Collections.Concurrent
{
	/// <summary>
	/// Represents a thread-safe, strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <remarks><para>The <see cref="BlockingConcurrentList{T}"/> class is the generic equivalent of the <see cref="ArrayList"/> class. It implements the <see cref="IList{T}"/> generic interface by using an array whose size is dynamically increased as required.</para>
	/// <para>This list is thread-safe, using a <see cref="ReaderWriterLockSlim"/> to synchronize all accesses. Enumerating the list is also synchronized.
	/// Due to this mechanic, the list is suitable for scenarios where multiple threads read and write the list frequently.
	/// Methods like <see cref="Add"/>, <see cref="Remove"/>, <see cref="o:Sort"/>, and <see cref="Clear"/> modify the elements in the collection.</para>
	/// <para>The <see cref="BlockingConcurrentList{T}"/> class provides the <see cref="Count"/> property for information about the list.
	/// It also provides the following methods: <see cref="Contains"/>, <see cref="GetEnumerator"/>, <see cref="IndexOf"/>.</para>
	/// <para>This class also provides the following methods to modify the list: <see cref="Add"/>, <see cref="Clear"/>, <see cref="Insert"/>, and <see cref="Remove"/>.
	/// The <see cref="CopyTo"/> method copies a part of the list to an array. The <see cref="RemoveAt"/> method deletes the list member at a specified index number.</para>
	/// </remarks>
	[System.Diagnostics.DebuggerDisplay("Count = {Count}")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "It implements IList<T>")]
	public sealed partial class BlockingConcurrentList<T> : IList<T>, ICollection<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IDisposable
	{
		#region Private fields
		private List<T> InternalList;
		private ReaderWriterLockSlim Lock;
		private TypeInfo ItemTypeInfo;
		private bool IsDisposed;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentList{T}"/> class that is empty and has the default initial capacity.
		/// </summary>
		public BlockingConcurrentList()
		{
			InternalList = new List<T>();
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			ItemTypeInfo = typeof(T).GetTypeInfo();
			IsDisposed = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentList{T}"/> class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new list can initially store.</param>
		public BlockingConcurrentList(int capacity)
		{
			InternalList = new List<T>(capacity);
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			ItemTypeInfo = typeof(T).GetTypeInfo();
			IsDisposed = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockingConcurrentList{T}"/> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		public BlockingConcurrentList(IList<T> collection)
		{
			InternalList = new List<T>(collection);
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			ItemTypeInfo = typeof(T).GetTypeInfo();
			IsDisposed = false;
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Destructor.
		/// </summary>
		~BlockingConcurrentList()
		{
			Dispose(false);
		}
		#endregion

		#region Public methods
		#region AddRange
		/// <summary>
		/// Adds the elements of the specified collection to the end of the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the <see cref="BlockingConcurrentList{T}"/>. The collection itself cannot be <see langword="null"/>, but it can contain elements that are <see langword="null"/>, if type <typeparamref name="T"/> is a reference type.</param>
		public void AddRange(IEnumerable<T> collection)
		{
			AssertDisposed();
			if (collection == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x12), nameof(collection));
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.AddRange(collection);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion

		#region InsertRange
		/// <summary>
		/// Inserts the elements of a collection into the <see cref="BlockingConcurrentList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted.</param>
		/// <param name="collection">The collection whose elements should be inserted into the <see cref="BlockingConcurrentList{T}"/>. The collection itself cannot be <see langword="null"/>, but it can contain elements that are <see langword="null"/>, if type <typeparamref name="T"/> is a reference type.</param>
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			AssertDisposed();
			if (collection == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x13), nameof(collection));
			}
			if (index < 0 || index >= InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x14), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.InsertRange(index, collection);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion

		#region RemoveRange
		/// <summary>
		/// Removes a range of elements from the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public void RemoveRange(int index, int count)
		{
			AssertDisposed();
			if (index < 0 || index >= InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x15), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
			}
			if (count < 0 || count > InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x16), nameof(count), index, Properties.Resources.Global_CountOutOfRange);
			}
			if (index + count > InternalList.Count)
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x17), Properties.Resources.Global_Range, nameof(index));
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.RemoveRange(index, count);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion

		#region Sort overloads
		/// <summary>
		/// Sorts the elements in the entire <see cref="BlockingConcurrentList{T}"/> using the default comparer.
		/// </summary>
		public void Sort()
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				InternalList.Sort();
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Sorts the elements in the entire <see cref="BlockingConcurrentList{T}"/> using the specified <see cref="System.Comparison{T}"/>.
		/// </summary>
		/// <param name="comparison">The <see cref="System.Comparison{T}"/> to use when comparing elements.</param>
		public void Sort(Comparison<T> comparison)
		{
			AssertDisposed();
			if (comparison == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x18), nameof(comparison));
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.Sort(comparison);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Sorts the elements in the entire <see cref="BlockingConcurrentList{T}"/> using the specified comparer.
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
		public void Sort(IComparer<T> comparer)
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				InternalList.Sort(comparer);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Sorts the elements in a range of elements in <see cref="BlockingConcurrentList{T}"/> using the specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			AssertDisposed();
			if (index < 0 || index >= InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x19), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
			}
			if (count < 0 || count > InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x1a), nameof(count), index, Properties.Resources.Global_CountOutOfRange);
			}
			if (index + count > InternalList.Count)
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x1b), Properties.Resources.Global_Range, nameof(index));
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.Sort(index, count, comparer);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion
		#endregion

		#region IList<T> implementation
		/// <summary>
		/// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="BlockingConcurrentList{T}"/>. The value can be <see langword="null"/> for reference types.</param>
		/// <returns>The zero-based index of the first occurrence of <paramref name="item"/> within the entire <see cref="BlockingConcurrentList{T}"/>, if found; otherwise, –1.</returns>
		public int IndexOf(T item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return InternalList.IndexOf(item);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Inserts an element into the <see cref="BlockingConcurrentList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <see langword="null"/> for reference types.</param>
		public void Insert(int index, T item)
		{
			AssertDisposed();
			if (index < 0 || index >= InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x1c), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.Insert(index, item);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Removes the element at the specified index of the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			AssertDisposed();
			if (index < 0 || index >= InternalList.Count)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x1d), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
			}
			try
			{
				Lock.EnterWriteLock();
				InternalList.RemoveAt(index);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[int index]
		{
			get
			{
				AssertDisposed();
				if (index < 0 || index >= Count)
				{
					throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x1e), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
				}
				try
				{
					Lock.EnterReadLock();
					return InternalList[index];
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
			set
			{
				AssertDisposed();
				if (index < 0 || index >= Count)
				{
					throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x1f), nameof(index), index, Properties.Resources.Global_IndexOutOfRange);
				}
				try
				{
					Lock.EnterWriteLock();
					InternalList[index] = value;
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}
		#endregion

		#region ICollection<T> implementation
		/// <summary>
		/// Adds an object to the end of the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="item">The object to be added to the end of the <see cref="BlockingConcurrentList{T}"/>. The value can be <see langword="null"/> for reference types.</param>
		public void Add(T item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				InternalList.Add(item);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Removes all elements from the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		public void Clear()
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				InternalList.Clear();
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Determines whether an element is in the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="BlockingConcurrentList{T}"/>. The value can be <see langword="null"/> for reference types.</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="BlockingConcurrentList{T}"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(T item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return InternalList.Contains(item);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Copies the entire <see cref="BlockingConcurrentList{T}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="BlockingConcurrentList{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins. </param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			AssertDisposed();
			if (array == null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x20), nameof(array));
			}
			if (arrayIndex < 0)
			{
				throw new CodedArgumentOutOfRangeException(Errors.CreateHResult(0x21), nameof(arrayIndex), arrayIndex, Properties.Resources.Global_ArrayIndexOutOfRange);
			}
			if (arrayIndex + InternalList.Count > array.Length)
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x22), string.Format(Properties.Resources.Global_CopyTo_NotEnoughSpace, arrayIndex), nameof(arrayIndex));
			}
			try
			{
				Lock.EnterReadLock();
				InternalList.CopyTo(array, arrayIndex);
			}
			finally
			{
				Lock.ExitReadLock();
			}
			InternalList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <value>The number of elements contained in the <see cref="BlockingConcurrentList{T}"/>.</value>
		public int Count
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalList.Count;
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="ICollection{T}"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentList{T}"/>, this property always returns <see langword="false"/>.</value>
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				AssertDisposed();
				return ((ICollection<T>)InternalList).IsReadOnly;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="BlockingConcurrentList{T}"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="BlockingConcurrentList{T}"/>. The value can be <see langword="null"/> for reference types.</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is successfully removed; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if <paramref name="item"/> was not found in the <see cref="BlockingConcurrentList{T}"/>.</returns>
		public bool Remove(T item)
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				return InternalList.Remove(item);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
		#endregion

		#region IEnumerable<T> implementation
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			AssertDisposed();
			return new Enumerator<T>(InternalList, Lock);
		}
		#endregion

		#region IList implementation
		/// <summary>
		/// Adds an item to the <see cref="IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="Object"/> to add to the <see cref="IList"/>.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object value)
		{
			AssertDisposed();
			if (value == null && default(T) != null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x23), nameof(value));
			}
			if (ItemTypeInfo.IsAssignableFrom(value.GetType().GetTypeInfo()))
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x24), string.Format(Properties.Resources.Global_Add_InvalidCast, value, typeof(T)), nameof(value));
			}
			try
			{
				Lock.EnterWriteLock();
				return ((IList)InternalList).Add(value);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="IList"/> contains a specific value.
		/// </summary>
		/// <param name="value">The <see cref="Object"/> to locate in the <see cref="IList"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="value"/> is found in the <see cref="IList"/>; otherwise, <see langword="false"/>.</returns>
		bool IList.Contains(object value)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return ((IList)InternalList).Contains(value);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="IList"/>.
		/// </summary>
		/// <param name="value">The object to locate in the <see cref="IList"/>.</param>
		/// <returns>The index of <paramref name="value"/> if found in the list; otherwise, –1.</returns>
		int IList.IndexOf(object value)
		{
			AssertDisposed();
			try
			{
				Lock.EnterReadLock();
				return ((IList)InternalList).IndexOf(value);
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Inserts an item to the <see cref="IList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param>
		/// <param name="value">The object to insert into the <see cref="IList"/>.</param>
		void IList.Insert(int index, object value)
		{
			AssertDisposed();
			if (value == null && default(T) != null)
			{
				throw new CodedArgumentNullException(Errors.CreateHResult(0x25), nameof(value));
			}
			if (ItemTypeInfo.IsAssignableFrom(value.GetType().GetTypeInfo()))
			{
				throw new CodedArgumentException(Errors.CreateHResult(0x26), string.Format(Properties.Resources.Global_Add_InvalidCast, value, typeof(T)), nameof(value));
			}
			try
			{
				Lock.EnterWriteLock();
				((IList)InternalList).Insert(index, value);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IList"/> has a fixed size.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IList"/> has a fixed size; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentList{T}"/>, this property always returns <see langword="false"/>.</value>
		bool IList.IsFixedSize
		{
			get
			{
				AssertDisposed();
				return ((IList)InternalList).IsFixedSize;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IList"/> is read-only.
		/// </summary>
		/// <value><see langword="true"/> if the <see cref="IList"/> is read-only; otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentList{T}"/>, this property always returns <see langword="false"/>.</value>
		bool IList.IsReadOnly
		{
			get
			{
				AssertDisposed();
				return ((IList)InternalList).IsReadOnly;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="IList"/>.
		/// </summary>
		/// <param name="value">The object to remove from the <see cref="IList"/>.</param>
		void IList.Remove(object value)
		{
			AssertDisposed();
			if ((value is T) || (value == null && default(T) == null))
			{
				try
				{
					Lock.EnterWriteLock();
					((IList)InternalList).Remove((T)value);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		object IList.this[int index]
		{
			get
			{
				AssertDisposed();
				try
				{
					Lock.EnterReadLock();
					return InternalList[index];
				}
				finally
				{
					Lock.ExitReadLock();
				}
			}
			set
			{
				AssertDisposed();
				if (value == null && default(T) != null)
				{
					throw new CodedArgumentNullException(Errors.CreateHResult(0x27), nameof(value));
				}
				if (!ItemTypeInfo.IsAssignableFrom(value.GetType().GetTypeInfo()))
				{
					throw new CodedArgumentException(Errors.CreateHResult(0x28), string.Format(Properties.Resources.Global_Add_InvalidCast, value, typeof(T)), nameof(value));
				}
				try
				{
					Lock.EnterWriteLock();
					((IList)InternalList)[index] = value;
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}
		#endregion

		#region ICollection implementation
		/// <summary>
		/// Copies the elements of the <see cref="ICollection"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			AssertDisposed();
			try
			{
				Lock.EnterWriteLock();
				((ICollection)InternalList).CopyTo(array, index);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value><see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread safe); otherwise, <see langword="false"/>. In the default implementation of <see cref="BlockingConcurrentList{T}"/>, this property always returns <see langword="false"/>.</value>
		bool ICollection.IsSynchronized
		{
			get { return true; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
		/// </summary>
		/// <value>An object that can be used to synchronize access to the <see cref="ICollection"/>. In the default implementation of <see cref="BlockingConcurrentList{T}"/>, this property always returns the current instance.</value>
		object ICollection.SyncRoot
		{
			get { return this; }
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			AssertDisposed();
			return new Enumerator<T>(InternalList, Lock);
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
			InternalList = null;
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
