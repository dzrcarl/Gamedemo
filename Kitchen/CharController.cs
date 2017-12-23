using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public float m_Speed = 10f;
	public float m_TurnSpeed = 100f;

	private Transform thing;
	private Rigidbody m_Rigidbody;
	private float m_MovementInputValue;
	private float m_TurnInputValue;
	private bool needFreeze = false;
	CharacterController m_ctrl;

	private Vector3 snapOffSet;

	static Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		snapOffSet = new Vector3(Mathf.Sin((this.transform.rotation.eulerAngles.y * Mathf.PI)/180) * 4f, 5f, Mathf.Cos((this.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * 4f);
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

		if (thing != null) {
			// Do pick up and drop down
		}
	}

	// character move
	private void CharacterMove()
	{
		m_MovementInputValue = Input.GetAxis("Vertical") * m_Speed * Time.deltaTime;
		m_TurnInputValue = Input.GetAxis("Horizontal") * m_TurnSpeed * Time.deltaTime;

        Vector3 movement = transform.forward * m_MovementInputValue;
        Quaternion turnRotation = Quaternion.Euler(0f,m_TurnInputValue,0f);
		//transform.Translate(0, 0, m_MovementInputValue);
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
		//transform.Rotate(0, m_TurnInputValue, 0);
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
	}
}
