using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChildAI : MonoBehaviour {
    GameObject[] enemies;
	GameObject[] nodes;
	//These are for the Cone search, distance of sight and sight angle
    public float panicDistance;
    public float searchAngle;
	string state;
	//The last known positions of the pinata the kid is chasing
	private float lastKnownX;
	private float lastKnownZ;
	//Timer keeps track of  how long ago the kid saw the pinata, and will go back to searching
	private int timer;
	UnityEngine.AI.NavMeshAgent agent;
	Vector3 ranPos;



	void Start(){
		ranPos = transform.position;
		nodes = GameObject.FindGameObjectsWithTag("Node");
		state = "SEARCH";
		timer = 300;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}


	void Update()
	{
        //agent.SetDestination (target.position);
		StateManager ();

	}


	void StateManager()
	{
		if (state.Equals ("SEARCH")) {	
			//print ("Searching");
			GameObject pinata = Search ();
			if (pinata != null)
				state = "ATTACK";
			if (transform.position.x - ranPos.x !=0 && transform.position.z - ranPos.z != 0) {
				agent.SetDestination (ranPos);
				//print ("movin");
			} else {
				ranPos = RandomNode ();
				//agent.SetDestination (ranPos);
				//print ("resettin");
			}
		} else if (state.Equals ("ATTACK")) {
			GameObject thing = Search ();
			if (thing != null)
				agent.SetDestination (thing.transform.position);
			else
				agent.SetDestination (new Vector3 (lastKnownX, 0, lastKnownZ));
			//If the kid hasnt seen a pinata for 200 frames, we go back to looking
			if (thing != null) {
				timer = 200;
			}
			if (thing == null)
				timer -= 1;
			if (timer == 0)
				state = "SWEEP";


			//Rotate towards the last known position
			/*Vector3 dir = (new Vector3 (lastKnownX, 0, lastKnownZ) - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5);
			*/

			//Move towards the thing
			/*if (lastKnownX < transform.position.x)
				transform.Translate (-.1f,0,0,Space.World);
			if (lastKnownX > transform.position.x)
				transform.Translate (.1f,0,0,Space.World);
			if (lastKnownZ < transform.position.z)
				transform.Translate (0,0,-.1f,Space.World);
			if (lastKnownZ > transform.position.z)
				transform.Translate (0,0,.1f,Space.World);
			*/
		} else if (state.Equals ("SWEEP")) {
			GameObject thing = Search ();
			float diff = transform.rotation.eulerAngles.z - 45;
			transform.rotation =  Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,0,45), Time.deltaTime * 2);
			if(thing !=null)
				state = "ATTACK";
			if (diff < 1)
				state = "SEARCH";
		}



	}


	Vector3 RandomNode()
	{
		int ran = Random.Range (0, nodes.Length -1);
		return nodes [ran].transform.position;
	}

	Vector3 RandomPositionSphere(Vector3 origin, float radius)
	{
		Vector3 randPosition = (Random.insideUnitSphere * radius)+origin;
		NavMeshHit hit;
		NavMesh.SamplePosition(randPosition, out hit, radius, -1);
		return hit.position;
	}


	GameObject Search () {
		//Returns a game object whithin the line of site with either a player tag or a Pinatas Tag, prioritizes player
		GameObject play;
        //Debug.DrawRay(transform.position, transform.forward * panicDistance, Color.yellow);

		play = GameObject.FindWithTag("Player");
		Vector3 xyzDistance = (play.transform.position - transform.position);
        float distanceToEnemy = xyzDistance.magnitude;
		if(distanceToEnemy <= panicDistance){
			float angleBetween = Vector3.Angle(play.transform.position - transform.position, transform.forward);
			if (angleBetween <= searchAngle) {
				for(int i =0; i<10; i++){
					float angle = 30f - (i / 10f) * searchAngle;
					Vector3 lookDirection = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
					RaycastHit hit;
					if (Physics.Raycast (transform.position, lookDirection, out hit, panicDistance)) {
						if (hit.collider.gameObject.tag == "Player") {
							lastKnownX = play.transform.position.x;
							lastKnownZ = play.transform.position.z;
							return play;

						}
					}
				}
		
			}
		}
		enemies = GameObject.FindGameObjectsWithTag("Pinatas");
        foreach(GameObject enemy in enemies)
        {
            //Debug.DrawLine(transform.position, enemy.transform.position);
            xyzDistance = (enemy.transform.position - transform.position);
			distanceToEnemy = xyzDistance.magnitude;

            if(distanceToEnemy <= panicDistance)
            {
                //Debug.DrawRay(transform.position, transform.forward, Color.black);
                float angleBetween = Vector3.Angle(enemy.transform.position - transform.position, transform.forward);
                if (angleBetween <= searchAngle)
                {
					for(int i =0; i<10; i++){
						float angle = 30f - (i / 10f) * searchAngle;
						Vector3 lookDirection = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
						RaycastHit hit;
						if (Physics.Raycast (transform.position, lookDirection, out hit, panicDistance)) {
							if (hit.collider.gameObject.tag == "Pinatas") {
								lastKnownX = enemy.transform.position.x;
								lastKnownZ = enemy.transform.position.z;
								return enemy;
							
							}
						}
					}

                }
            }
        }
		return null;
	}
}
