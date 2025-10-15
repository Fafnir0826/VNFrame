using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {

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
            Character Stella = CharacterManager.instance.CreateCharacter("Stella");
            yield return new WaitForSeconds(1f);
            yield return Stella.Hide();
            yield return new WaitForSeconds(1f);
            yield return Stella.Show();
            yield return Stella.Say("Hello");

        }
    }
}