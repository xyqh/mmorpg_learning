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
    private List<NCharacterInfo> chars = new List<NCharacterInfo>();

    // Use this for initialization
    void Start () {
        UserService.Instance.OnCreateCharacter = OnCreateCharacter;
        updateCharList();
    }

    private void OnEnable()
    {
        OnSelectCharacter(0);
    }

    void updateCharList()
    {
        foreach(var go in characters)
        {
            Destroy(go);
        }
        characters.Clear();

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

        if(chars.Count > 0)
        {
            OnSelectCharacter(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCreateCharacter(Result result, string msg)
    {
        if(result == Result.Success)
        {
            updateCharList();
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
        if(curIdx >= 0)
        {
            UserService.Instance.SendGameEnter(curIdx);
        }
    }

    void OnSelectCharacter(int idx)
    {
        Debug.Log(string.Format("OnSelectCharacter {0}", idx));
        if(idx < 0 || idx >= characters.Count)
        {
            return;
        }

        if(0 <= curIdx && curIdx < characters.Count)
        {
            characters[curIdx].GetComponent<UICharInfo>().Selected = false;
        }
        curIdx = idx;
        characters[curIdx].GetComponent<UICharInfo>().Selected = true;
        var cha = chars[idx];
        User.Instance.CurrentCharacter = cha;
        EventManager.Instance.dispatchCustomEvent("changeShowCharacter", cha.Tid);
    }
}
