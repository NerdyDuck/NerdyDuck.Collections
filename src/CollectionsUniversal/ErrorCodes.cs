#region Copyright
/*******************************************************************************
 * <copyright file="ErrorCodes.cs" owner="Daniel Kopp">
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
 * <file name="ErrorCodes.cs" date="2016-04-13">
 * Error codes for the NerdyDuck.Collections assembly.
 * </file>
 ******************************************************************************/
#endregion

namespace NerdyDuck.Collections
{
	/// <summary>
	/// Error codes for the NerdyDuck.Collections assembly.
	/// </summary>
	internal enum ErrorCodes
	{
		/// <summary>
		/// 0x0001; BlockingConcurrentDictionary.Add(TKey,TValue); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Add_KeyNull = 0x01,
		/// <summary>
		/// 0x0002; BlockingConcurrentDictionary.Remove(TKey); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Remove_KeyNull,
		/// <summary>
		/// 0x0003; BlockingConcurrentDictionary.Item_Set(TKey); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Item_Set_KeyNull,
		/// <summary>
		/// 0x0004; BlockingConcurrentDictionary.Add(KeyValuePair(Of TKey,TValue)); item.Key is null.
		/// </summary>
		BlockingConcurrentDictionary_Add_ItemKeyNull,
		/// <summary>
		/// 0x0005; BlockingConcurrentDictionary.Add(KeyValuePair(Of TKey, TValue)); An item with the same key has already been added.
		/// </summary>
		BlockingConcurrentDictionary_Add_KeyExists,
		/// <summary>
		/// 0x0006; BlockingConcurrentDictionary.Add(object,object); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Add_ObjectKeyNull,
		/// <summary>
		/// 0x0007; BlockingConcurrentDictionary.Add(object,object); value is null.
		/// </summary>
		BlockingConcurrentDictionary_Add_ObjectValueNull,
		/// <summary>
		/// 0x0008; BlockingConcurrentDictionary.Add(object,object); An item with the same key has already been added.
		/// </summary>
		BlockingConcurrentDictionary_Add_ObjectKeyExists,
		/// <summary>
		/// 0x0009; BlockingConcurrentDictionary.Add(object,object); key is not of a type that can be added to the generic dictionary.
		/// </summary>
		BlockingConcurrentDictionary_Add_KeyInvalidType,
		/// <summary>
		/// 0x000a; BlockingConcurrentDictionary.Add(object,object); value is not of a type that can be added to the generic dictionary.
		/// </summary>
		BlockingConcurrentDictionary_Add_ValueInvalidType,
		/// <summary>
		/// 0x000b; BlockingConcurrentDictionary.Contains(object); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Contains_KeyNull,
		/// <summary>
		/// 0x000c; BlockingConcurrentDictionary.Remove(object); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Remove_ObjectKeyNull,
		/// <summary>
		/// 0x000d; BlockingConcurrentDictionary.Item_get(object); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Item_get_KeyNull,
		/// <summary>
		/// 0x000e; BlockingConcurrentDictionary.Item_set(object); key is null.
		/// </summary>
		BlockingConcurrentDictionary_Item_set_KeyNull,
		/// <summary>
		/// 0x000f; BlockingConcurrentDictionary.Item_set(object); value is null.
		/// </summary>
		BlockingConcurrentDictionary_Item_set_ValueNull,
		/// <summary>
		/// 0x0010; BlockingConcurrentDictionary.Item_set(object); key is not of a type that can be added to the generic dictionary.
		/// </summary>
		BlockingConcurrentDictionary_Item_set_KeyInvalidType,
		/// <summary>
		/// 0x0011; BlockingConcurrentDictionary.Item_set(object); value is not of a type that can be added to the generic dictionary.
		/// </summary>
		BlockingConcurrentDictionary_Item_set_ValueInvalidType,
		/// <summary>
		/// 0x0012; BlockingConcurrentList.AddRange; collection is null.
		/// </summary>
		BlockingConcurrentList_AddRange_ArgNull,
		/// <summary>
		/// 0x0013; BlockingConcurrentList.InsertRange; collection is null.
		/// </summary>
		BlockingConcurrentList_InsertRange_ArgNull,
		/// <summary>
		/// 0x0014; BlockingConcurrentList.InsertRange; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_InsertRange_IndexOutOfBounds,
		/// <summary>
		/// 0x0015; BlockingConcurrentList.RemoveRange; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_RemoveRange_IndexOutOfBounds,
		/// <summary>
		/// 0x0016; BlockingConcurrentList.RemoveRange; count is out of bounds.
		/// </summary>
		BlockingConcurrentList_RemoveRange_CountOutOfBounds,
		/// <summary>
		/// 0x0017; BlockingConcurrentList.RemoveRange; index and count no not denote a valid range in the list.
		/// </summary>
		BlockingConcurrentList_RemoveRange_InvalidRange,
		/// <summary>
		/// 0x0018; BlockingConcurrentList.Sort(IComparison); comparison is null.
		/// </summary>
		BlockingConcurrentList_Sort_ArgNull,
		/// <summary>
		/// 0x0019; BlockingConcurrentList.Sort(int,int,IComparer); index is out of bounds.
		/// </summary>
		BlockingConcurrentList_Sort_IndexOutOfBounds,
		/// <summary>
		/// 0x001a; BlockingConcurrentList.Sort(int,int,IComparer); count is out of bounds.
		/// </summary>
		BlockingConcurrentList_Sort_CountOutOfBounds,
		/// <summary>
		/// 0x001b; BlockingConcurrentList.Sort(int,int,IComparer); index and count do not denote a valid range in the list.
		/// </summary>
		BlockingConcurrentList_Sort_InvalidRange,
		/// <summary>
		/// 0x001c; BlockingConcurrentList.Insert; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_Insert_IndexOutOfBounds,
		/// <summary>
		/// 0x001d; BlockingConcurrentList.RemoveAt; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_RemoveAt_IndexOutOfBounds,
		/// <summary>
		/// 0x001e; BlockingConcurrentList.Item_get; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_Item_get_IndexOutOfBounds,
		/// <summary>
		/// 0x001f; BlockingConcurrentList.Item_set; index is out of bounds.
		/// </summary>
		BlockingConcurrentList_Item_set_IndexOutOfBounds,
		/// <summary>
		/// 0x0020; BlockingConcurrentList.CopyTo; array is null.
		/// </summary>
		BlockingConcurrentList_CopyTo_ArrayNull,
		/// <summary>
		/// 0x0021; BlockingConcurrentList.CopyTo; arrayIndex is less than 0.
		/// </summary>
		BlockingConcurrentList_CopyTo_IndexLessThan0,
		/// <summary>
		/// 0x0022; BlockingConcurrentList.CopyTo; Not enough space in array.
		/// </summary>
		BlockingConcurrentList_CopyTo_NotEnoughSpace,
		/// <summary>
		/// 0x0023; BlockingConcurrentList.Add(object); value is null.
		/// </summary>
		BlockingConcurrentList_Add_ObjectValueNull,
		/// <summary>
		/// 0x0024; BlockingConcurrentList.Add(object); value is not of a type that can be added to the generic list.
		/// </summary>
		BlockingConcurrentList_Add_ObjectInvalidType,
		/// <summary>
		/// 0x0025; BlockingConcurrentList.Insert(object); value is null.
		/// </summary>
		BlockingConcurrentList_Insert_ObjectValueNull,
		/// <summary>
		/// 0x0026; BlockingConcurrentList.Insert(object); value is not of a type that can be added to the generic list.
		/// </summary>
		BlockingConcurrentList_Insert_ObjectInvalidType,
		/// <summary>
		/// 0x0027; BlockingConcurrentList.Item_set(object); value is null.
		/// </summary>
		BlockingConcurrentList_Item_set_ObjectValueNull,
		/// <summary>
		/// 0x0028; BlockingConcurrentList.Item_set(object); value is not of a type that can be added to the generic list.
		/// </summary>
		BlockingConcurrentList_Item_set_InvalidType,
		/// <summary>
		/// 0x0029; NonBlockingConcurrentDictionary.Add(TKey,TValue); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_KeyNull,
		/// <summary>
		/// 0x002a; NonBlockingConcurrentDictionary.Remove(TKey); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Remove_KeyNull,
		/// <summary>
		/// 0x002b; NonBlockingConcurrentDictionary.Item_set(TKey); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_set_KeyNull,
		/// <summary>
		/// 0x002c; NonBlockingConcurrentDictionary.Add(KeyValuePair(Of TKey,TValue)); item.Key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_ItemKeyNull,
		/// <summary>
		/// 0x002d; NonBlockingConcurrentDictionary.Add(KeyValuePair(Of TKey,TValue)); An item with the same key has already been added.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_KeyExists,
		/// <summary>
		/// 0x002e; NonBlockingConcurrentDictionary.Add(object,object); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_ObjectKeyNull,
		/// <summary>
		/// 0x002f; NonBlockingConcurrentDictionary.Add(object,object); value is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_ObjectValueNull,
		/// <summary>
		/// 0x0030; NonBlockingConcurrentDictionary.Add(object,object); An item with the same key has already been added.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_ObjectKeyExists,
		/// <summary>
		/// 0x0031; NonBlockingConcurrentDictionary.Add(object,object); key is not of a type that can be added to the generic dictionary.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_KeyInvalidType,
		/// <summary>
		/// 0x0032; NonBlockingConcurrentDictionary.Add(object,object); value is not of a type that can be added to the generic dictionary.
		/// </summary>
		NonBlockingConcurrentDictionary_Add_ValueInvalidType,
		/// <summary>
		/// 0x0033; NonBlockingConcurrentDictionary.Contains(object); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Contains_KeyNull,
		/// <summary>
		/// 0x0034; NonBlockingConcurrentDictionary.Remove(object); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Remove_ObjectKeyNull,
		/// <summary>
		/// 0x0035; NonBlockingConcurrentDictionary.Item_get(object); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_get_ObjectKeyNull,
		/// <summary>
		/// 0x0036; NonBlockingConcurrentDictionary.Item_set(object); key is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_set_ObjectKeyNull,
		/// <summary>
		/// 0x0037; NonBlockingConcurrentDictionary.Item_set(object); value is null.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_set_ObjectValueNull,
		/// <summary>
		/// 0x0038; NonBlockingConcurrentDictionary.Item_set(object); key is not of a type that can be added to the generic dictionary.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_set_KeyInvalidType,
		/// <summary>
		/// 0x0039; NonBlockingConcurrentDictionary.Item_set(object); value is not of a type that can be added to the generic dictionary.
		/// </summary>
		NonBlockingConcurrentDictionary_Item_set_ValueInvalidType,
		/// <summary>
		/// 0x003a; NonBlockingConcurrentList.AddRange; collection is null.
		/// </summary>
		NonBlockingConcurrentList_AddRange_ArgNull,
		/// <summary>
		/// 0x003b; NonBlockingConcurrentList.InsertRange; collection is null.
		/// </summary>
		NonBlockingConcurrentList_InsertRange_ArgNull,
		/// <summary>
		/// 0x003c; NonBlockingConcurrentList.InsertRange; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_InsertRange_IndexOutOfBounds,
		/// <summary>
		/// 0x003d; NonBlockingConcurrentList.RemoveRange; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_RemoveRangee_IndexOutOfBounds,
		/// <summary>
		/// 0x003e; NonBlockingConcurrentList.RemoveRange; count is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_RemoveRange_CountOutOfBounds,
		/// <summary>
		/// 0x003f; NonBlockingConcurrentList.RemoveRange; index and count no not denote a valid range in the list.
		/// </summary>
		NonBlockingConcurrentList_RemoveRange_InvalidRange,
		/// <summary>
		/// 0x0040; NonBlockingConcurrentList.Sort(IComparison); comparison is null.
		/// </summary>
		NonBlockingConcurrentList_Sort_ArgNull,
		/// <summary>
		/// 0x0041; NonBlockingConcurrentList.Sort(int,int,IComparer); index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_Sort_IndexOutOfBounds,
		/// <summary>
		/// 0x0042; NonBlockingConcurrentList.Sort(int,int,IComparer); count is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_Sort_CountOutOfBounds,
		/// <summary>
		/// 0x0043; NonBlockingConcurrentList.Sort(int,int,IComparer); index and count no not denote a valid range in the list.
		/// </summary>
		NonBlockingConcurrentList_Sort_InvalidRange,
		/// <summary>
		/// 0x0044; NonBlockingConcurrentList.Insert; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_Insert_IndexOutOfBounds,
		/// <summary>
		/// 0x0045; NonBlockingConcurrentList.RemoveAt; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_RemoveAt_IndexOutOfBounds,
		/// <summary>
		/// 0x0046; NonBlockingConcurrentList.Item_get; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_Item_get_IndexOutOfBounds,
		/// <summary>
		/// 0x0047; NonBlockingConcurrentList.Item_set; index is out of bounds.
		/// </summary>
		NonBlockingConcurrentList_Item_set_IndexOutOfBounds,
		/// <summary>
		/// 0x0048; NonBlockingConcurrentList.CopyTo; array is null.
		/// </summary>
		NonBlockingConcurrentList_CopyTo_ArrayNull,
		/// <summary>
		/// 0x0049; NonBlockingConcurrentList.CopyTo; arrayIndex is less than 0.
		/// </summary>
		NonBlockingConcurrentList_CopyTo_IndexLessThan0,
		/// <summary>
		/// 0x004a; NonBlockingConcurrentList.CopyTo; Not enough space in array.
		/// </summary>
		NonBlockingConcurrentList_CopyTo_NotEnoughSpace,
		/// <summary>
		/// 0x004b; NonBlockingConcurrentList.Add(object); value is null.
		/// </summary>
		NonBlockingConcurrentList_Add_ObjectValueNull,
		/// <summary>
		/// 0x004c; NonBlockingConcurrentList.Add(object); value is not of a type that can be added to the generic list.
		/// </summary>
		NonBlockingConcurrentList_Add_InvalidType,
		/// <summary>
		/// 0x004d; NonBlockingConcurrentList.Insert(object); value is null.
		/// </summary>
		NonBlockingConcurrentList_Insert_ObjectValueNull,
		/// <summary>
		/// 0x004e; NonBlockingConcurrentList.Insert(object); value is not of a type that can be added to the generic list.
		/// </summary>
		NonBlockingConcurrentList_Insert_InvalidType,
		/// <summary>
		/// 0x004f; NonBlockingConcurrentList.Item_set(object); value is null.
		/// </summary>
		NonBlockingConcurrentList_Item_set_ObjectValueNull,
		/// <summary>
		/// 0x0050; NonBlockingConcurrentList.Item_set(object); value is not of a type that can be added to the generic list.
		/// </summary>
		NonBlockingConcurrentList_Item_set_InvalidType
	}
}
