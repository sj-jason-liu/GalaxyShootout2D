using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    private bool _isEnemyShooting = false;

    // Update is called once per frame
    void Update()
    {
        if(!_isEnemyShooting)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public void AssignEnemyLaser()
    {
        _isEnemyShooting = true;
        gameObject.tag = "EnemyLaser";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyShooting)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy" && !_isEnemyShooting)
        {
            if (other.GetComponent<Enemy>() == null)
            {
                other.GetComponent<Enemy_DetectBomb>().LaserHit();
            }
            else
            {
                other.GetComponent<Enemy>().LaserHit();
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Powerup" && _isEnemyShooting)
        {
            other.GetComponent<Powerup>().EnemyLaserHit();
            Destroy(gameObject);
        }    
    }

    private void OnBecameInvisible()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }
}
