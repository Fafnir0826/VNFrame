using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEditor.PackageManager.Requests;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfiguration;
        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";

        public string characterPrefabNameFormat => $"Character - [{CHARACTER_NAME_ID}]";

        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";


        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;

        private void Awake()
        {
            instance = this;
        }
        public CharacterConfig GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }
        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
                return characters[characterName.ToLower()];
            else if (createIfDoesNotExist)
                return CreateCharacter(characterName);
            return null;
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
            characters.Add(info.name, character);

            return character;
        }
        private CHARACTERINFO GetCharacterInfo(string characterName)
        {
            CHARACTERINFO result = new CHARACTERINFO();
            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;
            result.config = config.GetConfig(result.castingName);
            result.prefab = GetPrefabForCharacter(result.castingName);
            result.rootCharacterFolder = FormatCharaterPath(characterRootPathFormat, result.castingName);
            return result;

        }
        private GameObject GetPrefabForCharacter(string characterName)
        {
            string pefabPath = FormatCharaterPath(characterPrefabPathFormat, characterName);

            return Resources.Load<GameObject>(pefabPath);
        }
        public string FormatCharaterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTERINFO info)
        {
            CharacterConfig config = info.config;
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new CharacterText(info.name, config);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new CharacterSprite(info.name, config, info.prefab, info.rootCharacterFolder);

                case Character.CharacterType.Live2D:
                    return new CharacterLive2D(info.name, config, info.prefab, info.rootCharacterFolder);

                case Character.CharacterType.Model3D:
                    return new CharacterModel3D(info.name, config, info.prefab, info.rootCharacterFolder);
                default:
                    return null;
            }
        }

        private class CHARACTERINFO
        {
            public string name = "";
            public string castingName = "";
            public string rootCharacterFolder = "";
            public CharacterConfig config;
            public GameObject prefab = null;
        }
    }
}