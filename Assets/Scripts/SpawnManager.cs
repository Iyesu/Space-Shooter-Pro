using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField] 
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;

    private float _ySpawn = 8.3f;
    private float _xSpawnMin = -7.0f;
    private float _xSpawnMax = 7.0f;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while(_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(_xSpawnMin, _xSpawnMax), _ySpawn, 0), Quaternion.identity);
            //sets the parent of the new enemy to the enemy container, so that we can easily organize our hierarchy and keep all enemies under one parent object
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(2.0f);
        }
    }

    private IEnumerator PowerUpSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(10.0f, 16.0f));

            //Random.Range is exclusive for Max Value. So Random.Range(0, 3) will return a random number between 0 and 2.
            Instantiate(_powerUps[Random.Range(0, 3)], new Vector3(Random.Range(_xSpawnMin, _xSpawnMax), _ySpawn, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
