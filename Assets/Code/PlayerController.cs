using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public List<KeyCode> ButtonCombo = new List<KeyCode>();

    public int _currentComboStage = 0;
    public KeyCode _currentComboKey;
    public KeyCode _nextComboKey;

    //public ComboManager ComboScript;

    //Start
    void Start ()
    {
        //NavMeshAgent and coroutine's definitions
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        agent = GetComponent<NavMeshAgent>();
        coroutine = NavUpdate(0.25f);
        StartCoroutine(coroutine);

        //Action definitions
        grabOffset = GameObject.Find("Grab Offset");

        print(ButtonCombo[0].ToString());

        _currentComboKey = ButtonCombo[0];
        _nextComboKey = ButtonCombo[1];

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
        if (Input.GetAxis("GrabRight") >= 1 || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GrabRight();

        }
        else if (Input.GetAxis("GrabRight") <= 1 && Input.GetKeyUp(KeyCode.RightArrow))
        {
            foreach (GameObject enemy in enemies)
            {
                NavMeshAgent enemyNav = enemy.GetComponentInParent<NavMeshAgent>();
                enemyNav.enabled = true;
            }

            agent.enabled = true;

            isGrabbed = false;
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
        }

        if (isGrabbedLeft == true || isGrabbedRight == true)
        {
            isGrabbed = true;
        }

        if (isGrabbedLeft == true && isGrabbedRight == true)
        {
            isDoubleGrabbed = true;
        }

        //Twist inputs
        if (isGrabbed = true && Input.GetKeyDown(KeyCode.D))
        {
            TwistRight();
        } 
        else 
        {
            isTwisted = false;
        }

        UpdateCombo();
    }

    void UpdateCombo(KeyCode _keyJustHit)
    {

        if (_currentComboKey == _keyJustHit && _currentComboStage + 1 < ButtonCombo.Count)
        {
            _currentComboKey = ButtonCombo[_currentComboStage + 1];
            _currentComboStage++;
        }
        else
        {
            _currentComboKey = ButtonCombo[0];
            _currentComboStage = 0;
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

            if(agent.enabled == true && closest)
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

    public void TwistRight()
    {
        isTwisted = true;
    }

    public void TwistLeft()
    {

    }
}
