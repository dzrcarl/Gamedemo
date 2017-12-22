using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanboMovement : MonoBehaviour {

    public float m_Speed = 10f;
    public float m_TurnSpeed = 100f;

    //private string m_MovementAxisName;
    //private string m_TurnAxisName;
    //private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private bool needFreeze = false;
    CharacterController m_ctrl;

    static Animator anim;

    // initial setup functions
    //private void Awake()
    //{
    //    m_Rigidbody = GetComponent<Rigidbody>();
    //}


    //private void OnEnable()
    //{
    //    m_Rigidbody.isKinematic = false;
    //    m_MovementInputValue = 0f;
    //    m_TurnInputValue = 0f;
    //}


    //private void OnDisable()
    //{
    //    m_Rigidbody.isKinematic = true;
    //}


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        //m_ctrl = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        CharacterMove();

        // state transition idle and walk
        // post cond': character is either moving forward or turning
        if (m_MovementInputValue != 0 || m_TurnInputValue != 0) {
            anim.SetBool("isWalking", true);
        }
        else {
            anim.SetBool("isWalking", false);
        }

        // state transition idle to pickUp(grib)
        // post cond': standing in front of food or bowl, not carrying anything, and PickUp key is pressed

        // state transition pickup to walk with stuff
        // post cond': has item on hand and walking
        // if () {}

        // state transition pickUp/walk with stuff to idle with stuff
        // post cond': has item on hand and not walking
        // if () {}

        // state transition idle with stuff to putDown
        // post cond': has item on hand, standing in front of desk/bowl, not walking(no speed)
        // if (m_MovementInputValue != 0 && m_TurnInputValue != 0 && ) {}

        // state transition putDown to idle
        // post cond': alwasy true! check nothing at hand
        // if () {}

        // state transition idle to cutting
        // post cond': food on the chopping board, player selected into cutting mode, cutting progress is not finished
        // if () {}

        // state transition cutting to idle
        // post cond': cutting progress finished, player chose to leave cutting mode
        // if () {}

        //state transition idle to washing
        // post cond': dish in the sink, player selected into washing mode, washing progress is not finished
        // if () {}
    }

    // character move
    private void CharacterMove()
    {
        m_MovementInputValue = Input.GetAxis("Vertical") * m_Speed * Time.deltaTime;
        m_TurnInputValue = Input.GetAxis("Horizontal") * m_TurnSpeed * Time.deltaTime;

        transform.Translate(0, 0, m_MovementInputValue);
        transform.Rotate(0, m_TurnInputValue, 0); // turn speed need optimization
    }


}
