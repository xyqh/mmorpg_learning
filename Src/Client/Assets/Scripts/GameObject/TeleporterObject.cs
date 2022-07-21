using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour {

    public int ID;
    Mesh mesh = null;

	// Use this for initialization
	void Start () {
         mesh = GetComponent<Mesh>();
    }

    // Update is called once per frame
    void Update () {

	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(mesh != null)
        {
            Gizmos.DrawWireMesh(mesh, transform.position + Vector3.up * transform.localScale.y * .5f, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1f, EventType.Repaint);
    }
#endif

    void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("TeleporterObject:OnTriggerEnter:{0}", ID);
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        if(playerController != null && playerController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.ITeleporters[ID];
            if(td == null)
            {
                Debug.LogFormat("TeleporterObject:Character [{0}] Enter Teleporter [{1}], But TeleporterDefine not existed", playerController.character.Info.Name, ID);
                return;
            }
            Debug.LogFormat("TeleporterObject:Character [{0}] Enter Teleporter [{1}:{2}]", playerController.character.Info.Name, td.ID, td.Name);
            if(td.LinkTo > 0)
            {
                if (DataManager.Instance.ITeleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleport(ID);
                }
                else
                {
                    Debug.LogFormat("Teleporter ID:{0} LinkID {1} error!", td.ID, td.LinkTo);
                }
            }
        }
    }
}
