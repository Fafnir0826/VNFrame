using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;
namespace CHARACTERS
{
    public class CharacterSprite : Character
    {
        public CharacterSprite(string name, CharacterConfig config) : base(name, config)
        {
            Debug.Log(name);
        }
    }
}