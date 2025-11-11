using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using COMMANDS;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class CMD_DatabaseExtension_GraphicPanels : CMDDataExtension
{
    private static string[] PARAM_PANEL = new string[] { "-p", "-panel" };
    private static string[] PARAM_LAYER = new string[] { "-l", "-layer" };
    private static string[] PARAM_MEDIA = new string[] { "-m", "-media" };
    private static string[] PARAM_SPEED = new string[] { "-spd", "-speed" };
    private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
    private static string[] PARAM_BLENDTEX = new string[] { "-b", "-blend" };
    private static string[] PARAM_USEVIDEOAUDIO = new string[] { "-aud", "-audio" };
    private static string[] PARAM_AMOUNT = new string[] { "-a", "-amount" };
    private static string[] PARAM_ZOOM = new string[] { "-z", "-zoom" };
    private static string[] PARAM_X = new string[] { "-x" };
    private static string[] PARAM_Y = new string[] { "-y" };
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
        database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
        database.AddCommand("setworldcanvasmedia", new Func<string[], IEnumerator>(SetWorldCanvasMedia));
        database.AddCommand("clearworldcanvasmedia", new Func<string[], IEnumerator>(ClearWorldCanvasMedia));

        // 鏡頭效果命令
        database.AddCommand("cameralookleft", new Func<string[], IEnumerator>(CameraLookLeft));
        database.AddCommand("cameralookright", new Func<string[], IEnumerator>(CameraLookRight));
        database.AddCommand("camerazoomin", new Func<string[], IEnumerator>(CameraZoomIn));
        database.AddCommand("camerazoomout", new Func<string[], IEnumerator>(CameraZoomOut));
        database.AddCommand("camerareset", new Func<string[], IEnumerator>(CameraReset));
        database.AddCommand("camerasetposition", new Func<string[], IEnumerator>(CameraSetPosition));
        database.AddCommand("camerasetbackgroundscale", new Func<string[], IEnumerator>(CameraSetBackgroundScale));
    }
    private static IEnumerator SetLayerMedia(string[] data)
    {
        //Parameters available to function
        string panelName = "";
        int layer = 0;
        string mediaName = "";
        float transitionSpeed = 0;
        bool immediate = false;
        string blendTexName = "";
        bool useAudio = false;
        string pathToGraphic = "";
        UnityEngine.Object graphic = null;
        Texture blendTex = null;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_PANEL, out panelName);
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
        if (panel == null)
        {
            Debug.LogError("can't to grab panel");
            yield break;
        }

        //Try to get the layer to apply this graphic to
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);

        //Try to get the graphic
        parameters.TryGetValue(PARAM_MEDIA, out mediaName);

        //Try to get if this is an immediate effect or not
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        //Try to get the speed of the transition if it is not an immediate effect
        if (!immediate) // 根據邏輯判斷，如果是立即效果，就不需要速度，所以這裡應該是 !immediate
        {
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        }

        //Try to get the blending texture for the media if we are using one.
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);

        //If this is a video, try to get whether we use audio from the video or not
        parameters.TryGetValue(PARAM_USEVIDEOAUDIO, out useAudio, defaultValue: false);

        pathToGraphic = GetPathToGraphic(Configs.resources_backgroundImages, mediaName);
        graphic = Resources.Load<Texture>(pathToGraphic);
        if (graphic == null)
        {
            pathToGraphic = GetPathToGraphic(Configs.resources_backgroundVideos, mediaName);
            graphic = Resources.Load<VideoClip>(pathToGraphic);
        }
        if (graphic == null)
        {
            Debug.LogError("cant find any media");
            yield break;
        }
        if (!immediate && blendTexName != string.Empty)
        {
            blendTex = Resources.Load<Texture>(Configs.resources_blendTextures + blendTexName);
        }

        GraphicLayer graphicLayer = panel.GetLayer(layer, true);

        if (graphic is Texture)
        {
            yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
        }


    }

    private static IEnumerator ClearLayerMedia(string[] data)
    {
        //Parameters available to function
        string panelName = "";
        int layer = 0;
        string mediaName = "";
        float transitionSpeed = 1;
        bool immediate = false;
        string blendTexName = "";
        Texture blendTex = null;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_PANEL, out panelName);
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
        if (panel == null)
        {
            Debug.LogError("can't to grab panel");
            yield break;
        }

        //Try to get the layer to apply this graphic to
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);

        //Try to get if this is an immediate effect or not
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
        //Try to get the speed of the transition if it is not an immediate effect
        if (!immediate) // 根據邏輯判斷，如果是立即效果，就不需要速度，所以這裡應該是 !immediate
        {
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        }

        //Try to get the blending texture for the media if we are using one.
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);
        if (!immediate && blendTexName != string.Empty)
            blendTex = Resources.Load<Texture>(Configs.resources_blendTextures + blendTexName);

        // 執行清除
        if (layer == -1)
            panel.Clear(transitionSpeed, blendTex, immediate);
        else
        {
            GraphicLayer graphicLayer = panel.GetLayer(layer);
            if (graphicLayer == null)
            {
                Debug.LogError("can not clear layer");
                yield break;
            }

            graphicLayer.Clear(transitionSpeed, blendTex, immediate);
        }

        // 如果不是立即清除，等待淡出動畫完成
        if (!immediate)
        {
            float duration = 1f / transitionSpeed;
            yield return new WaitForSeconds(duration);
        }
    }
    private static string GetPathToGraphic(string defaultPath, string graphicName)
    {
        if (graphicName.StartsWith(HOME_DIRECTORY_SYMBOL))
            return graphicName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        return defaultPath + graphicName;
    }

    // WorldCanvas 專用命令
    private static GraphicPanel worldCanvasPanel = null;
    private static Dictionary<int, GraphicLayer> worldCanvasLayers = new Dictionary<int, GraphicLayer>();
    
    private static GraphicPanel GetWorldCanvasPanel()
    {
        if (worldCanvasPanel == null)
        {
            GameObject worldCanvasRoot = GameObject.Find("WorldCanvas/1 - Background");
            if (worldCanvasRoot == null)
            {
                Debug.LogError("找不到 WorldCanvas/1 - Background 物件");
                return null;
            }

            worldCanvasPanel = new GraphicPanel
            {
                panelName = "WorldCanvas",
                rootPanel = worldCanvasRoot
            };
            
            Debug.Log($"GetWorldCanvasPanel: 初始化完成，根物件: {worldCanvasRoot.name}");
        }
        return worldCanvasPanel;
    }
    
    // 獲取或創建 WorldCanvas 的圖層
    private static GraphicLayer GetOrCreateWorldCanvasLayer(int layerDepth)
    {
        if (!worldCanvasLayers.ContainsKey(layerDepth))
        {
            GraphicPanel panel = GetWorldCanvasPanel();
            if (panel == null) return null;
            
            GraphicLayer layer = panel.GetLayer(layerDepth, createIfDoesNotExist: true);
            worldCanvasLayers[layerDepth] = layer;
            Debug.Log($"GetOrCreateWorldCanvasLayer: 創建/獲取圖層 {layerDepth}");
        }
        return worldCanvasLayers[layerDepth];
    }
    
    // 清除 WorldCanvas 的所有圖層
    private static void ClearAllWorldCanvasLayers(float transitionSpeed, Texture blendTex, bool immediate)
    {
        GameObject worldCanvasRoot = GameObject.Find("WorldCanvas/1 - Background");
        if (worldCanvasRoot == null) return;
        
        // 直接找到所有子物件並清除
        foreach (Transform child in worldCanvasRoot.transform)
        {
            if (child.name.StartsWith("Layer:"))
            {
                Debug.Log($"ClearAllWorldCanvasLayers: 清除 {child.name}");
                if (immediate)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
                else
                {
                    // 淡出後銷毀
                    CanvasGroup cg = child.GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        GraphicPanelManager.instance.StartCoroutine(FadeOutAndDestroy(child.gameObject, cg, transitionSpeed));
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(child.gameObject);
                    }
                }
            }
        }
        
        // 清空追蹤
        worldCanvasLayers.Clear();
        worldCanvasPanel = null;
    }
    
    // 淡出並銷毀
    private static IEnumerator FadeOutAndDestroy(GameObject obj, CanvasGroup canvasGroup, float speed)
    {
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= speed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Max(0, alpha);
            yield return null;
        }
        UnityEngine.Object.Destroy(obj);
    }

    private static IEnumerator SetWorldCanvasMedia(string[] data)
    {
        //Parameters available to function
        int layer = 0;
        string mediaName = "";
        float transitionSpeed = 0;
        bool immediate = false;
        string blendTexName = "";
        bool useAudio = false;
        string pathToGraphic = "";
        UnityEngine.Object graphic = null;
        Texture blendTex = null;

        GraphicPanel panel = GetWorldCanvasPanel();
        if (panel == null)
        {
            yield break;
        }

        var parameters = ConverDataToParameters(data);

        //Try to get the layer to apply this graphic to
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);

        //Try to get the graphic
        parameters.TryGetValue(PARAM_MEDIA, out mediaName);

        //Try to get if this is an immediate effect or not
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        //Try to get the speed of the transition if it is not an immediate effect
        if (!immediate)
        {
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        }

        //Try to get the blending texture for the media if we are using one.
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);

        //If this is a video, try to get whether we use audio from the video or not
        parameters.TryGetValue(PARAM_USEVIDEOAUDIO, out useAudio, defaultValue: false);

        pathToGraphic = GetPathToGraphic(Configs.resources_backgroundImages, mediaName);
        graphic = Resources.Load<Texture>(pathToGraphic);
        if (graphic == null)
        {
            pathToGraphic = GetPathToGraphic(Configs.resources_backgroundVideos, mediaName);
            graphic = Resources.Load<VideoClip>(pathToGraphic);
        }
        if (graphic == null)
        {
            Debug.LogError("找不到任何媒體: " + mediaName);
            yield break;
        }
        if (!immediate && blendTexName != string.Empty)
        {
            blendTex = Resources.Load<Texture>(Configs.resources_blendTextures + blendTexName);
        }

        GraphicLayer graphicLayer = GetOrCreateWorldCanvasLayer(layer);
        if (graphicLayer == null)
        {
            Debug.LogError("SetWorldCanvasMedia: 無法創建圖層");
            yield break;
        }

        if (graphic is Texture)
        {
            yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
        }
    }

    private static IEnumerator ClearWorldCanvasMedia(string[] data)
    {
        //Parameters available to function
        int layer = 0;
        float transitionSpeed = 1;
        bool immediate = false;
        string blendTexName = "";
        Texture blendTex = null;

        var parameters = ConverDataToParameters(data);

        //Try to get the layer to apply this graphic to
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);

        //Try to get if this is an immediate effect or not
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        //Try to get the speed of the transition if it is not an immediate effect
        if (!immediate)
        {
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        }

        //Try to get the blending texture for the media if we are using one.
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);
        if (!immediate && blendTexName != string.Empty)
            blendTex = Resources.Load<Texture>(Configs.resources_blendTextures + blendTexName);

        Debug.Log($"ClearWorldCanvasMedia: 開始清除 Layer={layer}, Speed={transitionSpeed}, Immediate={immediate}");

        // 直接清除場景中的物件
        if (layer == -1)
        {
            // 清除所有圖層
            ClearAllWorldCanvasLayers(transitionSpeed, blendTex, immediate);
            Debug.Log("ClearWorldCanvasMedia: 清除所有圖層");
        }
        else
        {
            // 清除特定圖層
            GameObject worldCanvasRoot = GameObject.Find("WorldCanvas/1 - Background");
            if (worldCanvasRoot != null)
            {
                Transform layerTransform = worldCanvasRoot.transform.Find($"Layer: {layer}");
                if (layerTransform != null)
                {
                    Debug.Log($"ClearWorldCanvasMedia: 找到並清除圖層 {layer}");
                    if (immediate)
                    {
                        UnityEngine.Object.Destroy(layerTransform.gameObject);
                    }
                    else
                    {
                        CanvasGroup cg = layerTransform.GetComponent<CanvasGroup>();
                        if (cg != null)
                        {
                            GraphicPanelManager.instance.StartCoroutine(FadeOutAndDestroy(layerTransform.gameObject, cg, transitionSpeed));
                        }
                        else
                        {
                            UnityEngine.Object.Destroy(layerTransform.gameObject);
                        }
                    }
                    
                    // 從追蹤中移除
                    if (worldCanvasLayers.ContainsKey(layer))
                        worldCanvasLayers.Remove(layer);
                }
                else
                {
                    Debug.LogWarning($"ClearWorldCanvasMedia: 找不到圖層 Layer: {layer}");
                }
            }
        }

        // 如果不是立即清除，等待淡出動畫完成
        if (!immediate)
        {
            float duration = 1f / transitionSpeed;
            Debug.Log($"ClearWorldCanvasMedia: 等待 {duration} 秒完成淡出");
            yield return new WaitForSeconds(duration);
        }
        
        Debug.Log("ClearWorldCanvasMedia: 完成");
    }

    // ==================== 鏡頭效果系統 ====================
    private static RectTransform worldCanvasTransform = null;
    private static Vector2 originalPosition = Vector2.zero;
    private static Vector3 originalScale = Vector3.one;
    private static Coroutine currentCameraCoroutine = null;

    // 背景安全縮放比例 - 避免露出黑邊（可動態調整）
    private static float backgroundSafeScale = 1.3f;
    
    private static RectTransform GetWorldCanvasTransform()
    {
        if (worldCanvasTransform == null)
        {
            GameObject worldCanvas = GameObject.Find("WorldCanvas/1 - Background");
            if (worldCanvas == null)
            {
                Debug.LogError("找不到 WorldCanvas/1 - Background 物件");
                return null;
            }
            worldCanvasTransform = worldCanvas.GetComponent<RectTransform>();
            originalPosition = worldCanvasTransform.anchoredPosition;
            originalScale = worldCanvasTransform.localScale;
            
            // 初始化時自動放大背景，避免露出黑邊
            worldCanvasTransform.localScale = originalScale * backgroundSafeScale;
        }
        return worldCanvasTransform;
    }

    // 向左看
    private static IEnumerator CameraLookLeft(string[] data)
    {
        float amount = 300f; // 預設移動距離
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_AMOUNT, out amount, defaultValue: 300f);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        Vector2 targetPosition = canvas.anchoredPosition + Vector2.left * amount;
        yield return MoveCameraToPosition(canvas, targetPosition, speed, immediate);
    }

    // 向右看
    private static IEnumerator CameraLookRight(string[] data)
    {
        float amount = 300f; // 預設移動距離
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_AMOUNT, out amount, defaultValue: 300f);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        Vector2 targetPosition = canvas.anchoredPosition + Vector2.right * amount;
        yield return MoveCameraToPosition(canvas, targetPosition, speed, immediate);
    }

    // 放大
    private static IEnumerator CameraZoomIn(string[] data)
    {
        float amount = 1.5f; // 預設縮放倍數
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_AMOUNT, out amount, defaultValue: 1.5f);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        Vector3 targetScale = canvas.localScale * amount;
        yield return ScaleCameraToSize(canvas, targetScale, speed, immediate);
    }

    // 縮小
    private static IEnumerator CameraZoomOut(string[] data)
    {
        float amount = 0.75f; // 預設縮放倍數
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_AMOUNT, out amount, defaultValue: 0.75f);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        Vector3 targetScale = canvas.localScale * amount;
        yield return ScaleCameraToSize(canvas, targetScale, speed, immediate);
    }

    // 重置相機
    private static IEnumerator CameraReset(string[] data)
    {
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        // 重置到安全縮放比例，避免露出黑邊
        Vector3 safeScale = originalScale * backgroundSafeScale;
        
        // 同時重置位置和縮放
        if (immediate)
        {
            canvas.anchoredPosition = originalPosition;
            canvas.localScale = safeScale;
        }
        else
        {
            yield return MoveCameraToPosition(canvas, originalPosition, speed, false);
            yield return ScaleCameraToSize(canvas, safeScale, speed, false);
        }
    }

    // 設置相機位置（更精確的控制）
    private static IEnumerator CameraSetPosition(string[] data)
    {
        float x = 0f;
        float y = 0f;
        float zoom = 1f;
        float speed = 1f;
        bool immediate = false;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_X, out x, defaultValue: 0f);
        parameters.TryGetValue(PARAM_Y, out y, defaultValue: 0f);
        parameters.TryGetValue(PARAM_ZOOM, out zoom, defaultValue: 1f);
        parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas == null) yield break;

        Vector2 targetPosition = new Vector2(x, y);
        // zoom 是基於安全縮放比例的額外縮放
        Vector3 targetScale = originalScale * backgroundSafeScale * zoom;

        if (immediate)
        {
            canvas.anchoredPosition = targetPosition;
            canvas.localScale = targetScale;
        }
        else
        {
            // 同時執行位置和縮放動畫
            yield return GraphicPanelManager.instance.StartCoroutine(
                AnimateCameraTransform(canvas, targetPosition, targetScale, speed)
            );
        }
    }

    // 平滑移動相機位置
    private static IEnumerator MoveCameraToPosition(RectTransform canvas, Vector2 targetPosition, float speed, bool immediate)
    {
        if (immediate)
        {
            canvas.anchoredPosition = targetPosition;
            yield break;
        }

        Vector2 startPosition = canvas.anchoredPosition;
        float elapsed = 0f;
        float duration = 1f / speed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // 使用 SmoothStep 讓動畫更平滑
            t = t * t * (3f - 2f * t);
            canvas.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        canvas.anchoredPosition = targetPosition;
    }

    // 平滑縮放相機
    private static IEnumerator ScaleCameraToSize(RectTransform canvas, Vector3 targetScale, float speed, bool immediate)
    {
        if (immediate)
        {
            canvas.localScale = targetScale;
            yield break;
        }

        Vector3 startScale = canvas.localScale;
        float elapsed = 0f;
        float duration = 1f / speed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // 使用 SmoothStep 讓動畫更平滑
            t = t * t * (3f - 2f * t);
            canvas.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        canvas.localScale = targetScale;
    }

    // 同時執行位置和縮放動畫
    private static IEnumerator AnimateCameraTransform(RectTransform canvas, Vector2 targetPosition, Vector3 targetScale, float speed)
    {
        Vector2 startPosition = canvas.anchoredPosition;
        Vector3 startScale = canvas.localScale;
        float elapsed = 0f;
        float duration = 1f / speed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // 使用 SmoothStep 讓動畫更平滑
            t = t * t * (3f - 2f * t);

            canvas.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            canvas.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        canvas.anchoredPosition = targetPosition;
        canvas.localScale = targetScale;
    }

    // 設置背景安全縮放比例
    private static IEnumerator CameraSetBackgroundScale(string[] data)
    {
        float scale = 1.3f;

        var parameters = ConverDataToParameters(data);
        parameters.TryGetValue(PARAM_AMOUNT, out scale, defaultValue: 1.3f);

        // 更新安全縮放比例
        backgroundSafeScale = scale;

        RectTransform canvas = GetWorldCanvasTransform();
        if (canvas != null)
        {
            // 立即應用新的縮放比例
            canvas.localScale = originalScale * backgroundSafeScale;
            Debug.Log($"背景安全縮放比例已設置為: {backgroundSafeScale}");
        }

        yield return null;
    }
}
