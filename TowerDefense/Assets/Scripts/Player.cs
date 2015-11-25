using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{ 
    public LivingThing status;
    private float lifeTime;

	// Use this for initialization
	void Start () {
        lifeTime = 0;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Time : " + lifeTime);
        GUI.Label(new Rect(10, 20, 200, 20), "Best : " + PlayerPrefs.GetFloat("Time survived"));
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
	    if(status.life <= 0)
        {
            float prev = PlayerPrefs.GetFloat("Time survived");
            PlayerPrefs.SetFloat("Time survived", Mathf.Max(prev, lifeTime));
            GUI.Label(new Rect(200, 200, 500, 250), "Game Over");
            Time.timeScale = 0;
        }
	}
}
