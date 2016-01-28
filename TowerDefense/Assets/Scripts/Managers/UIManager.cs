using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager {

    private Dictionary<string, GameObject> panels;
    static private UIManager instance;
    private Stack<GameObject> open;

    private UIManager()
    {
        panels = new Dictionary<string, GameObject>();
    }

    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            instance = new UIManager();
        }
        return instance;
    }

    public bool RegisterPanel(GameObject c, string name)
    {
        if (!panels.ContainsKey(name))
        {
            panels.Add(name, c);
            return true;
        }
        return false;
    }

    public bool ShowPanel(string name)
    {
        if (panels.ContainsKey(name))
        {
            GameObject panel = panels[name];
			if (panel != null)
                panel.SetActive(true);
            return true;
        }
        return false;
    }

	public bool FadeInPanel(string name){
		if (ShowPanel (name)) {
			panels [name].GetComponent<UIPanel> ().FadeIn ();
			return true;
		}
		return false;
	}

    public bool HidePanel(string name)
    {
        if (panels.ContainsKey(name))
        {
            GameObject panel = panels[name];
            if(panel != null)
                panel.SetActive(false);
            return true;
        }
        return false;
    }

	public bool FadeOutPanel(string name){
		if (ShowPanel (name)) {
			panels [name].GetComponent<UIPanel> ().FadeOut ();
			return true;
		}
		return false;
	}

	public void HideAll() {
		foreach (GameObject go in panels.Values) {
			go.SetActive (false);
		}
	}

}
