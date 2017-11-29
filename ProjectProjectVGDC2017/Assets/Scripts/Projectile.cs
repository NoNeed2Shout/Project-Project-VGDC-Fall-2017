using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private Vector2 position;
    private float speed, angle;
    public Vector2 direction;
    private bool aimed;
    //angle for shots fired at a specific angle (360 degrees)
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (aimed)
        {
            transform.position = Vector2.MoveTowards(transform.position,(Vector2)transform.position-direction,speed);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + (speed * Mathf.Cos(Mathf.Deg2Rad * angle)), transform.position.y - (speed * Mathf.Sin(Mathf.Deg2Rad * angle)));
        }
    }
    public void spawnAimed(Vector2 p,float s, Vector2 target)
    {
        transform.position = p;
        speed = s;
        direction = (Vector2)transform.position-target;
        aimed = true;
    }
    public void spawnDirectional(Vector2 p, float s, float dir)
    {
        transform.position = p;
        speed = s;
        angle = dir;
        aimed = false;
    }
    public Vector2 getPosition()
    {
        return transform.position;
    }
    public float getSpeed()
    {
        return speed;
    }
    public void incrementSpeed(float increment)
    {
        speed += increment;
    }
    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public Vector2 getDirection()
    {
        return direction;
    }
    public void setDirection(Vector2 v)
    {
        direction = v;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Stuff happens when hitting player
			Debug.Log("Projectile hit player");
        }
    }
}
