using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookBoss : MonoBehaviour {
    //not used to bosses rotating between patterns so my current idea is that the int for pattern determines the pattern currently used
    //timer is how much time left on the pattern, and if there are specific actions (eg projectiles spawning) then they spawn on
    //timer%something

    //I'm numbering phases in the order I create them, I guess. phase 1: aimed projectiles. phase 2: move side-to-side, unaimed projectiles

    //put in a berserk bool in case we want to make the boss have a low hp rage mode or desperation attack - not looking likely right now
    private int health=100,maxHealth=100,fired=0,phasesUntilTennis=4, pattern, timer;//fired to be used for bursts of projectiles
    private bool moving=false,berserk=false,timing=true;
    private float moveSpeed=.05f; //I don't plan to use physics when moving these guys. May need to add a baseMoveSpeed if we want to have movement
    //accelerate during some attacks
    public GameObject shot,boss,player,laserPrefab,tennisPrefab;
    private GameObject laser,tennis; //only one should exist at a time for phase 3 - an attack where the boss tries to match the player's x position
    //while charging a laser, then stops moving while firing straight down
    private List<GameObject> shots;
    //No animations for now
	// Use this for initialization
	void Start () {
        move(0);
        shots = new List<GameObject>();
        //changePattern();
		//only works properly when pattern = 3?
		pattern = 3;
        //change what Random.value is multiplied by to change the number of patterns - make the patterns first!
        timer = 100; //Leave it like this for now. If we want patterns to take different amount of times, use if statements later
    }
	
	// Update is called once per frame
	void Update () {
        //pattern = 2; //For testing a specific pattern
        if(health>0)
        {
            if (timing)
                --timer;
            if (phasesUntilTennis == 0)
            {
                tennis = Instantiate(tennisPrefab);
                tennis.GetComponent<TennisProjectile>().spawnAimed(boss.transform.position,.05f);
                phasesUntilTennis = 5; //defaulting to 4+1 for now so the shot will be fired again 4 phases after the phase it spawned in at earliest (see changePattern)
            }
            if (timer != 0)
            {
                if (pattern == 1) //move back and forth while throwing projectiles straight down
                {
                    if (transform.position.x <= -3 || transform.position.x >= 3)
                        moveSpeed = -moveSpeed;
                    move(1);
                    if (timer % 30 == 0) //Adds a projectile once every 30 updates - I tend to treat them like frames
                    {
                        shots.Add(Instantiate(shot));
                        shots[shots.Count - 1].GetComponent<Projectile>().spawnDirectional(transform.position, .1f, 90);
                    }
                }
                else if (pattern == 2) //throw projectiles at the player. Dunno if he'll move during it yet
                {
                    if (timer % 10 == 0 && fired < 4) //4 shot burst for now
                    {
                        ++fired;
                        shots.Add(Instantiate(shot));
                        if (fired % 2 == 0)
                            shots[shots.Count - 1].GetComponent<Projectile>().spawnAimed(transform.position + new Vector3(.5f, 0, 0), .15f, player.transform.position);
                        else
                            shots[shots.Count - 1].GetComponent<Projectile>().spawnAimed(transform.position - new Vector3(.5f, 0, 0), .15f, player.transform.position);
                    }
                    if (timer % 81 == 0 && fired == 4) //allows a wait time so a pattern 2 following a pattern 2 will feel like 2 4-shot bursts instead of 1 8-shot
                        fired = 0;
                }
                else if (pattern == 3) //charge a laser while tracking the player. Stop moving while firing (thin line drops down). Damage only
                                       //when laser is expanding or full
                {
                    if (laser == null)
                    {
                        timing = false;
                        laser = Instantiate(laserPrefab);
                        laser.GetComponent<ChargedLaser>().spawnCharge(transform.position.y, 2, 1, 60, 20, 40);
                    }
                    if (laser.GetComponent<ChargedLaser>().getPhase() == 0)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), Mathf.Abs(moveSpeed * 2));
                    if (laser.GetComponent<ChargedLaser>().getPhase() == 3)
                    {
                        timing = true;
                        timer = 1;
                        Destroy(laser);
                    }
                }
            }
			else
			{
				changePattern();
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
                if (x>5||x<-5||y>5||y<-5) //Destroys shots that go out of bounds
                    Destroy(s);
            }
        }
        if (tennis != null)
        {
            if (tennis.transform.position.x > 5 || tennis.transform.position.x < -5 || tennis.transform.position.y > 5 || tennis.transform.position.y < -5)
                Destroy(tennis);
			//need to add an if it hit the player or the boss, then destroy
        }
	}

    void changePattern()
    {
        if(tennis==null) //countdown to tennis show won't go down while a tennis shot exists
            --phasesUntilTennis;
        pattern = (int)(Random.value * 3) + 1; //If we want some patterns to follow others, this can be changed to if-elses too
        timer = 100; //Same as in Start for now
    }

    void move(int direction) //for now, 0=vertical,1=horizontal. Multiply moveSpeed by -1 to change direction
    //Maybe we'll need specific angles for the knight (if we get that far), but the rook and bishop bosses can probably make do with 0-3
    {
        if (direction == 0)
            transform.position = new Vector2(transform.position.x, transform.position.y+moveSpeed);
        else if(direction==1)
            transform.position = new Vector2(transform.position.x+moveSpeed, transform.position.y);
        //else if(direction==2)
        //    transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed);
        //else
        //    transform.position = new Vector2(transform.position.x-moveSpeed, transform.position.y);
    }
		
    public void takeDamage(int amount)
    {
        health -= amount;
        victory();//runs check on health, then whatever happens on win
    }

    void victory()
    {
        if (health <= 0)
        {
            Object.Destroy(this.gameObject); 
            //Currently would delete the boss as soon as health hits 0, would just pop out of existence suddenly
            //I think you're supposed to use coroutines if you wanna give it, say, some sprite flashing before death
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //stuff about boss getting hit by sword and/or triggering contact damage to player goes here
        //projectiles created by boss will need to have their own script, sprite
		if (other.gameObject.CompareTag("Attack")) {
			Debug.Log ("Attacked");
			takeDamage (25); //for testing, killed in 4 hits
		}
    }
}
