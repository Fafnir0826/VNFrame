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
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
        database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
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
        float transitionSpeed = 0;
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

        if (layer == -1)
            panel.Clear(transitionSpeed, blendTex, immediate);
        else
        {
            GraphicLayer graphicLayer = panel.GetLayer(layer);
            if (graphicLayer == null)
            {
                Debug.LogError("can not clear layer");
            }

            graphicLayer.Clear(transitionSpeed,blendTex,immediate);
        }


    }
    private static string GetPathToGraphic(string defaultPath, string graphicName)
    {
        if (graphicName.StartsWith(HOME_DIRECTORY_SYMBOL))
            return graphicName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        return defaultPath + graphicName;
    }
}
