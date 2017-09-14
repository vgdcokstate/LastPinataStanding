using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("left"))
			transform.Translate (.2f, 0,0);
		if(Input.GetKey("right") )
			transform.Translate (-.2f, 0,0);
		if(Input.GetKey("up"))
			transform.Translate (0,0,-.2f);
		if(Input.GetKey("down"))
			transform.Translate (0, 0,.2f);
	
	}
}
