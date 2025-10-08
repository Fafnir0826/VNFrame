using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class TestDialogueFiles : MonoBehaviour
{
    void Start()
    {
        StartConversation();

    }

    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("textFile");

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                return;
                
            Debug.Log($"Segmenting line '{line}'");
            DialogueLine dlLine = DialogueParser.Parse(line);
            int i = 0;
            foreach (DialogueData.DIALOGUE_SEGMENT segment in dlLine.dialogue.segments)
            {
                Debug.Log($"Segment[{i++}] = '{segment.dialogue}' [signal={segment.startSignal.ToString()}{(segment.signalDelay > 0 ? $"{segment.signalDelay}" : $"")}]");
            }
        }

        //DialogueSystem.instance.Say(lines);
    }
}

