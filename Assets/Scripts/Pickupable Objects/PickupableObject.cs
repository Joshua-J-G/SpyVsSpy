using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUps
{
    none,
    ShowEnemiesToPlayer,
    freeze,
    teleport
}


public class PickupableObject : MonoBehaviour
{

    [SerializeField]
    PowerUps powerUps;

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            switch (powerUps)
            { 
                case PowerUps.ShowEnemiesToPlayer:
                    other.gameObject.GetComponent<PlayerController>().StartSeeALLPowerUp();
                    Destroy(gameObject);
                    break;
                case PowerUps.freeze:
                    other.gameObject.GetComponent<PlayerController>().PowerUpSlot = PowerUps.freeze;
                    Destroy(gameObject);
                    break;
            }

           
        }
    }
}
