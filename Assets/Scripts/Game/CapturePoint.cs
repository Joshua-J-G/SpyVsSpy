using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField]
    private Team team;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            switch (pc.team)
            {
                case Team.Black:
                    if (team == Team.White)
                    {
                        pc.HoldingFlag = true;
                        pc.CanUsePowerups = false;
                        PlayerManager.Instance.ShowOtherPlayers(team);
                    }

                    if(team == Team.Black && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.rightCapCount++;
                    }
                    break;


                case Team.White:
                    if (team == Team.Black)
                    {
                        pc.HoldingFlag = true;
                        pc.CanUsePowerups = false;
                        PlayerManager.Instance.ShowOtherPlayers(team);
                    }

                    if (team == Team.White && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.leftCapCount++; 
                    }
                    break;
            }
        }
    }
}
