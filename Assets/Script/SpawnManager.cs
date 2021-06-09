using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _detectBomb;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private UIMananger _uiManager;

    private bool _stopSpawn = false;
    private bool _isSpawning = false;

    [SerializeField]
    private int[] _waveCounts;
    private int _currentWave;
    private int _enemySpawned;
    private int _normalEnemySpawned = 0;

    private int _debugCallTime;

    void Start()
    {
        _currentWave = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIMananger>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager couldn't locate!");
        }
    }

    public void StartSpawning()
    {
        _isSpawning = true;
        StartCoroutine(NewWaveSpawning());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator NewWaveSpawning()
    {
        while(true)
        {
            _currentWave++;
            _enemySpawned = 0;
            _uiManager.WaveStart(_currentWave);
            yield return new WaitForSeconds(5f);
            while (_isSpawning)
            {
                while(_enemySpawned < _currentWave * 5)
                {
                    if(_normalEnemySpawned > 2 && _normalEnemySpawned % 4 == 0)
                    {
                        SpawnDetectBomb();
                    }
                    SpawnEnemy();
                    yield return new WaitForSeconds(Random.Range(2f, 5f));
                }
                if(_enemyContainer.transform.childCount == 0) 
                {
                    if(_currentWave < _waveCounts.Length)
                    {
                        _currentWave++;
                        _enemySpawned = 0;
                        _uiManager.WaveStart(_currentWave);
                        yield return new WaitForSeconds(5f);
                    }
                    else
                    {
                        _isSpawning = false;
                    } 
                }
                yield return new WaitForSeconds(0.01f);
            }
            break;
        }
    }

    void SpawnDetectBomb()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-12f, 12f), 10f, 0);
        GameObject newEnemy = Instantiate(_detectBomb, posToSpawn, Quaternion.identity);
    }
    
    void SpawnEnemy()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-12f, 12f), 10f, 0);
        GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
        _normalEnemySpawned++;
        _enemySpawned++;
        Debug.Log("Enemy current count: " + _enemySpawned + " / " + _currentWave * 5);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        int _normalPowerupCount = 0;
        int _tripleshotCount = 0;
        while (_stopSpawn == false)
        {
            Vector3 posToSpwan = new Vector3(Random.Range(-12f, 12f), 10f, 0);
            int randomPowerup = Random.Range(0, _powerups.Length - 2);
            int randomPoisonSpawn = Random.Range(5, 8);
            if(randomPowerup == 0)
            {
                _tripleshotCount++;
            }
            if(_normalPowerupCount >= randomPoisonSpawn)
            {
                Instantiate(_powerups[6], posToSpwan, Quaternion.identity);
                Debug.Log("Poison Launched!");
                _normalPowerupCount = 0;
            }
            else if(_tripleshotCount == 4)
            {
                Instantiate(_powerups[5], posToSpwan, Quaternion.identity);
                _tripleshotCount = 0;
            }
            else
            {
                Instantiate(_powerups[randomPowerup], posToSpwan, Quaternion.identity);
                _normalPowerupCount++;
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void PlayerDeath()
    {
        _stopSpawn = true;
    }

    void DebugByFrame()
    {
        _debugCallTime++;
        Debug.Log("Call: " + _debugCallTime);
        Debug.Break();
    }
}
