using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _moveAXaxis = 10f;

    [SerializeField]
    private GameObject[] _movePosition;
    private GameObject _player;
    [SerializeField]
    private GameObject _attackAPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _bossHealthBar;
    [SerializeField]
    private GameObject _rightDamage, _leftDamage;

    private Animator _anim;

    private UIMananger _uiManager;

    private Vector3 _posToSpawn;
    
    private int _movementID;
    private int _movePositionNum = 0;
    private int _attackID;
    private int _enemySpawned = 0;
    private int _bossHealth = 100;

    private bool _attackMethodAStarted = false;
    private bool _attackMethodBStarted = false;
    private bool _isDefeated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if(_player == null)
        {
            Debug.LogError("Player is missing!");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIMananger>();
        if(_uiManager == null)
        {
            Debug.LogError("UI is mission");
        }
        _anim = GetComponent<Animator>();
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(MovementIDSwitcher());
        StartCoroutine(XAxisSwitcher());
        StartCoroutine(AttackIDSwitcher());
        Invoke("AttackASwitch", 6f);
        Invoke("AttackBSwitch", 8f);
    }

    // Update is called once per frame
    void Update()
    {
        _posToSpawn = new Vector3(Random.Range(-12f, 12f), 10f, 0);
        if(!_isDefeated)
        {
            if (transform.position.y >= 5.6f)
            {
                transform.Translate(Vector3.down * 2f * Time.deltaTime);
            }
            else
            {
                GetComponent<CircleCollider2D>().enabled = true;
                switch (_movementID)
                {
                    case 0:
                        MovementA();
                        break;
                    case 1:
                        MovementB();
                        break;
                }
                FaceRotation();
            }
            Attacking();
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        Debug.Log(_movementID);
    }

    void MovementA()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-_moveAXaxis, 5.5f, -1), _speed * Time.deltaTime);
    }

    void MovementB()
    {
        int p = _movePositionNum % _movePosition.Length;
        
        if (Vector3.Distance(transform.position, _movePosition[p].transform.position) < 0.5f)
        {
            _movePositionNum++;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _movePosition[p].transform.position, _speed * Time.deltaTime);
        }
    }

    void FaceRotation()
    {
        Vector3 playerDir = (_player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
        float offset = 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    void Attacking()
    {
        if (_attackID == 0 && _attackMethodAStarted)
        {
            Instantiate(_attackAPrefab, transform.position, transform.rotation);
            Invoke("AttackASwitch", Random.Range(3f, 5f));
            _attackMethodAStarted = false;
        }
        else if (_attackID == 1 && _attackMethodBStarted)
        {
            _enemySpawned = 0;
            StartCoroutine(AttackMethodB());
            _attackMethodBStarted = false;
        }
    }
    
    IEnumerator AttackMethodB()
    {
        while(true)
        {
            while(_enemySpawned < 5)
            {
                Instantiate(_enemyPrefab, _posToSpawn, Quaternion.identity);
                _enemySpawned++;
                yield return new WaitForSeconds(1f);
            }
            Invoke("AttackBSwitch", Random.Range(5f, 8f));
            break;
        }
    }

    public void LaserHit()
    {
        _bossHealth -= 2;
        Damage();
        _player.transform.GetComponent<Player>().AddScore(20);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _bossHealth -= 10;
            Damage();
            _player.transform.GetComponent<Player>().Damage();
        }
    }

    void Damage()
    {
        _uiManager.BossHealthUpdate(_bossHealth);
        _anim.SetTrigger("Damage");

        if (_bossHealth <= 0)
        {
            _isDefeated = true;
            _attackMethodAStarted = false;
            _attackMethodBStarted = false;
            _anim.SetTrigger("Defeated");
            _bossHealthBar.SetActive(false);
            Destroy(gameObject, 5f);
        }
        else if(_bossHealth > 0 && _bossHealth <= 25)
        {
            //enable left smoke animation
            _leftDamage.SetActive(true);
        }
        else if(_bossHealth < 50 && _bossHealth > 25)
        {
            //enable right smoke animation
            _rightDamage.SetActive(true);
        }
    }

    IEnumerator XAxisSwitcher()
    {
        while(true)
        {
            _moveAXaxis *= -1;
            yield return _moveAXaxis;
            yield return new WaitForSeconds(5f);
        }
    }
    
    IEnumerator MovementIDSwitcher()
    {
        while(true)
        {
            _movementID = Random.Range(0, 2);
            yield return new WaitForSeconds(10f);
        }   
    }

    IEnumerator AttackIDSwitcher()
    {
        while(true)
        {
            _attackID = Random.Range(0, 2);
            yield return new WaitForSeconds(12f);
        } 
    }

    void AttackASwitch()
    {
        _attackMethodAStarted = true;
    }

    void AttackBSwitch()
    {
        _attackMethodBStarted = true;
    }
}
