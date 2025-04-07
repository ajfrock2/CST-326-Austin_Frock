using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime = 10f;
    
    private Transform player;
    private NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    private List<Transform> waypointsList = new List<Transform>();
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = patrolSpeed;
        timer = 0;
        
        //Move to first waypoint and get others
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }
        
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }
    
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        
        //Sound
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }
        
        //Check for waypoint arrival and then on to next
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        //Transition to idle
        if (timer >= patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }
        
        //Transition to chase
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceToPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Stop agent movement
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel.Stop();
    }
}
