using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    #region INIT
    private Transform playerTransform;
    private Transform cameraTransformBeh;
    private Quaternion cameraRot;
    #endregion

    #region SPEEDS and RANGES
    [Header("Позиция камеры от игрока")]
    [Tooltip("Установите эелаемое положение, перед запуском проэкта")] public Vector3 cameraOffset = new Vector3(0f,3f, -6.65f);
    [Header("Чувствительность камеры")] 
    [Range(1,3)] public float speedRotate = 1.5f;
    [Header("ОГРАНИЧЕНИЕ КАМЕРЫ (ось Y)")]
    [Range(-100,200)] public int ClampMin = 1;
    [Range(-100, 200)] public int ClampMax = 44;
    [Header("ИНВЕРТИРОВАННОЕ УПРАВЛЕНИЕ")]
    [Tooltip("Установите галочку, если хотите инвертировать управление")] public bool invert = false;
    #endregion

    #region PLATFORMS
    [Header("ВЫБОР ПЛАТФОРМЫ")]
    [Tooltip("Снимите, если хотите использовать управление под WINDOWS")] public bool MOBILE = true;
    [Tooltip("Требуется динамический джойстик")] public DynamicJoystick joystick;

    #endregion

    /// <summary>
    /// АВТОР СКРИПТА (C#) : "Афонин Александр Игоревич" @.
    /// </summary>

    #region UNITY "METHODS"
    void Start()
    {
        Initializer();
       
    }

    
    void Update()
    {
        cameraTransformBeh.position = playerTransform.position;
        
    }

    private void LateUpdate()
    {
        CameraRotate(ClampMin,ClampMax,speedRotate, invert,MOBILE);

    }
    #endregion

    #region Camera "ROTATION" / INPUT PLATFORM
    private void CameraRotate(int min, int max, float speed, bool invert,bool mobile)
    {
        int invertial = invert ? -1 : 1;

        if (mobile) { PlatformInput(speed, invertial, "ANDROID"); }
        else { PlatformInput(speed, invertial, "WINDOWS"); }

        cameraRot.x = Mathf.Clamp(cameraRot.x, min, max);

        cameraTransformBeh.localRotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
    }
    /// <summary>
    /// ВЫБОР ПЛАТФОРМЫ (УПРАВЛЕНИЕ) "ANDROID" или "WINDOWS"
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="invertial"></param>
    /// <param name="platform"></param>
    private void PlatformInput(float speed, int invertial,string platform)
    {
        if(platform == "ANDROID")
        {
            cameraRot.x += joystick.Vertical * -invertial * speed;
            cameraRot.y += joystick.Horizontal * invertial * speed;
        }
        else if(platform == "WINDOWS")
        {
            joystick.gameObject.SetActive(false);
            cameraRot.x += Input.GetAxis("Mouse Y") * -invertial * speed;
            cameraRot.y += Input.GetAxis("Mouse X") * invertial * speed;
        }



    }
    #endregion

    #region INITIALIZER [for start game]
    private void Initializer()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        cameraTransformBeh = GameObject.Find("CameraBehavior").GetComponent<Transform>();

        this.transform.position = playerTransform.TransformPoint(cameraOffset);
    }
    #endregion
}
