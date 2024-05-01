using CitizenFX.RedM.Native;

namespace CitizenFX.RedM
{
	public static partial class Inventory
	{
		private const int c_inventoryId = 1;
		
		public static bool FitsSlotId(uint itemHash, SlotId slotId)
		{
			return Natives.Call<bool>(Hash._INVENTORY_FITS_SLOT_ID, itemHash, slotId);
		}
		
		public static SlotId GetDefaultItemSlotInfo(uint itemHash, ItemCategories categories)
		{
			SlotId slotId = (SlotId)Natives.GetDefaultItemSlotInfo(itemHash, (uint)categories);
			return slotId;
		}
		
		public static ItemGroup GetItemGroup(uint itemHash)
		{
			ItemInfo itemInfo = SSC_GetItemInfo(itemHash);
			return itemInfo?.ItemGroup ?? 0;
		}
		
		public static bool IsGuidValid(InventoryGuid guid)
		{
			return SSC_IsGuidValid(guid);
		}
		
		public static InventoryGuid AddItemToInventory(uint itemHash, uint quantity = 1, AddReason addReason = AddReason.Default)
		{
			return SSC_AddItemToInventory(itemHash, quantity, addReason);
		}
		
		public static InventoryGuid AddItemToInventory(string itemName, uint quantity = 1, AddReason addReason = AddReason.Default)
		{
			var itemHash = Game.GenerateHash(itemName);
			return SSC_AddItemToInventory(itemHash, quantity, addReason);
		}
		
		public static ItemInfo GetItemInfo(uint itemHash)
		{
			return SSC_GetItemInfo(itemHash);
		}
		
		public static ItemInfo GetItemInfo(string itemName)
		{
			return SSC_GetItemInfo(Game.GenerateHash(itemName));
		}
		
		public static SlotInfo GetItemSlotInfo(uint itemHash)
		{
			return SSC_GetItemSlotInfo(itemHash);
		}
		
		public static SlotInfo GetItemSlotInfo(string itemName)
		{
			return SSC_GetItemSlotInfo(Game.GenerateHash(itemName));
		}
		
		public static InventoryGuid GetPlayerInventoryGuid()
		{
			return SSC_GetGuidFromItemId(ItemCategories.Character, SlotId.None);
		}
		
		public static InventoryGuid GetPlayerWardrobeGuid()
		{
			InventoryGuid character = GetPlayerInventoryGuid();
			
			return Inventory.GetGuidFromItemId(ItemCategories.Wardrobe, character, SlotId.Wardrobe);
		}
		
		public static InventoryGuid GetGuidFromItemId(ItemCategories categoryHash, InventoryGuid inGuid, SlotId slotId)
		{
			return SSC_GetGuidFromItemId(categoryHash, inGuid, slotId);
		}
		
		public static bool AddItemWithGuid(uint itemHash, InventoryGuid guid, SlotInfo slotInfo, uint quantity, AddReason addReason = AddReason.Default)
		{
			return SSC_AddItemWithGuid(itemHash, guid, slotInfo, quantity, addReason);
		}
		
		public static bool AddItemWithGuid(uint itemHash, InventoryGuid guid1, InventoryGuid guid2, SlotId slotId, uint quantity, AddReason addReason = AddReason.Default)
		{
			return SSC_AddItemWithGuid(itemHash, guid1, guid2, slotId, quantity, addReason);
		}
		
		public static bool EquipItemByGuid(InventoryGuid guid, bool equipped)
		{
			return SSC_EquipItemByGuid(guid, equipped);
		}
		
		/// <returns>Returns true if the hash is a valid item, false otherwise</returns>
		public static bool IsHashAValidItem(uint itemHash)
		{
			return Natives.Call<bool>(Hash._ITEMDATABASE_IS_KEY_VALID, itemHash, 0);
		}
	}
}
