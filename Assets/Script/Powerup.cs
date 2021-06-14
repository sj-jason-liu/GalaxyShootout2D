using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField] //0 = triple shot, 1 = speed, 2 = shields
    private int _powerupID;
    [SerializeField]
    private GameObject _powerupDestroyPrefab;
    private GameObject _player;
    [SerializeField]
    private AudioClip _powerupAudioClip;
    private AudioSource _powerupAudioSource;
    private bool _pressedCollect = false;

    private void Start()
    {
        _powerupAudioSource = GetComponent<AudioSource>();
        if(_powerupAudioSource == null)
        {
            Debug.Log("Audio Source is missing!");
        }
        else
        {
            _powerupAudioSource.clip = _powerupAudioClip;
        }
        _player = GameObject.Find("Player");
        if(_player == null)
        {
            Debug.LogError("Player couldn't found!");
        }
    }

    void Update()
    {
        if(_pressedCollect)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 3 * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y <= -8f)
            {
                Destroy(gameObject);
            }
        }  
    }

    public void StartCollect()
    {
        _pressedCollect = true;
        Invoke("StopCollect", 3f);
    }

    void StopCollect()
    {
        _pressedCollect = false;
    }

    public void EnemyLaserHit()
    {
        //play destroied animation
        _speed = 0f;
        Instantiate(_powerupDestroyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            _powerupAudioSource.Play();
            if(player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.AmmoCollected(); 
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.SpeedBoostActive();
                        break;
                    case 4:
                        player.AddLives();
                        break;
                    case 5:
                        player.FireworkshotActive();
                        break;
                    case 6:
                        player.PoisonActived();
                        break;
                }
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            Destroy(gameObject, 1f);
        }
    }
}
