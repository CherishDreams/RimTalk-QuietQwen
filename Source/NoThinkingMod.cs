using UnityEngine;
using Verse;

namespace RimTalkNoThinking
{
    public class NoThinkingMod : Mod
    {
        public static NoThinkingSettings Settings;
        private Vector2 _scrollPosition = Vector2.zero;

        public NoThinkingMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<NoThinkingSettings>();
        }

        public override string SettingsCategory()
        {
            return "RimTalk No Thinking";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Calculate content height off-screen
            GUI.BeginGroup(new Rect(-9999, -9999, 1, 1));
            Text.Font = GameFont.Small;
            var calcListing = new Listing_Standard();
            Rect calcRect = new Rect(0, 0, inRect.width - 16f, 9999f);
            calcListing.Begin(calcRect);
            DrawSettingsContent(calcListing);
            float contentHeight = calcListing.CurHeight;
            calcListing.End();
            GUI.EndGroup();

            // Real draw with scroll view
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, contentHeight);
            _scrollPosition = GUI.BeginScrollView(inRect, _scrollPosition, viewRect);

            Text.Font = GameFont.Small;
            var listing = new Listing_Standard();
            listing.Begin(viewRect);
            DrawSettingsContent(listing);
            listing.End();

            GUI.EndScrollView();
        }

        private void DrawSettingsContent(Listing_Standard listing)
        {
            listing.Label("禁用阿里云 Qwen 模型的思考功能");
            listing.Gap(5f);

            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label("启用后，将自动向 aliyuncs.com 域名的 API 请求注入 'enable_thinking = false' 参数。");
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap(10f);
            listing.CheckboxLabeled("启用禁用思考注入", ref Settings.Enabled);

            listing.Gap(10f);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label("此 Mod 仅影响发往 aliyuncs.com 的请求，与 RimTalk 内置的思考控制设置独立运作。");
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
        }
    }

    public class NoThinkingSettings : ModSettings
    {
        public bool Enabled = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Enabled, "enabled", true);
        }
    }
}
