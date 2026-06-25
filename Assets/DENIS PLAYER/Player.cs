using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Animator    playerAnim;
    public Rigidbody   playerRigid;
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Attack")]
    public KeyCode     attackKey      = KeyCode.Mouse0;  // tombol attack
    public float       attackDuration = 0.5f;            // durasi animasi attack (detik)
    private bool       isAttacking;
    private Vector3    preAttackPos;                     // posisi sebelum attack
    private Quaternion preAttackRot;                     // rotasi sebelum attack

    [Header("Speeds")]
    public float walkSpeed   = 5f;
    public float backSpeed   = 3f;
    public float runBoost    = 3f;
    public float rotateSpeed = 100f;

    private bool isWalking;
    private bool isRunning;

    void Start()
    {
        if (playerAnim  == null) playerAnim  = GetComponentInChildren<Animator>();
        if (playerRigid == null) playerRigid = GetComponent<Rigidbody>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop        = true;
    }

    void Update()
    {
        // --- Input attack ---
        if (!isAttacking && Input.GetKeyDown(attackKey))
        {
            StartCoroutine(DoAttack());
        }

        // Kirim parameter gerak ke Animator
        playerAnim.SetBool("isRunning", isRunning);
        playerAnim.SetBool("isWalking", isWalking);
    }

    void FixedUpdate()
    {
        // Jika sedang attack, jangan jalan/putar
        if (isAttacking) return;

        bool forward  = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool runKey   = Input.GetKey(KeyCode.LeftShift);

        isRunning = forward && runKey;
        isWalking = (forward || backward) && !isRunning;

        // Movement
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
        playerRigid.linearVelocity = moveDir;

        // Rotation
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up,  rotateSpeed * Time.deltaTime);

        HandleFootstepAudio();
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;

        // 1. simpan posisi & rotasi sekarang
        preAttackPos = transform.position;
        preAttackRot = transform.rotation;

        // 2. jalankan animasi attack
        playerAnim.SetTrigger("attack");

        // pause footstep audio
        if (audioSource.isPlaying) audioSource.Pause();

        // 3. tunggu sampai anim selesai
        yield return new WaitForSeconds(attackDuration);

        // 4. kembalikan posisi & rotasi
        transform.position = preAttackPos;
        transform.rotation = preAttackRot;

        // 5. restore state
        isAttacking = false;
        if (isWalking || isRunning)
            audioSource.UnPause();
    }

    private void HandleFootstepAudio()
    {
        if (isRunning || isWalking)
        {
            AudioClip target = isRunning ? runClip : walkClip;
            if (audioSource.clip != target)
            {
                audioSource.clip = target;
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
