using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

    private Dictionary<int, GameObject> gos = new Dictionary<int, GameObject>();
    private GameObject cacheCharObject = null;
    private int preCharId = -1;

    // Use this for initialization
    void Start () {
        EventManager.Instance.addEventListener("changeShowCharacter", changeShowCharacter);
    }

    // Update is called once per frame
    void Update () {

    }

    void changeShowCharacter(params object[] param)
    {
        int charId = (int)param[0];
        if(!DataManager.Instance.ICharacters.ContainsKey(charId))
        {
            return;
        }

        if (gos.ContainsKey(preCharId))
        {
            gos[preCharId].SetActive(false);
        }

        preCharId = charId;
        CharacterDefine cDef = DataManager.Instance.ICharacters[charId];
        GameObject go = null;
        if (gos.ContainsKey(charId))
        {
            Debug.Log("changeShowCharacter find in dic");
            go = gos[charId];
        }
        else
        {
            Debug.Log("changeShowCharacter can not find in dic");
            cacheCharObject = Resloader.Load<GameObject>(cDef.Resource);
            go = Instantiate(cacheCharObject, GameObject.Find("Root").transform);
            go.GetComponent<PlayerInputController>().isActive = false;
            gos[charId] = go;
        }

        go.transform.localPosition = new Vector3(0, 0, 0);
        go.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.Instance.removeEventListener("changeShowCharacter", changeShowCharacter);
    }
}
