using Inventory.Models;

namespace Inventory.Interfaces
{
    interface ICreatedEditedCurrency
    {
        void CreatedEditedCurrency(Currency currency, bool wasCreated);
    }
}
