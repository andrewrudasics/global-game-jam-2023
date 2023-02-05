using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int FloorLayer = 1 << 8;

    public float moveSpeed = 5.0f;
    public int PlayerIndex = -1;

    private Vector2 moveAxis, mousePos;
    private Camera mainCamera;
    private Rigidbody rb;
    
    private GameObject characterSprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public GameObject crosshair;


    void Start()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Crosshair Position
        Ray aim = mainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        
        if (Physics.Raycast(aim, out hit, 1000, FloorLayer))
        {
            crosshair.transform.position = hit.point;
        }

        // Player Movement
        Vector3 camDir = mainCamera.transform.forward;
        Vector3 camForward = new Vector3(camDir.x, 0, camDir.z).normalized;
        Vector3 camRight = Vector3.Cross(camForward, mainCamera.transform.up.normalized).normalized;


        if (spriteRenderer != null)
        {
            if (moveAxis.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveAxis.x > 0)
            {
                spriteRenderer.flipX = true;
            }
        }

        Vector3 moveDir = (-1.0f * camRight * moveAxis.x) + (camForward * moveAxis.y);

        if (anim != null)
        {
            if (moveDir.magnitude > 0.001f)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }
        }

        rb.MovePosition(transform.position + (moveDir * moveSpeed * Time.deltaTime));
    }

    // [Important] Gameplay Actions
    void OnMove(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
    }

    void OnShoot()
    {
        Debug.Log("Shoot Pressed");
    }

    void OnAim(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }

    // [Important] Menu Actions
    void OnJoin()
    {
        PlayerManager.Instance.JoinPlayer(PlayerIndex);
    }
    void OnLeft()
    {
        GameMenu.Instance.OnLeft(PlayerIndex);
    }
    void OnRight()
    {
        GameMenu.Instance.OnRight(PlayerIndex);
    }
    void OnStart()
    {
        GameMenu.Instance.OnStart(PlayerIndex);
    }

    public void SetCharacterSprite(GameObject spriteObject)
    {
        characterSprite = spriteObject;
        if (characterSprite != null)
        {
            spriteRenderer = characterSprite.GetComponent<SpriteRenderer>();
            anim = characterSprite.GetComponent<Animator>();
        }
    }
}
