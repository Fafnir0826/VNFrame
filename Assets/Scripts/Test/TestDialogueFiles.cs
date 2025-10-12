using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DIALOGUE;
using Unity.VisualScripting;
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

        DialogueSystem.instance.Say(lines);
        // Debug.Log(lines);
        // for (int i = 0; i < lines.Count; i++)
        // {
        //     string line = lines[i];
        //     if (string.IsNullOrWhiteSpace(line))
        //         continue;
        //     DialogueLine dl = DialogueParser.Parse(line);

        //     Debug.Log($"{dl.speaker.name} as [{(dl.speaker.castName != string.Empty ? dl.speaker.castName : dl.speaker.name)}]at{dl.speaker.castPosition}");

        //     List<(int l, string ex)> expr = dl.speaker.CastExpressions;

        //     for (int c = 0; c < expr.Count; c++)
        //     {

        //         Debug.Log($"[Layer[{expr[c].l}]  = '{expr[c].ex}']");
        //     }
        // }

        // foreach (string line in lines)
        // {
        //     if (string.IsNullOrEmpty(line))
        //         continue;


        //     DialogueLine dl = DialogueParser.Parse(line);
        //     for (int i = 0; i < dl.commandData.commands.Count; i++)
        //     {
        //         CommandData.Command command = dl.commandData.commands[i];

        //         Debug.Log($"Command [{i}]  '{command.name}' has arguments[{string.Join(", ", command.arguments)}]");
        //     }
        //}


    }
}

