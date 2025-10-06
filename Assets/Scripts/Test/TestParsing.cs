using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
    public class TestParsing : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        // Start is called before the first frame update
        void Start()
        {
            SendFileToParse();
            // string line = "Spaker \"Dialogue \\\"Goes In\\\" Here!\"Command(arguments)";
            // DialogueParser.Parse(line);
        }

        void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset(file);


            foreach (string line in lines)
            {
                if (line == string.Empty)
                    continue;
                DialogueLine dl = DialogueParser.Parse(line);
            }
        }
    }
}