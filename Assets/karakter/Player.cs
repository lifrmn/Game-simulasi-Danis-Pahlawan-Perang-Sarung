using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Animator playerAnim;
    public Rigidbody playerRigid;

    [Header("Speeds")]
    public float walkSpeed  = 5f;
    public float backSpeed  = 3f;
    public float runBoost   = 3f;
    public float rotateSpeed = 100f;

    // State flags
    private bool isWalking;
    private bool isRunning;

    void Start()
    {
        // Auto‑assign jika belum diset di Inspector
        if (playerAnim  == null) playerAnim  = GetComponent<Animator>();
        if (playerRigid == null) playerRigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Input
        bool forward  = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool runKey   = Input.GetKey(KeyCode.LeftShift);

        // Hitung state
        isRunning = forward && runKey;
        isWalking = (forward || backward) && !isRunning;

        // Hitung arah & kecepatan
        Vector3 moveDir = Vector3.zero;
        if (forward)
        {
            float speed = walkSpeed + (isRunning ? runBoost : 0f);
            moveDir = transform.forward * speed;
        }
        else if (backward)
        {
            moveDir = -transform.forward * backSpeed;
        }

        // Terapkan ke Rigidbody
        playerRigid.linearVelocity = moveDir;

        // Rotasi
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up,  rotateSpeed * Time.deltaTime);
    }

    void Update()
    {
        // Update parameter Animator
        playerAnim.SetBool("isRunning", isRunning);
        playerAnim.SetBool("isWalking", isWalking);
    }
}
