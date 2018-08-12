using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCrowdController : MonoBehaviour {

    GameObject[] goalLocations;
    [SerializeField] GameObject threat;
    NavMeshAgent agent;
    Animator animator;
    float speedMultiplier;
    float detectionRadius = 10; // Distance from detectable object
    float fleeRadius = 20; // How far I will run away before I forget about it

    void Start() {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        //animator.SetFloat("walkOffset", Random.Range(0,1)); // ! Must be a set parameter in the animation controller
        ResetAgent();
    }

    void Update() {
        if (agent.remainingDistance < 1) {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }

        DetectThreat(threat.transform.position);
    }

    void ResetAgent() {
        speedMultiplier = Random.Range(1f, 2f);
        agent.speed = 2 * speedMultiplier;
        agent.angularSpeed = 120;

        // Call animation triggers in the AC
        //animator.SetFloat("speedMultiplier"); // ! Must be a set parameter in the animation controller
        //animator.setTrigger("isWalking"); // ! Must be a set parameter in the animation controller

        agent.ResetPath();
    }

    public void DetectThreat(Vector3 threatPosition) {
        if (Vector3.Distance(threatPosition, this.transform.position) < detectionRadius) {

            // Get to a position away from the threat
            Vector3 fleeDirection = (this.transform.position - threatPosition).normalized; 
            // direction normalized to get a vector of length 1 and to be multiplied by radius below
            Vector3 newGoal = this.transform.position  + fleeDirection * fleeRadius;
            
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);
            
            // Check if new path is valid on the NavMesh
            if (path.status != NavMeshPathStatus.PathInvalid) {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                //animator.setTrigger("isRunning"); // ! Must be a set parameter in the animation controller
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
}