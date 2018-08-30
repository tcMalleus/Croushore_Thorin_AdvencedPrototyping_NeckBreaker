using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    //Action vars
    public bool isGrabbed;
    public bool isTwisted;
    public GameObject grabOffset;

    //NavMeshAgent's
    public Transform closest;
    private NavMeshAgent agent;
    private IEnumerator coroutine;
    GameObject[] enemies;

    //Enemy trigger volume's
    private bool triggered = false;
    private bool nearEnemy;
    Collider other;

    //Start
	void Start ()
    {
        //NavMeshAgent and coroutine's declarations
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        agent = GetComponent<NavMeshAgent>();
        coroutine = NavUpdate(0.25f);
        StartCoroutine(coroutine);

        //Action declarations
        grabOffset = GameObject.Find("Grab Offset");

	}
	
    //Update
	void Update ()
    {
        //OnTriggerExit update check
        if (triggered && !other)
        {
            nearEnemy = false;
        }

        //Inputs
        //Grab inputs
        if (Input.GetAxis("GrabRight") >= 1 || Input.GetKeyDown("right"))
        {
            GrabRight();
        }
        else if (Input.GetAxis("GrabRight") <= 1 && Input.GetKeyUp("right"))
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = true;
            }

            agent.enabled = true;

            isGrabbed = false;
        }
        
        if (Input.GetAxis("GrabLeft") >= 1 || Input.GetKeyDown("left"))
        {
            GrabLeft();
        }
        else if (Input.GetAxis("GrabRight") <= 1 && Input.GetKeyUp("left"))
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = true;
            }

            agent.enabled = true;
        }
        
        //Twist inputs
        if (isGrabbed = true && Input.GetKeyDown("d"))
        {
            TwistRight();
        } 
        else 
        {
            isTwisted = false;
        }
    }

    //NavMeshAgent's destination is nearest enemy
    private IEnumerator NavUpdate(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject go in enemies)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;

                if (curDistance < distance)
                {
                    closest = go.transform;
                    distance = curDistance;
                }
            }

            agent.destination = closest.position;
        }
    }


    //Entering and exiting enemy's collider
    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
        this.other = other;

        //Enter trigger logic
        if (other.isTrigger == true && other.tag == "Enemy")
        {
            nearEnemy = true;
        }
    }


    //Action functions
    public void GrabRight()
    {

        if (nearEnemy == true)
        {
            isGrabbed = true;

            closest.position = Vector3.Lerp(closest.position, grabOffset.transform.position, 20);

            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = false;
            }

            agent.enabled = false;
        }      
    }

    public void GrabLeft()
    {
        Debug.Log("GrabbedLeft");
        isGrabbed = true;
    }

    public void TwistRight()
    {
        isTwisted = true;
    }

    public void TwistLeft()
    {

    }
}
