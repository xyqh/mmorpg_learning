using Common.Data;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCreate : MonoBehaviour {

    private Dictionary<int, CharacterDefine> chars = new Dictionary<int, CharacterDefine>();
    private List<CharacterDefine> lChars = new List<CharacterDefine>();
    private List<Button> lButton = new List<Button>();
    private Dictionary<int, GameObject> gos = new Dictionary<int, GameObject>();
    private int preSelTag = -1;

    static GameObject cacheButtonObject = null;
    static GameObject cacheCharObject = null;

    public List<Sprite> pressSprites;
    public List<Sprite> normalSprites;
    public List<Texture> topNames;
    public InputField charName;
    public RawImage charNameTop;
    public Text charDesc;

    // Use this for initialization
    void Start () {
        if (DataManager.Instance.ICharacters == null) return;

        foreach(KeyValuePair<int, CharacterDefine> kv in DataManager.Instance.ICharacters) {
            if(kv.Key < 1000)
            {
                lChars.Add(kv.Value);
            }
        }

        int charNum = lChars.Count;
        for(int i = 0; i < charNum; ++i)
        {
            cacheButtonObject = Resloader.Load<GameObject>("UI/Button_sel");
            GameObject go = Instantiate(cacheButtonObject, GameObject.Find("Panel_create").transform);
            go.transform.localPosition = new Vector3(330 + i * 150, -29, 0);
            Button button = go.GetComponent<Button>();

            button.GetComponent<Image>().sprite = normalSprites[i];
            SpriteState state = button.spriteState;
            state.pressedSprite = pressSprites[i];
            button.spriteState = state;

            button.name = i.ToString();
            Text txt = button.transform.Find("Text").gameObject.GetComponent<Text>();
            txt.text = lChars[i].Name;
            
            //button.onClick.AddListener(OnClickButton);
            button.onClick.AddListener(delegate ()
            {
                OnClickButton(button);
            });
            lButton.Add(button);

            if(i == 0)
            {
                OnClickButton(button);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnClickButton(Button btn)
    {
        Debug.Log("OnClickButton" + btn.name);
        string name = btn.name;
        int _name = Convert.ToInt32(name);
        if (_name < 0 || _name >= lChars.Count)
        {
            Debug.Log("OnClickButton _name invalid");
            return;
        }

        changeCurSelChar(_name);
    }

    public void changeCurSelChar(int _name)
    {
        CharacterDefine cDef = lChars[_name];
        charNameTop.texture = topNames[cDef.TID - 1];
        charDesc.text = cDef.Description;

        if (gos.ContainsKey(preSelTag))
        {
            gos[preSelTag].SetActive(false);
        }

        preSelTag = cDef.TID;
        GameObject go = null;
        if (gos.ContainsKey(cDef.TID))
        {
            Debug.Log("OnClickButton find in dic");
            go = gos[cDef.TID];
        }
        else
        {
            Debug.Log("OnClickButton can not find in dic");
            cacheCharObject = Resloader.Load<GameObject>(string.Format("Models/{0}", cDef.Resource));
            go = Instantiate(cacheCharObject, GameObject.Find("Root").transform);
            gos[cDef.TID] = go;
        }

        go.transform.localPosition = new Vector3(0, 0, 0);
        go.SetActive(true);
    }

    public void OnClickCreateChar()
    {
        Debug.Log(string.Format("OnClickCreateChar {0}", preSelTag));
        if (string.IsNullOrEmpty(charName.text))
        {
            MessageBox.Show("请输入角色名称");
            return;
        }
        if (preSelTag < 0 || preSelTag >= lChars.Count)
        {
            return;
        }
        UserService.Instance.SendCreateCharacter(lChars[preSelTag].Name, (CharacterClass)preSelTag + 1);
    }
}
