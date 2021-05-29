using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{ 
    private Vector3 _originPosition;
    [SerializeField]
    private float _shakeRadius = 0.3f;
    [SerializeField]
    private float _shakeDecrease = 4f; //shake time decrease value
    private float _shakeValue; //set the float to engage the shake

    void Start()
    {
        _originPosition = transform.position; //set teh originposition to camera position
    }

    void Update()
    {
        if (_shakeValue > 0)
        {
            transform.position = _originPosition + Random.insideUnitSphere * _shakeRadius;
            _shakeValue -= Time.deltaTime * _shakeDecrease; //the higher the decrease number, the shorter the shake
        }
        else
        {
            _shakeValue = 0;
        }
    }

    public void EngageShake(int _shake) //receive int 1 when get damage
    {
        _shakeValue = _shake;
    }
}
