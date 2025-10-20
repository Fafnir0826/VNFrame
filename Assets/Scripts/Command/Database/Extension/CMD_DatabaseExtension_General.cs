using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using UnityEngine;

public class CMD_DatabaseExtension_General : CMDDataExtension
{
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
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
