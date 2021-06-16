using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject[] _enemy;
    private GameObject _nearestEnemy;
    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    
    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if(_enemy == null)
        {
            return;
        }
        FindTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(_nearestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _nearestEnemy.transform.position, _speed * Time.deltaTime);

            Vector3 dir = (_nearestEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float offset = -90f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy")
        {
            if(other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().LaserHit();
            }
            else if(other.GetComponent<Enemy_DetectBomb>() != null)
            {
                other.GetComponent<Enemy_DetectBomb>().LaserHit();
            }
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void FindTarget()
    {
        float nearestDist = Mathf.Infinity;

        foreach(GameObject en in _enemy)
        {
            float dist = Vector3.Distance(transform.position, en.transform.position);
            if(dist < nearestDist)
            {
                nearestDist = dist;
                _nearestEnemy = en;
            }
        }
        
    }
}
