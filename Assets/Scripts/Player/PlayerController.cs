using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    #region Variables
    public Team team = Team.none;

    public bool CanUsePowerups = true;
  
    public PowerUps PowerUpSlot;

    public bool HoldingFlag = false;


    [SerializeField] GameObject IcePowerIcon, SeePowerIcon; 

    [SerializeField] GameObject FreezeOverlay;
    public GameObject EyeOverlay;

    public LayerMask WhiteTeam;

    public LayerMask BlackTeam;


    public LayerMask SeelALl;

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
    public float StickSensitivity = 100f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;




    [SerializeField]
    private ParticleSystem MuzzelFash;


    [SerializeField]
    private ParticleSystem Smoke;

    public GameObject HoldingFlagicon;


    #endregion

    #region Enables and Disabled
    private void OnEnable()
    {
        Input = new();
        Input.Enable();
        
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    #endregion

    #region GameStartandBasicFunctions
    public void Respawn()
    {
        CC.enabled = false;
        gameObject.gameObject.transform.position = PlayerManager.Instance.Spawnpoint(team).position;
        CC.enabled = true;
    }

    public void StartPlayer()
    {

        Respawn();
        Time.timeScale = 1f;
    }

    [SerializeField] bool SinglePlayer;

    public void Start()
    {
        CanUsePowerups = true;
       // Input.Player1.Attack.performed += _ => Shoot();

        Time.timeScale = 0f;
        
        if(SinglePlayer)
        {
            Time.timeScale = 1f;
            canMove = true;
        }

        Cursor.lockState = CursorLockMode.Locked;

        if (!SinglePlayer)
        {
            PlayerManager.Instance.RegisterUserToTeam(this);
        }


    }
    public bool CullingMaskReset = false;
    private void Update()
    {


        switch(PowerUpSlot)
        {
            case PowerUps.none:
                IcePowerIcon.SetActive(false);
                    SeePowerIcon.SetActive(false);
                break;
            case PowerUps.freeze:
                IcePowerIcon.SetActive(true);
                SeePowerIcon.SetActive(false);
                break;
            case PowerUps.ShowEnemiesToPlayer:
                IcePowerIcon.SetActive(false);
                SeePowerIcon.SetActive(true);
                break;
        }



        if (!InPowerUP && !CullingMaskReset)
        {
            switch (team)
            {
                case Team.none:
                    return;
                case Team.White:
                    playerCamera.cullingMask = BlackTeam;

                    break;
                case Team.Black:
                    playerCamera.cullingMask = WhiteTeam; 
                    break;

            }
        }

        //UpdateCamera();
        MovePlayer();

        JumpGravity();
    }

    #endregion

    #region PowerUps
    float durationSeeAll = 5f;
    public void StartSeeALLPowerUp()
    {
        PowerUpSlot = PowerUps.ShowEnemiesToPlayer;
       
    }

    public void EndSeeALLPowerUP()
    {
        PowerUpSlot = PowerUps.none;
        InPowerUP = false;
        foreach (PlayerController c in PlayerManager.Instance.GetTeam(team))
        {
            if (!c.HoldingFlag)
            {
                c.EyeOverlay.SetActive(false);
            }
        }
    }



    public void UsePowerUpSlot()
    {
        if(!CanUsePowerups)
        {
            return;
        }

        switch (PowerUpSlot)
        {
            case PowerUps.ShowEnemiesToPlayer:

                InPowerUP = true;

                foreach (PlayerController c in PlayerManager.Instance.GetTeam(team))
                {
                    c.EyeOverlay.SetActive(true);
                }

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
        FreezeOverlay.SetActive(true);
        CancelInvoke("endFreeze");
        
        canMove = false;
        Invoke("endFreeze", timeofFreeze);
    }

    private void endFreeze()
    {
        canMove = true;
        FreezeOverlay.SetActive(false);
    }

    #endregion

    #region WeaponBasics

    public GameObject suitcase;

    public void Dammage()
    {
        if (HoldingFlag)
        {


            HoldingFlag = false;
            CanUsePowerups = true;
            PlayerManager.Instance.HideOtherPlayer(team);
            HoldingFlagicon.SetActive(false);
            EyeOverlay.SetActive(false);
            suitcase.SetActive(true);
        }



        Respawn();
    }

    private void LateUpdate()
    {

        UpdateCamera();
    }

    private void FixedUpdate()
    {
     
      
    }

    public void PauseMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PlayerManager.Instance.Pause();
        }
    }


    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
          
            MuzzelFash.Play();
            Smoke.Play();

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
 
    }
    #endregion

    #region PlayerMovement
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

        CC.Move((velocity * Time.deltaTime) + PlayerMovement);
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

    private Vector3 PlayerMovement;

    public void MovePlayer()
    {
        if (!canMove && !SinglePlayer)
        {
            return;

        }

        Vector3 move = transform.right * Move.x + transform.forward * Move.y;

        PlayerMovement = move * PlayerSpeed * Time.deltaTime;
        //CC.Move(move * PlayerSpeed * Time.deltaTime);
    }

    float xRotation = 0f;

    public void UpdateCamera()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        Vector2 StickPosition = (CameraMove * StickSensitivity) * (Time.deltaTime);
        xRotation -= StickPosition.y;
        gameObject.transform.Rotate(Vector3.up * StickPosition.x);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    #endregion
    #region WeaponSwitching

    int SelectedSlot = 0;



    /// <summary>
    /// Called By the New InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeaponRight(InputAction.CallbackContext context)
    {
        //context.performed is called on the down action of the button and not the up action meaning this is called once instead of twice
        if(context.performed)
        {
            //selected slot should increase by 1 and should probably be capped at 3


            SwitchWeapon();
        }
    }

    /// <summary>
    /// Called By the New InputSystem
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeaponLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //selected slot should decrese by 1 and should probably be capped at 0

            SwitchWeapon();
        }
    }

    /// <summary>
    /// Switch Weapons For Player
    /// </summary>
    public void SwitchWeapon()
    {
       //for creating new weapons look at how the pistol gameobject and script is setup 
       //i would just recomend having other weapons being hiddend and showed when the slot is selected 
    }

    #endregion
}
