using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    private Rigidbody rb;

    public bool isPlayerOne = true;
    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 camDir = mainCamera.transform.forward;
        Vector3 camForward = new Vector3(camDir.x, 0, camDir.z).normalized;
        Vector3 camRight = Vector3.Cross(camForward, mainCamera.transform.up.normalized).normalized;


        // Replace with input from unity
        Vector2 moveAxis = GetMoveAxis(isPlayerOne);

        if (moveAxis.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveAxis.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        Vector3 moveDir = camRight * moveAxis.x + camForward * moveAxis.y;

        if (moveDir.magnitude > 0.001f)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        rb.MovePosition(transform.position + (moveDir * moveSpeed * Time.deltaTime));

    }

    // Replace with input from unity
    private Vector2 GetMoveAxis(bool playerOne)
    {
        Vector2 moveAxis = new Vector2();

        if (playerOne) {

            if (Input.GetKey(KeyCode.W))
            {
                moveAxis += new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveAxis -= new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveAxis += new Vector2(1, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveAxis -= new Vector2(1, 0);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveAxis += new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveAxis -= new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveAxis += new Vector2(1, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveAxis -= new Vector2(1, 0);
            }
        }

        return moveAxis;
    }
}
