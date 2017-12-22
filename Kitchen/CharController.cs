using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public float m_Speed = 10f;
	public float m_TurnSpeed = 100f;

	private float m_MovementInputValue;
	private float m_TurnInputValue;
	private bool needFreeze = false;
	CharacterController m_ctrl;

	static Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		CharacterMove();

		if (m_MovementInputValue != 0 || m_TurnInputValue != 0) {
			anim.SetBool("isWalking", true);
		}
		else {
			anim.SetBool("isWalking", false);
		}
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
