using Entities;
using Managers;
using Models;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;

    public UICreatureInfo targetUI;

    // Use this for initialization
    protected override void OnStart () {
        this.UpdateAvatar();
        this.targetUI.gameObject.SetActive(false);
        BattleManager.Instance.OnTargetChanged += OnTargetChanged;
	}

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacterInfo.Name, User.Instance.CurrentCharacterInfo.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BackToCharSelect()
    {
        Services.UserService.Instance.SendGameLeave();
    }

    public void updateMiniMap()
    {
        gameObject.GetComponentInChildren<UIMiniMap>().updateMap();
    }

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.setTitle("测试弹窗标题");
        test.OnClose += Test_OnClose;
    }

    public void OnClickBackPack()
    {
        UIManager.Instance.Show<UIBackPack>();
    }

    public void OnClickEquip()
    {
        UICharEquip charEquip = UIManager.Instance.Show<UICharEquip>();
        charEquip.RefreshEquips();
    }

    public void OnClickSkill()
    {
        UIManager.Instance.Show<UISkillView>();
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的：" + result, "对话框响应结果", MessageBoxType.Information);
    }

    void OnTargetChanged(Creature target)
    {
        if(target != null)
        {
            if (!targetUI.isActiveAndEnabled)
            {
                targetUI.gameObject.SetActive(true);
                targetUI.Target = target;
            }
            else
            {
                targetUI.gameObject.SetActive(false);
            }
        }
    }
}
