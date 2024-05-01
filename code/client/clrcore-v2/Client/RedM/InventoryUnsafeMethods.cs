using System;
using System.Security;
using CitizenFX.Core;
using CitizenFX.RedM.Native;

namespace CitizenFX.RedM
{
	public static partial class Inventory
	{
		/// <returns>Returns the GUID for the item id or null if arguments are invalid</returns>
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetGuidFromItemId(ItemCategories category, InventoryGuid inGuid, SlotId slotId)
		{
			InventoryGuid outGuid = new InventoryGuid();
			
			fixed (UnsafeInventoryGuid* uInGuid = &inGuid.m_unsafeInventoryGuid)
			fixed (UnsafeInventoryGuid* uOutGuid = &outGuid.m_unsafeInventoryGuid)
			{
				// INVENTORY_GET_GUID_FROM_ITEMID
				bool success = Natives.Call<bool>(0x886DFD3E185C8A89, c_inventoryId, uInGuid, category, slotId, uOutGuid);
				
				return success ? outGuid : null;
			}
		}
		
		/// <returns>Returns the GUID for the item id or null if arguments are invalid</returns>
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetGuidFromItemId(ItemCategories category, SlotId slotId)
		{
			InventoryGuid outGuid = new InventoryGuid();
			
			fixed (UnsafeInventoryGuid* uOutGuid = &outGuid.m_unsafeInventoryGuid)
			{
				byte nullVal = 0;
				// INVENTORY_GET_GUID_FROM_ITEMID
				bool success = Natives.Call<bool>(0x886DFD3E185C8A89, c_inventoryId, nullVal, category, slotId, uOutGuid);
				
				return success ? outGuid : null;
			}
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
		// TODO: Move to ItemDatabase static class
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
		// TODO: Take enum
		// TODO: Move to ItemDatabase static class
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
		private static SlotInfo SSC_GetItemSlotInfo(uint itemHash)
		{
			UnsafeSlotInfo unsafeSlotInfo = new UnsafeSlotInfo
			{
				guid = GetPlayerInventoryGuid().m_unsafeInventoryGuid,
				slotId = SlotId.Satchel
			};

			ItemGroup group = GetItemGroup(itemHash);
			switch (group)
			{
				case ItemGroup.Clothing:
				{
					if (!FitsSlotId(itemHash, SlotId.Wardrobe))
					{
						unsafeSlotInfo.guid = SSC_GetGuidFromItemId(ItemCategories.Wardrobe, new InventoryGuid(unsafeSlotInfo.guid), SlotId.Wardrobe).m_unsafeInventoryGuid;
						unsafeSlotInfo.slotId = GetDefaultItemSlotInfo(itemHash, ItemCategories.Wardrobe);
					}
					else
					{
						unsafeSlotInfo.slotId = SlotId.Wardrobe;
					}
					break;
				}
				case ItemGroup.Weapon:
				{
					// TODO: Implement 
					throw new NotImplementedException("Weapon ItemGroup not implemented yet");
				}
				case ItemGroup.Emote:
				{
					// TODO: Implement 
					throw new NotImplementedException("Emote ItemGroup not implemented yet");
				}
				case ItemGroup.Horse:
				{
					unsafeSlotInfo.slotId = SlotId.ActiveHorse;
					break;
				}	
				case ItemGroup.Upgrade:
				{
					// TODO: Test 
					// TODO: Place CI_CATEGORY_* into an enum
					if (SSC_ItemDatabaseInternalFitsInSlot(itemHash, Game.GenerateHashASCII("CI_CATEGORY_CAMP")))
					{
						unsafeSlotInfo.guid = SSC_GetGuidFromItemId(ItemCategories.KitCamp, new InventoryGuid(unsafeSlotInfo.guid), SlotId.Satchel).m_unsafeInventoryGuid;
						unsafeSlotInfo.slotId = GetDefaultItemSlotInfo(itemHash, ItemCategories.KitCamp);
					}
					else if (SSC_ItemDatabaseInternalFitsInSlot(itemHash, Game.GenerateHashASCII("CI_CATEGORY_WARDROBE")))
					{
						unsafeSlotInfo.guid = SSC_GetGuidFromItemId(ItemCategories.Wardrobe, new InventoryGuid(unsafeSlotInfo.guid), SlotId.Wardrobe).m_unsafeInventoryGuid;
						unsafeSlotInfo.slotId = GetDefaultItemSlotInfo(itemHash, ItemCategories.Wardrobe);
					}
					else if (FitsSlotId(itemHash, SlotId.Upgrade))
					{
						unsafeSlotInfo.slotId = SlotId.Upgrade;
					}
					break;
				}
				default:
				{
					if (FitsSlotId(itemHash, SlotId.Satchel))
					{
						unsafeSlotInfo.slotId = SlotId.Satchel;
					}
					else if (FitsSlotId(itemHash, SlotId.Wardrobe))
					{
						unsafeSlotInfo.slotId = SlotId.Wardrobe;
					}
					else
					{
						unsafeSlotInfo.slotId = GetDefaultItemSlotInfo(itemHash, ItemCategories.Character);
					}
					break;
				}
			}
			
			return new SlotInfo(unsafeSlotInfo);
		}	
		
		/// <returns></returns>
		[SecuritySafeCritical]
		private static unsafe bool SSC_AddItemWithGuid(uint itemHash, InventoryGuid guid1, InventoryGuid guid2, SlotId slotId, uint quantity, AddReason addReason)
		{
			fixed (UnsafeInventoryGuid* unsafeGuid1 = &guid1.m_unsafeInventoryGuid)
			fixed (UnsafeInventoryGuid* unsafeGuid2 = &guid2.m_unsafeInventoryGuid)
			{
				return Natives.Call<bool>(Hash._INVENTORY_ADD_ITEM_WITH_GUID, c_inventoryId, unsafeGuid1, unsafeGuid2, itemHash, slotId, quantity, addReason);
			}
		}	
		
		/// <returns></returns>
		[SecuritySafeCritical]
		private static unsafe bool SSC_AddItemWithGuid(uint itemHash, SlotInfo slotInfo, uint quantity, AddReason addReason)
		{
			fixed (UnsafeInventoryGuid* unsafeSlotGuid = &slotInfo.Guid.m_unsafeInventoryGuid)
			{
				int bindVar = 0;
				return Natives.Call<bool>(Hash._INVENTORY_ADD_ITEM_WITH_GUID, c_inventoryId, &bindVar, unsafeSlotGuid, itemHash, slotInfo.SlotId, quantity, addReason);
			}
		}	
		
		/// <returns></returns>
		private static bool SSC_AddItemWithGuid(uint itemHash, InventoryGuid guid, SlotInfo slotInfo, uint quantity, AddReason addReason)
		{
			return SSC_AddItemWithGuid(itemHash, guid, slotInfo.Guid, slotInfo.SlotId, quantity, addReason);
		}	
		
		
		[SecuritySafeCritical]
		private static unsafe InventoryGuid SSC_GetInventoryItem(SlotInfo slotInfo)
		{
			InventoryGuid outGuid = new InventoryGuid();
			fixed (UnsafeSlotInfo* slotGuid = &slotInfo.m_unsafeSlotInfo)
			fixed (UnsafeInventoryGuid* unsafeOutGuid = &outGuid.m_unsafeInventoryGuid)
			{
				bool success = Natives.Call<bool>(Hash.INVENTORY_GET_INVENTORY_ITEM, c_inventoryId, slotGuid, unsafeOutGuid);
				return success ? outGuid : null;
			}
		}
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_EquipItemByGuid(InventoryGuid guid, bool equipped)
		{
			UnsafeInventoryGuid unsafeGuid = guid.m_unsafeInventoryGuid;
			return Natives.Call<bool>(Hash._INVENTORY_EQUIP_ITEM_WITH_GUID, c_inventoryId, &unsafeGuid, equipped);
		}
		
		// not a Security Safe Critical but we want the add reason to be passed as as string so we still declare it like one
		private static InventoryGuid SSC_AddItemToInventory(uint itemHash, uint quantity, AddReason addReason)
		{
			SlotInfo slotInfo = SSC_GetItemSlotInfo(itemHash);
			InventoryGuid guid = SSC_GetGuidFromItemId((ItemCategories)itemHash, slotInfo.Guid, slotInfo.SlotId);
			if (guid == null)
			{
				return null;
			}
			
			if (SSC_AddItemWithGuid(itemHash, guid, slotInfo, quantity, addReason))
			{
				return slotInfo.Guid;
			}
			
			return null;
		}
		
		[SecuritySafeCritical]
		private static unsafe bool SSC_IsGuidValid(InventoryGuid guid1)
		{
			if (guid1 == null) return false;
			fixed (UnsafeInventoryGuid* rawGuid1 = &guid1.m_unsafeInventoryGuid)
			{
				return Natives.Call<bool>(Hash._INVENTORY_IS_GUID_VALID, rawGuid1);
			}
		}
		
	}
}
