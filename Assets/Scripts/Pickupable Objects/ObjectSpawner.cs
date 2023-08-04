using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private float timebeforeRespawn;
    [SerializeField]
    private GameObject BaseObject;

    private GameObject aboveObjects;
    bool respawning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator RespawnItem()
    {
        respawning = false;
        yield return new WaitForSeconds(timebeforeRespawn);
        aboveObjects = Instantiate(BaseObject,transform);
        respawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(aboveObjects == null && respawning)
        {
            StartCoroutine(RespawnItem());
        }
    }
}
