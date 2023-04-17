using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 12f;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {  
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0){
            Vector3 move = transform.right * x + transform.forward * z;
            move = move * speed;
            rb.MovePosition(transform.position + move * Time.fixedDeltaTime);
        }
    }
}
