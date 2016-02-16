#region Copyright
/*******************************************************************************
 * <copyright file="Resources.cs" owner="Daniel Kopp">
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
 * <file name="Resources.cs" date="2016-02-16">
 * Helper class to access localized string resources.
 * </file>
 ******************************************************************************/
#endregion

using System;

namespace NerdyDuck.Collections.Properties
{
	/// <summary>
	/// Helper class to access localized string resources.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Resources.tt", "1.0.0.0")]
	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	internal static class Resources
	{
		#region String resource properties
		/// <summary>
		/// Gets a localized string similar to "The value "{0}" is not of type "{1}" and cannot be used in this generic dictionary.".
		/// </summary>
		internal static string Global_AddDictionary_InvalidCast
		{
			get { return GetResource("Global_AddDictionary_InvalidCast"); }
		}

		/// <summary>
		/// Gets a localized string similar to "The key "{0}" is not of type "{1}" and cannot be used as key in this generic dictionary.".
		/// </summary>
		internal static string Global_AddDictionary_InvalidCastKey
		{
			get { return GetResource("Global_AddDictionary_InvalidCastKey"); }
		}

		/// <summary>
		/// Gets a localized string similar to "An item with the same key has already been added.".
		/// </summary>
		internal static string Global_Add_Duplicate
		{
			get { return GetResource("Global_Add_Duplicate"); }
		}

		/// <summary>
		/// Gets a localized string similar to "The value "{0}" is not of type "{1}" and cannot be used in this generic collection.".
		/// </summary>
		internal static string Global_Add_InvalidCast
		{
			get { return GetResource("Global_Add_InvalidCast"); }
		}

		/// <summary>
		/// Gets a localized string similar to "Array index may not be less than 0.".
		/// </summary>
		internal static string Global_ArrayIndexOutOfRange
		{
			get { return GetResource("Global_ArrayIndexOutOfRange"); }
		}

		/// <summary>
		/// Gets a localized string similar to "There is not enough space to copy all elements to array, beginning at index {0}.".
		/// </summary>
		internal static string Global_CopyTo_NotEnoughSpace
		{
			get { return GetResource("Global_CopyTo_NotEnoughSpace"); }
		}

		/// <summary>
		/// Gets a localized string similar to "count may not be less than 0 or larger than Count.".
		/// </summary>
		internal static string Global_CountOutOfRange
		{
			get { return GetResource("Global_CountOutOfRange"); }
		}

		/// <summary>
		/// Gets a localized string similar to "index may not be less than 0 or larger than or equal to Count.".
		/// </summary>
		internal static string Global_IndexOutOfRange
		{
			get { return GetResource("Global_IndexOutOfRange"); }
		}

		/// <summary>
		/// Gets a localized string similar to "index and count do not denote a valid range in the list.".
		/// </summary>
		internal static string Global_Range
		{
			get { return GetResource("Global_Range"); }
		}
		#endregion

#if WINDOWS_UWP
		#region Private fields
		private static Windows.ApplicationModel.Resources.Core.ResourceMap mResourceMap;
		private static Windows.ApplicationModel.Resources.Core.ResourceContext mContext;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the main resource map of the assembly.
		/// </summary>
		internal static Windows.ApplicationModel.Resources.Core.ResourceMap ResourceMap
		{
			get
			{
				if (object.ReferenceEquals(mResourceMap, null))
				{
					mResourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
				}

				return mResourceMap;
			}
		}

		/// <summary>
		/// Gets or sets the resource context to use when retrieving resources.
		/// </summary>
		internal static Windows.ApplicationModel.Resources.Core.ResourceContext Context
		{
			get { return mContext; }
			set { mContext = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retrieves a string resource using the resource map.
		/// </summary>
		/// <param name="name">The name of the string resource.</param>
		/// <returns>A localized string.</returns>
		internal static string GetResource(string name)
		{
			Windows.ApplicationModel.Resources.Core.ResourceContext context = Context;
			if (context == null)
			{
				context = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse();
			}

			Windows.ApplicationModel.Resources.Core.ResourceCandidate resourceCandidate = ResourceMap.GetValue("NerdyDuck.Collections/Resources/" + name, context);

			if (resourceCandidate == null)
			{
				throw new ArgumentOutOfRangeException(nameof(name));
			}

			return resourceCandidate.ValueAsString;
		}
		#endregion
#endif

#if WINDOWS_DESKTOP
		#region Private fields
		private static System.Resources.ResourceManager mResourceManager;
		private static System.Globalization.CultureInfo mResourceCulture;
		#endregion

		#region Properties
		/// <summary>
		/// Returns the cached ResourceManager instance used by this class.
		/// </summary>
		[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(mResourceManager, null))
				{
					System.Resources.ResourceManager temp = new System.Resources.ResourceManager("NerdyDuck.Collections.Properties.Resources", typeof(Resources).Assembly);
					mResourceManager = temp;
				}
				return mResourceManager;
			}
		}

		/// <summary>
		/// Overrides the current thread's CurrentUICulture property for all resource lookups using this strongly typed resource class.
		/// </summary>
		[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static System.Globalization.CultureInfo Culture
		{
			get { return mResourceCulture; }
			set { mResourceCulture = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retrieves a string resource using the resource manager.
		/// </summary>
		/// <param name="name">The name of the string resource.</param>
		/// <returns>A localized string.</returns>
		internal static string GetResource(string name)
		{
			return ResourceManager.GetString(name, mResourceCulture);
		}
		#endregion
#endif
	}
}
