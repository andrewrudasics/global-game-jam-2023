using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SkillHUD : MonoBehaviour
{
    public GUISkin skin;
    public Texture2D knifeSprite;

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
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            // Skills
            GUILayout.BeginHorizontal(UnityEditor.EditorStyles.helpBox);
                if (player.SelectedCharacter == 0) {
                    GUILayoutPotatoAbilities();
                } else {
                    GUILayoutCarrotAbilities();
                }
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    // AbilityController.UseAbility1 / UseAbility2
    // Maps to PotatoAbilityController.UseAbility1 or CarrotAbilityController.UseAbility1
    // AbilityController.GetABilityCooldown1 [returns 0-1 number]

    void GUILayoutAbility(string text) {
        GUIStyle abilityStyle = new GUIStyle(GUI.skin.box);
        abilityStyle.fixedHeight = 64;
        abilityStyle.fixedWidth = 64;
        abilityStyle.normal.textColor = Color.white;
        abilityStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Box(text, abilityStyle);
    }

    void GUILayoutPotatoAbilities() {
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Block");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Slam");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Leap");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Pound");
        GUILayout.FlexibleSpace();
    }

    void GUILayoutCarrotAbilities() {
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Dodge");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Rain");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Spikes");
        GUILayout.FlexibleSpace();
        GUILayoutAbility("Charge");
        GUILayout.FlexibleSpace();
    }
}
