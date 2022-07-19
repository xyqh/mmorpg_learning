using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    // 单例不能写Start，要写重载过的OnStart
    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    void OnCharacterLeave(Character cha)
    {
        if (Characters.ContainsKey(cha.entityId) && Characters[cha.entityId] != null)
        {
            Destroy(Characters[cha.entityId]);
            Characters.Remove(cha.entityId);
        }
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            //GameObject go = (GameObject)Instantiate(obj);
            GameObject go = (GameObject)Instantiate(obj, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), gameObject.transform);
            Debug.LogFormat("scene name:{0}", go.gameObject.scene.name);
            go.name = "Character_" + character.entityId + "_" + character.Info.Name;

            // 逻辑坐标转成世界坐标
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            Characters[character.entityId] = go;


            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        InitGameObject(Characters[character.entityId], character);
    }

    private void InitGameObject(GameObject go, Character character)
    {
        // 逻辑坐标转成世界坐标
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
        }
            
        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.entityId == User.Instance.CurrentCharacter.Entity.Id)
            {
                // 当前角色对象
                User.Instance.currentCharacterObj = go;
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
    }
}
