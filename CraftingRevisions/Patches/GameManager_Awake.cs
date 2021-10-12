using HarmonyLib;
using MelonLoader;

namespace CraftingRevisions.Patches
{
	[HarmonyPatch(typeof(GameManager), "Awake")]
	internal class GameManager_Awake
	{
		private static void Postfix()
		{
			try { BlueprintMapper.MapBlueprints(); }
			catch (System.Exception e)
			{
				MelonLogger.Error($"Blueprint Mapping failed: {e}");
			}
		}
	}
}
