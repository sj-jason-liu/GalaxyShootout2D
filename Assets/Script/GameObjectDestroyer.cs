using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }
}
