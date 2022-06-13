using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {

    public SkillBridge.Message.NCharacterInfo info;

    public Text charClass;
    public Text chatName;
    public Image highlight;

    public bool Selected
    {
        get
        {
            return highlight.IsActive();
        }
        set
        {
            highlight.gameObject.SetActive(value);
        }
    }

	// Use this for initialization
	void Start () {
		if(info != null)
        {
            charClass.text = info.Class.ToString();
            chatName.text = info.Name;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
