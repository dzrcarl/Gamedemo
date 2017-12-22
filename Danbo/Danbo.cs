using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danbo : MonoBehaviour {

	static Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) {
            anim.SetBool("isWalking", true);
        }
        else {
            anim.SetBool("isWalking", false);
        }
	}
}
