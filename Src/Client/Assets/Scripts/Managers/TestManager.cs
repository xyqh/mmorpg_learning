using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Singleton<TestManager> {
    public void Init()
    {
        EventManager.Instance.addEventListener("InvokeShop", OnNpcInvokeShop);
        EventManager.Instance.addEventListener("InvokeInsrance", OnNpcInvokeInsrance);
    }

    private void OnNpcInvokeShop(params object[] param)
    {
        NpcDefine npcDefine = (NpcDefine)param[0];
        Debug.LogFormat("TestManager.OnNpcInvokeShop :NPC:[{0}:{1}] Type:{2} Func:{3}", npcDefine.ID, npcDefine.Name, npcDefine.Type, npcDefine.Function);
        //UITest uiTest = UIManager.Instance.Show<UITest>();
        //uiTest.setTitle(npcDefine.Name);
        UIShop uiShop = UIManager.Instance.Show<UIShop>();
        uiShop.InitData(npcDefine.Param);
    }

    private void OnNpcInvokeInsrance(params object[] param)
    {
        NpcDefine npcDefine = (NpcDefine)param[0];
        Debug.LogFormat("TestManager.OnNpcInvokeInsrance :NPC:[{0}:{1}] Type:{2} Func:{3}", npcDefine.ID, npcDefine.Name, npcDefine.Type, npcDefine.Function);
        MessageBox.Show("点击了NPC:" + npcDefine.Name, "NPC对话");
    }
}
