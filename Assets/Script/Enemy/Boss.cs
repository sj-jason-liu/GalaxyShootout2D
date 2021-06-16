using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _moveAXaxis = 10f;
    
    private int _movementID;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MovementIDSwitcher());
        StartCoroutine(XAxisSwitcher());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_movementID);
        switch(_movementID)
        {
            case 0:
                //movement 1
                break;
            case 1:
                //movement 2
                break;
        }
    }

    void MovementA()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-_moveAXaxis, transform.position.y, 0), _speed * Time.deltaTime);
    }

    IEnumerator XAxisSwitcher()
    {
        while(true)
        {
            _moveAXaxis *= -1;
            yield return _moveAXaxis;
            yield return new WaitForSeconds(10f);
        }
    }
    
    IEnumerator MovementIDSwitcher()
    {
        while(true)
        {
            _movementID = Random.Range(0, 2);
            yield return _movementID;
            yield return new WaitForSeconds(10f);
        }   
    }
}
