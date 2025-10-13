using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class CharacterText : Character
    {
        public CharacterText(string name) : base(name)
        {
            Debug.Log(name);
        }
    }
}