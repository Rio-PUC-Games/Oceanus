﻿using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{


    public bool CanSpawn;
    public GameObject EnemyType1;
    public GameObject EnemyType2;
    public float EnemyCount;
    public float TypeToStartSpawn;

    public GameObject[] EnemySpanws;
    public GameObject[] BuildingArray;
    private GameObject[] EnemyTypesArray;
    public Transform PrimaryTarget;
    private Quaternion _angleToCenter;
    public float TimeBetSpawn;

	private static EnemySpawn _instance;
	public static EnemySpawn instance{get{ return _instance;}}
   
	void Awake(){
		_instance = this;
	}

	// Use this for initialization
	void Start ()
	{
      EnemySpanws = GameObject.FindGameObjectsWithTag("EnemySpawn");
	  BuildingArray = GameObject.FindGameObjectsWithTag("Building");
	  Invoke("SetSpawnOn",TypeToStartSpawn);
		EnemyTypesArray = new GameObject[]{ EnemyType1, EnemyType2 };
		// EnemyTypesArray nao havia sido inicializado (uso de new)
	    //EnemyTypesArray[0] = EnemyType1; 
        //EnemyTypesArray[1] = EnemyType2;
    }

    void SetSpawnOn()
    {
        CanSpawn = true;
    }

    void SpawnEnemy(GameObject Spawn)
    {

    Vector3 dist = PrimaryTarget.transform.position - Spawn.transform.position;
    _angleToCenter = Quaternion.AngleAxis(Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg, Vector3.forward);
        Debug.Log(_angleToCenter);
        int random = Random.Range((int) 0, 2);
        Debug.Log(random);
        if (random == 0)
        {
            GameObject SpawningEnemy = (GameObject)Instantiate(EnemyType1, Spawn.transform.position, _angleToCenter);
            SpawningEnemy.GetComponent<EnemyMovement>().target = GameObject.Find("Player").transform;
        }
        else if (random == 1)
        {
          GameObject SpawningEnemy = (GameObject)Instantiate(EnemyType2, Spawn.transform.position, _angleToCenter);
			SpawningEnemy.GetComponent<EnemyMovement> ().target = getBuilding ().transform;
        }
       
        CanSpawn = false;
        EnemyCount++;
        

    }
	//Gambiarra vai ficar sorteando ate encontrar building.
	//Horrivel e nao condizente com o funcionamento. Mas quebra o galho a curto prazo
	public static GameObject getBuilding(){
		GameObject obj = null;
		int count=0;
		while (obj == null) {
			if (count++ > 20)
				return GameObject.Find ("Player");
			obj = _instance.BuildingArray [Random.Range (0, 3)];
		}
		return obj;
	}
	
	// Update is called once per frame
	void Update () {

	    if (CanSpawn)
	    {
            foreach (GameObject enemySpawn in EnemySpanws)
            {
              
              SpawnEnemy(enemySpawn);
            }
            
            Invoke("SetSpawnOn",TimeBetSpawn);
	    }
	
	}
}
