using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

public class playerControl : MonoBehaviour {

	//private Rigidbody2D rb2d;

	public float moveSpeed;
	private int grayShots = 17, whiteShots=17; //projectile damage
	private Animator anim;
	private bool playerMoving;
	private GameObject player;
	private Vector2 lastMove;
	private bool lightAttack;
	private string direction;

	// Use this for initialization
	void Start () {
		//rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		lastMove.y = -1f;
		direction = "down";
		player = GameObject.FindGameObjectWithTag ("Player");

	}
	void playerTakeDamage(int amount)
	{
		player.GetComponent<HealthBar>().TakeDamage(amount);
	}
	bool playerDead()
	{
		return player.GetComponent<HealthBar>().PlayerDead ();
	}
	int getPlayerHealth()
	{
		return player.GetComponent<HealthBar> ().currentHealth;
	}
	// Update is called once per frame
	void Update () {
		if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Blend Tree")) {
			gameObject.transform.GetChild (0).gameObject.SetActive (false);
		}
		lightAttack = false;
		playerMoving = false;
		Debug.Log (getPlayerHealth ());


		//player movements
		if ((Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) && (Input.GetAxisRaw ("Vertical") == 0f))
		{
			transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime,0f,0f));
			playerMoving = true;
			lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
			if (Input.GetAxisRaw ("Horizontal") > 0.5f)
			{
				direction = "right";
			}
			else
			{
				direction = "left";
			}
		}
		else if ((Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) && (Input.GetAxisRaw ("Horizontal") == 0f))
		{
			transform.Translate (new Vector3 (0f,Input.GetAxisRaw ("Vertical") * moveSpeed * Time.deltaTime,0f));
			playerMoving = true;
			lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
			if (Input.GetAxisRaw ("Vertical") > 0.5f)
			{
				direction = "up";
			}
			else
			{
				direction = "down";
			}
		}

		//player attacks
		if (Input.GetKeyDown(KeyCode.U)){
			lightAttack = true;
			if (GameObject.Find ("PlayerAttack") == null) {
				//Debug.Log ("Attacking");
				if (direction == "up") 
				{
					Debug.Log ("attack up");
					gameObject.transform.GetChild (0).transform.position = new Vector3 (transform.position.x+0.03f, transform.position.y+0.3f, 0f);
				}
				else if (direction == "down") 
				{
					Debug.Log ("attack down");
					gameObject.transform.GetChild (0).transform.position = new Vector3 (transform.position.x+0.03f, transform.position.y-0.3f, 0f);
				}
				else if (direction == "right") 
				{
					Debug.Log ("attack right");
					gameObject.transform.GetChild (0).transform.position = new Vector3 (transform.position.x+0.12f, transform.position.y, 0f);
				}
				else if (direction == "left") 
				{
					Debug.Log ("attack left");
					gameObject.transform.GetChild (0).transform.position = new Vector3 (transform.position.x-0.15f, transform.position.y, 0f);
				}
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
			}
		}
		if (Input.GetKeyDown(KeyCode.I)){
			Debug.Log("I key was pressed.");
		}

		anim.SetFloat ("moveX", Input.GetAxisRaw ("Horizontal"));
		anim.SetFloat ("moveY", Input.GetAxisRaw ("Vertical"));
		anim.SetBool("playerMoving",playerMoving);
		anim.SetFloat ("lastMoveX", lastMove.x);
		anim.SetFloat ("lastMoveY", lastMove.y);
		anim.SetBool("animateAttack",lightAttack);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//stuff about boss getting hit by sword and/or triggering contact damage to player goes here
		//projectiles created by boss will need to have their own script, sprite
		if (other.gameObject.CompareTag("Boss")) {
			Debug.Log ("Player collides with boss");
			//take damage
		}
		if (other.gameObject.CompareTag("Projectile"))
		{
			//take damage
			playerTakeDamage(grayShots);

		}
		if (other.gameObject.CompareTag("TennisProjectile"))
		{
			//take damage
			playerTakeDamage(whiteShots);

		}
		if (playerDead ()) 
		{
			//die
			Debug.Log(getPlayerHealth());
			Debug.Log ("Im dead");
		}
	}
}
