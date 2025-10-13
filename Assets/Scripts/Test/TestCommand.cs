using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using COMMANDS;
public class TestCommand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //startCoroutine(Running());
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        //     CommandManager.instance.Execute("moveCharDemo", "left");
        // else if (Input.GetKeyDown(KeyCode.RightArrow))
        //     CommandManager.instance.Execute("moveCharDemo", "right");
    }
    IEnumerator Running()
    {
        yield return CommandManager.instance.Execute("print");
        yield return CommandManager.instance.Execute("print_lp", "Hello Word!");
        yield return CommandManager.instance.Execute("print_mp", "Line1", "Line2", "Line3");

        yield return CommandManager.instance.Execute("lambda");
        yield return CommandManager.instance.Execute("lambda_lp", "Hello Lambda!");
        yield return CommandManager.instance.Execute("lambda_mp", "Lambda1", "Lambda2", "Lambda3");

        yield return CommandManager.instance.Execute("process");
        yield return CommandManager.instance.Execute("process_lp", "3");
        yield return CommandManager.instance.Execute("process_mp", "ProcessLine1", "ProcessLine2", "ProcessLine3");
    }
}
