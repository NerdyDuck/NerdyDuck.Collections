#region Copyright
/*******************************************************************************
 * <copyright file="NonBlockingConcurrentListTest.cs" owner="Daniel Kopp">
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
 * <assembly name="NerdyDuck.Tests.ParameterValidation">
 * Unit tests for NerdyDuck.ParameterValidation assembly.
 * </assembly>
 * <file name="NonBlockingConcurrentListTest.cs" date="2016-02-15">
 * Contains test methods to test the
 * NerdyDuck.Collections.Concurrent.NonBlockingConcurrentList class.
 * </file>
 ******************************************************************************/
#endregion

#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
#if WINDOWS_DESKTOP || NETCORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
#endif
using NerdyDuck.CodedExceptions;
using NerdyDuck.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyDuck.Tests.Collections
{
	/// <summary>
	/// Contains test methods to test the NerdyDuck.Collections.Concurrent.NonBlockingConcurrentList class.
	/// </summary>
#if WINDOWS_DESKTOP
	[ExcludeFromCodeCoverage]
#endif
	[TestClass]
	public class NonBlockingConcurrentListTest
	{
#if WINDOWS_DESKTOP || NETCORE
		[TestMethod]
		public void AddWhileEnum()
		{
			AutoResetEvent EnumEvent = new AutoResetEvent(false);
			AutoResetEvent AddDoneEvent = new AutoResetEvent(false);
			AutoResetEvent AddEvent = new AutoResetEvent(false);
			BlockingConcurrentList<int> lst = new BlockingConcurrentList<int>();
			for (int i = 0; i < 500; i++)
			{
				lst.Add(i);
			}

			Task.Run(() =>
			{
				int i = 0;
				foreach (int j in lst)
				{
					i++;
					if (i == 2)
					{
						EnumEvent.Set();
						AddEvent.WaitOne();
						Thread.Sleep(100);
					}
				}
				EnumEvent.Set();
			});

			EnumEvent.WaitOne();

			Task.Run(() =>
			{
				AddEvent.Set();
				lst.Add(500);
				AddDoneEvent.Set();
			});

			EnumEvent.WaitOne();
			AddDoneEvent.WaitOne();
			Thread.Sleep(50);
			Assert.AreEqual(501, lst.Count);
		}
#endif

#if WINDOWS_UWP
		[TestMethod]
		public async Task AddWhileEnum()
		{
			AutoResetEvent EnumEvent = new AutoResetEvent(false);
			AutoResetEvent AddDoneEvent = new AutoResetEvent(false);
			AutoResetEvent AddEvent = new AutoResetEvent(false);
			BlockingConcurrentList<int> lst = new BlockingConcurrentList<int>();
			for (int i = 0; i < 500; i++)
			{
				lst.Add(i);
			}

			Task.Run(async () =>
			{
				int i = 0;
				foreach (int j in lst)
				{
					i++;
					if (i == 2)
					{
						EnumEvent.Set();
						AddEvent.WaitOne();
						await Task.Delay(100);
					}
				}
				EnumEvent.Set();
			});

			EnumEvent.WaitOne();

			Task.Run(() =>
			{
				AddEvent.Set();
				lst.Add(500);
				AddDoneEvent.Set();
			});

			EnumEvent.WaitOne();
			AddDoneEvent.WaitOne();
			await Task.Delay(50);
			Assert.AreEqual(501, lst.Count);
		}
#endif
	}
}
