using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _oponnentContainer;

    [SerializeField]
    GameObject[] _oponnentPrefabs, _spawnPoints;


    Coroutine _spawnRoutine;

    Transform _player;

    int _currentSpawnLane = 0;
    [SerializeField]
    int _maxSpawnLanes = 100;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {

        _spawnRoutine = StartCoroutine(StartSpawning( 2f));
        HUDManager.Instance.SetCurrentSpawnLane(1);
        HUDManager.Instance.SetMaxSpawnLane(_maxSpawnLanes);


    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator StartSpawning( float spawntime)
    {
        EnemyController enemyController = null;
        bool spawned = false;
        for (int i = 0; i < _oponnentPrefabs.Length; i++)
        {
            ISpawnable spawnableObject = _oponnentPrefabs[i].GetComponent<ISpawnable>();
            bool canSpawn = (_maxSpawnLanes * spawnableObject.FromSpawnLanePercentage) <= _currentSpawnLane ? true : false;
             if (Random.value <= spawnableObject.SpawnChance && canSpawn)
            {
                int randPointIndex = Random.Range(0, _spawnPoints.Length);
                enemyController
                        = Instantiate(_oponnentPrefabs[i], _spawnPoints[randPointIndex].transform.position, Quaternion.identity).GetComponent<EnemyController>();
                enemyController.transform.parent = _oponnentContainer.transform;
               
                spawned = true;
 
            }
        }

        if (spawned)
        {
            _currentSpawnLane += 1;
            if (_currentSpawnLane != 1)
            { //no time to think about what is called sooner, if HUDManager or SpawnManager
                HUDManager.Instance.SetCurrentSpawnLane(_currentSpawnLane);
            }
        }

        GameManager.Instance.AddEnemy();
        if (_currentSpawnLane == _maxSpawnLanes)
        {
            GameManager.Instance.LastEnemy = true;
            StopCoroutine(_spawnRoutine);
      
        }
        yield return new WaitForSeconds(spawntime);

        if (!GameManager.Instance.LastEnemy) { 

            _spawnRoutine = StartCoroutine(StartSpawning(2f));
        } 
        

    }

 
}


