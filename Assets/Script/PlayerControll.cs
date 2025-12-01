using System;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerControll : MonoBehaviour
{
    [Header("移動設定")]
    private float moveSpeed = 5f;
    private float gravity = -9.81f;
    private float groundGravity = -2f;

    [Header("視点設定")]
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 2f;
    private float verticalLookLimit = 50f;
    private float mouseMultiplier = 35f;

    public Transform cam;

    [Header("視点スムーズ")]
    [SerializeField] private float SmoothX = 0.1f;
    [SerializeField] private float SmoothY = 0.0f;

    //[SerializeField] private PlayerFootStep footstep;
    //[SerializeField] private float stepInterval = 0.5f;
    //private float stepTImer;

    private float targetXRot;
    private float smoothXRot;
    private float yRot;
    private float smoothYRot;

    private CharacterController characterController;
    private Vector3 velocity;

    // ---- プレイヤー停止判定用 ----
    public bool IsStopped { get; private set; }
    public Vector3 LastMoveVelocity { get; private set; }

    private Vector3 lastPlayerPos;
    private Vector3 lastMousePos;
    private float checkTImer;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // マウスとプレイヤーの位置初期化
        lastMousePos = Input.mousePosition;
        lastPlayerPos = transform.position;
    }

    private void Update()
    {
        Look();
        Move();
    }

    // プレイヤー視点
    private void Look()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY;


        // 上下回転
        targetXRot -= mouseY;
        targetXRot = Mathf.Clamp(targetXRot, -verticalLookLimit, verticalLookLimit);
        yRot += mouseX;

        // スムージング
        smoothXRot = Mathf.Lerp(smoothXRot,targetXRot,SmoothX);
        smoothYRot = yRot;

        // 反映
        cam.localRotation = Quaternion.Euler(smoothXRot, 0f, 0f);
        transform.rotation = Quaternion.Euler(0, smoothYRot, 0f);
    }
    
    // 移動
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // 重力を適用
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = groundGravity;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        checkTImer += Time.deltaTime;

        if(checkTImer >= 0.1) // 0.1秒ごとに判定
        {
            Vector3 delta = transform.position - lastPlayerPos;
            float speed = delta.magnitude / checkTImer;

            LastMoveVelocity = delta / checkTImer;

            // ほぼ動いていなければ停止
            IsStopped = speed < 0.05f;

            lastPlayerPos = transform.position;
            checkTImer = 0;
        }
    }

   public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }
}
