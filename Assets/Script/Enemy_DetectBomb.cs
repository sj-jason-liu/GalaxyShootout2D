using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DetectBomb : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Vector3 _randomCirclePosition;
    [SerializeField]
    private float _randomCircleRadius = 5f;

    private Player _player;
    [SerializeField]
    private AudioClip _enemyExplosionClip;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _exploPrefab;
    [SerializeField]
    private float _detectRange = 5f;
    private bool _isPlayerInRange = false;

    void Start()
    {
        StartCoroutine(InstantMovement());
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player component is missing!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("EnemyAudio is Missing!");
        }
    }

    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= _detectRange)
        {
            _isPlayerInRange = true;
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            _isPlayerInRange = false;
        }
    }

    IEnumerator InstantMovement()
    {
        int moveCount = 0;
        while (true)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y <= 6f && moveCount < 5)
            {
                while (moveCount < 5 && !_isPlayerInRange)
                {
                    Vector3 rCP = _randomCirclePosition;
                    rCP = Random.insideUnitCircle * _randomCircleRadius;
                    if (rCP.y >= 6f || rCP.y <= -5f)
                    {
                        yield return null;
                    }
                    else
                    {
                        transform.position = rCP;
                        moveCount++;
                    }
                    yield return new WaitForSeconds(3f);
                }
            }
            else if (transform.position.y < -8)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_randomCirclePosition, _randomCircleRadius);
        Gizmos.DrawWireSphere(transform.position, _detectRange);
        Gizmos.color = Color.red;
    }

    public void LaserHit()
    {
        if (_player != null)
        {
            _player.AddScore(10); //communicate to player to add score
        }
        Instantiate(_exploPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }
            Instantiate(_exploPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f);
        }
    }
}
