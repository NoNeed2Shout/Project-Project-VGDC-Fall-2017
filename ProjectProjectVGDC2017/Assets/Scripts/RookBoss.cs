using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookBoss : MonoBehaviour {
    //not used to bosses rotating between patterns so my current idea is that the int for pattern determines the pattern currently used
    //timer is how much time left on the pattern, and if there are specific actions (eg projectiles spawning) then they spawn on
    //timer%something

    //I'm numbering phases in the order I create them, I guess. phase 1: aimed projectiles. phase 2: move side-to-side, unaimed projectiles
    
    //put in a berserk bool in case we want to make the boss have a low hp rage mode or desperation attack
    private int health=100,maxHealth=100,timer,pattern;
    private bool moving=false,berserk=false;
    private float moveSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (pattern == 1)
        {
        }
        else if (pattern == 2)
        {
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //stuff about boss getting hit by sword, triggering contact damage to player goes here
        //projectiles created by boss will need to have their own script, sprite
    }
}
