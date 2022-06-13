using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIRegister : MonoBehaviour {

    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;
    public Button buttonCancel;

    public GameObject loginImg;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnRegister = this.OnRegister;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            MessageBox.Show("请输入用户名");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            MessageBox.Show("请输入密码");
        }
        else if(string.IsNullOrEmpty(passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
        }
        else if(!string.Equals(password.text, passwordConfirm.text))
        {
            MessageBox.Show("您两次输入的密码不一致");
        }
        else
        {
            UserService.Instance.SendRegister(username.text, password.text);
        }
    }

    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        UnityAction action = null;
        if (result == SkillBridge.Message.Result.Success)
        {
            action = this.ShowLogin;
        }

        MessageBox.Show(msg, result.ToString(), MessageBoxType.Information, "", "", action);
    }

    void ShowLogin()
    {
        loginImg.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
