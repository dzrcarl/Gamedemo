using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script for pot & pan.
// check: if pot&pan hasFood and isOnStove, then start cooking timer
// adjust the status of food inside accordingly
// snap pot&pan onto the center of stove when put down by the player (function done in HoldItem.cs)
public class Cooking : MonoBehaviour {

    public bool isOnStove;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //

        // snap position method from unityForum
        //void OnTriggerEnter(Collider other) {
        //    if (other.gameObject.CompareTag("the_tag_which_you_kept_earlier") {
        //        other.rigidbody.MovePosition(transform.position);
        //    }
        //}
    }
}
