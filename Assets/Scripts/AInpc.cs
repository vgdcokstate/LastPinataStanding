using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AInpc : MonoBehaviour {
    GameObject[] enemies;
    List<GameObject> foundEnemies;
    enum state {wander, move, hide, found, death}

    public float panicDistance;
    public float searchAngle;
    public float distanceError;
    public float runDistance;
    public float speed;
    public float wanderSpeed;
    public float runSpeed;
    public Transform[] waypoints;
    public Transform[] hidingSpots;

    state currentState = state.move;
    NavMeshAgent agent;
    int moveTo = 0;
    Timer timer;
    Timer wanderTimer;
    Timer runTimer;
    bool setupDone = false;
    bool moving = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = gameObject.AddComponent<Timer>();
        wanderTimer = gameObject.AddComponent<Timer>();
        runTimer = gameObject.AddComponent<Timer>();
    }

    void Update () {
        foundEnemies = ConeSearch();
        if (foundEnemies.Count != 0 && !runTimer.isTimerRunning)
        {
            currentState = state.found;
            runTimer.Begin(Random.Range(5f, 10f));
        }
        // if this.health <= 0 currentState = state.death
        StateManager();
	}

    void StateManager()
    {
        switch (currentState)
        {
            case state.wander:
                Wander();
                break;
            case state.move:
                Move();
                break;
            case state.hide:
                Hide();
                break;
            case state.found:
                Run();
                break;
            case state.death:
                Death();
                break;
            default:
                break;
        }
    }

    void Move()
    {
        float dist = Vector3.Distance(transform.position, waypoints[moveTo].position);
        if (!moving)
        {
            moveTo = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[moveTo].position);
            moving = true;
        }
        else if (moving && dist < distanceError && dist > -distanceError)
        {
            currentState = state.wander;
            timer.Begin(Random.Range(10f, 28f)); //prep for wander state
            moving = false;
        }
    }

    void Wander()
    {
        if (!timer.isTimerRunning)
        {
            moving = false;
            currentState = state.move;
        }
        else if (!moving)
        {
            wanderTimer.Begin(Random.Range(4f, 7f));
            agent.SetDestination(RandomPositionSphere(transform.position, 5f));
            agent.speed = wanderSpeed;
            moving = true;
        }
        else
        {
            if (!wanderTimer.isTimerRunning)
                moving = false;
        }
    }
    
    void Hide()
    {

    }

    void Run()
    {
        agent.speed = runSpeed;
        GameObject closestEnemy = null;
        float closestDistance = 100000000;
        if(!setupDone)
        {
            for (int i = 0; i < foundEnemies.Count; i++)
            {
                float distance = Vector3.Magnitude(foundEnemies[i].transform.position - transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = foundEnemies[i];
                }
            }
            Vector3 runDirection = transform.position - closestEnemy.transform.position;
            runDirection = new Vector3(runDirection.x, 0f, runDirection.z);

            Vector3 runPosition = runDirection.normalized * runDistance + transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(runPosition, out hit, runDistance, -1);

            agent.SetDestination(hit.position);
            setupDone = true;
        }
        if (!runTimer.isTimerRunning)
        {
            currentState = state.move;
            agent.speed = speed;
            setupDone = false;
            moving = false;
        }
    }

    void Death()
    {

    }
    
     Vector3 RandomPositionSphere(Vector3 origin, float radius)
    {
        Vector3 randPosition = (Random.insideUnitSphere * radius)+origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randPosition, out hit, radius, -1);
        return hit.position;
    }

    List<GameObject> ConeSearch()
    {
        List<GameObject> found = new List<GameObject>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 xyzDistance = (enemy.transform.position - transform.position);
            float distanceToEnemy = xyzDistance.magnitude;
            if (distanceToEnemy <= panicDistance)
            {
                float angleBetween = Vector3.Angle(enemy.transform.position - transform.position, transform.forward);
                if (angleBetween <= searchAngle)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        float angle = 30f - (i / 10f) * searchAngle;
                        Vector3 lookDirection = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, lookDirection, out hit, panicDistance))
                        {
                            if (hit.collider.gameObject.tag == "Enemy")
                            {
                                if (!found.Contains(hit.collider.gameObject))
                                {
                                    found.Add(hit.collider.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
        return found;
    }
}
