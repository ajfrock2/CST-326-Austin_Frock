using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    public float chaseSpeed = 6f;
    
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;
    
    //Zigzag variables
    public float switchTime = 1.5f;
    public float maxZigzagOffset = 3;
    private float switchTimer = 0;
    private bool goingRight = true;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = chaseSpeed;
        goingRight = Random.value > 0.5f;
    }
    
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //Sound
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }
        
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        
        //Zigzag
        switchTimer += Time.deltaTime;
        if (switchTimer >= switchTime)
        {
            goingRight = !goingRight;
            switchTimer = 0;
        }

        float posOffset = Mathf.Clamp(distanceToPlayer / 2f, 0f, maxZigzagOffset); //2 Means the furthest offset pre clamping is half the distance to the player
        Vector3 playerRight = player.right;
        Vector3 offsetVector;
        if (goingRight)
        {
            offsetVector = playerRight * -posOffset;
        }
        else
        {
            offsetVector = playerRight * posOffset;
        }

        Vector3 zigzagPosition = player.position + offsetVector;
        
        agent.SetDestination(zigzagPosition);
        
        animator.transform.LookAt(player);
        
        
        
        //Chase Stop
        if (distanceToPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }
        
        //Agent attack
        if (distanceToPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Stop agent movement
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel.Stop();
    }
}
