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
    [HideInInspector]
    public int WinningPlayer = -1;

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
        HealthBarManager.Instance.ResetHealth();
    }

    public void CharacterSelect() {
        PlayerManager.Instance.DespawnPlayers();
        matchStatus = MatchStatus.WaitingForPlayers;
    }

    public void OnPlayerDeath(int playerIndex) {
        matchStatus = MatchStatus.Ended;
        if (playerIndex == 0) {
            WinningPlayer = 1;
        } else {
            WinningPlayer = 0;
        }
        PlayerManager.Instance.SwitchActionMaps("menu");
    }

    void Start() {
        if (matchStatus == MatchStatus.InProgress) {
            StartGame();
        }
    }
}
