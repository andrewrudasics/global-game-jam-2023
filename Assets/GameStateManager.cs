using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MatchStatus
{
    WaitingForPlayers,
    InProgress,
    Ended,
}

public class GameStateManager : MonoBehaviour
{

    public MatchStatus matchStatus = MatchStatus.WaitingForPlayers;

    private static GameStateManager _instance;

    public static GameStateManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void StartGame() {
        PlayerManager.Instance.SpawnPlayers();
        matchStatus = MatchStatus.InProgress;
        PlayerManager.Instance.SwitchActionMaps("gameplay");
    }

    void Start() {
        if (matchStatus == MatchStatus.InProgress) {
            StartGame();
        }
    }
}
