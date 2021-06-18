using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserStorm : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Destroy(gameObject);
        }
        else if (other.tag == "Powerup")
        {
            other.GetComponent<Powerup>().EnemyLaserHit();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
