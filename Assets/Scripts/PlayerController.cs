using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public InputDevice Device;
    private static readonly int FloorLayer = 1 << 8;

    public float moveSpeed = 5.0f;
    public int PlayerIndex = -1;
    public int SelectedCharacter = -1;
    private bool _isBlocking = false;

    public bool IsBlocking
    {
        get { return _isBlocking; }
        set {
            _isBlocking = value;
            IsRooted = value;
            IsUsingAbility = value;
        }
    }
    public bool IsRooted { get; set; }
    public bool IsUsingAbility { get; set; }
    private float rootedDuration = 0;
    public bool IsSlowed { get; set; }
    private float slowedDuration = 0;

    private Vector2 moveAxis, lookAxis, mousePosScreenSpace, mousePosProjected;
    private Camera mainCamera;
    private Rigidbody rb;
    
    private GameObject characterSprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public GameObject crosshair;

    public GameObject Projectile;
    public float ProjectileSpeed = 5;
    public int ProjectileCount = 5;

   

    public Vector2 GetProjectedCursorPosition() {
        return new Vector2(crosshair.transform.position.x, crosshair.transform.position.z);
    }

    public AbilityControllerBase GetAbilityController() {
        return this.GetComponentInChildren<AbilityControllerBase>();
    }

    public void SetRootedStatus(float duration) {
        IsRooted = true;
        rootedDuration = duration;
    }

    public void SetSlowStatus(float duration) {
        IsSlowed = true;
        slowedDuration = duration;
    }

    void Start()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
        lookAxis = new Vector2(1, 0);
    }

    void Update() {
        if (IsRooted && rootedDuration > 0) {
            rootedDuration -= Time.deltaTime;
            if (rootedDuration < 0) {
                IsRooted = false;
                rootedDuration = -1;
            }
        }

        if (IsSlowed) {
            slowedDuration -= Time.deltaTime;
            if (slowedDuration < 0) {
                IsSlowed = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!crosshair) {
            return;
        }

        // Crosshair Position
        if (Device is Gamepad) {
            Vector2 offsetPosition = lookAxis.normalized;
            crosshair.transform.position = transform.position + (new Vector3(offsetPosition.x, 0.01f, offsetPosition.y));
            float angle = Mathf.Atan2(offsetPosition.x, offsetPosition.y) + Mathf.PI / 2.0f;
            crosshair.transform.rotation = Quaternion.AngleAxis(angle / Mathf.PI * 180.0f, new Vector3(0, 1, 0));
        } else {
            Vector2 offsetPosition = (mousePosProjected - new Vector2(transform.position.x, transform.position.z)).normalized;
            crosshair.transform.position = transform.position + (new Vector3(offsetPosition.x, 0.01f, offsetPosition.y));
            float angle = Mathf.Atan2(offsetPosition.x, offsetPosition.y) + Mathf.PI / 2.0f;
            crosshair.transform.rotation = Quaternion.AngleAxis(angle / Mathf.PI * 180.0f, new Vector3(0, 1, 0));
        }

        // Player Movement
        Vector3 camDir = mainCamera.transform.forward;
        Vector3 camForward = new Vector3(camDir.x, 0, camDir.z).normalized;
        Vector3 camRight = Vector3.Cross(camForward, mainCamera.transform.up.normalized).normalized;

        if (!IsRooted) {
            if (spriteRenderer != null) {
                if (moveAxis.x < 0) {
                    spriteRenderer.flipX = false;
                } else if (moveAxis.x > 0) {
                    spriteRenderer.flipX = true;
                }
            }

            Vector3 moveDir = (-1.0f * camRight * moveAxis.x) + (camForward * moveAxis.y);
            if (anim != null) {
                if (moveDir.magnitude > 0.001f) {
                    anim.SetBool("moving", true);
                } else {
                    anim.SetBool("moving", false);
                }
            }
            float modifiedMovementSpeed = IsSlowed ? moveSpeed * 0.3f : moveSpeed;
            rb.MovePosition(transform.position + (moveDir * modifiedMovementSpeed * Time.deltaTime));
        }

        if (anim != null)
            anim.SetFloat("speed", rb.velocity.magnitude);
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
        if (Device is Gamepad) {
            Vector2 _value = value.Get<Vector2>();
            if (_value.magnitude > 0.1f) {
                lookAxis = _value;
            }
        } else {
            // Project mousePosScreenSpace onto the level surface
            mousePosScreenSpace = value.Get<Vector2>();
            Ray aim = mainCamera.ScreenPointToRay(mousePosScreenSpace);
            RaycastHit hit;
            if (Physics.Raycast(aim, out hit, 1000, FloorLayer)) {
                mousePosProjected = new Vector2(hit.point.x, hit.point.z);
            }
        }
    }

    // [Important] Menu Actions
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
