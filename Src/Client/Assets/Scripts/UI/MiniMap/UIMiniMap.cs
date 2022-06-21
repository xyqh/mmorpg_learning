using Entities;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {

    public Character character;
    public Text mapName;
    public Image map;
    public Image arrow;
    public Collider mapBoundingBox;
    private Transform playerTransform;

	// Use this for initialization
	void Start () {
        updateMap();
    }
	
	// Update is called once per frame
	void Update () {
        float realWidth = mapBoundingBox.bounds.size.x;
        float realHeight = mapBoundingBox.bounds.size.z;

        // xz平面
        float posX = playerTransform.position.x - mapBoundingBox.bounds.min.x;
        float posY = playerTransform.position.z - mapBoundingBox.bounds.min.z;

        // 计算中心点
        float pivotX = posX / realWidth;
        float pivotY = posY / realHeight;

        // 改变中心点
        map.rectTransform.pivot = new Vector2(pivotX, pivotY);
        // 设0
        map.rectTransform.localPosition = Vector2.zero;

        // 箭头旋转
        arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }

    public void updateMap()
    {
        mapName.text = User.Instance.currentMapDef.Name;
        // overrideSprite 优先展示override，当override为null时，展示sprite
        map.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();

        map.SetNativeSize();
        map.transform.localPosition = Vector3.zero;
        this.playerTransform = User.Instance.currentCharacterObj.transform;
    }
}
