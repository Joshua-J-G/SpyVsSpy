using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Team
{
    none,
    White,
    Black
}





public class PlayerController : MonoBehaviour
{
    public Team team = Team.none;

  
    public PowerUps PowerUpSlot;


    [SerializeField]
    private LayerMask WhiteTeam;
    [SerializeField]
    private LayerMask BlackTeam;

    [SerializeField]
    private LayerMask SeelALl;

    [Space]

    bool InPowerUP = false;


    [Header("PlayerRequired")]
    [SerializeField]
    private CharacterController CC;

    public Camera playerCamera;
    [SerializeField]
    public LayerMask GroundMask;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.2f;


    [Header("Weapon Prefab")]
    [SerializeField]
    private GameObject WeaponPrefab;

    Vector3 velocity;
    bool isGrounded;
    private Player Input;

    [Space]


    [Header("PlayerStats")]
    [SerializeField]
    private float PlayerSpeed = 12f;
    [SerializeField]
    private float StickSensitivity = 100f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;

    private void OnEnable()
    {
        Input = new();
        Input.Enable();
        
    }

    private void OnDisable()
    {
        Input.Disable();
    }


    public void Respawn()
    {
        CC.enabled = false;
        gameObject.gameObject.transform.position = PlayerManager.Instance.SpawnPoints[Random.Range(0, PlayerManager.Instance.SpawnPoints.Count)].transform.position;
        CC.enabled = true;
    }

    public void StartPlayer()
    {

        Respawn();
        Time.timeScale = 1f;
    }

    public void Start()
    {
        Time.timeScale = 0f;
       

        Cursor.lockState = CursorLockMode.Locked;
        PlayerManager.Instance.RegisterUserToTeam(this);


    }

    private void Update()
    {
        if (!InPowerUP)
        {
            switch (team)
            {
                case Team.none:
                    return;
                case Team.White:
                    playerCamera.cullingMask = BlackTeam;

                    break;
                case Team.Black:
                    playerCamera.cullingMask = WhiteTeam; break;

            }
        }
      
        MovePlayer();
     
        JumpGravity();
        //UpdateCamera();
       
    }


    float durationSeeAll = 5f;
    public void StartSeeALLPowerUp()
    {
        PowerUpSlot = PowerUps.ShowEnemiesToPlayer;
       
    }

    public void EndSeeALLPowerUP()
    {
        PowerUpSlot = PowerUps.none;
        InPowerUP = false;
    }



    public void UsePowerUpSlot()
    {
        switch (PowerUpSlot)
        {
            case PowerUps.ShowEnemiesToPlayer:

                InPowerUP = true;
                playerCamera.cullingMask = SeelALl;
                Invoke("EndSeeALLPowerUP", durationSeeAll);


                break;
            case PowerUps.freeze:
                FreezeOtherPlayers();
                break;
        
        
        }

    }


    public void FreezeOtherPlayers()
    {
        PowerUpSlot = PowerUps.none;
        foreach (PlayerController c in PlayerManager.Instance.GetTeam(team))
        {
            c.freeze();
        }
           

    }

    bool canMove = true;

    [SerializeField] private float timeofFreeze = 3f;

    public void freeze()
    {
        CancelInvoke("endFreeze");
        
        canMove = false;
        Invoke("endFreeze", timeofFreeze);
    }

    private void endFreeze()
    {
        canMove = true;
    }

    public void Dammage()
    {
        Respawn();
    }

    private void  LateUpdate()
    {
        UpdateCamera();
      
    }

    public void Shoot()
    {
        switch (team)
        {
            case Team.none:
                return;
            case Team.White:
                WeaponPrefab.GetComponent<IWeapon>().Shoot(BlackTeam);

                break;
            case Team.Black:
                WeaponPrefab.GetComponent<IWeapon>().Shoot(WhiteTeam); break;

        }

 
    }

    public void JumpGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);

        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }


        if((jumpDown) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;

        CC.Move(velocity * Time.deltaTime);
    }


    Vector2 Move;
    Vector2 CameraMove;

    bool jumpDown;

    [SerializeField]
    GameObject CamContainer;

    public void MovementUpdated(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    public void CameraUpdated(InputAction.CallbackContext context)
    {
        CameraMove = context.ReadValue<Vector2>();
    }

    public void JumpUpdated(InputAction.CallbackContext context)
    {
        jumpDown = context.ReadValue<float>() == 1;
    }







    public void MovePlayer()
    {
        if(!canMove)
        {
            return;

        }

        Vector3 move = transform.right * Move.x+ transform.forward * Move.y;


        CC.Move(move * PlayerSpeed * Time.deltaTime);
    }

    float xRotation = 0f;

    public void UpdateCamera()
    {
        Vector2 StickPosition = CameraMove * StickSensitivity * (Time.deltaTime);
        xRotation -= StickPosition.y;
        gameObject.transform.Rotate(Vector3.up * StickPosition.x);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

  
}
