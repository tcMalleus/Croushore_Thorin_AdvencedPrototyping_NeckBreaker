using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    private NavMeshAgent agent;
    private bool playerInTrigger;
    private PlayerController playerRef;

	void Start ()
    {
        playerRef = FindObjectOfType<PlayerController>();

        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        playerInTrigger = false;
	}
	
	void Update ()
    {
        if (playerInTrigger == true)
        {
            agent.destination = playerRef.transform.position;

            if (playerRef.isTwisted == true && playerRef.closest == gameObject.transform)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = true;
        }
    }
}
