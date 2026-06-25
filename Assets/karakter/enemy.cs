using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;          // Assign Player di Inspector atau FindWithTag
    private NavMeshAgent agent;
    private Animator anim;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (target == null)
            target = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > agent.stoppingDistance)
        {
            // Jalan menuju target
            agent.isStopped = false;
            agent.SetDestination(target.position);
            
            // Animasi lari
            anim.SetBool("attack", false);
            anim.SetFloat("speed", agent.velocity.magnitude);
        }
        else
        {
            // Berhenti dan menyerang
            agent.isStopped = true;
            anim.SetBool("attack", true);
        }
    }
}
