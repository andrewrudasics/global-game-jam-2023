using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int FloorLayer = 1 << 8;

    public float moveSpeed = 5.0f;
    public int PlayerIndex = -1;
    public int SelectedCharacter = -1;

    private Vector2 moveAxis, mousePosScreenSpace, mousePosProjected;
    private Camera mainCamera;
    private Rigidbody rb;
    
    private GameObject characterSprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public GameObject crosshair;

    public GameObject Projectile;
    public float ProjectileSpeed = 5;
    public int ProjectileCount = 5;

    public Vector2 GetProjectedMousePosition() {
        return mousePosProjected;
    }

    public AbilityControllerBase GetAbilityController() {
        return this.GetComponentInChildren<AbilityControllerBase>();
    }

    void Start()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!crosshair) {
            return;
        }

        // Crosshair Position
        crosshair.transform.position = new Vector3(mousePosProjected.x, 0.01f, mousePosProjected.y);

        SetCrosshairColor();

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
    void OnMelee()
    {
        GetAbilityController().AttackMelee();
    }
    void OnShoot()
    {
        GetAbilityController().AttackRanged();
    }
    void OnAbility1()
    {
        GetAbilityController().UseAbility1();
    }
    void OnAbility2()
    {
        GetAbilityController().UseAbility2();
    }
    void OnAbility3()
    {
        GetAbilityController().UseAbility3();
    }
    void OnAbility4()
    {
        GetAbilityController().UseAbility4();
    }

    void OnAim(InputValue value)
    {
        mousePosScreenSpace = value.Get<Vector2>();

        // Project mousePosScreenSpace onto the level surface
        Ray aim = mainCamera.ScreenPointToRay(mousePosScreenSpace);
        RaycastHit hit;
        if (Physics.Raycast(aim, out hit, 1000, FloorLayer)) {
            mousePosProjected = new Vector2(hit.point.x, hit.point.z);
        }
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

    // Sprite Related Functions
    public void SetCharacterSprite(GameObject spriteObject)
    {
        characterSprite = spriteObject;
        if (characterSprite != null)
        {
            spriteRenderer = characterSprite.GetComponent<SpriteRenderer>();
            anim = characterSprite.GetComponent<Animator>();
        }
    }

    public void SetCrosshairColor()
    {
        if (characterSprite != null)
        {
            Color charColor = characterSprite.GetComponent<ColorInfo>().CharacterColor;
            SpriteRenderer crosshairRenderer = crosshair.GetComponentInChildren<SpriteRenderer>();
            crosshairRenderer.color = charColor;
        }
    }
}
