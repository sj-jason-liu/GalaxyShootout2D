using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLaser : MonoBehaviour
{
    [SerializeField]
    private float _scaleSpeed = 1f;
    private Vector3 _scaleRange;

    private void Start()
    {
        _scaleRange = new Vector3(_scaleSpeed, _scaleSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Time.deltaTime * _scaleRange;
        if(transform.localScale.x > 4)
        {
            Destroy(gameObject);
        }
    }
}
