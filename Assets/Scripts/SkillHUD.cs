using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SkillHUD : MonoBehaviour
{
    public GUISkin skin;
    public Texture2D knifeSprite;
    public Texture2D dodgeTexture;
    public Texture2D blockTexture;
    public Texture2D lungeTexture;
    public Texture2D carrotRainTexture;

    void OnGUI()
    {
        MatchStatus status = GameStateManager.Instance.matchStatus;
        if (status != MatchStatus.InProgress) {
            return;
        }

        GUILayoutSkillDisplay(PlayerManager.Instance.GetPlayerController(0));
        GUILayoutSkillDisplay(PlayerManager.Instance.GetPlayerController(1));
    }

    void GUILayoutSkillDisplay(PlayerController player) {
        GUIStyle containerStyle = new GUIStyle();
        // GUI.skin=skin;
        // containerStyle.normal.background = menuTexture;
        // containerStyle.margin=new RectOffset(11,22,33,44);
        
        const int SKILLBAR_WIDTH = 324;
        const int SKILLBAR_HEIGHT = 124;
        const int HORIZONTAL_MARGIN = 40;
        const int VERTICAL_MARGIN = 20;
        Rect menuRect = new Rect (HORIZONTAL_MARGIN, Screen.height - VERTICAL_MARGIN - SKILLBAR_HEIGHT, SKILLBAR_WIDTH, SKILLBAR_HEIGHT);
        if (player.PlayerIndex == 1) {
            menuRect = new Rect (Screen.width - HORIZONTAL_MARGIN - SKILLBAR_WIDTH, Screen.height - VERTICAL_MARGIN - SKILLBAR_HEIGHT, SKILLBAR_WIDTH, SKILLBAR_HEIGHT);
        }

        AbilityControllerBase abilityController = player.GetAbilityController();
        GUILayout.BeginArea(menuRect);
        GUILayout.BeginVertical();
            // Projectile Count
            GUILayout.BeginHorizontal();
                GUIStyle knifeSpriteStyle = new GUIStyle();
                knifeSpriteStyle.fixedHeight = 48;
                knifeSpriteStyle.fixedWidth = 48;
                for (int i = 0; i < abilityController.ProjectileCount; i++) {
                    GUILayout.Box(knifeSprite, knifeSpriteStyle);
                }
                GUILayout.Box("", knifeSpriteStyle);
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            // Skills
            GUILayout.BeginHorizontal(UnityEditor.EditorStyles.helpBox);
                if (player.SelectedCharacter == 0) {
                    GUILayoutPotatoAbilities(player);
                } else {
                    GUILayoutCarrotAbilities(player);
                }
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void GUILayoutAbility(PlayerController player, string text, int abilityIndex, Texture2D icon = null) {
        const int SIZE = 64;
        const int MARGIN = 4;
        GUIStyle abilityStyle = new GUIStyle(GUI.skin.box);
        abilityStyle.fixedHeight = SIZE;
        abilityStyle.fixedWidth = SIZE;
        abilityStyle.normal.textColor = Color.white;
        abilityStyle.alignment = TextAnchor.MiddleCenter;
        if (!icon) {
            GUILayout.Box(text, abilityStyle);
        } else {
            GUILayout.Box(icon, abilityStyle);
        }
        float cdPercent = player.GetAbilityController().GetAbilityCooledDownPercent(abilityIndex);
        if (cdPercent < 1) {
            float width = Mathf.Ceil(SIZE * cdPercent);
            if (width > 4) {
                GUILayout.Space(-SIZE - MARGIN);
                GUILayout.BeginVertical();
                    abilityStyle.fixedHeight = 6;
                    abilityStyle.fixedWidth = width;
                    abilityStyle.normal.background = Texture2D.whiteTexture;
                    GUILayout.Space(SIZE - MARGIN);
                    GUILayout.Box("", abilityStyle);
                GUILayout.EndVertical();
                GUILayout.Space(SIZE - width);
            }
        }
    }

    void GUILayoutPotatoAbilities(PlayerController player) {
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Block", 0, blockTexture);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Slam", 1, lungeTexture);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Leap", 2);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Shockwave", 3);
        GUILayout.FlexibleSpace();
    }

    void GUILayoutCarrotAbilities(PlayerController player) {
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Dodge", 0, dodgeTexture);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Rain", 1, carrotRainTexture);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Spikes", 2);
        GUILayout.FlexibleSpace();
        GUILayoutAbility(player, "Charge", 3);
        GUILayout.FlexibleSpace();
    }
}
