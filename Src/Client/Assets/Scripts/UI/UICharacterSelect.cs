using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using Models;

public class UICharacterSelect : MonoBehaviour {

    private List<GameObject> characters = new List<GameObject>();

    public GameObject panelCreate;
    public GameObject panelSelect;
    public Transform scrollView;

    private int curIdx = -1;
    List<NCharacterInfo> chars = new List<NCharacterInfo>();

    // Use this for initialization
    void Start () {
        UserService.Instance.OnCreateCharacter = OnCreateCharacter;
        chars = User.Instance.Info.Player.Characters;

        GameObject cacheObject = Resloader.Load<GameObject>("UI/UICharInfo");
        for (int i = 0; i < chars.Count; ++i)
        {
            NCharacterInfo info = chars[i];
            GameObject buttonObject = Instantiate(cacheObject, scrollView);
            UICharInfo chrInfo = buttonObject.GetComponent<UICharInfo>();
            chrInfo.info = info;

            Button button = buttonObject.GetComponent<Button>();
            int idx = i;
            button.onClick.AddListener(() =>
            {
                OnSelectCharacter(idx);
            });
            characters.Add(buttonObject);
            buttonObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCreateCharacter(Result result, string msg)
    {
        if(result == Result.Success)
        {
            changeView(false);
        }
    }

    void changeView(bool isCreate)
    {
        panelCreate.SetActive(isCreate);
        panelSelect.SetActive(!isCreate);
    }

    public void OnClickCreate()
    {
        changeView(true);
    }

    public void OnClickEnter()
    {

    }

    void OnSelectCharacter(int idx)
    {
        Debug.Log(string.Format("OnSelectCharacter {0}", idx));
        curIdx = idx;
        var cha = chars[idx];
        User.Instance.CurrentCharacter = cha;
        panelCreate.GetComponent<UIPanelCreate>().changeCurSelChar(cha.Tid - 1);
    }
}
