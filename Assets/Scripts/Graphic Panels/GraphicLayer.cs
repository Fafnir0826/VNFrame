using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphicLayer
{
    public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
    public int layerDepth = 0;
    public Transform panel;

    public GraphicObject currentGraphic = null;
    private List<GraphicObject> oldGraphics = new List<GraphicObject>();


    public Coroutine SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null, bool immediate = false)
    {
        Texture tex = Resources.Load<Texture>(filePath);

        if (tex == null) // 根據錯誤日誌判斷，這裡的條件應該是判斷是否載入失敗
        {
            Debug.LogError($"Could not load graphic texture from path '{filePath}'. Please ensure it exists within Resources!");
            return null;
        }

        return SetTexture(tex, transitionSpeed, blendingTexture, filePath);
    }

    public Coroutine SetTexture(Texture tex, float transitionSpeed = 1f, Texture blendingTexture = null, string filePath = "")
    {
        return CreateGraphic(tex, transitionSpeed, filePath, blendingTexture: blendingTexture);
    }
    private Coroutine CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null, bool immediate = false)
    {
        GraphicObject newGraphic = null;

        if (graphicData is Texture)
            newGraphic = new GraphicObject(this, filePath, graphicData as Texture);

        if (currentGraphic != null && !oldGraphics.Contains(currentGraphic))
            oldGraphics.Add(currentGraphic);

        currentGraphic = newGraphic;
        return currentGraphic.FadeIn(transitionSpeed, blendingTexture);
    }
    public void DestroyOldGraphic()
    {
        foreach (var g in oldGraphics)
            Object.Destroy(g.renderer.gameObject);

        oldGraphics.Clear();
    }
    public void Clear()
    {
        if (currentGraphic != null)
            currentGraphic.FadeOut();

        foreach (var g in oldGraphics)
            g.FadeOut();
    }
}
