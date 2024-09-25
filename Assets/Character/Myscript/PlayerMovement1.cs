using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    // พารามิเตอร์การเคลื่อนที่ของตัวละคร
    public float speed = 10.0f;           // ความเร็วในการเคลื่อนที่
    public float jumpForce = 8.0f;        // แรงกระโดด
    public float gravity = 20.0f;         // แรงโน้มถ่วง
    public float rotationSpeed = 100.0f;  // ความเร็วในการหมุน
    public int maxJumpCount = 2;          // จำนวนครั้งที่กระโดดได้สูงสุด

    // สถานะการอนิเมชัน
    public bool isGrounded = false;       // ตัวละครอยู่บนพื้นหรือไม่
    public bool isDef = false;            // สถานะป้องกัน
    public bool isDancing = false;        // สถานะเต้น
    public bool isWalking = false;        // สถานะเดิน

    private Animator animator;            // อ้างอิงถึง Animator
    private CharacterController characterController; // อ้างอิงถึง CharacterController
    private Vector3 inputVector = Vector3.zero;      // เวกเตอร์อินพุต
    private Vector3 targetDirection = Vector3.zero;  // ทิศทางเป้าหมาย
    private Vector3 moveDirection = Vector3.zero;    // ทิศทางการเคลื่อนที่
    private Vector3 velocity = Vector3.zero;         // ความเร็ว
    private int currentJumpCount = 0;                // ตัวแปรติดตามจำนวนการกระโดดปัจจุบัน

    void Awake()
    {
        // เริ่มต้นคอมโพเนนต์
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Time.timeScale = 1; // ตั้งค่าสเกลเวลาในเกม
        isGrounded = characterController.isGrounded; // ตรวจสอบว่าตัวละครอยู่บนพื้น
    }

    void Update()
    {
        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        animator.SetFloat("inputX", x);
        animator.SetFloat("inputZ", z);

        if (z != 0 || x != 0)
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
        else
        {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }

        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            currentJumpCount = 0; // รีเซ็ตจำนวนการกระโดดเมื่อสัมผัสพื้น
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButtonDown("Jump")) // กด space bar เพื่อกระโดด
            {
                moveDirection.y = jumpForce;
                currentJumpCount++; // เพิ่มจำนวนการกระโดด
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && currentJumpCount < maxJumpCount)
            {
                moveDirection.y = jumpForce; // กระโดดเพิ่ม
                currentJumpCount++; // เพิ่มจำนวนการกระโดด
            }
        }

        moveDirection.y -= gravity * Time.deltaTime; // ใช้แรงโน้มถ่วง

        characterController.Move(moveDirection * Time.deltaTime);

        inputVector = new Vector3(x, 0, z);
        updateMovement();

        void updateMovement()
        {
            Vector3 motion = inputVector;
            motion = ((Mathf.Abs(motion.x) > 1) || (Mathf.Abs(motion.z) > 1)) ? motion.normalized : motion;

            rotateTowardMovement();
            viewRelativeMovement();
        }

        void rotateTowardMovement()
        {
            // หมุนตัวละครไปในทิศทางที่เคลื่อนที่
            if (inputVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        void viewRelativeMovement()
        {
            // คำนวณการเคลื่อนที่ตามมุมมองของกล้อง
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0.0f;
            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
            targetDirection = (Input.GetAxis("Horizontal") * right) + (Input.GetAxis("Vertical") * forward);
        }
    }
}
