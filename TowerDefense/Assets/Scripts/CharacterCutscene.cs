using UnityEngine;
using System.Collections;

public class CharacterCutscene : MonoBehaviour {

    Animator animator;
	public bool walk;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
		walk = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
		if(walk) {
        	animator.SetFloat("Walk", .5f);
	        //animator.SetFloat("Run", 0.2f);
	        //animator.SetFloat("Turn", h);
		}
    }
}
