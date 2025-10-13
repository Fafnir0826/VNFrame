using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{

    [CreateAssetMenu(fileName = "Dialogue Configuration Asset", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfiguration;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
    }
}