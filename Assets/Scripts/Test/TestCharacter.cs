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

            // Character Stella = CharacterManager.instance.CreateCharacter("Stella");
            // Character Stella2 = CharacterManager.instance.CreateCharacter("Stella");
            // Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            Character Ben = CharacterManager.instance.CreateCharacter("Benjamin");

            List<string> lines = new List<string>()
            {
                "Hi there",
                "My name is Elen.",
                "What's your name",
                "OH,{wa 1} that's very nice",
            };
            yield return Elen.Say(lines);

            Elen.SetNameColor(Color.red);
            Elen.SetDialogueColor(Color.green);

            lines = new List<string>()
            {
                "I am Adam",
                "More lines{c}Here"

            };
            yield return Adam.Say(lines);


            yield return Ben.Say("THis is a line tha i wnat ot say.{a} simapline");
            Debug.Log("Finished");
        }
    }
}