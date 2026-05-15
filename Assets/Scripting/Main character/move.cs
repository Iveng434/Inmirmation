using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] float speed = 5; //поле в редактор, дл€ удобства настроек в будущем
    private Rigidbody2D rigbody;
    private Camera cam;

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        cam = rigbody.GetComponent<Camera>();
        cam.fieldOfView = 90;

    }

    private void FixedUpdate()
    {
        Vector2 moveVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            moveVector.y = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector.y = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x = speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x = -speed;
        }
        rigbody.MovePosition(rigbody.position + moveVector * Time.fixedDeltaTime * (cam.fieldOfView / 15)); //Time - дл€ более плавного движени€, Ќ≈ относительно к/c
                                                                                                            //(cam.fieldOfView / 15) - скорость движени€ камеры, в зависимости от отдалени€ обзора(дальше камера -> больше скорость)
    }
    private void Update()
    {
        Vector2 scrollVector = Input.mouseScrollDelta;
        if (scrollVector.y < 0) { cam.fieldOfView += 10; }
        if (scrollVector.y > 0) { cam.fieldOfView -= 10; }
    }
}
