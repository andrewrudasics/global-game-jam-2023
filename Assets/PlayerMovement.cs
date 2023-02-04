using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;

    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camDir = mainCamera.transform.forward;
        Vector3 camForward = new Vector3(camDir.x, 0, camDir.z).normalized;
        Vector3 camRight = Vector3.Cross(camForward, mainCamera.transform.up.normalized).normalized;

        Vector3 moveDir = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.W))
        {
            moveDir += camForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir -= camForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += camRight;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir -= camRight;
        }

        transform.position = transform.position + (moveDir * moveSpeed * Time.deltaTime);

    }

    public void MovePlayer(Vector3 dir)
    {

    }
}
