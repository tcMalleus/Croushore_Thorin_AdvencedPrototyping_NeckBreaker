using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private NavMeshAgent agent;
    private bool playerInTrigger;
    private PlayerController player;

    //Start
	void Start ()
    {
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        playerInTrigger = false;
	}
	
    //Update
	void Update ()
    {
        //NavMeshAgent destination and death conditions
        if (playerInTrigger == true)
        {
            agent.destination = player.transform.position;

            if (player.isTwisted == true && player.closest == gameObject.transform)
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
