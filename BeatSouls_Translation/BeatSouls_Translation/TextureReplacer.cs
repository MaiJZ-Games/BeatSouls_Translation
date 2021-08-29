using System;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MozaicFree
{
    [BepInPlugin(GUID: "TextureReplacer", Name: "Kanro.TextureReplacer", Version: "0.1")]
    class TextureReplacer : BaseUnityPlugin
    {
        public string ImagesPath { get; } = @"BepInEx/images";
        private readonly Dictionary<string, string> images = new Dictionary<string, string>();
        public void Start()
        {
            Console.WriteLine(">>TextureReplacer start...");
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            GetImages();
            ReplaceImages();
            ReplaceSpriteImages();
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            GetImages();
            ReplaceImages();
        }

        private void GetImages()
        {
            if (!Directory.Exists(ImagesPath))
            {
                return;
            }

            try
            {
                Logger.Log(LogLevel.Debug, "Fetching Images...");

                foreach (string file in Directory.GetFiles(ImagesPath))
                {
                    if (Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(">>>>" + file);
                        images.Add(Path.GetFileNameWithoutExtension(file), file);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex.ToString());
            }
        }

        private void ReplaceImages()
        {
            UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));

            foreach (UnityEngine.Object t in textures)
            {
                try
                {
                    Texture2D tex = (Texture2D)t;
                    //Console.WriteLine(">>>>Texture2D Name:" + t.name);
                    if (images.ContainsKey(t.name))
                    {
                        Logger.Log(LogLevel.Debug, "Replacing Image: " + tex.name);
                        byte[] fileData = File.ReadAllBytes(images[t.name]);
                        tex.LoadRawTextureData(fileData);
                        tex.Apply();
                    }
                }
                catch (Exception ex)
                {

                    Logger.Log(LogLevel.Error, ex.ToString());
                }
            }

            Resources.UnloadUnusedAssets();
        }

        private void ReplaceSpriteImages()
        {
            UnityEngine.Object[] objects = Resources.FindObjectsOfTypeAll(typeof(Sprite));
            foreach (UnityEngine.Sprite obj in objects)
            {
                Console.WriteLine(">>>>Sprite Name:" + obj.name);
                Console.WriteLine("    Texture Name:" + obj.texture.name);
                try
                {
                        Sprite spr = obj;
                        var sprName = spr.name;
                        if (images.ContainsKey(spr.name))
                        {
                            Logger.Log(LogLevel.Debug, "Replacing Image: " + sprName);

                            //byte[] fileData = File.ReadAllBytes(images[sprName]);
                            //Texture2D tex = duplicateTexture2(spr.texture, fileData);

                            //var newSpr = Resources.Load<Sprite>(images[sprName]);
                            //sr.sprite = newSpr;
                        }
                }
                catch (Exception ex)
                {

                    Logger.Log(LogLevel.Error, ex.ToString());
                }
            }

            Resources.UnloadUnusedAssets();
        }


        private void ReplaceSpriteImages2()
        {
            //UnityEngine.Object[] sprites = Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour));
            UnityEngine.Object[] objects = Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour));
            foreach (UnityEngine.MonoBehaviour obj in objects)
            {
                //Console.WriteLine(">>>>MonoBehaviour Name:" + obj.name);
                try
                {
                    var sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Sprite spr = sr.sprite;
                        var sprName = spr.name;
                        Console.WriteLine(">>>>Sprite Name:" + sprName);
                        if (images.ContainsKey(spr.name))
                        {
                            Logger.Log(LogLevel.Debug, "Replacing Image: " + sprName);

                            //byte[] fileData = File.ReadAllBytes(images[sprName]);
                            //Texture2D tex = duplicateTexture2(spr.texture, fileData);


                            var newSpr = Resources.Load<Sprite>(images[sprName]);
                            sr.sprite = newSpr;
                        }
                    }
                }
                catch (Exception ex)
                {

                    Logger.Log(LogLevel.Error, ex.ToString());
                }
            }


            Resources.UnloadUnusedAssets();
        }




        Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        Texture2D duplicateTexture2(Texture2D source)
        {
            byte[] pix = source.GetRawTextureData();
            Texture2D readableText = new Texture2D(source.width, source.height, source.format, false);
            readableText.LoadRawTextureData(pix);
            readableText.Apply();
            return readableText;
        }
        Texture2D duplicateTexture2(Texture2D source, byte[] pix)
        {
            Texture2D readableText = new Texture2D(source.width, source.height, source.format, false);
            readableText.LoadRawTextureData(pix);
            readableText.Apply();
            return readableText;
        }

    }

}
