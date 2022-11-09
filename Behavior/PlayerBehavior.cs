using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerBehavior : MonoBehaviour
{
    #region SPEEDS and RANGES
    [Header(" ������������ ������")]
    [Tooltip("�������� ������")][SerializeField] private float speedWalk = 4f;
    [Tooltip("�������� ����")][SerializeField] private float speedRun = 10f;
    [Tooltip("�������� �������� ������")][SerializeField] private float speedRotate = 117f;
    [Tooltip("���� ������")][SerializeField] private float speedJump = 4f;
    [Space]
    [Header(" �������� ������ �� ������� ������")]
    [Tooltip("��������� ������ ������������� ������ � ����������� ������ ������, �������� false, ����� ��������� �������� ��� ������� �� ������� (W) | " +
        "(��� ������������� ���.������ �������� ����� ������� �������������)")] 
    public bool PlayerRotateToCamera = false;
    [Header("����-����� (��� ������)")]
    [Tooltip("����� ����� ������� ������ � ��������� ����")][SerializeField] private LayerMask layer;
    #endregion

    #region INIT
    private Rigidbody rbPlayer;
    private CapsuleCollider playerCollider;
    private GameObject cameraTransform;
    private Animator animationPlayer;
    [Space]
    #endregion

    #region PLATFORMS
    [Header("����� ���������")]
    [Tooltip("��������� ������������ ��������")] public DynamicJoystick joystick;
    [Tooltip("�������, ���� ������ ������������ ���������� ��� WINDOWS")] public bool MOBILE = true;
    #endregion

    #region CONST(float-int)
    private const float setRadiusOffCollider = 0.01f;

    #endregion

    #region INPUT "AXIS"
    private float horizontal;
    private float vertical;
    #endregion

    /// <summary>
    /// ����� ������� (C#) : "������ ��������� ��������" @.
    /// </summary>

    #region UNITY "METHODS"
    void Start()
    {
        Initializer();
    }

   

    void Update()
    {
        Input(MOBILE);
       



    }
    private void LateUpdate()
    {
        PlayerRotateToCameraForward(PlayerRotateToCamera);
    }



    private void FixedUpdate()
    {
        PlayerMoveForward();

        PlayerRotate();

        Jump(IsGrounded());


    }
    #endregion


    #region INITIALIZER [for start game]
    private void Initializer()
    {
        if (MOBILE) { PlayerRotateToCamera = true; }
        cameraTransform = GameObject.Find("CameraBehavior");
        rbPlayer = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        animationPlayer = GetComponent<Animator>();

        //����� ������� � �����
        animationPlayer.SetBool("Idle", true);
        animationPlayer.SetBool("Walk",false);

    }
    #endregion

    #region INPUT (AXIS)/ PLATFORMS
    private void Input(bool mobile)
    {
        if (mobile) { PlatformInput("ANDROID"); }
        else { PlatformInput("WINDOWS"); }
    }
    /// <summary>
    /// ����� ��������� (����������) "ANDROID" ��� "WINDOWS"
    /// </summary>
    /// <param name="platform"></param>
    private void PlatformInput(string platform)
    {
        if(platform == "WINDOWS")
        {
            joystick.gameObject.SetActive(false);
            horizontal = UnityEngine.Input.GetAxis("Horizontal");
            vertical = UnityEngine.Input.GetAxis("Vertical");
        }

        else if(platform == "ANDROID")
        {
            horizontal = Mathf.Lerp(horizontal, joystick.Horizontal, 1);
            vertical = Mathf.Lerp(vertical, joystick.Vertical, 1);
        }
        
    }
    #endregion


    #region MOVE (RIGIDBODY)

    #region FORWARD
    void PlayerMoveForward()
    {
        float speed = UnityEngine.Input.GetKey(KeyCode.LeftShift) ? speedRun : speedWalk;

        #region ANIMATIONS "IDLE" - "WALK" - "RUN"
        if (vertical > 0)
        {
            animationPlayer.SetBool("Idle", false);
            if (speed == speedWalk) { animationPlayer.SetBool("Walk", true); animationPlayer.SetBool("Run", false); }
            else {
                cameraTransform.transform.LookAt(this.transform);
                animationPlayer.SetBool("Run", true); animationPlayer.SetBool("Walk", false); }
        }
        else if (vertical == 0) { animationPlayer.SetBool("Idle", true); animationPlayer.SetBool("Walk", false); animationPlayer.SetBool("Run", false); }

        #endregion

        rbPlayer.MovePosition(this.transform.position + transform.forward * vertical * speed *  Time.fixedDeltaTime);
    }
    #endregion


    #region ROTATE "HORIZONTAL" [move-idle]  (WINDOWS) | [idle] (ANDROID)  
    /// <summary>
    /// ������������� ������ � ����������� ���������� �� ��� "HORIZONTAL" (������ ��� ����������� ���� ��������, ���� ����� ��������� )
    /// </summary>
    void PlayerRotate()
    {
       
            Vector3 rotateUp = Vector3.up * speedRotate * horizontal;
            Quaternion rotationPlayerEngle = Quaternion.Euler(rotateUp * Time.fixedDeltaTime);

            rbPlayer.MoveRotation(rotationPlayerEngle * rbPlayer.rotation);
        
    }
    #endregion

    #region UP (JUMP)
    /// <summary>
    ///��������� ������, isGrounded - �������� �� "ENVIRONMENT" : |[������������ ��� WINDOWS, ������������� �������� ��� ���.������]|
    /// </summary>
    /// <param name="isGrounded"></param>
    void Jump(bool isGrounded)
    {
        if (isGrounded && UnityEngine.Input.GetKey(KeyCode.Space))
        {
            Vector3 up = Vector3.up * speedJump;

            rbPlayer.AddForce(up, ForceMode.Impulse);

        }
    }
    #endregion

    #endregion



    #region CAMERA AND PLAYER "AUTO ROTATION"

    /// <summary>
    /// ����� ������������� ������������� ������ � ����������� ������ : [����������� true, ��� ���� ���������]
    /// </summary>
    /// <param name="isRotatePlayerToCamera"></param>
    void PlayerRotateToCameraForward(bool isRotatePlayerToCamera)
    {
        if (isRotatePlayerToCamera  && (UnityEngine.Input.GetKey(KeyCode.W) || joystick.Vertical > 0))
        {
            Quaternion playerEngle = cameraTransform.transform.rotation;
            playerEngle.x = this.transform.rotation.x;
            playerEngle.y = cameraTransform.transform.rotation.y;
            playerEngle.z = this.transform.rotation.z;

            transform.rotation = Quaternion.Slerp(this.transform.rotation, playerEngle, Time.fixedDeltaTime);
        }
    }
    #endregion


    #region "Check "GROUND" (IsGrounded)
    /// <summary>
    /// ����� ������ true, ���� ����� ����� �� �������� ����
    /// </summary>
    /// <returns></returns>
    public bool  IsGrounded()
    {
        Vector3 buttom = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z);

        bool isGrounded = Physics.CheckCapsule(playerCollider.bounds.center,buttom,setRadiusOffCollider,layer,QueryTriggerInteraction.Ignore);
        return isGrounded;
    }
    #endregion


}
