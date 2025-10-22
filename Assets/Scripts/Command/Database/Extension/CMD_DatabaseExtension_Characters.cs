using System;
using System.Collections;
using COMMANDS;
using UnityEngine;
using CHARACTERS;
using System.Collections.Generic;
using System.Linq;


public class CMD_DatabaseExtension_Characters : CMDDataExtension
{
    private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
    private static string[] PARAM_ENABLE => new string[] { "-e", "-enabled" };
    private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
    private static string[] PARAM_SMOOTH => new string[] { "-sm", "-smooth" };
    private static string PARAM_XPOS => "-x";
    private static string PARAM_YPOS => "-y";
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
        database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
        database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
        database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));

        //Add commands to characters
        CommandDatabase baseCommands = CommandManager.instance.CreateSubDatabase(CommandManager.DATABASE_CHARACTERS_BASE);
        baseCommands.AddCommand("move", new Func<string[], IEnumerator>(MoveCharacter));
        baseCommands.AddCommand("show", new Func<string[], IEnumerator>(Show));
        baseCommands.AddCommand("hide", new Func<string[], IEnumerator>(Hide));
        baseCommands.AddCommand("setpriority", new Action<string[]>(SetPriority));
        baseCommands.AddCommand("setposition", new Action<string[]>(SetPosition));
        baseCommands.AddCommand("setcolor", new Func<string[], IEnumerator>(SetColor));
        baseCommands.AddCommand("highlight", new Func<string[], IEnumerator>(Highlight));
        baseCommands.AddCommand("unhighlight", new Func<string[], IEnumerator>(Unhighlight));

        //Add character specific databases
        CommandDatabase spriteCommands = CommandManager.instance.CreateSubDatabase(CommandManager.DATABASE_CHARACTERS_SPRITE);
        spriteCommands.AddCommand("SetSprite", new Func<string[], IEnumerator>(SetSprite));
    }
    public static void CreateCharacter(string[] data)
    {
        string characterName = data[0];
        bool enable = false;
        bool immediate = false;
        var parameters = ConverDataToParameters(data, startingIndex: 1);
        parameters.TryGetValue(PARAM_ENABLE, out enable, defaultValue: false);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        Character character = CharacterManager.instance.CreateCharacter(characterName);
        if (!enable)
            return;

        if (immediate)
            character.isVisible = true;
        else
            character.Show();
    }
    public static IEnumerator MoveCharacter(string[] data)
    {
        string characterName = data[0];
        Character character = CharacterManager.instance.GetCharacter(characterName);

        if (character == null)
            yield break;

        float x = 0, y = 0;
        float speed = 1;
        bool smooth = false;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);

        //try to get the x axis position
        parameters.TryGetValue(PARAM_XPOS, out x);
        parameters.TryGetValue(PARAM_YPOS, out y);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1);
        parameters.TryGetValue(PARAM_SMOOTH, out smooth, defaultValue: false);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        Vector2 position = new Vector2(x, y);
        if (immediate)
            character.SetPosition(position);
        else
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.SetPosition(position); });
            yield return character.MoveToPosition(position, speed, smooth);
        }
    }
    public static IEnumerator ShowAll(string[] data)
    {
        List<Character> characters = new List<Character>();
        bool immediate = false;

        foreach (string s in data)
        {
            Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
            if (character != null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count == 0)
        {
            yield break;
        }

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        foreach (Character character in characters)
        {
            if (immediate)
                character.isVisible = true;
            else
                character.Show();
        }
        if (!immediate)
        {
            while (characters.Any(c => c.isRevealing))
                yield return null;
        }
    }
    public static IEnumerator HideAll(string[] data)
    {
        List<Character> characters = new List<Character>();
        bool immediate = false;

        foreach (string s in data)
        {
            Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
            if (character != null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count == 0)
        {
            yield break;
        }

        var parameters = ConverDataToParameters(data);

        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        foreach (Character character in characters)
        {
            if (immediate)
                character.isVisible = false;
            else
                character.Hide();
        }
        if (!immediate)
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
            {
                foreach (Character character in characters)
                    character.isVisible = true;
            });
            while (characters.Any(c => c.isHiding))
                yield return null;
        }
    }

    public static IEnumerator HighlightAll(string[] data)
    {
        List<Character> characters = new List<Character>();
        bool immediate = false;
        bool handleUnspecifiedCharacters = true;
        List<Character> unspecifiedCharacters = new List<Character>();

        //Add any characters specified to be highlighted.
        for (int i = 0; i < data.Length; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
            if (character != null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count == 0)
            yield break;

        //Grab the extra parameters
        var parameters = ConverDataToParameters(data, startingIndex: 1);

        parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);
        parameters.TryGetValue(new string[] { "-o", "-only" }, out handleUnspecifiedCharacters, defaultValue: true);

        //Make all characters perform the logic
        foreach (Character character in characters)
        {
            character.Hightlight(immediate: immediate);
        }

        //If we are forcing any unspecified characters to use the opposite highlighted status
        if (handleUnspecifiedCharacters)
        {
            foreach (Character character in CharacterManager.instance.allCharacters)
            {
                if (characters.Contains(character))
                    continue;

                unspecifiedCharacters.Add(character);
                character.UnHightlight(immediate: immediate);
            }
        }

        //Wait for all characters to finish highlighting
        //Wait for all characters to finish highlighting.
        if (!immediate)
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
            {
                foreach (var character in characters)
                {
                    character.Hightlight(immediate: true);
                }

                if (handleUnspecifiedCharacters)
                {
                    foreach (var character in unspecifiedCharacters)
                    {
                        character.UnHightlight(immediate: true);
                    }
                }
            });

            while (characters.Any(c => c.isHightlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.isUnHightlighting)))
            {
                yield return null;
            }
        }
    }

    public static IEnumerator UnhighlightAll(string[] data)
    {
        List<Character> characters = new List<Character>();
        bool immediate = false;
        bool handleUnspecifiedCharacters = true;
        List<Character> unspecifiedCharacters = new List<Character>();

        //Add any characters specified to be highlighted.
        for (int i = 0; i < data.Length; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
            if (character != null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count == 0)
            yield break;

        //Grab the extra parameters
        var parameters = ConverDataToParameters(data, startingIndex: 1);

        parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);
        parameters.TryGetValue(new string[] { "-o", "-only" }, out handleUnspecifiedCharacters, defaultValue: true);

        //Make all characters perform the logic
        foreach (Character character in characters)
        {
            character.Hightlight(immediate: immediate);
        }

        //If we are forcing any unspecified characters to use the opposite highlighted status
        if (handleUnspecifiedCharacters)
        {
            foreach (Character character in CharacterManager.instance.allCharacters)
            {
                if (characters.Contains(character))
                    continue;

                unspecifiedCharacters.Add(character);
                character.Hightlight(immediate: immediate);
            }
        }

        //Wait for all characters to finish highlighting
        //Wait for all characters to finish highlighting.
        if (!immediate)
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
            {
                foreach (var character in characters)
                {
                    character.Hightlight(immediate: true);
                }

                if (handleUnspecifiedCharacters)
                {
                    foreach (var character in unspecifiedCharacters)
                    {
                        character.UnHightlight(immediate: true);
                    }
                }
            });

            while (characters.Any(c => c.isUnHightlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.isHightlighting)))
            {
                yield return null;
            }
        }
    }

    public static IEnumerator SetSprite(string[] data)
    {
        //Format: SetSprite(character sprite)
        CharacterSprite character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false) as CharacterSprite;
        int layer = 0;
        string spriteName;
        bool immediate = false;
        float speed;

        if (character == null || data.Length < 2)
            yield break;

        //Grab the extra parameters
        var parameters = ConverDataToParameters(data, startingIndex: 1);

        //Try to get the sprite name
        parameters.TryGetValue(new string[] { "-s", "-sprite" }, out spriteName);

        //Try to get the layer
        parameters.TryGetValue(new string[] { "-l", "-layer" }, out layer, defaultValue: 0);

        //Try to get the transition speed
        bool specifiedSpeed = parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 0.1f);

        //Try to get whether this is an immediate transition or not
        if (!specifiedSpeed)
        {
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: true);
        }

        //Run the logic
        Sprite sprite = character.GetSprite(spriteName);

        if (sprite == null)
            yield break;

        if (immediate)
        {
            character.SetSprite(sprite, layer);
        }
        else
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() => character?.SetSprite(sprite, layer));
            yield return character.TransitionSprite(sprite, layer, speed);
        }
    }


    public static IEnumerator Show(string[] data)
    {
        string characterName = data[0];
        Character character = CharacterManager.instance.GetCharacter(characterName);

        if (character == null)
            yield break;

        bool immediate = false;
        var parameters = ConverDataToParameters(data, startingIndex: 1);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        if (immediate)
        {
            character.isVisible = true;
        }
        else
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() => character?.Show());
            yield return character.Show();
        }
    }
    public static IEnumerator Hide(string[] data)
    {
        string characterName = data[0];
        Character character = CharacterManager.instance.GetCharacter(characterName);

        if (character == null)
            yield break;

        bool immediate = false;
        var parameters = ConverDataToParameters(data, startingIndex: 1);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        if (immediate)
        {
            character.isVisible = false;
        }
        else
        {
            CommandManager.instance.AddTerminationActionToCurrentProcess(() => character?.Hide());
            yield return character.Hide();
        }
    }
    public static void SetPriority(string[] data)
    {

    }
    public static void SetPosition(string[] data)
    {

    }
    public static IEnumerator SetColor(string[] data)
    {
        yield return null;
    }
    public static IEnumerator Highlight(string[] data)
    {
        string characterName = data[0];
        Character character = CharacterManager.instance.GetCharacter(characterName);

        if (character == null)
            yield break;

        bool immediate = false;
        var parameters = ConverDataToParameters(data, startingIndex: 1);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        CommandManager.instance.AddTerminationActionToCurrentProcess(() => character?.Hightlight(immediate: true));
        yield return character.Hightlight(immediate: immediate);
    }
    public static IEnumerator Unhighlight(string[] data)
    {
        string characterName = data[0];
        Character character = CharacterManager.instance.GetCharacter(characterName);

        if (character == null)
            yield break;

        bool immediate = false;
        var parameters = ConverDataToParameters(data, startingIndex: 1);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        CommandManager.instance.AddTerminationActionToCurrentProcess(() => character?.UnHightlight(immediate: true));
        yield return character.UnHightlight(immediate: immediate);
    }

}
