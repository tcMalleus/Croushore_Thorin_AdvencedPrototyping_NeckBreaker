using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public Transform enemy;
    public bool IsGrabbed;

    private Transform closest;
    private NavMeshAgent agent;
    private IEnumerator coroutine;

    GameObject[] enemies;

	// Use this for initialization
	void Start ()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        agent = GetComponent<NavMeshAgent>();

        coroutine = NavUpdate(0.25f);
        StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("GrabRight") >= 1)
        {
            GrabRight();
        }
        if (Input.GetAxis("GrabLeft") >= 1)
        {
            GrabRight();
        }
        //if (IsGrabbed = true && )
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
        if (other.isTrigger == false && other.tag == "Enemy")
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = false;
            }

            agent.enabled = false;
        }
    }


    void GrabRight()
    {
        Debug.Log("GrabbedRight");
        IsGrabbed = true;
    }

    void GrabLeft()
    {
        Debug.Log("GrabbedLeft");
        IsGrabbed = true;
    }

    void TwistRight()
    {

    }

    void TwistLeft()
    {

    }
}
