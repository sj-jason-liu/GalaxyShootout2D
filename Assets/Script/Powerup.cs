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
    private AudioClip _powerupAudioClip;
    private AudioSource _powerupAudioSource;

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
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -8f)
        {
            Destroy(gameObject);
        }
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
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoCollected();
                        break;
                    case 4:
                        player.AddLives();
                        break;
                    case 5:
                        player.PoisonActived();
                        break;
                    case 6:
                        player.FireworkshotActive();
                        break;
                }
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            Destroy(gameObject, 1f);
        }
    }
}
