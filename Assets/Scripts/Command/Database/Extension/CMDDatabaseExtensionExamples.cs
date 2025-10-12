using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


// all command need static
public class CMDDatabaseExtensionExamples : CMDDataExtension
{
    new public static void Extend(CommandDatabase database)
    {
        //Add command with no paramters;
        database.AddCommand("print", new Action(PrintDefaultMessage));
        database.AddCommand("print_lp", new Action<string>(PrintUsermessage));
        database.AddCommand("print_mp", new Action<string[]>(PrintLines));
        // Add lambad with no paramters;
        database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a dafault message from lambda"); }));
        database.AddCommand("lambda_lp", new Action<string>(((arg) => { Debug.Log($"log user lambd message: '{arg}'"); })));
        database.AddCommand("lambda_mp", new Action<string[]>(((args) => { Debug.Log(string.Join(", ", args)); })));
        // Add coroutine with no paramters;
        database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
        database.AddCommand("process_lp", new Func<string, IEnumerator>(LineProcess));
        database.AddCommand("process_mp", new Func<string[], IEnumerator>(MutliLineProcess));
        //specail Example
        database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));
    }

    private static IEnumerator MoveCharacter(string direction)
    {
        bool left = direction.ToLower() == "left";

        Transform character = GameObject.Find("Image").transform;
        float moveSpeed = 15;
        float targetX = left ? -8 : 8;
        float currentX = character.position.x;

        while (Mathf.Abs(targetX - currentX) > 0.1f)
        {
            Debug.Log($"Moving character to {(left ? "left" : "right")} [{currentX}/{targetX}]");
            currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            character.position = new Vector3(currentX, character.position.y, character.position.z);
            yield return null;

        }
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing a dafault message");
    }

    private static void PrintUsermessage(string message)
    {
        Debug.Log($"User message:'{message}'");
    }
    private static void PrintLines(string[] lines)
    {
        int i = 1;
        foreach (string line in lines)
        {
            Debug.Log($"{i++}. '{line}'");
        }
    }

    private static IEnumerator SimpleProcess()
    {
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log($"process running.... [{i}]");
            yield return new WaitForSeconds(1);
        }
    }

    private static IEnumerator LineProcess(string data)
    {
        if (int.TryParse(data, out int num))
        {
            for (int i = 1; i <= num; i++)
            {
                Debug.Log($"process running.... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

    }
    private static IEnumerator MutliLineProcess(string[] data)
    {
        foreach (string line in data)
        {
            Debug.Log($"process running.... [{line}]");
            yield return new WaitForSeconds(0.5f);
        }

    }
}
