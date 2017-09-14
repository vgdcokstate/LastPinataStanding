using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerup : Powerup {

	/// <summary>Initializes a new instance of the <see cref="InvincibilityPowerup"/> class.</summary>
	/// <param name="p">Powerup priority</param>
	/// <param name="n">Powerup name</param>
	/// <param name="t">Timer interval in milliseconds</param>
	public InvincibilityPowerup(int p, string i, int t) : base(p, i, t)	{}

	/// <summary>Causes powerup to act upon the character by rendering the character invincible</summary>
	/// <remarks>Implementation of <see cref="Powerup"/>'s  abstract Act() method</remarks>
	public override void Act()
	{
		character.SetInvincible(true);
	}
}
