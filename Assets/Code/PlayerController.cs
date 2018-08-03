using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    public Transform enemy;

    private Transform closest;
    private NavMeshAgent agent;
    private IEnumerator coroutine;

	// Use this for initialization
	void Start ()
    {
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
        //if (Input.GetAxis("GrabLeft"))
       // {
       //     GrabRight();
       // }
    }

    private IEnumerator NavUpdate(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject go in gos)
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


    void GrabRight()
    {
        Debug.Log("GrabbedRight");
    }

    void GrabLeft()
    {

    }

    void TwistRight()
    {

    }

    void TwistLeft()
    {

    }
}
