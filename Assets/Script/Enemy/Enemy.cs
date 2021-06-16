using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private GameObject _shieldEffect;

    [SerializeField]
    private int _movementID;

    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioClip _enemyExplosionClip;
    private AudioSource _enemyExplosionSource;
    private float _fireRate = 3f;
    private float _canFireTime = -1f;
    private int _ranNum;

    [SerializeField]
    private float _detectRange = 5f;

    private bool _fireCheck = true;
    private bool _isShieldEnable = false;
    private bool _isLaserComing = false;

    void Start()
    {
        _movementID = Random.Range(-1, 2);
        _ranNum = Random.Range(0, 2) * 2 - 1;
        int _luckyShield = Random.Range(0, 3);
        if(_luckyShield == 1)
        {
            _shieldEffect.SetActive(true);
            _isShieldEnable = true;
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player component is missing!");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("Animator is missing!");
        }

        _enemyExplosionSource = GetComponent<AudioSource>();
        if(_enemyExplosionSource == null)
        {
            Debug.LogError("EnemyAudio is Missing!");
        }
        else
        {
            _enemyExplosionSource.clip = _enemyExplosionClip;
        }
    }

    void Update()
    {
        
        if(_isLaserComing)
        {
            transform.Translate(new Vector3(_ranNum * 5, -1, 0) * _speed * Time.deltaTime);
        }
        else
        {
            switch (_movementID)
            {
                case 0:
                    NormalMovement();
                    break;
                default:
                    DiagonalMovement(_movementID);
                    break;
            }
        }    
        
        RapidLaser(); 
    }

    public void LaserDetected(bool status)
    {
        _isLaserComing = status;
    }

    void NormalMovement()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange)
        {
           transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        BoundaryRespawn();
    }

    void DiagonalMovement(int direct)
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(direct, -1, 0).normalized * _speed * Time.deltaTime);
        }
        BoundaryRespawn();
    }

    void BoundaryRespawn()
    {
        if (transform.position.y <= -8.5f)
        {
            float randomX = Random.Range(-12f, 12f);
            transform.position = new Vector3(randomX, 10f, 0);
        }
        if (transform.position.x > 13.3f || transform.position.x < -13.3f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
    }

    void RapidLaser()
    {
        if (Time.time > _canFireTime && _fireCheck)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFireTime = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser laserScript = enemyLaser.GetComponentInChildren<Laser>();
            laserScript.AssignEnemyLaser();
        }
    }
    
    public void FireLaser()
    {
        GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        Laser laserScript = enemyLaser.GetComponentInChildren<Laser>();
        laserScript.AssignEnemyLaser();
    }
    
    public void LaserHit()
    {
        if(!_isShieldEnable)
        {
            _fireCheck = false;
            if (_player != null)
            {
                _player.AddScore(10); //communicate to player to add score
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemyExplosionSource.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            _speed = 0.5f;
            Destroy(gameObject, 2.4f);
        }
        else
        {
            _isShieldEnable = false;
            _shieldEffect.SetActive(false);
        } 
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        if(!_isShieldEnable)
        {
            _fireCheck = false;
            Player player = other.GetComponent<Player>();
            if (other.tag == "Player")
            {
                if (player != null)
                {
                    player.Damage();
                }
                _anim.SetTrigger("OnEnemyDeath");
                _enemyExplosionSource.Play();
                GetComponent<BoxCollider2D>().enabled = false;
                _speed = 0.5f;
                Destroy(gameObject, 2.4f);
            }
        }
        else
        {
            _isShieldEnable = false;
            _shieldEffect.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
