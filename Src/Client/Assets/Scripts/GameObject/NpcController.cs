using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using System;
using Models;

public class NpcController : MonoBehaviour {

    public int npcId;
    Animator anim;
    NpcDefine npcDefine;
    SkinnedMeshRenderer renderer;
    Color originColor;
    private bool inInteractive = false;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        npcDefine = NPCManager.Instance.GetNpcDefine(npcId);
        renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = renderer.sharedMaterial.color;
        StartCoroutine(Actions());
	}

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            }
            Relax();
        }
    }

    private void Relax()
    {
        anim.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInterative());
        }
    }

    IEnumerator DoInterative()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npcDefine))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.currentCharacterObj.transform.position - this.transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(gameObject.transform.forward, faceTo)) > 5)
        {
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnMouseDown()
    {
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if(renderer.sharedMaterial.color != Color.white)
            {
                renderer.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (renderer.sharedMaterial.color != originColor)
            {
                renderer.sharedMaterial.color = originColor;
            }
        }
    }
}
