using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.LookAt(player.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.destination = player.transform.position;
        }
    }
}
