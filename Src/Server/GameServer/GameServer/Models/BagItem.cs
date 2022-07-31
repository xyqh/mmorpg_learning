using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameServer.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BagItem
    {
        public ushort itemId;
        public ushort count;

        public static BagItem zero = new BagItem { itemId = 0, count = 0 };

        public BagItem(int itemId, int count)
        {
            this.itemId = (ushort)itemId;
            this.count = (ushort)count;
        }
    }
}