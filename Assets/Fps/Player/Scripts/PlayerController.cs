using System;
using UnityEngine;


[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (AudioSource))]
public class PlayerController : MonoBehaviour {

	[SerializeField] private float walkSpeed = 5f;
	[SerializeField] private float runSpeed = 10f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float gravityMultiplier = 2;
	[SerializeField] private PlayerMouseLook m_PlayerMouseLook;


	private Vector3 moveDirection = Vector3.zero;
	private Vector2 moveInput = Vector2.zero;
	private CharacterController charController;
	private bool isJump = false;
	private bool isLastTimeGrounded = false;

	private Camera m_Camera;
	private AudioSource m_AudioSource;
	private Vector3 m_CameraOriginalPosition;

	// Use this for initialization
	void Start () {
		charController = GetComponent<CharacterController> ();
		m_Camera = Camera.main;
		m_CameraOriginalPosition = m_Camera.transform.localPosition;
		m_AudioSource = GetComponent<AudioSource> ();

		m_PlayerMouseLook.Init (transform, m_Camera.transform);
	}
	
	// Update is called once per frame
	private void Update () {
		RotateView ();


		if (!isJump) {
			if(Input.GetButtonDown ("Jump"))
			{
				Jump ();
			}

		}

		if (!isLastTimeGrounded && charController.isGrounded) {
			moveDirection.y = 0f;
			//isJump = false;
		} else {

		}

		isLastTimeGrounded = charController.isGrounded;

	}

	void FixedUpdate()
	{
		Move ();


		UpdateCameraPosition ();
		m_PlayerMouseLook.UpdateCursorLock ();
	}


	#region movements
	private void Move()
	{
		// read input
		float moveX = Input.GetAxis ("Horizontal");
		float moveForward = Input.GetAxis ("Vertical");
		moveInput = new Vector2 (moveX, moveForward);
		if (moveInput.sqrMagnitude > 1) {
			moveInput.Normalize ();
		}

		// handle move input 
		Vector3 desiredPos = transform.forward * moveInput.y + transform.right * moveInput.x;

		moveDirection.z = desiredPos.z * runSpeed;
		moveDirection.x = desiredPos.x * runSpeed;

		// check ground or jumping
		if (charController.isGrounded) {
			if (isJump) {
				moveDirection.y = jumpForce;
				isJump = false;
			}
		} else {
			moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
		}


		charController.Move (moveDirection * Time.fixedDeltaTime);
	}

	private void Jump()
	{
		isJump = true;
	}


	private void UpdateCameraPosition()
	{
		
	}

	private void RotateView()
	{
		m_PlayerMouseLook.LookRotation (transform, m_Camera.transform);
	}
	#endregion
}
