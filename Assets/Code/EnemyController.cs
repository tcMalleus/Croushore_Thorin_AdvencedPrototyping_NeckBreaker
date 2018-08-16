using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;
    private NavMeshAgent agent;
    private bool playerInTrigger;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerInTrigger = false;
	}
	
	void Update ()
    {
        if (playerInTrigger == true)
        {
            agent.destination = player.transform.position;

            if (player.GetComponent<PlayerController>().isTwisted == true && playerController.closest == gameObject.transform)
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
