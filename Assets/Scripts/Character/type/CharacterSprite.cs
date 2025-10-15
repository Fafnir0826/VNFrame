using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;
namespace CHARACTERS
{
    public class CharacterSprite : Character
    {
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
        public CharacterSprite(string name, CharacterConfig config, GameObject prefab) : base(name, config, prefab)
        {
            rootCG.alpha = 0;
          
            Debug.Log(name);
        }
        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }
            co_revealing = null;
            co_hiding = null;
        }
    }
}