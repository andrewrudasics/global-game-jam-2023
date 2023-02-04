using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyPlayerController : MonoBehaviour
{
    private Vector2 inputMove;
    public int PlayerIndex = -1;

    void Update()
    {
        transform.position += (new Vector3(inputMove.x, 0, inputMove.y)) * 0.1f;
    }

    // Gameplay Actions
    void OnMove(InputValue value) {
        inputMove = value.Get<Vector2>();
    }

    // Menu Actions
    void OnJoin() {
        PlayerManager.Instance.JoinPlayer(PlayerIndex);
    }
    void OnLeft() {
        GameMenu.Instance.OnLeft(PlayerIndex);
    }
    void OnStart() {
        GameMenu.Instance.OnStart(PlayerIndex);
    }

    void OnRight() {
        GameMenu.Instance.OnRight(PlayerIndex);
    }
}
