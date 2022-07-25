using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager> {
    public NpcDefine GetNpcDefine(int npcId) {
        return DataManager.Instance.INPCs[npcId];
    }

    public bool Interactive(int npcId)
    {
        if (DataManager.Instance.INPCs.ContainsKey(npcId))
        {
            NpcDefine npcDefine = DataManager.Instance.INPCs[npcId];
            return Interactive(npcDefine);
        }
        return false;
    }

    public bool Interactive(NpcDefine npcDefine)
    {
        Debug.LogFormat("NPCManager.Interactive NPC:[ID:{0} Name:{1} Type:{2}]", npcDefine.ID, npcDefine.Name, npcDefine.Type);
        if(npcDefine.Type == NpcType.Task)
        {
            return DoTaskInteractive(npcDefine);
        }
        else if(npcDefine.Type == NpcType.Functional)
        {
            return DoFunctionalInteractive(npcDefine);
        }
        return false;
    }

    private bool DoTaskInteractive(NpcDefine npcDefine)
    {
        //MessageBox.Show("点击了NPC:" + npcDefine.Name);
        EventManager.Instance.dispatchCustomEvent("InvokeShop", npcDefine);
        return true;
    }

    private bool DoFunctionalInteractive(NpcDefine npcDefine)
    {
        if(npcDefine.Type != NpcType.Functional)
        {
            return false;
        }
        EventManager.Instance.dispatchCustomEvent("InvokeInsrance", npcDefine);
        return true;
    }
}
