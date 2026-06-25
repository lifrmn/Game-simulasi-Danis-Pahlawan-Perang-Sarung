using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float walkSpeed = 5f;
    public float backSpeed = 3f;
    public float runBoost = 3f;
    public float rotateSpeed = 100f;
    public Transform playerTrans;

    private bool isWalking = false;
    private bool isRunning = false;

    void FixedUpdate()
    {
        Vector3 moveDirection = Vector3.zero;
        float currentSpeed = walkSpeed;

        // Input
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool running = Input.GetKey(KeyCode.LeftShift);

        // Kecepatan
        if (forward && running)
        {
            currentSpeed += runBoost;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Arah gerak
        if (forward)
        {
            moveDirection = transform.forward * currentSpeed;
            isWalking = true;
        }
        else if (backward)
        {
            moveDirection = -transform.forward * backSpeed;
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // Terapkan ke Rigidbody (INI yang benar)
        playerRigid.linearVelocity = moveDirection;

        // Rotasi
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

    void Update()
    {
        // Animasi
        playerAnim.SetBool("run", isRunning);
        playerAnim.SetBool("walk", isWalking && !isRunning);
        // Hapus walkback jika tidak ada
        // playerAnim.SetBool("walkback", isWalking && Input.GetKey(KeyCode.S));
    }
}
