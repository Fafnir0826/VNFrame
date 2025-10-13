using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using DIALOGUE;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfiguration;
        private void Awake()
        {
            instance = this;
        }

        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"a character called '{characterName}' alread exists");
                return null;
            }

            CHARACTERINFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);
            characters.Add(characterName.ToLower(), character);

            return character;
        }
        private CHARACTERINFO GetCharacterInfo(string characterName)
        {
            CHARACTERINFO result = new CHARACTERINFO();

            result.name = characterName;
            result.config = config.getConfig(characterName);
            return result;

        }

        private Character CreateCharacterFromInfo(CHARACTERINFO info)
        {
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new CharacterText(info.name);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new CharacterSprite(info.name);

                case Character.CharacterType.Live2D:
                    return new CharacterLive2D(info.name);

                case Character.CharacterType.Model3D:
                    return new CharacterModel3D(info.name);
                default:
                    return null;
            }
        }

        private class CHARACTERINFO
        {
            public string name = "";
            public CharacterConfig config;
        }
    }
}