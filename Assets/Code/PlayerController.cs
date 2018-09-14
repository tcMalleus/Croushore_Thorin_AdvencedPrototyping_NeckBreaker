using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Action vars
    public bool isGrabbedLeft;
    public bool isGrabbedRight;
    public bool isGrabbed;
    public bool isDoubleGrabbed;
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


    //Combo manager's
    public ComboManager ComboManager;

    private int Health = 2;

    private GameObject _rightArm;
    private Vector3 _rightArmOrigPos;
    private GameObject _leftArm;
    private Vector3 _leftArmOrigPos;

    public GameObject gameOverScreen;



    //Start
    void Start ()
    {
        ComboManager = FindObjectOfType<ComboManager>();

        gameOverScreen.SetActive(false);


        _leftArm = GameObject.Find("Arm_Left");
        _leftArmOrigPos = _leftArm.transform.localPosition;
        _rightArm = GameObject.Find("Arm_Right");
        _rightArmOrigPos = _rightArm.transform.localPosition;



        //NavMeshAgent and coroutine's definitions
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        agent = GetComponent<NavMeshAgent>();
        coroutine = NavUpdate(0.25f);
        StartCoroutine(coroutine);

        //Action definitions
        grabOffset = GameObject.Find("Grab Offset");
	}
	
    //Update
	void Update ()
    {
        //OnTriggerExit update check
        if (triggered && !other)
        {
            nearEnemy = false;
            isGrabbed = false;
        }


        //Inputs
        //Grab inputs
        if (Input.GetAxis("GrabRight") >= 1 || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GrabRight();
        }
        else if (Input.GetAxis("GrabRight") <= 1 && Input.GetKeyUp(KeyCode.RightArrow))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy)
                {
                    NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                    enemyNav.enabled = true;
                }
            }

            agent.enabled = true;

            _rightArm.transform.localPosition = _rightArmOrigPos;

            isGrabbedRight = false;
        }
        
        if (Input.GetAxis("GrabLeft") >= 1 || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GrabLeft();
            
        }
        else if (Input.GetAxis("GrabRight") <= 1 && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = true;
            }

            agent.enabled = true;

            _leftArm.transform.localPosition = _leftArmOrigPos;

            isGrabbedLeft = false;
        }

        if (isGrabbedLeft == true || isGrabbedRight == true)
        {
            isGrabbed = true;
            if (closest && nearEnemy == true)
            {
                closest.position = Vector3.MoveTowards(closest.position, grabOffset.transform.position, 20 * Time.deltaTime);
            }
        }
        else
        {
            isGrabbed = false;
        }

        if (isGrabbedLeft == true && isGrabbedRight == true)
        {
            isDoubleGrabbed = true;
            if (closest && nearEnemy == true)
            {
                closest.position = Vector3.MoveTowards(closest.position, grabOffset.transform.position, 20 * Time.deltaTime);
            }
        }
        else
        {
            isDoubleGrabbed = false;
        }

        //Twist inputs
        if (isGrabbed == true && Input.GetKeyDown(KeyCode.D))
        {
            TwistRight();
        } 
        else 
        {
            isTwisted = false;
        }


        //Push key to ComboManager
        if (Input.inputString != string.Empty && isGrabbed == true)
        {
            closest.GetComponentInParent<ComboManager>().UpdateCombo(Input.inputString);
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

                    if (curDistance < 1)
                    {
                        Health = Health - closest.GetComponentInParent<EnemyController>().damage;
                        Destroy(closest.gameObject);
                    }
                }
            }

            if (Health <= 0)
            {
                Time.timeScale = 0;
                gameOverScreen.SetActive(true);
            }

            if(agent.enabled == true && closest)
            {
                agent.destination = closest.position;
            }

            Debug.Log(Health);
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
        _rightArm.transform.localPosition = _rightArm.transform.localPosition + new Vector3(0.5f, 0, 0);

        if (nearEnemy == true)
        {
            isGrabbedRight = true;

            /*foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = false;
            }*/
            agent.enabled = false;
        }      
    }

    public void GrabLeft()
    {
        _leftArm.transform.localPosition = _leftArm.transform.localPosition + new Vector3(0.5f, 0, 0);


        if (nearEnemy == true)
        {
            isGrabbedLeft = true;

            /*foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = false;
            }*/

            agent.enabled = false;
        }
    }

    public void TwistRight()
    {
        isTwisted = true;
    }

    public void TwistLeft()
    {

    }
}
