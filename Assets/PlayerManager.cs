using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject CarrotCharacter;
    public GameObject PotatoCharacter;
    public Transform[] SpawnLocations;

    private static PlayerManager _instance;

    public static PlayerManager Instance { get { 
        return _instance; 
    } }

    bool[] hasPlayerJoined = new bool[2];
    PlayerInput[] playerInputs = new PlayerInput[2];
    public bool HasPlayerJoined(int playerIndex) {
        return hasPlayerJoined[playerIndex];
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {
        // Add the prefabs to the scene so that we can start getting PlayerInput
        playerInputs[0] = PlayerInput.Instantiate(PlayerPrefab, controlScheme: "Player1", pairWithDevice: Keyboard.current);
        playerInputs[0].gameObject.GetComponent<DummyPlayerController>().PlayerIndex = 0;
        playerInputs[1] = PlayerInput.Instantiate(PlayerPrefab, controlScheme: "Player2", pairWithDevice: Keyboard.current);
        playerInputs[1].gameObject.GetComponent<DummyPlayerController>().PlayerIndex = 1;
        SwitchActionMaps("menu");
    }

    public void SwitchActionMaps(string name) {
        foreach (PlayerInput input in playerInputs)
        {
            input.SwitchCurrentActionMap(name);
        }
    }

    public void JoinPlayer(int playerIndex) {
        hasPlayerJoined[playerIndex] = true;
    }

    public void SpawnPlayers() {
        // Attach models to the player objects
        foreach (PlayerInput input in playerInputs) {
            if (GameMenu.Instance.selectedCharacter[input.playerIndex] == 0) {
                Object.Instantiate(PotatoCharacter, input.gameObject.transform);
            } else {
                Object.Instantiate(CarrotCharacter, input.gameObject.transform);
            }
            input.gameObject.transform.position = SpawnLocations[input.playerIndex].position;
        }
    }

    public void DespawnPlayers() {
        // TODO: Remove models from player objects
    }

    void OnPlayerJoined(PlayerInput player) {
    }
}