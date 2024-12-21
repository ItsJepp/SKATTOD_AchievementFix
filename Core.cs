using MelonLoader;
using HarmonyLib;
using UnityEngine;

[assembly: MelonInfo(typeof(SKATTOD_AchievementFix.Core), "SKATTOD_AchievementFix", "1.0.0", "Jepp", null)]
[assembly: MelonGame("Janius Digital", "Scoot Kaboom and the Tomb of Doom")]

namespace SKATTOD_AchievementFix
{
    [HarmonyPatch(typeof(Checkpoint), "OnTriggerEnter2D")]
    public class Core : MelonMod
    {
        private static void Prefix(Collider2D other, Checkpoint __instance, out bool __state)
        {
            // Only trigger for player colliders.
            // Matches check in the original code for triggering section title and achievement.
            __state = other.tag == "Player" && __instance.showSectionTitle && !__instance.hasHit;
        }

        private static void Postfix(Collider2D other, Checkpoint __instance, bool __state)
        {
            // Only trigger for player colliders and if the condition has been met.
            if(other.tag != "Player" || !__state) return;

            // Show section title.
            UIManager.instance.sectionTitleText.text = SectionManager.instance.sectionRef.sectionName;
            foreach(GameObject allSectionTitle in UIManager.instance.allSectionTitles)
                allSectionTitle.SetActive(false);
            UIManager.instance.allSectionTitles[SectionManager.instance.sectionRef.sectionTitleNumber].SetActive(true);
            UIManager.instance.sectionTitleAnim.SetTrigger("ShowSectionTitle");

            // Trigger achievement check.
            SteamAchievementTracker.instance.CheckAreaAchievement(__instance.areaForAchievement);
        }
    }
}