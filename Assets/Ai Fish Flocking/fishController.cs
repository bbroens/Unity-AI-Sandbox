using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishController : MonoBehaviour {
	public AiFlockController flockController; // * Defined when generating fish in AiFlockcontroller.cs
	private float fishSpeed;

	// Flocking rules inits
	Vector3 flockCenter= Vector3.zero;
	Vector3 avoidanceVector = Vector3.zero;
	float averageGroupSpeed = 0.01f;
	float neighbourDistance;
	int groupSize = 0;

	void Start () {
		fishSpeed = Random.Range(flockController.minSpeed, flockController.maxpeed);
	}
	
	void Update () {
		Bounds b = new Bounds(flockController.transform.position, flockController.swimLimits * 2);
		RaycastHit raycastHit = new RaycastHit();
		Vector3 direction = Vector3.zero;
		
		// When too far from flock, return to avg centre of flock
		if (!b.Contains(this.transform.position)) {
			direction = flockController.transform.position - this.transform.position;
			turnBack(direction); 			
		}
		else if (Physics.Raycast(this.transform.position, 
		this.transform.forward * flockController.fishCollisionAvoidance, out raycastHit)) 
		{
			/* Debug.DrawRay(this.transform.position, 
							this.transform.forward * flockController.fishCollisionAvoidance, 
							Color.red); 
			*/
			// Reflect in K-angle from raycast hit to
			direction = Vector3.Reflect(this.transform.forward, raycastHit.normal);
			turnBack(direction);
		}
		else {
			processBehaviour();
		}

		this.transform.Translate(0, 0, Time.deltaTime * fishSpeed);
	}

	// Return to avg centre of flock
	void turnBack(Vector3 direction) {
		
		// Turn towards the centre of the flock
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
														Quaternion.LookRotation(direction),
														flockController.rotationSpeed * Time.deltaTime);
	}

	void processBehaviour() {
		if (Random.Range(0,100) < 10) { // 10% chance for speed change
			fishSpeed = Random.Range(flockController.minSpeed, flockController.maxpeed);
		}

		if (Random.Range(0,100) < 20) { // 20% chance - lighten load of constant calculation
			followFlockingRules();
		}
	}

	void followFlockingRules () {
		// Holder for all fish in current flock
		GameObject[] flock;
		flock = flockController.allFish;

		foreach (GameObject fish in flock) {
			// Check if fish in flock is not current fish - we do not need to find average of itself
			if (fish != this.gameObject) {
				neighbourDistance = Vector3.Distance(fish.transform.position, this.transform.position);
				heedNeighbours(fish, neighbourDistance); 
			}
		}

		// Only redirect fish when not swimming alone
		if (groupSize > 0) {
			redirectFish ();
		}
	}

	// process awareness of neighbours
	void heedNeighbours(GameObject fish, float neighbourDistance) {
		// Check if fish has neighbours within defined range
		if (neighbourDistance <= flockController.neighbourRange) {
			flockCenter += fish.transform.position;
			groupSize++;

			// Add to avoidance vector if we come too close to another fish
			if (neighbourDistance < flockController.flockDensity) {
				avoidanceVector += (this.transform.position - fish.transform.position);
			}

			// Add this fish's speed to average group speed
			fishController thisFishController = fish.GetComponent<fishController>();
			averageGroupSpeed += thisFishController.fishSpeed;
		}
	}

	void redirectFish() {
		flockCenter = flockCenter / groupSize + (flockController.flockGoalPosition - this.transform.position);
		fishSpeed = averageGroupSpeed / groupSize;

		Vector3 fishDirection = (flockCenter + avoidanceVector) - this.transform.position;

		// Check if fish still needs to rotate in new direction?
		if (fishDirection != Vector3.zero) {
			transform.rotation = Quaternion.Slerp(transform.rotation, 
												Quaternion.LookRotation(fishDirection), 
												flockController.rotationSpeed * Time.deltaTime);
		}
	}
}