using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUps
{
    ShowEnemiesToPlayer
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
            }

           
        }
    }
}
