using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class BattleManager : Singleton<BattleManager>
    {
        // 当前目标
        public Creature Target { get; set; }
        // 施法位置
        public Vector3 Position { get; set; }
    }
}