using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour {

    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;

    public virtual System.Type Type { get { return GetType(); } }

    public enum WindowResult {
        None = 0,
        Yes,
        No
    }

    public void Close(WindowResult result = WindowResult.None)
    {
        UIManager.Instance.Close(Type);
        if(OnClose != null)
        {
            OnClose(this, result);
        }
        OnClose = null;
    }

    public virtual void OnCloseClick()
    {
        Close();
    }

    public virtual void OnYesClick()
    {
        Close(WindowResult.Yes);
    }

    private void OnMouseDown()
    {
        //Debug.Log()
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
