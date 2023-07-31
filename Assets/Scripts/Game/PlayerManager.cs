using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;


    [SerializeField]
    private GameObject PlayerPrefab;

    public PlayerInputManager manager;


    [SerializeField]
    private List<GameObject> Players = new();


    public List<GameObject> SpawnPoints = new();



    [SerializeField]
    private List<PlayerController> Whiteteam = new();
    [SerializeField]
    private List<PlayerController> Blackteam = new();

    [SerializeField]
    ControllerWait cwait;


    public void PlayerJoined(PlayerInput input)
    {
        Debug.Log("PlayerJoined");
     
    }

 
    public List<PlayerController> GetTeam(Team team)
    {
        switch (team)
        {
            case Team.Black:

                return Whiteteam;

                

            case Team.White:

                return Blackteam;

                
        
        }

        return null;

    }



    public void RegisterUserToTeam(PlayerController player)
    {
        
     

        if(!Players.Contains(player.gameObject))
        {
            Players.Add(player.gameObject);

        }

        if (!Whiteteam.Contains(player) && !Blackteam.Contains(player))
        {
            

            if (Whiteteam.Count == Blackteam.Count)
            {
                Whiteteam.Add(player);
                player.team = Team.White;
                foreach(Transform child in player.transform)
                {
                    child.gameObject.layer = 31;
                    foreach(Transform child2 in child)
                    {
                        child2.gameObject.layer = 31;
                        foreach (Transform child3 in child2)
                        {
                            child3.gameObject.layer = 31;
                            foreach (Transform child4 in child3)
                            {
                                child4.gameObject.layer = 31;

                            }
                        }
                    }
                }

                player.gameObject.layer = 31; 
            
            }
            else
            {
                Blackteam.Add(player);
                player.team = Team.Black;
                foreach (Transform child in player.transform)
                {
                    child.gameObject.layer = 30;

                    foreach (Transform child2 in child)
                    {
                        child2.gameObject.layer = 30;
                        foreach (Transform child3 in child2)
                        {
                            child3.gameObject.layer = 30;
                            foreach (Transform child4 in child3)
                            {
                                child4.gameObject.layer = 30;

                            }
                        }
                    }
                }
                player.gameObject.layer = 30;
            }
        }


        if (Players.Count >= 2)
        {
            foreach (GameObject c in Players)
            {
                c.GetComponent<PlayerController>().StartPlayer();
            }
        }
    }


    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Players.Count == 2)
        {
            Players[0].GetComponent<PlayerController>().playerCamera.rect = new Rect(0, 0, 1, 0.5f);
            Players[1].GetComponent<PlayerController>().playerCamera.rect = new Rect(0, 0.5f, 1, 0.5f);
        };*/
    }
}
