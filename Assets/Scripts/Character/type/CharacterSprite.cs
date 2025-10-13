using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;
namespace CHARACTERS
{
    public class CharacterSprite : Character
    {
        public CharacterSprite(string name) : base(name)
        {
            Debug.Log(name);
        }
    }
}