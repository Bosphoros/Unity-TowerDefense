using UnityEngine;
using System.Collections;

public class MainCanvas : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
