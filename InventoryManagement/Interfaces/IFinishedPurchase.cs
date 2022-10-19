using Inventory.Models;

namespace Inventory.Interfaces
{
    interface IFinishedPurchase
    {
        void FinishedPurchase(Purchase purchase);
    }
}
