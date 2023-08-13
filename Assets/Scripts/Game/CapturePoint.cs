using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField]
    private Team team;


    [SerializeField]
    GameObject Suitcase;


    [SerializeField]
    CapturePoint opisite;

    public void Update()
    {
        Suitcase.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
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
                        pc.EyeOverlay.SetActive(true);
                        pc.HoldingFlagicon.SetActive(true);
                        Suitcase.SetActive(false);
                        pc.suitcase = Suitcase;
                    }

                    if(team == Team.Black && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.rightCapCount++;
                        pc.HoldingFlagicon.SetActive(false);
                        pc.EyeOverlay.SetActive(false);
                        opisite.Suitcase.SetActive(true);
                        pc.suitcase = null ;
                    }
                    break;


                case Team.White:
                    if (team == Team.Black)
                    {
                        pc.HoldingFlag = true;
                        pc.CanUsePowerups = false;
                        PlayerManager.Instance.ShowOtherPlayers(team);
                        pc.EyeOverlay.SetActive(true);
                        pc.HoldingFlagicon.SetActive(true);
                        Suitcase.SetActive(false);
                        pc.suitcase = Suitcase;
                    }

                    if (team == Team.White && pc.HoldingFlag)
                    {
                        pc.HoldingFlag = false;
                        pc.CanUsePowerups = true;
                        Debug.Log("CapturePoint");
                        PlayerManager.Instance.HideOtherPlayer(team);
                        PlayerManager.Instance.leftCapCount++;
                        pc.EyeOverlay.SetActive(false);
                        pc.HoldingFlagicon.SetActive(false);
                        opisite.Suitcase.SetActive(true);
                        pc.suitcase = null;
                    }
                    break;
            }
        }
    }
}
