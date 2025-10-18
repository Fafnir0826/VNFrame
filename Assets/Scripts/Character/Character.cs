using System.Collections;
using System.Collections.Generic;
using System.Collections;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public const bool ENABLE_ON_START = true;
        private const float UNHIGHLIGHTED_DARKEN_STRENGTH = 0.65f;
        public const bool DEFAULT_ORIENTAION_IS_FACING_LEFT = true;
        public const string ANIMATION_REFRESH_TRIGGER = "Refresh";
        public string name = "";
        public string displayname = "";
        public RectTransform root = null;
        public CharacterConfig config;
        public Animator animator;
        public Color color { get; protected set; } = Color.white;
        protected Color displayColor => highlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHLIGHTED_DARKEN_STRENGTH, color.g * UNHIGHLIGHTED_DARKEN_STRENGTH, color.b * UNHIGHLIGHTED_DARKEN_STRENGTH, color.a);
        public bool highlighted { get; protected set; } = true;
        protected bool faceLeft = DEFAULT_ORIENTAION_IS_FACING_LEFT;
        public int priority { get; protected set; }

        protected CharacterManager CharacterManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;
        //Coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_flipping;
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public bool isChanginColor => co_changingColor != null;
        public bool isHightlighting => (highlighted && co_highlighting != null);
        public bool isUnHightlighting => (!highlighted && co_highlighting != null);
        public virtual bool isVisible { get; set; }
        public bool isFacingLeft => faceLeft;
        public bool isFacingRight => !faceLeft;
        public bool isFlipping => co_flipping != null;
        public Character(string name, CharacterConfig config, GameObject prefab)
        {
            this.name = name;
            displayname = name;
            this.config = config;
            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, CharacterManager.characterPanel);
                ob.name = CharacterManager.FormatCharaterPath(CharacterManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayname);
            dialogueSystem.ApplySpeakerDataToDialogueContainer(name);
            UpdateTextCustomizationsOnScreen();
            return dialogueSystem.Say(dialogue);
        }
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) { config.dialogueColor = color; }
        public void RestConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name);
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public virtual Coroutine Show()
        {
            if (isRevealing)
                return co_revealing;

            if (isHiding)
                CharacterManager.StopCoroutine(co_hiding);

            co_revealing = CharacterManager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;
        }
        public virtual Coroutine Hide()
        {
            if (isHiding)
                return co_hiding;

            if (isRevealing)
                CharacterManager.StopCoroutine(co_revealing);

            co_hiding = CharacterManager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }
        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Show/Hide");
            yield return null;
        }

        public virtual void SetPosition(Vector2 position)
        {
            if (root == null)
                return;
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTarget(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }
        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null)
                return null;
            if (isMoving)
                CharacterManager.StopCoroutine(co_moving);

            co_moving = CharacterManager.StartCoroutine(MovingToPosition(position, speed, smooth));
            return co_moving;
        }
        private IEnumerator MovingToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTarget(position);

            Vector2 padding = root.anchorMax - root.anchorMin;
            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                    Vector2.Lerp(root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                }
                yield return null;
            }
            Debug.Log("Done moving");
            co_moving = null;
        }
        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTarget(Vector2 position)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;
            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;
            return (minAnchorTarget, maxAnchorTarget);

        }
        public virtual void SetColor(Color color)
        {
            this.color = color;
        }
        public Coroutine TransitionColor(Color color, float speed = 1)
        {
            this.color = color;
            if (isChanginColor)
                CharacterManager.StopCoroutine(co_changingColor);

            co_changingColor = CharacterManager.StartCoroutine(ChangingColor(displayColor, speed));

            return co_changingColor;
        }
        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Set color...");
            yield return null;
        }
        public Coroutine Hightlight(float speed = 1f)
        {
            if (isHightlighting)
                return co_highlighting;
            if (isUnHightlighting)
                CharacterManager.StopCoroutine(co_highlighting);

            highlighted = true;
            co_highlighting = CharacterManager.StartCoroutine(Highlighting(highlighted, speed));

            return co_highlighting;
        }

        public Coroutine UnHightlight(float speed = 1f)
        {
            if (isUnHightlighting)
                return co_highlighting;

            if (isHightlighting)
                CharacterManager.StopCoroutine(co_highlighting);

            highlighted = false;
            co_highlighting = CharacterManager.StartCoroutine(Highlighting(highlighted, speed));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            yield return null;
        }
        public Coroutine Flip(float speed = 1, bool immediate = false)
        {
            if (isFacingLeft)
                return FaceRight(speed, immediate);
            else
                return FaceLeft(speed, immediate);
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                CharacterManager.StopCoroutine(co_flipping);

            faceLeft = true;
            co_flipping = CharacterManager.StartCoroutine(FaceDirection(faceLeft, speed, immediate));
            return co_flipping;
        }
        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                CharacterManager.StopCoroutine(co_flipping);

            faceLeft = false;
            co_flipping = CharacterManager.StartCoroutine(FaceDirection(faceLeft, speed, immediate));
            return co_flipping;

        }
        public virtual IEnumerator FaceDirection(bool FaceLeft, float speedMultiplier, bool immediate)
        {
            yield return null;
        }
        public void SetPriority(int priority, bool autoSortCharatersOnUI = true)
        {
            this.priority = priority;
            if (autoSortCharatersOnUI)
                CharacterManager.SortCharacters();
        }

        public void Animate(string animation)
        {
            animator.SetTrigger(animation);
        }
        public void Animate(string animation, bool state)
        {
            animator.SetBool(animation, state);
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER);
        }

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D,
        }

    }
}