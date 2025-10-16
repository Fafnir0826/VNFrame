using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        // Start is called before the first frame update
        void Start()
        {

            // Character Stella = CharacterManager.instance.CreateCharacter("Female Student 2");
            // Character Stella2 = CharacterManager.instance.CreateCharacter("Stella");
            // Character Adam = CharacterManager.instance.CreateCharacter("Adam");

            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            CharacterSprite guard1 = CreateCharacter("Guard1 as Generic") as CharacterSprite;
            // CharacterSprite Raelin = CreateCharacter("Raelin") as CharacterSprite;
            // CharacterSprite student = CreateCharacter("Female Student 2") as CharacterSprite;

           
            Sprite sl = guard1.GetSprite("Characters-Monk");
            guard1.SetSprite(sl);

            yield return null;

        }
    }
}