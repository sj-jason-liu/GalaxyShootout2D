﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _thrusterSpeed = 3f;
    [SerializeField]
    private float _speedAddValue = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private int _laserCounts = 15;
    
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
    private Animator _playerTurningAnim;
    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _laserAudioSource;
    [SerializeField]
    private AudioClip _laserExaustedClip;
    private Color _shieldAlpha;
    private CameraShaker _cameraShaker;

    void Start()
    {
        transform.position = new Vector3(0, -2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIMananger>();
        _laserAudioSource = GetComponent<AudioSource>();
        _shieldAlpha = _shieldEffect.GetComponent<SpriteRenderer>().color;
        _playerTurningAnim = GetComponent<Animator>();
        _cameraShaker = GameObject.Find("Main Camera").GetComponent<CameraShaker>();

        if (_spawnManager == null)
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

        if(_cameraShaker == null)
        {
            Debug.LogError("CameraShaker is missing!");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += _thrusterSpeed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed -= _thrusterSpeed;
        }
        
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
            _laserCounts--;
            _uiManager.UpdateAmmo(_laserCounts);
        }             
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        _playerTurningAnim.SetFloat("Turning", horizontalInput);
        
        if(!_isDestroyed)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = 
            new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.6f, 2), 0);

        if (transform.position.x > 13.3f || transform.position.x < -13.3f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;       
        if(_laserCounts > 0)
        {
            if (_isTripleShotActived)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            }           
        }
        else
        {
            _laserAudioSource.clip = _laserExaustedClip;
        }
        _laserAudioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActived)
        {         
            if(_shieldAlpha.a < 0.4f)
            {
                _isShieldActived = false;
                _shieldEffect.SetActive(false);
            }
            else
            {
                _shieldAlpha.a -= 0.4f; //set alpha decrease when damage
                _shieldEffect.GetComponent<SpriteRenderer>().color = _shieldAlpha;
            }     
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

        _cameraShaker.EngageShake(1);
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
        _shieldAlpha.a = 1f; //set the alpha of shield to full
        _shieldEffect.GetComponent<SpriteRenderer>().color = _shieldAlpha;
    }

    public void AmmoCollected()
    {
        _laserCounts += 15;
        _uiManager.UpdateAmmo(_laserCounts);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
