using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public abstract class Powerup {

	/// <summary>Kind of powerup</summary>
	protected string id;

	/// <summary>Powerup priority -- the higher the number, the more precedence it should have</summary>
	protected int priority;

	/// <summary>Powerup expiration timer</summary>
	protected System.Timers.Timer timer;

	/// <summary>Whether this powerup has expired</summary>
	protected bool expired = false;

	/// <summary>The reference to the <see cref="PlayerStats"/> that this powerup is acting upon</summary>
	/// <remarks>Set by the <see cref="PlayerStats"/> in <see cref="PlayerStats.AddPowerup()"/>  </remarks>
	protected PlayerStats character;

	/// <summary>Initializes a new instance of the <see cref="Powerup"/> class.</summary>
	/// <param name="p">Powerup priority</param>
	/// <param name="n">Powerup name</param>
	/// <param name="t">Timer interval in milliseconds</param>
	public Powerup(int p, string i, int t)
	{
		priority = p;
		id = i;
		timer = new System.Timers.Timer (t);
		timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
		timer.Start ();
	}

	// ABSTRACT
	/// <summary>Causes powerup to act upon the character</summary>
	/// <remarks>Abstract -- must be implemented by subclasses</remarks>
	public abstract void Act();

	// TIMER MAGIC

		/// <summary>Handles everything that should happen when the powerup's timer runs out</summary>
		public void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			expired = true;
			timer.Stop ();
		}

		/// <summary>Gets how much time is left in the timer</summary>
		/// <returns>The time left.</returns>
		/*public int GetTimeLeft()
		{
			//TODO: this
		}*/

	// ACCESSORS

		/// <summary>Gets the powerup name.</summary>
		/// <returns>Powerup ID</returns>
		public string GetID()
		{
			return id;
		}

		/// <summary>Gets the powerup's priority.</summary>
		/// <returns>Integer representing powerup priority, where a higher number refers to a higher priority.</returns>
		public int GetPriority()
		{
			return priority;
		}

		/// <summary>Gets whether this powerup has expired.</summary>
		/// <returns><c>true</c> if this powerup has expired; otherwise, <c>false</c>.</returns>
		public bool IsExpired()
		{
			return expired;
		}

		/// <summary>Sets the reference to the character this powerup acts upon.</summary>
		/// <remarks>Should be called by <see cref="Character.AddPowerup()"/> </remarks>
		/// <param name="c"><see cref="Character"/> in possession of this powerup</param>
		public void SetCharacter(PlayerStats c)
		{
			character = c;
		}
}
