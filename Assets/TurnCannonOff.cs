using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCannonOff : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("TurnOff", 5f);
    }
    private void TurnOff() 
    {
        gameObject.SetActive(false);
    }
}
