using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playerControl : MonoBehaviour {

	public float moveSpeed;

	private Animator anim;

	private bool playerMoving;

	private Vector2 lastMove;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

		playerMoving = false;
		//player attacks
		if (Input.GetKeyDown(KeyCode.U)){
			Debug.Log("U key was pressed.");
		}
		if (Input.GetKeyDown(KeyCode.I)){
			Debug.Log("I key was pressed.");
		}


		//player movements
		if ((Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) && (Input.GetAxisRaw ("Vertical") == 0f))
		{
			transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime,0f,0f));
			playerMoving = true;
			lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		}
		else if ((Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) && (Input.GetAxisRaw ("Horizontal") == 0f))
		{
			transform.Translate (new Vector3 (0f,Input.GetAxisRaw ("Vertical") * moveSpeed * Time.deltaTime,0f));
			playerMoving = true;
			lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
		}

		anim.SetFloat ("moveX", Input.GetAxisRaw ("Horizontal"));
		anim.SetFloat ("moveY", Input.GetAxisRaw ("Vertical"));
		anim.SetBool("playerMoving",playerMoving);
		anim.SetFloat ("lastMoveX", lastMove.x);
		anim.SetFloat ("lastMoveY", lastMove.y);
	}
}
