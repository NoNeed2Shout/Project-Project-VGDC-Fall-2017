using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : MonoBehaviour 
{
	void Start()
	{
	}
	void Update(){}

	public const int maxHealth = 100;
	public int currentHealth = maxHealth;

	public void TakeDamage(int amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
		}
	}
	public bool PlayerDead()
	{
		return (currentHealth <= 0);
	}
}
