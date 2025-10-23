using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfig
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;


        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;
        public float nameFontSize;
        public float dialogueFontSize;

        public CharacterConfig Copy()
        {
            CharacterConfig result = new CharacterConfig();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;
            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);
            result.dialogueFontSize = dialogueFontSize;
            result.nameFontSize = nameFontSize;
            return result;
        }

        private static Color defaultColor => DialogueSystem.instance.config.defaultTextColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;
        public static CharacterConfig Default
        {
            get
            {
                CharacterConfig result = new CharacterConfig();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameFont = defaultFont;
                result.dialogueFont = defaultFont;
                result.nameColor = defaultColor;
                result.dialogueColor = defaultColor;
                result.dialogueFontSize = DialogueSystem.instance.config.defaultDialogueFontSize;
                result.nameFontSize = DialogueSystem.instance.config.defaultNameFontSize;
                return result;
            }
        }

    }
}