using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnPoints : MonoBehaviour {

    [SerializeField]
    private GameObject enemyTarget;
    public List<GameObject> spawnPoints;

    public Vector3 GetSpawnPoint()
    {
        // Vælg 2 tilfældige spawn-points, og vælg et sted mellem dem. Det bliver til vores spawn-position
        Vector3 newPos = Vector3.Lerp(spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Random.value);

        return newPos;
    }

    public Transform GetTarget()
    {
        return enemyTarget.transform;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
