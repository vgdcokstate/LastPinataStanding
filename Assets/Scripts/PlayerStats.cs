using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	// BASE CHARACTER STATS

		/// <summary>Character's current **base** health</summary>
		public double health;
	
		/// <summary>Character's current position</summary>
		public Vector3 position;
	
		/// <summary>Power-ups which are currently acting upon the character</summary>
		public SortedList<int,Powerup> powerups;

	// MULTIPLIERS AND MODIFIERS
	
		/// <summary>Whether the character is currently invincible</summary>
		public bool invincible = false;

		/// <summary>Factor by which the character's speed should change</summary>
		public double speedMultiplier = 1.0;


	// HEALTH METHODS

		/// <summary>Gets the character's current health</summary>
		/// <returns>Character's health</returns>
		public double GetHealth()
		{
			return health;
		}

		/// <summary>Replaces character's health with the given health value</summary>
		/// <param name="h">Character's new health</param>
		public void SetHealth(double h)
		{
			health = h;
		}

		/// <summary>Increases or decreases character's health by a given amount</summary>
		/// <param name="offset">Amount to modify character's health by. Positive to increase health, negative to decrease health.</param>
		public void ModifyHealth(double offset)
		{
			health += offset;
		}

		/// <summary>Determines whether this character is dead</summary>
		/// <returns><c>true</c> if this character's health is less than or equal to 0; otherwise, <c>false</c>.</returns>
		public bool IsDead()
		{
			// did he died
			return health <= 0;
		}

	// POSITION METHODS

		/// <summary>Gets the character's current position</summary>
		/// <returns>A 3D vector consisting of the character's X, Y, and Z coordinates</returns>
		public Vector3 GetPosition()
		{
			return position;
		}

	// POWERUP METHODS

		/// <summary>Gets the list of all powerups acting upon the character</summary>
		/// <returns>SortedList of powerups, sorted by priority</returns>
		public SortedList<int, Powerup> GetPowerups()
		{
			return powerups;
		}

		/// <summary>Adds a powerup to the list of active powerups</summary>
		/// <param name="powerup">Powerup to add</param>
		public void AddPowerup(Powerup powerup)
		{
			Debug.Log(powerup.GetID() + " has been added to the character");
			powerups.Add (powerup.GetPriority(), powerup);
			powerup.SetCharacter (this);
		}

		/// <summary>Parses through active powerups and processes them</summary>
		private void ProcessPowerups()
		{
			List<Powerup> powerupsToRemove = new List<Powerup>();
			foreach (Powerup powerup in powerups.Values)
			{
				if (powerup.IsExpired ())
				{
					// Queue powerup to be removed
					powerupsToRemove.Add (powerup);
				}
				else 
				{
					// Powerup is not expired so it should act upon the character
					powerup.Act ();
				}
			}
			foreach (Powerup powerup in powerupsToRemove)
			{
				powerups.Remove (powerup.GetPriority());
				Debug.Log(powerup.GetID() + " has been removed");
			}
		}

	// METHODS FOR MODIFIERS

	/// <summary>Sets the character's invincibility status.</summary>
	/// <param name="invincibility">If set to <c>true</c>, character will be made invincible.</param>
	public void SetInvincible(bool invincibility)
	{
		invincible = invincibility;
	}

	/// <summary>Determines whether this character is invincible.</summary>
	/// <returns><c>true</c> if this instance is invincible; otherwise, <c>false</c>.</returns>
	public bool IsInvincible()
	{
		return invincible;
	}


	/// <summary>Sets the modifiers and multipliers to their neutral states for the next powerup processing</summary>
	private void ResetModifiers()
	{
		speedMultiplier = 1.0;
		invincible = false;
	}

	// RUNTIME METHODS

		// Use this for initialization
		/// <summary>Initializes character</summary> 
		void Start ()
		{
			powerups = new SortedList<int,Powerup>();
		}
		
		// Update is called once per frame
		/// <summary>Updates internally</summary>
		void Update ()
		{
			ResetModifiers ();

			position = transform.position;

			ProcessPowerups ();
		}
}
