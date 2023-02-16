using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace CraftingRevisions.Patches
{
	// load the blueprints when the crafting panel is initialized
	// (needed to be this late otherwise we have no instance of BlueprintManager)
	[HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.Initialize))]
	internal class Panel_Crafting_Initialize
	{
		private static void Postfix()
		{
			// call TLD LoadAllUserBlueprints
			InterfaceManager.m_Instance.m_BlueprintManager.LoadAllUserBlueprints();
		}
	}


	// patched into postfix BlueprintManager.LoadAllUserBlueprints
	// (BlueprintManager.RemoveUserBlueprints is not guaranteed to get called)
	[HarmonyPatch(typeof(Il2CppTLD.Gear.BlueprintManager), nameof(Il2CppTLD.Gear.BlueprintManager.LoadAllUserBlueprints))]
	internal class BlueprintManager_LoadAllUserBlueprints_Postfix
	{
		private static void Postfix(Il2CppTLD.Gear.BlueprintManager __instance)
		{
			// check we have a HasSet and it has contents
			if (BlueprintManager.jsonUserBlueprints != null && BlueprintManager.jsonUserBlueprints.Count > 0)
			{
				// loop over the items
				foreach (string jsonUserBlueprint in BlueprintManager.jsonUserBlueprints)
				{
					// load the blueprint into the game
					bool loaded = __instance.LoadUserBlueprint(jsonUserBlueprint);

#warning TODO - there appears to be no way to define an icon for this blueprint (UserBlueprintData still a WIP?)
#warning TODO - get CraftingAudio working, is a string but unclear what value it wants. (see UserBlueprintData.MakeRuntimeWwiseEvent)
#warning TODO - work on loading validation if it fails
#warning TODO - Should we attempt to detect & convert "old" format json ?

				}
			}
		}
	}
}
