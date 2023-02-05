using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
public class GameMenu : MonoBehaviour
{
    private int MatchCountdown = -1;
    public Texture2D carrotPortraitTexture;
    public Texture2D potatoPortraitTexture;
    public Texture2D titleTexture;
    public Texture2D startTexture;
    public Texture2D quitTexture;
    public Texture2D creditsTexture;
    public Texture2D selectorTexture;
    bool hasStarted = false;

    private int selectedStartMenuIndex = 0;

    private static GameMenu _instance;

    public static GameMenu Instance { get { return _instance; } }

    [HideInInspector]
    public int[] selectedCharacter = new int[2];

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        selectedCharacter[0] = 0;
        selectedCharacter[1] = 1;
        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
    }

    void OnButtonPressed(InputControl button)
    {
        if (GameStateManager.Instance.matchStatus != MatchStatus.SplashScreen) {
            return;
        }

        switch (button.name) {
            case "w":
            case "upArrow":
            case "up":
                selectedStartMenuIndex -= 1;
                if (selectedStartMenuIndex < 0) {
                    selectedStartMenuIndex = 2;
                }
                break;
            case "s":
            case "downArrow":
            case "down":
                selectedStartMenuIndex += 1;
                if (selectedStartMenuIndex > 2) {
                    selectedStartMenuIndex = 0;
                }
                break;
            case "start":
            case "enter":
            case "space":
                if (selectedStartMenuIndex == 0) {
                    GameStateManager.Instance.matchStatus = MatchStatus.WaitingForPlayers;
                } else if (selectedStartMenuIndex == 1) {

                } else if (selectedStartMenuIndex == 2) {
                    // Quit Game
                    Application.Quit();
                }
                break;
        }
    }

    public void OnLeft(int playerIndex) {
        if (playerIndex < 0 || playerIndex > 1) {
            Debug.LogError("Invalid playerIndex! " + playerIndex);
            return;
        }

        selectedCharacter[playerIndex] -= 1;
        if (selectedCharacter[playerIndex] < 0) {
            selectedCharacter[playerIndex] = 1;
        }
    }

    public void OnRight(int playerIndex) {
        if (playerIndex < 0 || playerIndex > 1) {
            Debug.LogError("Invalid playerIndex! " + playerIndex);
            return;
        }

        selectedCharacter[playerIndex] += 1;
        if (selectedCharacter[playerIndex] > 1) {
            selectedCharacter[playerIndex] = 0;
        }
    }

    public async void OnStart(int playerIndex) {
        if (GameStateManager.Instance.matchStatus == MatchStatus.Ended) {
            GameStateManager.Instance.CharacterSelect();
        } else if (GameStateManager.Instance.matchStatus == MatchStatus.WaitingForPlayers) {
            // Begin a countdown to start the game
            if (hasStarted) { return; }
            hasStarted = true;
            MatchCountdown = 3;
            await Task.Delay(1000);
            while (MatchCountdown > 0)
            {
                MatchCountdown -= 1;
                await Task.Delay(1000);
            }
            GameStateManager.Instance.StartGame();
            MatchCountdown = -1;
            hasStarted = false;
        }
    }

    void OnGUI()
    {
        MatchStatus status = GameStateManager.Instance.matchStatus;
        switch (GameStateManager.Instance.matchStatus) {
            case MatchStatus.SplashScreen:
                OnGUISplashScreen();
                break;
            case MatchStatus.WaitingForPlayers:
                OnGUIWaitingForPlayers();
                break;
            case MatchStatus.Ended:
                OnGUIMatchEnded();
                break;
        }
    }

    void GUILayoutDrawSelector() {
        GUIStyle selectorStyle = new GUIStyle();
        selectorStyle.imagePosition = ImagePosition.ImageOnly;
        selectorStyle.fixedHeight = 50;
        selectorStyle.fixedWidth = 172;
        GUILayout.Space(12);
        GUILayout.Box(selectorTexture, selectorStyle);
    }

    void OnGUISplashScreen() {
        const int PADDING = 40;

        GUIStyle btnStyle = new GUIStyle();
        btnStyle.imagePosition = ImagePosition.ImageOnly;


        Rect menuRect = new Rect (PADDING, PADDING, Screen.width - 2 * PADDING, Screen.height - 2 * PADDING);
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginVertical();
            btnStyle.fixedHeight = 150;
            btnStyle.fixedWidth = 563;
            GUILayout.Box(titleTexture, btnStyle);
            btnStyle.fixedHeight = 75;
            btnStyle.fixedWidth = 277;
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                GUILayout.Box(startTexture, btnStyle);
                if (selectedStartMenuIndex == 0) {
                    GUILayoutDrawSelector();
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                GUILayout.Box(creditsTexture, btnStyle);
                if (selectedStartMenuIndex == 1) {
                    GUILayoutDrawSelector();
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                GUILayout.Box(quitTexture, btnStyle);
                if (selectedStartMenuIndex == 2) {
                    GUILayoutDrawSelector();
                }
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void OnGUIWaitingForPlayers() {
        GUIStyle containerStyle = new GUIStyle();
        // containerStyle.normal.background = menuTexture;
        // containerStyle.margin=new RectOffset(11,22,33,44);
        
        // int menuWidth = 500;
        Rect menuRect = new Rect (100, 100, Screen.width-200, Screen.height-200);
        GUILayout.BeginArea(menuRect, UnityEditor.EditorStyles.helpBox);
        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Character Select");
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Player 1", UnityEditor.EditorStyles.whiteLargeLabel);
                        GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    CharacterSelectorLayout(0);
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                            GUILayout.Label("Player 2");
                        GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    CharacterSelectorLayout(1);
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            MatchStartText();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void OnGUIMatchEnded() {
        Rect menuRect = new Rect (100, 100, Screen.width-200, Screen.height-200);
        GUILayout.BeginArea(menuRect, UnityEditor.EditorStyles.helpBox);
        GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                int winningPlayer = GameStateManager.Instance.WinningPlayer + 1;
                GUILayout.Label("Player " + winningPlayer + " is the SOUP SURVIVOR!!");
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Press [Enter] on keyboard or START on controller to play again");
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void CharacterSelectorLayout(int playerIndex) {
        GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            CharacterPortrait(playerIndex);
            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void CharacterPortrait(int playerIndex) {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 24;
        labelStyle.margin = new RectOffset(24, 24, 24, 24);

        if (!PlayerManager.Instance.HasPlayerJoined(playerIndex)) {
            int keyboardJoinKey = playerIndex + 1;
            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Press ["+keyboardJoinKey+"] on keyboard or (A) on controller to join");
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        } else {
            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Label("←", labelStyle);
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                Texture2D texture = potatoPortraitTexture;
                if (selectedCharacter[playerIndex] == 1) {
                    texture = carrotPortraitTexture;
                }
                GUILayout.Box(texture, GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Label("→", labelStyle);
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }

    void MatchStartText() {
        GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!PlayerManager.Instance.HasPlayerJoined(0) || !PlayerManager.Instance.HasPlayerJoined(1)) {
                GUILayout.Label("Waiting for players to join...");
            } else if (MatchCountdown > 0) {
                GUILayout.Label("Match starting in " + MatchCountdown + "...");
            } else if (MatchCountdown == 0) {
                GUILayout.Label("Match starting ...");
            } else {
                GUILayout.Label("Press [Enter] on keyboard or START on controller to begin match");
            }
            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
