using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using DIALOGUE;
using UnityEngine;

public class CMD_DatabaseExtension_General : CMDDataExtension
{
    private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
    private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
        database.AddCommand("showui", new Func<string[], IEnumerator>(ShowDialogueSystem));
        database.AddCommand("hideui", new Func<string[], IEnumerator>(HideDialogueSystem));

        database.AddCommand("showdb", new Func<string[], IEnumerator>(ShowDialogueBox));
        database.AddCommand("hidedb", new Func<string[], IEnumerator>(HideDialogueBox));
    }

    private static IEnumerator ShowDialogueSystem(string[] data)
    {
        float speed;
        bool immediate;

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        yield return DialogueSystem.instance.Show(speed, immediate);
    }

    private static IEnumerator HideDialogueSystem(string[] data)
    {
        float speed;
        bool immediate;

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        yield return DialogueSystem.instance.Hide(speed, immediate);
    }

    private static IEnumerator ShowDialogueBox(string[] data)
    {
        float speed;
        bool immediate;

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
        yield return DialogueSystem.instance.dialogueContainer.Show(speed, immediate);
    }


    private static IEnumerator HideDialogueBox(string[] data)
    {
        float speed;
        bool immediate;

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        yield return DialogueSystem.instance.dialogueContainer.Hide(speed, immediate);
    }



    // 1 reference

    private static IEnumerator Wait(string data)
    {
        if (float.TryParse(data, out float time))
        {
            yield return new WaitForSeconds(time);
        }
    }
}
