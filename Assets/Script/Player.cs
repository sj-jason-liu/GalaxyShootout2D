using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedAddValue = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isTripleShotActived = false;
    private bool _isShieldActived = false;
    private bool _isDestroyed = false;

    [SerializeField]
    private GameObject _shieldEffect;

    [SerializeField]
    private int _score;
    private UIMananger _uiManager;
    [SerializeField]
    private GameObject _rightDamage, _leftDamage;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _laserAudioSource;

    void Start()
    {
        transform.position = new Vector3(0, -2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIMananger>();
        _laserAudioSource = GetComponent<AudioSource>();
        
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is missing!");
        }
        
        if(_uiManager == null)
        {
            Debug.LogError("UIManager is missing!");
        }

        if(_laserAudioSource == null)
        {
            Debug.LogError("LaserAudioSource is missing!");
        }
        else
        {
            _laserAudioSource.clip = _laserAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }             
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        if(!_isDestroyed)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = 
            new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f || transform.position.x < -11.3f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;  

        if(_isTripleShotActived)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _laserAudioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActived)
        {
            _isShieldActived = false;
            _shieldEffect.SetActive(false);
            return;
        }

        _lives--;

        switch (_lives)
        {
            case 2:
                _rightDamage.SetActive(true);
                break;
            case 1:
                _leftDamage.SetActive(true);
                break;
        }

        _uiManager.UpdateSprite(_lives);

        if (_lives < 1)
        {
            _isDestroyed = true;
            _spawnManager.PlayerDeath();
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActived = true;
        StartCoroutine(TripleShotDeactivate());
    }

    IEnumerator TripleShotDeactivate()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActived = false;
    }

    public void SpeedBoostActive()
    {
        _speed += _speedAddValue;
        StartCoroutine(SpeedBoostDeactivate());
    }

    IEnumerator SpeedBoostDeactivate()
    {
        yield return new WaitForSeconds(5f);
        _speed -= _speedAddValue;
    }

    public void ShieldActive()
    {
        _isShieldActived = true;
        _shieldEffect.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
