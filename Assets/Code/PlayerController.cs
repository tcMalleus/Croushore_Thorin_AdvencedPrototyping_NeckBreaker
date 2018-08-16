using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public Transform enemy;
    public bool IsGrabbed;
    public GameObject grabOffset;
    public bool isTwisted;

    public Transform closest;
    private NavMeshAgent agent;
    private IEnumerator coroutine;
    private bool nearEnemy;

    GameObject[] enemies;

	void Start ()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        agent = GetComponent<NavMeshAgent>();
        grabOffset = GameObject.Find("Grab Offset");

        coroutine = NavUpdate(0.25f);
        StartCoroutine(coroutine);
	}
	
	void Update ()
    {
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

            IsGrabbed = false;
        }

        /*
        if (Input.GetAxis("GrabLeft") >= 1 || Input.GetKeyDown("left"))
        {
            GrabRight();
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = true;
            }

            agent.enabled = true;
        }
        */

        if (IsGrabbed = true && Input.GetKeyDown("d"))
        {
            TwistRight();
        } 
        else 
        {
            isTwisted = false;
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true && other.tag == "Enemy")
        {
            nearEnemy = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == true && other.tag == "Enemy")
        {
            nearEnemy = false;
        }
    }


    public void GrabRight()
    {

        if (nearEnemy == true)
        {
            IsGrabbed = true;

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
        IsGrabbed = true;
    }

    public void TwistRight()
    {
        isTwisted = true;
    }

    public void TwistLeft()
    {

    }
}
