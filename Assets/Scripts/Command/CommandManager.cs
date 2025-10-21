using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.Data;
using UnityEngine.Events;


namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager instance { get; private set; }
        private CommandDatabase database;
        private List<CommandProcess> activeProcess = new List<CommandProcess>();
        private CommandProcess topProcess => activeProcess.Last();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                database = new CommandDatabase();
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMDDataExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { database });
                }
            }
            else
                DestroyImmediate(gameObject);
        }


        public CoroutineWrapper Execute(string commandName, params string[] args)
        {
            Delegate command = database.GetCommand(commandName);

            if (command == null)
                return null;

            return StartProcess(commandName, command, args);

        }

        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            System.Guid processId = System.Guid.NewGuid();
            CommandProcess cmd = new CommandProcess(processId, commandName, command, null, args, null);
            activeProcess.Add(cmd);
            Coroutine co = StartCoroutine(RunningProcess(cmd));

            cmd.runningProcess = new CoroutineWrapper(this, co);

            return cmd.runningProcess;
        }

        public void StopCurrentProcess()
        {
            if (topProcess != null)
                KillProcess(topProcess);

        }
        public void StopAllProcesses()
        {
            foreach (var c in activeProcess)
            {
                if (c.runningProcess != null && !c.runningProcess.isDone)
                    c.runningProcess.Stop();

                c.onTerminateAction?.Invoke();
            }

            activeProcess.Clear();

        }


        private IEnumerator RunningProcess(CommandProcess process)
        {
            yield return WaitingForProcessToComplete(process.command, process.args);
            KillProcess(process);
        }
        public void KillProcess(CommandProcess cmd)
        {
            activeProcess.Remove(cmd);

            if (cmd.runningProcess != null && !cmd.runningProcess.isDone)
                cmd.runningProcess.Stop();

            cmd.onTerminateAction?.Invoke();
        }
        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
                command.DynamicInvoke();
            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);
            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);
            else if (command is Func<IEnumerator>)
                yield return ((Func<IEnumerator>)command)();
            else if (command is Func<string, IEnumerator>)
                yield return ((Func<string, IEnumerator>)command)(args[0]);
            else if (command is Func<string[], IEnumerator>)
                yield return ((Func<string[], IEnumerator>)command)(args);
        }

        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            CommandProcess process = topProcess;
            if (topProcess == null)
                return;

            process.onTerminateAction = new UnityEvent();
            process.onTerminateAction.AddListener(action);
        }
    }
}
