using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
    int[] deviceIds = new int[2];
    PlayerInput[] playerInputs = new PlayerInput[2];
    public bool HasPlayerJoined(int playerIndex) {
        return hasPlayerJoined[playerIndex];
    }

    public PlayerController GetPlayerController(int playerIndex) {
        return playerInputs[playerIndex].gameObject.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
    }

    void OnButtonPressed(InputControl button)
    {
        if (hasPlayerJoined[0] && hasPlayerJoined[1]) {
            return;
        }

        InputDevice device = button.device;

        // Check for duplicate gamepads since they can't join twice
        if (device is Gamepad) {
            foreach (int deviceId in deviceIds) {
                if (deviceId == device.deviceId) {
                    return;
                }
            }
        }

        if (device is Gamepad) {
            if (button.name != "buttonSouth") {
                return;
            }
            // If (A) was pressed, pair the gamepad with whichever player needs to be paired still
            if (!hasPlayerJoined[0]) {
                JoinPlayer(0, "Gamepad", device);
            } else if (!hasPlayerJoined[1]) {
                JoinPlayer(1, "Gamepad", device);
            }
        } else if (device is Keyboard) {
            // Press [1] to connect Player 1 with Keyboard and Mouse device
            if (button.name == "1") {
                if (!hasPlayerJoined[0]) {
                    JoinPlayer(0, "Keyboard Left", device);
                } else if (!hasPlayerJoined[1]) {
                    JoinPlayer(1, "Keyboard Right", device);
                }
            }
            // Press [2] to connect Player 2 with Keyboard
            else if (!hasPlayerJoined[1] && button.name == "2") {
                JoinPlayer(1, "Keyboard Right", device);
            }
        }
    }

    void JoinPlayer(int playerIndex, string controlScheme, InputDevice device) {
        hasPlayerJoined[playerIndex] = true;
        playerInputs[playerIndex] = PlayerInput.Instantiate(PlayerPrefab, controlScheme: controlScheme, pairWithDevice: device);
        playerInputs[playerIndex].gameObject.GetComponent<PlayerController>().PlayerIndex = playerIndex;
        playerInputs[playerIndex].gameObject.GetComponent<PlayerController>().Device = device;
        playerInputs[playerIndex].SwitchCurrentActionMap("menu");
        deviceIds[playerIndex] = device.deviceId;
    }

    public void SwitchActionMaps(string name) {
        foreach (PlayerInput input in playerInputs)
        {
            input.SwitchCurrentActionMap(name);
        }
    }

    public void SpawnPlayers() {
        // Attach models to the player objects
        foreach (PlayerInput input in playerInputs) {
            GameObject spriteObject;
            if (GameMenu.Instance.selectedCharacter[input.playerIndex] == 0) {
                spriteObject = Object.Instantiate(PotatoCharacter, input.gameObject.transform);
            } else {
                spriteObject = Object.Instantiate(CarrotCharacter, input.gameObject.transform);
            }
            PlayerController player = input.gameObject.GetComponent<PlayerController>();
            player.SelectedCharacter = GameMenu.Instance.selectedCharacter[input.playerIndex];
            player.SetCharacterSprite(spriteObject);
            input.gameObject.transform.position = SpawnLocations[input.playerIndex].position;
        }
    }

    public void DespawnPlayers() {
        foreach (PlayerInput input in playerInputs) {
            Component component = input.transform.GetComponentInChildren<ColorInfo>();
            if (component) {
                Destroy(component.gameObject);
            }
        }
    }
}
