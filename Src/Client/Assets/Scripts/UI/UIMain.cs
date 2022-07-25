﻿using Models;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;

    // Use this for initialization
    protected override void OnStart () {
        ++NetClient.Instance.cnt;
        Debug.LogFormat("UIMainCity --------------------- {0}", NetClient.Instance.cnt);
        this.UpdateAvatar();

	}

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
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

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的：" + result, "对话框响应结果", MessageBoxType.Information);
    }
}