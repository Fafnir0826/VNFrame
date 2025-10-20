using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public abstract class CMDDataExtension
    {
        public static void Extend(CommandDatabase database) { }

        public static CommandParamters ConverDataToParameters(string[] data) => new CommandParamters(data);
    }
}