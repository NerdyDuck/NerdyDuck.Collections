#region Copyright
/*******************************************************************************
 * <copyright file="BlockingConcurrentListTest.cs" owner="Daniel Kopp">
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
 * <file name="BlockingConcurrentListTest.cs" date="2016-02-15">
 * Contains test methods to test the
 * NerdyDuck.Collections.Concurrent.BlockingConcurrentList class.
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
	/// Contains test methods to test the NerdyDuck.Collections.Concurrent.BlockingConcurrentList class.
	/// </summary>
#if WINDOWS_DESKTOP
	[ExcludeFromCodeCoverage]
#endif
	[TestClass]
	public class BlockingConcurrentListTest
	{
		[TestMethod]
		public void AddWhileEnum()
		{
			NonBlockingConcurrentList<int> lst = new NonBlockingConcurrentList<int>();
			lst.Add(1);
			lst.Add(2);
			lst.Add(3);
			lst.Add(4);
			int i = 0;

			foreach (int j in lst)
			{
				i++;
				if (i == 2)
				{
					lst.Add(5);
					Assert.AreEqual(5, lst.Count);
				}
			}

			Assert.AreEqual(5, lst.Count);
		}

		private void Sleep(int milliseconds)
		{
#if WINDOWS_UWP
			Task.Delay(milliseconds).RunSynchronously();
#endif
#if WINDOWS_DESKTOP || NETCORER
			Thread.Sleep(milliseconds);
#endif
		}
	}
}
