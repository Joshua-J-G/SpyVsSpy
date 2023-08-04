using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField]
    private Team SpawnPointTeam;
    public void Start()
    {
        switch(SpawnPointTeam)
        {
            case Team.none:
                PlayerManager.Instance.SpawnPoints.Add(this.gameObject);
                break;
            case Team.Black:
                PlayerManager.Instance.SpawnPointsBlack.Add(this.gameObject);
                break;
            case Team.White:
                PlayerManager.Instance.SpawnPointsWhite.Add(this.gameObject);
                break;
        }
       
    }
}
