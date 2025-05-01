using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Walking/running speed in units per second")]
    public float moveSpeed = 5f;

    [Tooltip("How quickly the character turns to face movement direction")]
    public float turnSpeed = 10f;

    [Tooltip("Gravity acceleration")]
    public float gravity = -9.81f;

    // Components
    private CharacterController cc;
    private Animator anim;

    // Vertical velocity for gravity
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1) Read input axes
        float h = Input.GetAxis("Horizontal");   // A/D or ←/→
        float v = Input.GetAxis("Vertical");     // W/S or ↑/↓
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // 2) Ground check + gravity reset
        if (cc.isGrounded && verticalVelocity < 0f)
        {
            // Small downward force to keep controller grounded
            verticalVelocity = -2f;
        }

        // 3) Compute movement direction & rotation
        Vector3 moveDir = Vector3.zero;
        if (inputDir.magnitude > 0.01f)
        {
            // Calculate target angle relative to camera
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                                + Camera.main.transform.eulerAngles.y;

            // Smoothly rotate towards that angle
            Quaternion targetRot = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );

            // Forward movement in the rotated direction
            moveDir = targetRot * Vector3.forward;
        }

        // 4) Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // 5) Combine horizontal and vertical into a final velocity
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        // 6) Move the CharacterController
        cc.Move(velocity * Time.deltaTime);

        // 7) Animate
        // Speed parameter drives the blend tree: 0 = Idle, 1 = Run
        anim.SetFloat("Speed", inputDir.magnitude);
    }
}