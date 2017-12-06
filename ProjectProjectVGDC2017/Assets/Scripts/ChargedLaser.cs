using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaser : MonoBehaviour {
    private int charging = 0, timeLeft, timeStays, timeGrow; //0 for charging, 1 for firing, 2 for full size, 3 is "should be removed"
    private float xScale, yScale, growthRate, y, maxWidth; //can just use boss's x, but y does differ from boss's at times
    public GameObject boss;
    // Use this for initialization
    void Start() {
        xScale = .25f;
        yScale = this.transform.localScale.x;
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update() {
		if (boss == null) {
			Destroy (gameObject);
		}
        transform.position = new Vector2(boss.transform.position.x, y);
        transform.localScale = new Vector2(xScale, yScale);
        if (charging == 0) //actually charging - if boss should match player's x during this time, do that in the boss script
        {
            --timeLeft;
            if (timeLeft == 0)
                charging = 1;
        }
        else if (charging == 1)
        {
            //may want the y stuff to be constant
            yScale += growthRate; //drops a line down
            y -= growthRate/2; //tentative, change in y since scale increases up and down
            --timeGrow;
            if (timeGrow == 0)
                charging = 2;
        }
        else if (charging == 2)
        {
            if (maxWidth > this.transform.localScale.x) //actually fires - laser expands scale to maxWidth, stays until timeStays runs out
            {
                xScale += growthRate/10;
            }
            --timeStays;
            if (timeStays == 0)
                charging = 3; //use getPhase to get rid of lasers where getPhase() returns 3
        }
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(xScale/2, yScale);
        //this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, y);
    }
    public int timeLeftInPlay() { return timeLeft + timeGrow + timeStays; } //if this ==0, attack ends - currently just despawn i guess
    public int getPhase() { return charging; }
    public void spawnCharge(float yPos, float w, float rate,int time,int growthDuration,int linger)
    {
        timeLeft = time; //time left charging
        growthRate = rate;
        y = yPos;
        maxWidth = w;
        timeGrow = growthDuration; //time left launching - should be really short
        timeStays = linger; //time until end of attack
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && charging == 2)
        {
            Debug.Log("Laser hit in damage phase");
        }
    }
}
