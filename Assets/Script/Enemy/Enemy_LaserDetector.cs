using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LaserDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().LaserDetected(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().LaserDetected(false);
        }
    }
}
