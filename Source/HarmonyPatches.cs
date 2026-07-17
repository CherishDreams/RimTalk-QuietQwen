using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.Networking;
using Verse;

namespace RimTalkNoThinking
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("community.rimtalk.nothinking");
            harmony.PatchAll();
            Log.Message("[RimTalk No Thinking] Harmony patches applied.");
        }
    }

    /// <summary>
    /// Patches UnityWebRequest.SendWebRequest to inject enable_thinking = false
    /// for requests sent to aliyuncs.com domains.
    /// </summary>
    [HarmonyPatch(typeof(UnityWebRequest), nameof(UnityWebRequest.SendWebRequest))]
    public static class Patch_UnityWebRequest_SendWebRequest
    {
        /// <summary>
        /// Prefix patch that modifies the upload data before the request is sent.
        /// Only affects POST requests to aliyuncs.com domains.
        /// </summary>
        public static void Prefix(UnityWebRequest __instance)
        {
            // Check if the mod is enabled
            if (NoThinkingMod.Settings == null || !NoThinkingMod.Settings.Enabled)
                return;

            // Only process POST requests
            if (!string.Equals(__instance.method, "POST", StringComparison.OrdinalIgnoreCase))
                return;

            // Check if the URL contains aliyuncs.com
            string url = __instance.url;
            if (string.IsNullOrEmpty(url))
                return;

            if (url.IndexOf("aliyuncs.com", StringComparison.OrdinalIgnoreCase) < 0)
                return;

            // Get the upload handler
            var uploadHandler = __instance.uploadHandler;
            if (uploadHandler == null)
                return;

            try
            {
                // Read data from UploadHandlerRaw.data (public property)
                byte[] originalData = uploadHandler.data;
                if (originalData == null || originalData.Length == 0)
                    return;

                string json = Encoding.UTF8.GetString(originalData);

                // Check if enable_thinking is already present (don't override user settings)
                if (json.Contains("\"enable_thinking\""))
                {
                    Log.Message("[RimTalk No Thinking] enable_thinking already present, skipping.");
                    return;
                }

                // Inject enable_thinking = false at top level
                int lastBrace = json.LastIndexOf('}');
                if (lastBrace < 0)
                    return;

                string injection = ",\"enable_thinking\":false";
                string modifiedJson = json.Substring(0, lastBrace) + injection + json.Substring(lastBrace);

                // Replace the upload handler with modified data
                byte[] modifiedData = Encoding.UTF8.GetBytes(modifiedJson);
                var newHandler = new UploadHandlerRaw(modifiedData);
                newHandler.contentType = uploadHandler.contentType;
                __instance.uploadHandler = newHandler;

                Log.Message($"[RimTalk No Thinking] Injected enable_thinking=false into request to {url}");
            }
            catch (Exception ex)
            {
                Log.Warning($"[RimTalk No Thinking] Failed to inject: {ex.Message}");
            }
        }
    }
}
