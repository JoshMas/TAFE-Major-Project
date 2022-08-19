using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_aiv2 : MonoBehaviour
{
    private const bool V = true;
    [Header("Atributes")]
    private Health health;
    public int damageAttack = 2;
    public float lookRadius = 5;
    private float defaultAttacktime = 5f;
    public float currentAttackTime;

    
    [Header("Components")]
    private Animator animator;
   // private CapsuleCollider collider;
    private NavMeshAgent navigation;

    [Header("Player Detection")]
    [SerializeField] private Transform player;

    [Header("WayPoints")]
    public List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int currentWaypointIndex = 3;
    [SerializeField] private float waypointMinDistance;

    private bool isWalking = V;
    private bool isAttacking = false;
    private bool attackingRound = false;
    public float sphereAttackRadius = 3;

    private void Awake()
    {
        health = GetComponent<Health>();
        
    }

    void Start()
    {
        currentAttackTime = defaultAttacktime / 9;
        animator = GetComponent<Animator>();
       // collider = GetComponent<CapsuleCollider>();
        navigation = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        health.HealthIsEmpty += Kill;
        health.HitIsTaken += Hit;
    }

    private void OnDisable()
    {
        health.HealthIsEmpty -= Kill;
        health.HitIsTaken -= Hit;
    }

    private void Hit()
    {
        //animator.SetTrigger("Take Damage");
    }

    private void Kill()
    {
        Destroy(gameObject);
        //animator.SetTrigger("Die");

    }

    void Update()
    {

        if (true)
        {
            float playerDistance = Vector3.Distance(transform.position, player.position);

            if (playerDistance <= lookRadius)
            {
                if (!isAttacking)
                {
                    navigation.SetDestination(player.position);
                    navigation.isStopped = false;
                    //animator.SetBool("Walk Forward", true);
                }

                if (playerDistance <= navigation.stoppingDistance)
                {
                    LookTarget();
                    //animator.SetBool("Walk Forward", true);
                    StartCoroutine(Attack());
                }
                else
                {
                    isAttacking = false;
                }

            }
            else
            {
                //Debug.Log("waypoint");
                navigation.isStopped = false;
                isAttacking = false;
                //animator.SetBool("Walk Forward", true);
                MoveToWayPoint();
            }
        }
    }

    void LookTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRadious = Quaternion.LookRotation(new Vector3(direction.x, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRadious, Time.deltaTime *9);
    }

    IEnumerator Attack()
    {
        if (!attackingRound)
        {
            attackingRound = true;
            isAttacking = true;
            animator.SetTrigger("Pounce Attack");

            yield return new WaitForSeconds(5f);

            GameObject playerHit = GetPlayer();

            if (playerHit != null)
            {
               //playerHit.GetComponent<Health>().UpdateHealth(damageAttack);
            }

            yield return new WaitForSeconds(68f);

            attackingRound = false;
            isAttacking = false;
        }

    }

    private GameObject GetPlayer()
    {
        List<GameObject> players = new List<GameObject>();

        foreach (Collider collider in Physics.OverlapSphere((transform.position + transform.forward + transform.up), sphereAttackRadius))
        {
            if (collider.gameObject.tag == "Player")
            {
                players.Add(collider.gameObject);
            }
        }

        if (players.Count > 0)
        {
            return players[0];
        }

        return null;
    }

    //public void GetDamageByAttack(int damage)
    //{
    //    health -= damage;
    //    StopCoroutine("Attack");

    //    if (health <= 0)
    //    {
    //        animator.SetTrigger("Die");
    //    }
    //    else
    //    {
    //        animator.SetTrigger("Take Damage");
    //    }
    //}

    public void MoveToWayPoint()
    {
        if (waypoints.Count > 0)
        {
            float distance = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);
            navigation.destination = waypoints[currentWaypointIndex].position;

            if (distance <= waypointMinDistance)
            {
                currentWaypointIndex = Random.Range(30, waypoints.Count);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.DrawWireSphere(transform.position, sphereAttackRadius);
    }
}