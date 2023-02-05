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
    public Texture2D showControlsTexture;
    public Texture2D hideControlsTexture;
    public Texture2D controlsTexture;
    public Texture2D quitTexture;
    public Texture2D selectorTexture;
    public Texture2D p1WinTexture;
    public Texture2D p2WinTexture;
    public Texture2D playagainTexture;
    public Texture2D charSelectTexture;
    public Texture2D potatoStatsTexture;
    public Texture2D carrotStatsTexture;
    public Texture2D joinTexture;
    public Texture2D startMatchPromptTexture;
    bool hasStarted = false;
    bool isShowingControls = false;

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
        if (GameStateManager.Instance.matchStatus != MatchStatus.SplashScreen && GameStateManager.Instance.matchStatus != MatchStatus.Ended) {
            return;
        }

        int maxMenuOptionsIndex = 2;
        if (GameStateManager.Instance.matchStatus == MatchStatus.Ended) {
            maxMenuOptionsIndex = 1;
        }

        switch (button.name) {
            case "w":
            case "upArrow":
            case "up":
                selectedStartMenuIndex -= 1;
                if (selectedStartMenuIndex < 0) {
                    selectedStartMenuIndex = maxMenuOptionsIndex;
                }
                break;
            case "s":
            case "downArrow":
            case "down":
                selectedStartMenuIndex += 1;
                if (selectedStartMenuIndex > maxMenuOptionsIndex) {
                    selectedStartMenuIndex = 0;
                }
                break;
            case "start":
            case "enter":
            case "space":
            case "buttonSouth":
                if (selectedStartMenuIndex == 0) {
                    GameStateManager.Instance.matchStatus = MatchStatus.WaitingForPlayers;
                } else if (selectedStartMenuIndex == 1) {
                    if (GameStateManager.Instance.matchStatus == MatchStatus.Ended) {
                        Application.Quit();
                    } else {
                        isShowingControls = !isShowingControls;
                    }
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
            GameStateManager.Instance.matchStatus = MatchStatus.WaitingForPlayers;
        } else if (GameStateManager.Instance.matchStatus == MatchStatus.WaitingForPlayers) {
            // Begin a countdown to start the game
            if (hasStarted) { return; }
            hasStarted = true;
            MatchCountdown = 3;
            await Task.Delay(500);
            // while (MatchCountdown > 0)
            // {
            //     MatchCountdown -= 1;
            //     await Task.Delay(1000);
            // }
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

    void GUILayoutDrawSelector(float height) {
        GUIStyle selectorStyle = new GUIStyle();
        selectorStyle.imagePosition = ImagePosition.ImageOnly;
        selectorStyle.fixedHeight = height;
        selectorStyle.fixedWidth = selectorStyle.fixedHeight / selectorTexture.height * selectorTexture.width;;
        GUILayout.Space(12);
        GUILayout.Box(selectorTexture, selectorStyle);
    }

    void OnGUISplashScreen() {
        const int PADDING = 40;

        GUIStyle btnStyle = new GUIStyle();
        btnStyle.imagePosition = ImagePosition.ImageOnly;

        int availableHeight = Screen.height - 2 * PADDING;
        Rect menuRect = new Rect (PADDING, PADDING, Screen.width - 2 * PADDING, availableHeight);
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginVertical();
            btnStyle.fixedHeight = 0.333f * availableHeight;
            btnStyle.fixedWidth = btnStyle.fixedHeight / titleTexture.height * titleTexture.width;
            GUILayout.Box(titleTexture, btnStyle);
            btnStyle.fixedHeight = 0.15f * availableHeight;
            btnStyle.fixedWidth = btnStyle.fixedHeight / startTexture.height * startTexture.width;
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                GUILayout.Box(startTexture, btnStyle);
                if (selectedStartMenuIndex == 0) {
                    GUILayoutDrawSelector(0.1f * availableHeight);
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                if (!isShowingControls) {
                    GUILayout.Box(showControlsTexture, btnStyle);
                } else {
                    GUILayout.Box(hideControlsTexture, btnStyle);
                }
                if (selectedStartMenuIndex == 1) {
                    GUILayoutDrawSelector(0.1f * availableHeight);
                }
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
                GUILayout.Box(quitTexture, btnStyle);
                if (selectedStartMenuIndex == 2) {
                    GUILayoutDrawSelector(0.1f * availableHeight);
                }
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        if (isShowingControls) {
            float controlsHeight = btnStyle.fixedHeight * 4.1f;
            float controlsWidth = controlsHeight / controlsTexture.height * controlsTexture.width;
            Rect controlsRect = new Rect (btnStyle.fixedWidth * 1.3f, 0.45f * availableHeight, controlsWidth, controlsHeight);
            GUILayout.BeginArea(controlsRect);
            GUILayout.BeginVertical();
                btnStyle.fixedHeight = controlsHeight;
                btnStyle.fixedWidth = controlsWidth;
                GUILayout.Box(controlsTexture, btnStyle);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    void OnGUIWaitingForPlayers() {
        const int PADDING = 80;
        int availableHeight = Screen.height - 2 * PADDING;
        int availableWidth = Screen.width - 2 * PADDING;

        // Textured Version
        GUIStyle bgStyle = new GUIStyle();
        bgStyle.imagePosition = ImagePosition.ImageOnly;
        bgStyle.fixedHeight = availableHeight;
        bgStyle.fixedWidth = bgStyle.fixedHeight / charSelectTexture.height * charSelectTexture.width;
        if (bgStyle.fixedWidth > availableWidth) {
            bgStyle.fixedWidth = availableWidth;
            bgStyle.fixedHeight = bgStyle.fixedWidth / charSelectTexture.width * charSelectTexture.height;
        }

        GUIStyle statsStyle = new GUIStyle();
        statsStyle.imagePosition = ImagePosition.ImageOnly;
        statsStyle.fixedHeight = availableHeight * 0.575f;
        statsStyle.fixedWidth = statsStyle.fixedHeight / potatoStatsTexture.height * potatoStatsTexture.width;
        
        Rect menuRect = new Rect (PADDING, PADDING, Screen.width - 2 * PADDING, availableHeight);
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginHorizontal();
            GUILayout.Space((availableWidth-bgStyle.fixedWidth) / 2); // Center the background
            GUILayout.Box(charSelectTexture, bgStyle);
            GUILayout.Space(-bgStyle.fixedWidth); // Go back to beginning of drawing the background
            // Bunch of magic numbers...
            GUILayout.BeginVertical();
                GUILayout.Space(bgStyle.fixedHeight * 0.29f);
                GUILayout.BeginHorizontal();
                    GUILayout.Space(0.145f * bgStyle.fixedWidth);
                    if (!PlayerManager.Instance.HasPlayerJoined(0)) {
                        GUILayout.Box(joinTexture, statsStyle);
                    } else if (selectedCharacter[0] == 0) {
                        GUILayout.Box(potatoStatsTexture, statsStyle);
                    } else {
                        GUILayout.Box(carrotStatsTexture, statsStyle);
                    }
                    GUILayout.Space(0.425f * bgStyle.fixedWidth);
                    if (!PlayerManager.Instance.HasPlayerJoined(1)) {
                        GUILayout.Box(joinTexture, statsStyle);
                    } else if (selectedCharacter[1] == 0) {
                        GUILayout.Box(potatoStatsTexture, statsStyle);
                    } else {
                        GUILayout.Box(carrotStatsTexture, statsStyle);
                    }
                GUILayout.EndHorizontal();
                if (PlayerManager.Instance.HasPlayerJoined(0) && PlayerManager.Instance.HasPlayerJoined(1)) {
                    GUILayout.Space(bgStyle.fixedWidth * -0.075f);
                    GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        statsStyle.fixedHeight = availableHeight * 0.15f;
                        statsStyle.fixedWidth = statsStyle.fixedHeight / startMatchPromptTexture.height * startMatchPromptTexture.width;
                        GUILayout.Space(statsStyle.fixedWidth * -0.2f);
                        GUILayout.Box(startMatchPromptTexture, statsStyle);
                        GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void OnGUIMatchEnded() {
        int availableHeight = Screen.height;
        int availableWidth = Screen.width;

        float height = availableHeight / 2.0f; 
        float width = height / p1WinTexture.height * p1WinTexture.width;

        GUIStyle bgStyle = new GUIStyle();
        bgStyle.imagePosition = ImagePosition.ImageOnly;
        bgStyle.fixedHeight = height;
        bgStyle.fixedWidth = width;

        GUIStyle btnStyle = new GUIStyle();
        btnStyle.imagePosition = ImagePosition.ImageOnly;
        btnStyle.fixedHeight = 0.15f * bgStyle.fixedHeight;
        btnStyle.fixedWidth = btnStyle.fixedHeight / playagainTexture.height * playagainTexture.width;

        Rect menuRect = new Rect (0, 0, Screen.width, Screen.height);
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginHorizontal();
            GUILayout.Space((availableWidth - width)/2.0f);
            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                if (GameStateManager.Instance.WinningPlayer == 0) {
                    GUILayout.Box(p1WinTexture, bgStyle);
                } else {
                    GUILayout.Box(p2WinTexture, bgStyle);
                }
                GUILayout.Space(-bgStyle.fixedHeight * 0.65f);
                GUILayout.BeginHorizontal();
                    GUILayout.Space((bgStyle.fixedWidth - btnStyle.fixedWidth)/2.0f);
                    GUILayout.Box(playagainTexture, btnStyle);
                    if (selectedStartMenuIndex == 0) {
                        GUILayoutDrawSelector(0.1f * height);
                    }
                GUILayout.EndHorizontal();
                GUILayout.Space(availableHeight * 0.05f);
                GUILayout.BeginHorizontal();
                    GUILayout.Space((bgStyle.fixedWidth - btnStyle.fixedWidth)/2.0f);
                    GUILayout.Box(quitTexture, btnStyle);
                    if (selectedStartMenuIndex == 1) {
                        GUILayoutDrawSelector(0.1f * height);
                    }
                GUILayout.EndHorizontal();
                GUILayout.Space(availableHeight * 0.05f);
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
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
