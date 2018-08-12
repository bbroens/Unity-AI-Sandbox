using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiGoalController : MonoBehaviour {

    [SerializeField] GameObject goal;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        goToGoal();
    }

    private void goToGoal()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goal.transform.position);
    }
}
