﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class ShopItemDefine
    {
        public int ShopItemID { get; set; }
        public int ItemID { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
    }
}