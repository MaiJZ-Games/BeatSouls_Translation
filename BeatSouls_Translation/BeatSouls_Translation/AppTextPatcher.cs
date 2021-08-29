using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Mono.Cecil;
using System.Reflection.Emit;

namespace BeatSouls_Translation
{
    public static class AppTextPatcher
    {
        // List of assemblies to patch
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        // Called before any patching occurs
        public static void Initialize()
        {
            //UnityEngine.Debug.Log("Translation Patcher Initialize...");
            System.Console.WriteLine("Translation Patcher Initialize...");
        }

        // Called after preloader has patched all assemblies and loaded them in
        // At this point it is fine to reference patched assemblies
        public static void Finish() { }

        // Patches the assemblies
        public static void Patch(AssemblyDefinition assembly)
        {
            if (assembly.Name.Name == "Assembly-CSharp")
            {
                // The assembly is Assembly-CSharp.dll
            }
            else if (assembly.Name.Name == "UnityEngine")
            {
                // The assembly is UnityEngine.dll
            }
        }


        /**
         * 加载默认文本为简体中文
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AppText), "LoadString")]
        public static bool LoadStringPatch(AppText __instance)
        {
            //以__instance实例创建Traverse      挖掘字段 m_Language    取 float 类型值
            //int m_Language = (int)Traverse.Create(__instance).Field("m_Language").GetValue();
            //System.Console.WriteLine("m_Language: " + m_Language);

            var m_AppTexts = GameText.GetAppTexts();

            //以__instance实例创建Traverse，挖掘字段 m_AppTexts，为 m_AppTexts 赋值
            Traverse.Create(__instance).Field("m_AppTexts").SetValue(m_AppTexts);


            return false; //拦截原方法不再执行
            //return true; //继续执行原方法
        }

        /**
         * 选择简体中文版 精灵图片
         */
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(IngameView), "StartUpView")]
        public static IEnumerable<CodeInstruction> StartUpViewPatch(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            System.Console.WriteLine(">>StartUpViewPatch: " + codes[114].operand);
            //codes[114].opcode = OpCodes.Ldstr;
            codes[114].operand = "_ch_kan";
            return instructions;
        }


    }
}