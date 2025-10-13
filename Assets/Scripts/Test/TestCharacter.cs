using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            Character Stella = CharacterManager.instance.CreateCharacter("Stella");
            Character Stella2 = CharacterManager.instance.CreateCharacter("Stella");
            Character Adam = CharacterManager.instance.CreateCharacter("Adam");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}