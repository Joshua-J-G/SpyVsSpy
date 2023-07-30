using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    [SerializeField]
    Transform Shootpoint;

    [SerializeField]
    float MaxDistance;

 
    

    public MonoBehaviour ReturnScript()
    {
        return this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }




    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(LayerMask team)
    {
        Debug.Log("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(Shootpoint.position, Shootpoint.forward, out hit, MaxDistance))
        {
            Debug.Log(hit.collider.gameObject.layer);
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.layer == 30 || hit.collider.gameObject.layer == 31)
            {
                hit.collider.gameObject.GetComponent<PlayerController>().Dammage();
            }
        }
    }
}
