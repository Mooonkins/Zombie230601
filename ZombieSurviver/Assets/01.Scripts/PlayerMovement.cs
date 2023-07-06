using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private PlayerInput playerInput;
    private Rigidbody playerRgd;
    private Animator playerAnim;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRgd = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>(); 
    }
    
    private void FixedUpdate()
    {
        Move();
        Rotate();
        playerAnim.SetFloat("Move", playerInput.move);
    }
    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRgd.MovePosition(playerRgd.position + moveDistance);
    }
    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

        playerRgd.rotation = playerRgd.rotation * Quaternion.Euler(0, turn, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        /*AmmoPack ammoPack = other.GetComponent<AmmoPack>();
        if (ammoPack != null)
        {
            ammoPack.Use(this.gameObject);
        }
        HealthPack healthPack = other.GetComponent<HealthPack>();
        if (healthPack != null)
        {
            healthPack.Use(this.gameObject);
        }*/
        IItem iitem = other.GetComponent<IItem>();
        if(iitem != null)
        {
            iitem.Use(this.gameObject);
        }
    }
}
