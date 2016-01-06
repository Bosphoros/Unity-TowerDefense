using UnityEngine;
using System.Collections;

public class CharacterCinematic : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat("Walk", .5f);
        //animator.SetFloat("Run", 0.2f);
        //animator.SetFloat("Turn", h);
    }
}
