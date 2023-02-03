using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarBehaviour : MonoBehaviour {

    public WheelJoint2D rearWheel, frontWheel;
    JointMotor2D rearMotor, frontMotor;

    public float torque;
    public float forwardSpeed;
    public float reverseSpeed;

    float speed = 0;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void OnMove(InputValue value) {
        float moveInput = value.Get<Vector2>().x;
        if (moveInput > 0) {
            speed = forwardSpeed;
            Debug.Log("Moving Forward");
        } else if (moveInput < 0) {
            speed = -reverseSpeed;
            Debug.Log("Moving Backwards");
        } else {
            speed = 0;
        }

        rearMotor.motorSpeed = speed;
        frontMotor.motorSpeed = speed;
        rearMotor.maxMotorTorque = torque;
        frontMotor.maxMotorTorque = torque;
        rearWheel.motor = rearMotor;
        frontWheel.motor = frontMotor;
    }
}
