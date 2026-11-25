using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float moveSpeed = 15f;   //横移動速度
    public float jumpForce = 7f;    //ジャンプ力

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //回転防止
    }

    void Update()
    {
        //WASD入力
        float moveX = Input.GetAxisRaw("Horizontal"); //A=-1, D=1
        float moveZ = Input.GetAxisRaw("Vertical");   //S=-1, W=1
        moveInput = new Vector3(moveX, 0, moveZ).normalized;

        //Transformを使って移動（物理干渉なし、すり抜けスムーズ）
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        //スペースでジャンプ（物理的な力）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //Y速度リセット
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("衝突したオブジェクト名: " + collision.gameObject.name);

    }
}