using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;

namespace BeatSouls_Translation
{
    [BepInPlugin("maijz.TranslationPlugin", "Translation Plugin", "1.0.0.0")]
    [BepInProcess("BeatSouls.exe")]
    public class TranslationPlugin : BaseUnityPlugin
    {
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            UnityEngine.Debug.Log(">>Translation Plugin awake...");
        }

        void Start()
        {
            UnityEngine.Debug.Log(">>Translation Plugin start...");
            Harmony.CreateAndPatchAll(typeof(AppTextPatcher));
        }



    }
}
