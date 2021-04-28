using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public string myName = "Jason";
    public int myAge = 31;
    bool isAwake = true;
    [Space]
    private float myWeight = 75.6f;
    public int foodWeight = 3;
    [Space]
    public int bill = 40;
    public float tipRate = 20f;
    public float totalAmount;

    // Start is called before the first frame update
    void Start()
    {
        //float tip = bill * (tipRate / 100);
        totalAmount = bill + (bill * (tipRate / 100));
        Debug.Log("Total amount is " + totalAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
