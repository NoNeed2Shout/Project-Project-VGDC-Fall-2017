using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour {
    private bool ramming = false;
    private float speed,xBound,yBound;
    private int direction,cooldown;
    //Should contain variables for a straightforward ramming attack. Maybe also adapt it to one where ramming a boundary spawns some projectiles.
    //Key for simple ramming: timer while keeping track of player (maybe make it stationary and rotating to face player for variety)
    //Include a visual cue before charge (should be a quick dash) as a "get out of the way" - probably same as the laser's?
    //Boss should be immobile for a little bit after end of charge (probably when hitting a wall) before rotating itself to default and moving back up

	// Use this for initialization
	void Start () {
        xBound = 5;//setting default out of bounds to 5 for now (bounds on other side will be -xBound and -yBound)
        yBound = 5;
	}
    public void startRam(float sp,int dir,int cd)
    {
        ramming = true;
        speed = sp;
        direction = dir;
        cooldown = cd;
    }
	// Update is called once per frame
	void Update () {
        if (ramming)
        {
            //transform.position=Vector2.MoveTowards(transform.position,(Vector2)transform.position-direction,speed);
            if (transform.position.x > xBound || transform.position.x < -xBound || transform.position.y > yBound || transform.position.y < -yBound)
            { ramming = false; }
        }
	}
}
