using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    void OnDestroy()
    {
        UIManager.GetInstance().HidePanel("TitleScreen");
        UIManager.GetInstance().ShowPanel("Hud");
    }
}
