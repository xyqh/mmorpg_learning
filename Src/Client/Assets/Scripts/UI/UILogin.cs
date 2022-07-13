using Common;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

    public InputField username;
    public InputField password;
    public Button buttonRegister;
    public Button buttonLogin;

    public GameObject register;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnLogin = this.OnLogin;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnClickRegister()
    {
        register.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            MessageBox.Show("请输入用户名");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            MessageBox.Show("请输入密码");
        }
        else
        {
            UserService.Instance.SendLogin(username.text, password.text);
        }
    }

    void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        if (result == SkillBridge.Message.Result.Success)
        {
            LoginSuccess();
        }
    }

    void LoginSuccess()
    {
        Debug.Log("LoadingMesager::OnLoginSuccess");
        SceneManager.Instance.LoadScene("CharSelect");
    }
}
