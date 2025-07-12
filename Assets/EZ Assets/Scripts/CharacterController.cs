using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterController : MonoBehaviour
{
    public SimpleJoystick joystick;
    public float speed = 5f;

    void Update()
    {
        Vector2 dir = joystick.Direction();
        Vector3 move = new (dir.x, 0, dir.y);
        transform.Translate(speed * Time.deltaTime * move, Space.World);
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }
}
