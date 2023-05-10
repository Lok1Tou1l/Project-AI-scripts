using UnityEngine;
using UnityEngine.AI;   // NavMeshAgent

public class EnemyBehaviour : MonoBehaviour
{
   public enum AIState
   {
      Idle,
      Walk,
      Chase,
      Attack1,
      Attack2, 
   }

   public AIState currentState;
   public Transform player;
   public Animator anim;

   private NavMeshAgent agent;

   public float idleTime = 0f;
   public float attackRange1 = 2f;
   public float attackRange2 = 2f;
   public float sightRange = 10f;

   private float idleTimer = 0f;


   private void Start()
   {
      agent = GetComponent<NavMeshAgent>();
      anim = GetComponent<Animator>();
      player = GameObject.FindGameObjectWithTag("Player").transform;
   }
   
   private void Update()
   {
      switch (currentState)
      {
         case AIState.Idle:
            IdleState();
            break;
         case AIState.Walk:
            WalkState();
            break;
         case AIState.Chase:
            ChaseState();
            break;
         case AIState.Attack1:
            Attack1State();
            break;
         case AIState.Attack2:
            Attack2State();
            break;
      }
   }
   
   private void IdleState()
   {
      idleTimer += Time.deltaTime;
      if (idleTimer >= idleTime)
      {
         idleTimer = 0f;
         currentState = AIState.Walk;
      }
   }
   
   private void WalkState()
   {
      agent.SetDestination(player.position);
      anim.SetBool("Walk", true);
      if (Vector3.Distance(transform.position, player.position) <= sightRange)
      {
         anim.SetBool("Walk",false);
         currentState = AIState.Chase;
      }
   }
   
   private void ChaseState()
   {
      agent.SetDestination(player.position);
      anim.SetBool("Chase",true);
      if (Vector3.Distance(transform.position, player.position) <= attackRange1)
      {
         anim.SetBool("Chase",false);
         currentState = AIState.Attack1;
      }
      else if (Vector3.Distance(transform.position, player.position) > sightRange)
      {
         anim.SetBool("Chase",false);
         currentState = AIState.Walk;
      }
   }
   
   private void Attack1State()
   {
      agent.SetDestination(transform.position);
      anim.SetBool("Attack1",true);
      if (Vector3.Distance(transform.position, player.position) > attackRange1 && Vector3.Distance(transform.position, player.position) <= sightRange)
      {
         anim.SetBool("Attack1",false);
         currentState = AIState.Chase;
      }
   }
   
   private void Attack2State()
   {
      agent.SetDestination(transform.position);
      anim.SetBool("Attack2",true);
      if (Vector3.Distance(transform.position, player.position) <= attackRange1)
      {
         currentState = AIState.Attack1;
      }
      
      else if (Vector3.Distance(transform.position, player.position) < attackRange2 && Vector3.Distance(transform.position , player.position) <= sightRange)
      {
         anim.SetBool("Attack2",false);
         currentState = AIState.Chase;
      }
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, attackRange1);
      Gizmos.DrawWireSphere(transform.position, attackRange2);
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, sightRange);
   }


}
