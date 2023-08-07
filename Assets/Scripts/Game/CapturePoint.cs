using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField]
    private Team team;

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
                        pc.EyeOverlay.SetActive(true);
                    }

                    if(team == Team.Black && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.rightCapCount++;
                        pc.EyeOverlay.SetActive(false);
                    }
                    break;


                case Team.White:
                    if (team == Team.Black)
                    {
                        pc.HoldingFlag = true;
                        pc.CanUsePowerups = false;
                        PlayerManager.Instance.ShowOtherPlayers(team);
                        pc.EyeOverlay.SetActive(true);
                    }

                    if (team == Team.White && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.leftCapCount++;
                        pc.EyeOverlay.SetActive(false);
                    }
                    break;
            }
        }
    }
}
