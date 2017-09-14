using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTester : MonoBehaviour {

	public PlayerStats ch;

	// Use this for initialization
	void Start () {
		Debug.Log("It's aliiiiiiive!");
		InvincibilityPowerup a = new InvincibilityPowerup(1, "a", 5000);
		InvincibilityPowerup b = new InvincibilityPowerup(2, "b", 2000);
		InvincibilityPowerup c = new InvincibilityPowerup(3, "c", 10000);
		ch.AddPowerup(a);
		ch.AddPowerup(b);
		ch.AddPowerup(c);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Character is invincible: " + ch.IsInvincible());
	}
}
