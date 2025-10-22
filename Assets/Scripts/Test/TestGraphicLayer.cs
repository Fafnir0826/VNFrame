using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class TestGraphicLayer : MonoBehaviour
{

    void Start()
    {

        StartCoroutine(Running());

    }
    IEnumerator Running()
    {
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        yield return new WaitForSeconds(1);
        Texture blendTex = Resources.Load<Texture>("Graphic/Transition Effects/hurricane");

        layer.SetTexture("Graphic/BG Images/2", blendingTexture: blendTex);
    }


}
