using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallBehaviour : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;
    float acceleration = 15f;

    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per physics frame (60 per second?)
    void FixedUpdate() {
        //if (Input.GetButton(""))
        rb.AddForce(moveInput * acceleration);
    }

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }
}
