using Inventory.Models;

namespace Inventory.Interfaces
{
    interface ICreatedEditedItemType
    {
        void CreatedEditedItemType(ItemType itemType, bool wasCreated);
    }
}
