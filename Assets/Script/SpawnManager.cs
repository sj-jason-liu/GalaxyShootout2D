using System.Collections;
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

    private Vector3 _posToSpawn;

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
        if (_uiManager == null)
        {
            Debug.LogError("UIManager couldn't locate!");
        }
    }

    private void Update()
    {
        _posToSpawn = new Vector3(Random.Range(-12f, 12f), 10f, 0);

    }

    public void StartSpawning()
    {
        _isSpawning = true;
        StartCoroutine(NewWaveSpawning());
        StartCoroutine(Tier1PowerupRoutine());
        StartCoroutine(Tier2PowerupRoutine());
        StartCoroutine(Tier3PowerupRoutine());
    }

    IEnumerator NewWaveSpawning()
    {
        while (true)
        {
            _currentWave++;
            _enemySpawned = 0;
            _uiManager.WaveStart(_currentWave);
            yield return new WaitForSeconds(5f);
            while (_isSpawning)
            {
                while (_enemySpawned < _currentWave * 5)
                {
                    if (_normalEnemySpawned > 2 && _normalEnemySpawned % 4 == 0)
                    {
                        SpawnDetectBomb();
                    }
                    SpawnEnemy();
                    yield return new WaitForSeconds(Random.Range(2f, 5f));
                }
                if (_enemyContainer.transform.childCount == 0)
                {
                    if (_currentWave < _waveCounts.Length)
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
        Instantiate(_detectBomb, _posToSpawn, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(_enemyPrefab, _posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
        _normalEnemySpawned++;
        _enemySpawned++;
        Debug.Log("Enemy current count: " + _enemySpawned + " / " + _currentWave * 5);
    }
    
    IEnumerator Tier1PowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        int _normalPowerupCount = 0;
        int _tripleshotCount = 0;
        while (_stopSpawn == false)
        {
            int tier1Powerup = Random.Range(0, 2);
            int randomPoisonSpawn = Random.Range(5, 8);
            if (tier1Powerup == 0)
            {
                _tripleshotCount++;
            }
            if (_normalPowerupCount >= randomPoisonSpawn && _normalPowerupCount % 5 == 0)
            {
                Instantiate(_powerups[6], _posToSpawn, Quaternion.identity);
                Debug.Log("Poison Launched!");
            }
            if (_tripleshotCount == 4)
            {
                Instantiate(_powerups[5], _posToSpawn, Quaternion.identity);
                _tripleshotCount = 0;
            }
            else
            {
                Instantiate(_powerups[tier1Powerup], _posToSpawn, Quaternion.identity);
                _normalPowerupCount++;
            }
            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }
    }

    IEnumerator Tier2PowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while(_stopSpawn == false)
        {
            int tier2Powerup = Random.Range(2, 4);
            Instantiate(_powerups[tier2Powerup], _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(8f, 11f));
        }
    }

    IEnumerator Tier3PowerupRoutine()
    {
        while(_stopSpawn == false)
        {
            if(_normalEnemySpawned > 8)
            {
                int tier3Powerup = 4;
                Instantiate(_powerups[tier3Powerup], _posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(12f, 15f));
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
