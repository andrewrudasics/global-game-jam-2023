using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MatchStatus
{
    SplashScreen,
    WaitingForPlayers,
    InProgress,
    Ended,
}

public class GameStateManager : MonoBehaviour
{
    public GameObject SplashScreenCamera;
    public GameObject GameCamera;
    public GameObject LevelObject;
    public GameObject HealthSystem;
    [HideInInspector]
    public int WinningPlayer = -1;

    private MatchStatus _matchStatus = MatchStatus.SplashScreen;
    public MatchStatus matchStatus {
        get { return _matchStatus; }
        set {
            if (value == MatchStatus.SplashScreen) {
                // Enable Splash Screen Camera
                SplashScreenCamera.SetActive(true);
                GameCamera.SetActive(false);
                LevelObject.SetActive(false);
                HealthSystem.SetActive(false);
            } else {
                // Enable Normal Camera
                SplashScreenCamera.SetActive(false);
                GameCamera.SetActive(true);
                LevelObject.SetActive(true);
                HealthSystem.SetActive(true);
            }
            _matchStatus = value;
        }
    }
    

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

        matchStatus = MatchStatus.SplashScreen;
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
