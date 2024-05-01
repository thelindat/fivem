using System.Runtime.InteropServices;
using System.Security;
using CitizenFX.Core;

namespace CitizenFX.RedM
{
	public class ItemInfo
	{
		internal UnsafeItemInfo m_unsafeItemInfo;

		public ItemGroup ItemGroup {
			get => m_unsafeItemInfo.itemGroup;
			set => m_unsafeItemInfo.itemGroup = value;
		}

		public ItemInfo() {}

		internal ItemInfo(UnsafeItemInfo unsafeItemInfo)
		{
			m_unsafeItemInfo = unsafeItemInfo;
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x38)]
	[SecuritySafeCritical]
	internal unsafe struct UnsafeItemInfo
	{
		[FieldOffset(0x00)] internal int f_0;
		[FieldOffset(0x08)] internal int f_1;
		[FieldOffset(0x10)] internal ItemGroup itemGroup;
		[FieldOffset(0x18)] internal int f_3;
		[FieldOffset(0x20)] internal int f_4;
		[FieldOffset(0x28)] internal int f_5;
		[FieldOffset(0x30)] internal int f_6;
	}

	public class SlotInfo
	{
		internal UnsafeSlotInfo m_unsafeSlotInfo;

		internal UnsafeInventoryGuid InternalGuid => m_unsafeSlotInfo.guid;

		public InventoryGuid Guid => new InventoryGuid(InternalGuid);
		public SlotId SlotId {
			get => m_unsafeSlotInfo.slotId;
			set => m_unsafeSlotInfo.slotId = value;
		}

		public SlotInfo() {}

		public SlotInfo(InventoryGuid guid, SlotId slotId)
		{
			m_unsafeSlotInfo.guid = guid.m_unsafeInventoryGuid;
			SlotId = slotId;
		}
		internal SlotInfo(UnsafeSlotInfo unsafeSlotInfo)
		{
			m_unsafeSlotInfo = unsafeSlotInfo;
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x28)]
	[SecuritySafeCritical]
	internal unsafe struct UnsafeSlotInfo
	{
		[FieldOffset(0x00)] internal UnsafeInventoryGuid guid;
		[FieldOffset(0x08)] internal int data2;
		[FieldOffset(0x10)] internal int data3;
		[FieldOffset(0x18)] internal int data4;
		[FieldOffset(0x20)] internal SlotId slotId;
	}
	public class InventoryGuid
	{
		internal UnsafeInventoryGuid m_unsafeInventoryGuid;

		public void DumpContent()
		{
			var guid = m_unsafeInventoryGuid;
			Debug.WriteLine($"data1: {guid.data1}");
			Debug.WriteLine($"data2: {guid.data2}");
			Debug.WriteLine($"data3: {guid.data3}");
			Debug.WriteLine($"data4: {guid.data4}");
		}

		public InventoryGuid() {}
		internal InventoryGuid(UnsafeInventoryGuid unsafeGuid)
		{
			m_unsafeInventoryGuid = unsafeGuid;
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x28)]
	[SecuritySafeCritical]
	internal unsafe struct UnsafeInventoryGuid
	{
		[FieldOffset(0x00)] internal uint data1;
		[FieldOffset(0x08)] internal uint data2;
		[FieldOffset(0x10)] internal uint data3;
		[FieldOffset(0x18)] internal uint data4;
	}
	
	
	[StructLayout(LayoutKind.Explicit, Size = 0x10)]
	[SecuritySafeCritical]
	internal unsafe struct UnsafeItemDbInfo 
	{
		[FieldOffset(0x00)] internal uint data1;
		[FieldOffset(0x08)] internal uint data2;
	}
}
