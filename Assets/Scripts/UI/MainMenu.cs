using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public Loader loader;

	public void SinglePlayer()
    {
        loader.Load("map");
    }
}
