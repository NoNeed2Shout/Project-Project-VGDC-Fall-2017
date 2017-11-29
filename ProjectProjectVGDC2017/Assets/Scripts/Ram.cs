using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : MonoBehaviour {
    private bool ramming = false,recovering=false;
    private float speed,xBound,yBound;
    private int direction,stunTime;
    private Vector2 returnTo;
    //Should contain variables for a straightforward ramming attack. Maybe also adapt it to one where ramming a boundary spawns some projectiles.
    //Key for simple ramming: timer while keeping track of player (maybe make it stationary and rotating to face player for variety)
    //Include a visual cue before charge (should be a quick dash) as a "get out of the way" - probably same as the laser's?
    //Boss should be immobile for a little bit after end of charge (probably when hitting a wall) before rotating itself to default and moving back up

	// Use this for initialization
	void Start () {
        xBound = 5;//setting default out of bounds to 5 for now (bounds on other side will be -xBound and -yBound)
        yBound = 5;
        stunTime = 20;//20 "frames" stunned on impact before moving back up for now
	}
    public void startRam(float sp,int dir,Vector2 dest)
    {
        ramming = true;
        speed = sp;
        direction = dir;
        returnTo = dest;
    }
	// Update is called once per frame
	void Update () {
        if (ramming)
        {
            transform.position = new Vector2(transform.position.x + (speed * Mathf.Cos(Mathf.Deg2Rad * direction)), transform.position.y - (speed * Mathf.Sin(Mathf.Deg2Rad * direction)));
            if (transform.position.x > xBound || transform.position.x < -xBound || transform.position.y > yBound || transform.position.y < -yBound)
            {
                ramming = false;
                recovering = true;
            }
        }
        else if (recovering)
        {
            //if we want the ramming attack to spawn projectiles, do it at an if(stunTime==20) at the top (would spawn all projectiles at once)
            if (stunTime != 0)
                --stunTime;
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - returnTo, speed);
                if ((Vector2)transform.position == returnTo)
                {
                    stunTime = 20;
                    recovering = false;
                }
                //moves back toward returnTo's position, then sets recovering to false
            }
        }
        //if recovering and ramming are false, it should do nothing
	}
}
