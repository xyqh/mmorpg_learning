using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public class AttributeData
    {
        public float[] Data = new float[(int)AttributeType.MAX];

        // 最大生命值
        public float MaxHP { get { return Data[(int)AttributeType.MaxHP]; } set { Data[(int)AttributeType.MaxHP] = value; } }
        // 最大法力值
        public float MaxMP { get { return Data[(int)AttributeType.MaxMP]; } set { Data[(int)AttributeType.MaxMP] = value; } }
        // 力量
        public float STR { get { return Data[(int)AttributeType.STR]; } set { Data[(int)AttributeType.STR] = value; } }
        // 智力
        public float INT { get { return Data[(int)AttributeType.INT]; } set { Data[(int)AttributeType.INT] = value; } }
        // 敏捷
        public float DEX { get { return Data[(int)AttributeType.DEX]; } set { Data[(int)AttributeType.DEX] = value; } }
        // 物理伤害
        public float AD { get { return Data[(int)AttributeType.AD]; } set { Data[(int)AttributeType.AD] = value; } }
        // 法术伤害
        public float AP { get { return Data[(int)AttributeType.AP]; } set { Data[(int)AttributeType.AP] = value; } }
        // 物理防御
        public float DEF { get { return Data[(int)AttributeType.DEF]; } set { Data[(int)AttributeType.DEF] = value; } }
        // 法术防御
        public float MDEF { get { return Data[(int)AttributeType.MDEF]; } set { Data[(int)AttributeType.MDEF] = value; } }
        // 攻击速度
        public float SPD { get { return Data[(int)AttributeType.SPD]; } set { Data[(int)AttributeType.SPD] = value; } }
        // 暴击概率
        public float CRI { get { return Data[(int)AttributeType.CRI]; } set { Data[(int)AttributeType.CRI] = value; } }

        public void Reset()
        {
            for(int i = 0; i < (int)AttributeType.MAX; ++i)
            {
                this.Data[i] = 0;
            }
        }
    }
}