using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisProjectile : Projectile {
    //private int reflectsLeft = 4;
    //Above line would be for requiring the player to hit the shot back and forth with the boss
    public bool reflected = false;
    private GameObject boss, player;
	// Use this for initialization

    //Slow-moving tracking shot, when hit by the player it will reflect and hit the boss (tracks to boss, faster speed, won't hurt player after reflection)
	void Start () {
        boss = GameObject.FindGameObjectWithTag("Boss");
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (!reflected)
        {
            setDirection(transform.position-player.transform.position); //should move straight at player initially
        }
        else
        {
            setDirection(transform.position-boss.transform.position); //will move straight at boss upon reflect
        }
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - getDirection(), getSpeed());
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!reflected)
        {
            if (other.gameObject.tag == "Player")
            {
                //Stuff happens when hitting player
				//player takes damage
				Destroy(gameObject);
            }
            if (other.gameObject.tag=="Attack") //Assumes attack will produce a hitbox where the sword swing is and that hitbox will be tagged "Attack"
            {
                reflected = true;
                setSpeed(getSpeed() * 2);//doubles speed on reflect - subject to change on test
            }
        }
        if (other.gameObject.tag == "Boss")
        {
            if (reflected)
            {
				Debug.Log("Hit boss");
                other.GetComponent<RookBoss>().takeDamage(34);//since I've set it to 100 max health, for now each hit will be 1/3 of health
                Destroy(this.gameObject);
            }
        }
    }
    public void spawnAimed(Vector2 p, float s) //use only spawnAimed for this
    {
        transform.position = p;
        setSpeed(s);
    }
}