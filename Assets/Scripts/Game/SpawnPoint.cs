using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void Start()
    {
        PlayerManager.Instance.SpawnPoints.Add(this.gameObject);
    }
}
