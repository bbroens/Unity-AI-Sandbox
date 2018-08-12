using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFlockController : MonoBehaviour {

	[Header("Flock Settings")]
	[SerializeField] GameObject[] fishPrefab;
	[SerializeField] int fishAmount = 20;
	[Range(0.1f, 5f)] public float flockDensity = 0.7f;
	public Vector3 swimLimits = new Vector3 (5, 5, 5); // Box range around this flock
	[HideInInspector] public Vector3 flockGoalPosition;

	[Header("Fish Settings")]
	[Range(0.0f, 5.0f)] public float minSpeed = 0.5f;
	[Range(0.0f, 5.0f)] public float maxpeed = 2.0f;
	[Range(2.0f, 10.0f)] public float neighbourRange = 4.0f;
	[Range(0.0f, 5.0f)] public float rotationSpeed = 1.0f;
	public float fishCollisionAvoidance = 50f;

	// Other inits
	public GameObject[] allFish;

	void Start () {
		allFish = new GameObject[fishAmount];
		for (int i = 0; i < fishAmount; i++)
		{
			spawnFish(i);
		}
	}
	
	void Update () {
		if (Random.Range(0,100) < 10) { // 10% chance for new goal
			flockGoalPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
																	Random.Range(-swimLimits.y, swimLimits.y),
																	Random.Range(-swimLimits.z, swimLimits.z));
		}
	}

	void spawnFish(int i) {
		Vector3 spawnRange = new Vector3 (Random.Range(-swimLimits.x, swimLimits.x), 
											Random.Range(-swimLimits.y, swimLimits.y),
										Random.Range(-swimLimits.z, swimLimits.z));
		Vector3 pos = this.transform.position + spawnRange;

		// Random fish from the chosen prefabs
		int fishChance = Random.Range(0, fishPrefab.Length);

		// Fill allFish array with random positions within swimLimits
		allFish[i] = (GameObject) Instantiate(fishPrefab[fishChance], pos, Quaternion.identity);

		// * This fish needs to have its flockController set to this to link them together 
		allFish[i].GetComponent<fishController>().flockController = this;
	}
}
