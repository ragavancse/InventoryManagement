﻿using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Interfaces
{
    interface ICreatedInventoryItem
    {
        void CreatedInventoryItem(InventoryItem item);
    }
}
