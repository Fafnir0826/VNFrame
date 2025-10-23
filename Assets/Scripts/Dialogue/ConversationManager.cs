using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using COMMANDS;
using CHARACTERS;
using System;
using System.Runtime.ConstrainedExecution;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;

        public bool isRunning => process != null;
        public TextArchitect architect = null;
        private bool userPrompt = false;

        private TagManager tagManager;
        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
            tagManager = new TagManager();
        }

        public void OnUserPrompt_Next()
        {
            userPrompt = true;
        }
        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
            return process;
        }
        public void StopConversation()
        {
            if (!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;
                DialogueLine line = DialogueParser.Parse(conversation[i]);

                // show dialogue
                if (line.hasDialogue)
                    yield return Line_RunDialogue(line);
                //Run any commands
                if (line.hasCommands)
                    yield return Line_RunCommands(line);

                if (line.hasDialogue)
                {

                    //Wait for user input
                    yield return WaitForUserInput();
                    CommandManager.instance.StopAllProcesses();
                }

            }
        }


        IEnumerator Line_RunDialogue(DialogueLine line)
        {
            if (line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }
            if (!dialogueSystem.dialogueContainer.isVisible)
                dialogueSystem.dialogueContainer.Show();

            yield return BuildLineSegments(line.dialogueData);

        }

        private void HandleSpeakerLogic(SpeakerData speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPostion || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

            if (speakerData.makeCharacterEnter && (!character.isVisible))
                character.Show();


            //Add character name to the UI
            dialogueSystem.ShowSpeakerName(tagManager.Inject(speakerData.displayname));

            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            if (speakerData.isCastingPostion)
                character.MoveToPosition(speakerData.castPosition);

            if (speakerData.isCastingExpressions)
            {
                foreach (var ce in speakerData.CastExpressions)
                    character.OnReceiveCastingExpression(ce.layer, ce.expression);
            }

        }
        IEnumerator Line_RunCommands(DialogueLine line)
        {
            List<CommandData.Command> commands = line.commandData.commands;

            foreach (CommandData.Command command in commands)
            {
                if (command.waitForCompletion || command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);

                    while (!cw.isDone)
                    {
                        if (userPrompt)
                        {
                            CommandManager.instance.StopCurrentProcess();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else
                    CommandManager.instance.Execute(command.name, command.arguments);
            }

            yield return null;
        }
        IEnumerator BuildLineSegments(DialogueData line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DialogueData.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText);

            }
        }
        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DialogueData.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.C:
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.WA:
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.WC:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            dialogue = tagManager.Inject(dialogue);
            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            dialogueSystem.prompt.Show();

            while (!userPrompt)
                yield return null;

            dialogueSystem.prompt.Hide();
            userPrompt = false;
        }

    }
}