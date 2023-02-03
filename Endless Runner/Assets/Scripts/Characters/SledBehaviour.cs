using System.Collections;
using System.Collections.Generic;
using Microsoft.Cci;
using UnityEngine;
using UnityEngine.InputSystem;

public class SledBehaviour : MonoBehaviour {

    public GameObject deathUi;
    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;

    float speed = 10f;
    float rotationSpeed = 25f;
    float maxAngularVelocity = 25f;
    float jumpForce = 175;

    float airTime = 0;
    float coyoteTime = .4f;

    bool jumpAvailable = true;
    bool alive = true;

    List<Collider2D> collidingWith = new List<Collider2D>();

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        // Move horizontally
        if (moveInput.y != 0 && !InAir()) {
            Vector2 xForce = moveInput.y * transform.right * speed;
            rb.AddForce(xForce);
        }

        // Rotate sled
        if (moveInput.x != 0 && InAir()) {
            if (Mathf.Abs(rb.angularVelocity) < maxAngularVelocity) {
                rb.AddTorque(moveInput.x * rotationSpeed * -1);
            }
        }
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);

        if (InAir()) {
            airTime += Time.fixedDeltaTime;
        }
    }

    void OnLand() {
        jumpAvailable = true;
        airTime = 0;
    }

    void OnTakeoff() {
        //Debug.Log("OnTakeoff in sled");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collidingWith.Add(collision.otherCollider);
        if (collidingWith.Count == 1) {
            gameObject.SendMessage("OnLand");
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        collidingWith.Remove(collision.otherCollider);
        if (collidingWith.Count == 0) {
            gameObject.SendMessage("OnTakeoff");
        }
    }

    public bool InAir() {
        return collidingWith.Count == 0;
    }

    public bool IsAlive() {
        return alive;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Dead!");
        deathUi.SetActive(true);
        alive = false;
        rb.simulated = false;
    }

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) {
        if (jumpAvailable) {
            Debug.Log("Jumping!");
            rb.AddForce(transform.up * jumpForce);
            if (InAir() && airTime > coyoteTime) {
                jumpAvailable = false;
            }
        }
    }
}
