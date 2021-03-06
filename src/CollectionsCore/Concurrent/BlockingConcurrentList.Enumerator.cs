﻿#region Copyright
/*******************************************************************************
 * <copyright file="BlockingConcurrentList.Enumerator.cs" owner="Daniel Kopp">
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
 * <file name="BlockingConcurrentList.Enumerator.cs" date="2016-02-15">
 * Thread-safe enumerator for the BlockingConcurrentList&lt;T&gt; class.
 * </file>
 ******************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NerdyDuck.Collections.Concurrent
{
	partial class BlockingConcurrentList<T>
	{
		/// <summary>
		/// Thread-safe enumerator for the <see cref="BlockingConcurrentList{T}"/> class.
		/// </summary>
		/// <typeparam name="TList">The type of elements in the list of the <see cref="BlockingConcurrentList{T}"/> being enumerated.</typeparam>
		public struct Enumerator<TList> : IEnumerator<TList>, IEnumerator, IDisposable
		{
			#region Private fields
			private ReaderWriterLockSlim Lock;
			private List<TList> InternalList;
			private List<TList>.Enumerator InternalEnumerator;
			private bool IsDisposed;
			private bool HasLock;
			#endregion

			#region Constructor
			/// <summary>
			/// Initializes a new instance of the <see cref="Enumerator{TList}"/> structure.
			/// </summary>
			/// <param name="list">The internal list to enumerate.</param>
			/// <param name="listLock">The <see cref="ReaderWriterLockSlim"/> that synchronizes the list.</param>
			internal Enumerator(List<TList> list, ReaderWriterLockSlim listLock)
			{
				InternalList = list;
				Lock = listLock;
				IsDisposed = false;
				HasLock = true;
				Lock.EnterReadLock();
				InternalEnumerator = InternalList.GetEnumerator();
			}
			#endregion

			#region IEnumerator<TList> implementation
			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public TList Current
			{
				get { return InternalEnumerator.Current; }
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				bool ReturnValue = InternalEnumerator.MoveNext();
				if (!ReturnValue && HasLock)
				{
					HasLock = false;
					Lock.ExitReadLock();
				}

				return ReturnValue;
			}
			#endregion

			#region IEnumerator implementation
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			/// <value>The current element in the collection.</value>
			object IEnumerator.Current
			{
				get { return InternalEnumerator.Current; }
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			void IEnumerator.Reset()
			{
				if (!HasLock)
				{
					HasLock = true;
					Lock.EnterReadLock();
				}
				((IEnumerator)InternalEnumerator).Reset();
			}
			#endregion

			#region IDisposable implementation
			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
			private void Dispose(bool disposing)
			{
				if (IsDisposed)
					return;

				IsDisposed = true;
				if (disposing)
				{
					InternalEnumerator.Dispose();
					if (HasLock)
					{
						HasLock = false;
						Lock.ExitReadLock();
					}
				}
			}
			#endregion
		}
	}
}
