using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public enum AttributeType
    {
        None = -1,

        // 最大生命值
        MaxHP = 0,

        // 最大法力值
        MaxMP = 1,

        // 力量
        STR = 2,

        // 智力
        INT = 3,

        // 敏捷
        DEX = 4,

        // 物理伤害
        AD = 5,

        // 法术伤害
        AP = 6,

        // 物理防御
        DEF = 7,

        // 法术防御
        MDEF = 8,

        // 速度
        SPD = 9,

        // 暴击概率
        CRI = 10,

        MAX
    }
}