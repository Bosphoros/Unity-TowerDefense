using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.Audio;

public class UIPanel : MonoBehaviour {

    public string panelID;
    public bool hideOnLoad = true;
	// Use this for initialization
	void Start () {
        UIManager.GetInstance().RegisterPanel(gameObject, panelID);
        if(hideOnLoad)
            UIManager.GetInstance().HidePanel(panelID);
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void ClosePanel()
    {
        UIManager.GetInstance().HidePanel(panelID);
    }

}
