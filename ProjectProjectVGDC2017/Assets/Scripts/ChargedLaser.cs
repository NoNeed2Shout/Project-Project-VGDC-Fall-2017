using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaser : MonoBehaviour {
    private int charging = 0, timeLeft, timeStays, timeGrow; //0 for charging, 1 for firing, 2 for full size, 3 is "should be removed"
    private float xScale, yScale, growthRate, y, maxWidth; //can just use boss's x, but y does differ from boss's at times
    public GameObject boss;
    // Use this for initialization
    void Start() {
        xScale = 1;
        yScale = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update() {
        this.transform.position = new Vector2(boss.transform.position.x, y);
        this.transform.localScale = new Vector2(xScale, yScale);
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
            y -= growthRate / 100; //tentative, change in y since scale increases up and down
            --timeGrow;
            if (timeGrow == 0)
                charging = 2;
        }
        else if (charging == 2)
        {
            if (maxWidth > this.transform.localScale.x) //actually fires - laser expands scale to maxWidth, stays until timeStays runs out
            {
                xScale += growthRate;
            }
            --timeStays;
            if (timeStays == 0)
                charging = 3; //use getPhase to get rid of lasers where getPhase() returns 3
        }
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"&&charging==2)
        {
            //Stuff happens when hitting player while laser is actually firing
        }
    }
}
