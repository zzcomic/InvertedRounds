using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using TMPro;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace InvertedRounds
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willuwontu.rounds.managers", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.zzcomic.ROUNDS.InvertedRounds", "Inverted_Rounds", "1.0.0")]
    [BepInProcess("Rounds.exe")]
    public class InvertedRoundsPlugin : BaseUnityPlugin
    {

        public static ConfigEntry<int> CursesPerWinConfig;
        public static int CursesPerWin;
        private void Awake()
        {
            CursesPerWinConfig = Config.Bind("SufferingFromSuccess", "Curses per win", 0, "Determines the number of curses the winner of a round will receive");

            var harmony = new Harmony("com.zzcomic.ROUNDS");
            harmony.PatchAll();
        }

        private void Start()
        {
            CursesPerWin = CursesPerWinConfig.Value;

            Unbound.RegisterCredits("Suffering From Success", new string[] { "ZZcomic" }, new string[] { "Youtube", "Github" }, new string[] { "https://www.youtube.com/c/ZZcomic", "https://github.com/zzcomic/InvertedRounds" });

            Unbound.RegisterMenu("Suffering From Success", () => { }, this.NewGUI, null, false);

            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, (gm) => AddCurseToWinnerPatch.patch());
            GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => AddCurseToWinnerPatch.reset());
        }

        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText("Suffering From Success Options", menu, out TextMeshProUGUI _, 45);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            void CursesChanged(float val)
            {
                InvertedRoundsPlugin.CursesPerWinConfig.Value = UnityEngine.Mathf.RoundToInt(val);
                InvertedRoundsPlugin.CursesPerWin = InvertedRoundsPlugin.CursesPerWinConfig.Value;
            }
            MenuHandler.CreateSlider("Curses Per win", menu, 1, 0f, 5f, InvertedRoundsPlugin.CursesPerWinConfig.Value, CursesChanged, out UnityEngine.UI.Slider CurseSlider, true);
        }
    }
}
