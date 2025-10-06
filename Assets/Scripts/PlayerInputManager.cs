using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                PromptAdvanece();

        }
        public void PromptAdvanece()
        {
            DialogueSystem.instance.OnUserPrompt_Next();
        }
    }
}
