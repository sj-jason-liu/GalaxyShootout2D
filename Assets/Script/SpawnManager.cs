using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawn = false;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        
        while (_stopSpawn == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-12f, 12f), 10f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        int _tripleshotCount = 0;
        while (_stopSpawn == false)
        {
            Vector3 posToSpwan = new Vector3(Random.Range(-12f, 12f), 10f, 0);
            int randomPowerup = Random.Range(0, _powerups.Length - 1);
            if(randomPowerup == 0)
            {
                _tripleshotCount++;
                Debug.Log("Tripleshot Called: " + _tripleshotCount);
            }
            if(_tripleshotCount == 4)
            {
                Instantiate(_powerups[5], posToSpwan, Quaternion.identity);
                _tripleshotCount = 0;
            }
            else
            {
                Instantiate(_powerups[randomPowerup], posToSpwan, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void PlayerDeath()
    {
        _stopSpawn = true;
    }
}
