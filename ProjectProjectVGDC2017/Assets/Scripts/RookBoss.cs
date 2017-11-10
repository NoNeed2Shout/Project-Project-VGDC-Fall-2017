using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookBoss : MonoBehaviour {
    //not used to bosses rotating between patterns so my current idea is that the int for pattern determines the pattern currently used
    //timer is how much time left on the pattern, and if there are specific actions (eg projectiles spawning) then they spawn on
    //timer%something

    //I'm numbering phases in the order I create them, I guess. phase 1: aimed projectiles. phase 2: move side-to-side, unaimed projectiles
    
    //put in a berserk bool in case we want to make the boss have a low hp rage mode or desperation attack
    private int health=100,maxHealth=100,timer,fired=0;//fired to be used for bursts of projectiles
    public int pattern;
    private bool moving=false,berserk=false,timing=true;
    private float moveSpeed=.05f; //I don't plan to use physics when moving these guys. May need to add a baseMoveSpeed if we want to have movement
    //accelerate during some attacks
    public GameObject shot,boss,player,laserPrefab;
    private GameObject laser; //only one should exist at a time for phase 3 - an attack where the boss tries to match the player's x position
    //while charging a laser, then stops moving while firing straight down
    private List<GameObject> shots;
    //No animations for now
	// Use this for initialization
	void Start () {
        move(0);
        shots = new List<GameObject>();
        pattern = (int)(Random.value * 2)+1; //This means 2 different patterns
        //change what Random.value is multiplied by to change the number of patterns - make the patterns first!
        timer = 100; //Leave it like this for now. If we want patterns to take different amount of times, use if statements later
    }
	
	// Update is called once per frame
	void Update () {
        //pattern = 2; //For testing a specific pattern
        if(timing)
            --timer;
        if (timer != 0)
        {
            if (pattern == 1) //move back and forth while throwing projectiles straight down
            {
                if (boss.transform.position.x <= -3 || boss.transform.position.x >= 3)
                    moveSpeed = -moveSpeed;
                move(1);
                if (timer % 30 == 0) //Adds a projectile once every 30 frames
                {
                    shots.Add(Instantiate(shot));
                    shots[shots.Count - 1].GetComponent<Projectile>().spawnDirectional(boss.transform.position, .1f, 90);
                }
            }
            else if (pattern == 2) //throw projectiles at the player. Dunno if he'll move during it yet
            {
                if (timer % 10 == 0 && fired < 4) //4 shot burst for now
                {
                    ++fired;
                    shots.Add(Instantiate(shot));
                    if (fired % 2 == 0)
                        shots[shots.Count - 1].GetComponent<Projectile>().spawnAimed(boss.transform.position + new Vector3(.5f, 0, 0), .09f, player.transform.position);
                    else
                        shots[shots.Count - 1].GetComponent<Projectile>().spawnAimed(boss.transform.position - new Vector3(.5f, 0, 0), .09f, player.transform.position);
                }
                if (timer % 81 == 0 && fired == 4)
                    fired = 0;
            }
            else if (pattern == 3) //charge a laser while tracking the player. Stop moving while firing (thin line drops down). Damage only
                //when laser is expanding or full
            {
                //gonna have to use Vector2.MoveTowards while tracking (when laser.getComponent<ChargedLaser>().getPhase()==0)
                //speed to be determined
                //do laser=Instantiate(laserPrefab); then laser.getComponent<ChargedLaser>().spawnCharge(y,maxWidth,expansion rate,charging time, prelaunch time (makde it short!), firing time)
                //destroy laser at the end of the pattern
                //During this pattern timer shouldn't decrement, set timer to 0 when laser is destroyed
                timing = true;//placeholder for now, although code currently doesn't let pattern = 3
            }
        }
        else
        {
            pattern = (int)(Random.value * 2)+1; //If we want some patterns to follow others, this can be changed to if-elses too
            timer = 100; //Same as in Start for now
            if (pattern == 3)
            {
                timing = false;
            }
            else
            {
                timing = true;
            }
        }
        for (int i = 0; i < shots.Count; ++i)
        {
            GameObject s = shots[i];
            if (s == null) { shots.Remove(s); }
            else
            {
                float x = s.GetComponent<Projectile>().getPosition().x;
                float y = s.GetComponent<Projectile>().getPosition().y;
                if (x>5||x<-5||y>5||y<-5)
                    Destroy(s);
            }
        }
	}

    void move(int direction) //for now, 0=vertical,1=horizontal. Multiply moveSpeed by -1 to change direction
    //Maybe we'll need specific angles for the knight, but the rook and bishop bosses can probably make do with 0-3
    {
        if (direction == 0)
            boss.transform.position = new Vector2(boss.transform.position.x,boss.transform.position.y+moveSpeed);
        else if(direction==1)
            boss.transform.position = new Vector2(boss.transform.position.x+moveSpeed, boss.transform.position.y);
        //else if(direction==2)
        //    boss.transform.position = new Vector2(boss.transform.position.x, boss.transform.position.y - moveSpeed);
        //else
        //    boss.transform.position = new Vector2(boss.transform.position.x-moveSpeed, boss.transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //stuff about boss getting hit by sword and/or triggering contact damage to player goes here
        //projectiles created by boss will need to have their own script, sprite
    }
}
