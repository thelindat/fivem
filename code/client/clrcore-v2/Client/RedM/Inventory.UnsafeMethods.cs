using System.Security;
using CitizenFX.RedM.Native;

namespace CitizenFX.RedM
{
	public static partial class Inventory
	{
		[SecuritySafeCritical]
		private static unsafe bool SSC_AddItemWithGuid(uint itemHash, InventoryGuid guid1, InventoryGuid guid2, SlotId slotId, uint quantity, AddReason addReason)
		{
			UnsafeInventoryGuid unsafeGuid1 = guid1.m_unsafeInventoryGuid;
			UnsafeInventoryGuid unsafeGuid2 = guid2.m_unsafeInventoryGuid;
			
			return Natives.Call<bool>(Hash._INVENTORY_ADD_ITEM_WITH_GUID, c_inventoryId, &unsafeGuid1, &unsafeGuid2, itemHash, slotId, quantity, addReason);
		}	
		
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetGuidFromItemId(ItemCategories category, InventoryGuid inGuid, SlotId slotId)
		{
			InventoryGuid outGuid = new InventoryGuid();
			
			UnsafeInventoryGuid uInGuid = inGuid.m_unsafeInventoryGuid;
			UnsafeInventoryGuid uOutGuid = outGuid.m_unsafeInventoryGuid;
			
			// INVENTORY_GET_GUID_FROM_ITEMID
			bool success = Natives.Call<bool>(0x886DFD3E185C8A89, c_inventoryId, &uInGuid, category, slotId, &uOutGuid);
				
			return success ? outGuid : null;
		}
		
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetGuidFromItemId(ItemCategories category, SlotId slotId)
		{
			InventoryGuid outGuid = new InventoryGuid();
			
			UnsafeInventoryGuid unsafeOutGuid = outGuid.m_unsafeInventoryGuid;
			
			byte nullVal = 0;
			// INVENTORY_GET_GUID_FROM_ITEMID
			bool success = Natives.Call<bool>(0x886DFD3E185C8A89, c_inventoryId, nullVal, category, slotId, &unsafeOutGuid);
				
			return success ? outGuid : null;
		}
		
		[SecuritySafeCritical]
		private static ItemInfo SSC_GetItemInfo(uint itemHash)
		{
			UnsafeItemInfo itemInfo = new UnsafeItemInfo();
			
			if (!IsHashAValidItem(itemHash))
			{
				return null;
			}
			
			unsafe
			{
				bool success = Natives.Call<bool>(Hash.ITEMDATABASE_FILLOUT_ITEM_INFO, itemHash, &itemInfo);
				return success ? new ItemInfo(itemInfo) : null;
			}
		}
		
		private static bool SSC_ItemValid(uint itemHash, uint param2 = 0)
		{
			if (itemHash == 0) return false;
			return Natives.Call<bool>(Hash._ITEMDATABASE_IS_KEY_VALID, itemHash, param2);
		}
		
		[SecuritySafeCritical]
		private static uint SSC_FilloutItemInfo(uint itemHash)
		{
			if (!SSC_ItemValid(itemHash))
			{
				return 0;
			}
			
			unsafe
			{
				UnsafeItemDbInfo dbInfo;
				var success = Natives.Call<bool>(Hash.ITEMDATABASE_FILLOUT_ITEM_INFO, itemHash, &dbInfo);
				return success ? dbInfo.data2 : 0;
			}
			
		}
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_ItemDatabaseInternalFitsInSlot(uint itemHash, uint categoryHash)
		{
			if (itemHash == 0 || categoryHash == 0)
			{
				return false;
			}
			
			uint slotId;
			uint unkItemInfo = SSC_FilloutItemInfo(itemHash);
			uint index = 0;
			int maxCount = Natives.Call<int>(Hash._ITEMDATABASE_GET_FITS_SLOT_COUNT, itemHash);

			while (index < maxCount)
			{
				if (Natives.Call<bool>(Hash._ITEMDATABASE_GET_FITS_SLOT_INFO, unkItemInfo, index, &slotId))
				{
					if (Natives.Call<bool>(Hash._ITEMDATABASE_CAN_EQUIP_ITEM_ON_CATEGORY, itemHash, categoryHash, slotId))
					{
						return true;
					}
				}
				index++;
			}
			return false;
		}
		
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_AddItemWithGuid(uint itemHash, SlotInfo slotInfo, uint quantity, AddReason addReason)
		{
			UnsafeInventoryGuid unsafeSlotGuid = slotInfo.Guid.m_unsafeInventoryGuid;
			int bindVar = 0;
			return Natives.Call<bool>(Hash._INVENTORY_ADD_ITEM_WITH_GUID, c_inventoryId, &bindVar, &unsafeSlotGuid, itemHash, slotInfo.SlotId, quantity, addReason);
		}	
		
		private static bool SSC_AddItemWithGuid(uint itemHash, InventoryGuid guid, SlotInfo slotInfo, uint quantity, AddReason addReason)
		{
			return SSC_AddItemWithGuid(itemHash, guid, slotInfo.Guid, slotInfo.SlotId, quantity, addReason);
		}	
		
		
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetInventoryItem(InventoryGuid inGuid)
		{
			InventoryGuid outGuid = new InventoryGuid();
			
			UnsafeInventoryGuid unsafeInGuid = inGuid.m_unsafeInventoryGuid;
			UnsafeInventoryGuid unsafeOutGuid = outGuid.m_unsafeInventoryGuid;
			
			bool success = Natives.Call<bool>(Hash.INVENTORY_GET_INVENTORY_ITEM, c_inventoryId, &unsafeInGuid, &unsafeOutGuid);
			return success ? outGuid : null;
		}
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_EquipItemByGuid(InventoryGuid guid, bool equipped)
		{
			UnsafeInventoryGuid unsafeGuid = guid.m_unsafeInventoryGuid;
			return Natives.Call<bool>(Hash._INVENTORY_EQUIP_ITEM_WITH_GUID, c_inventoryId, &unsafeGuid, equipped);
		}
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_IsGuidValid(InventoryGuid guid1)
		{
			if (guid1 == null) return false;
			UnsafeInventoryGuid rawGuid1 = guid1.m_unsafeInventoryGuid;
			return Natives.Call<bool>(Hash._INVENTORY_IS_GUID_VALID, &rawGuid1);
		}
		
	}
}
