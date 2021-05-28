﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaser;

    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioClip _enemyExplosionClip;
    private AudioSource _enemyExplosionSource;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser laserScript = enemyLaser.GetComponentInChildren<Laser>();
            laserScript.AssignEnemyLaser();
        }
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8.5f)
        {
            float randomX = Random.Range(-12f, 12f);
            transform.position = new Vector3(randomX, 10f, 0);
        }
    }

    public void LaserHit()
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
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
}
