using HarmonyLib;
using Il2Cpp;

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
				foreach (string jsonUserBlueprint in CraftingRevisions.BlueprintManager.jsonUserBlueprints)
				{

					// load the blueprint into the game
					bool loaded = __instance.LoadUserBlueprint(jsonUserBlueprint);

					if (loaded == false)
					{
						// WIP
						//BlueprintManager.ValidateJsonBlueprint(jsonUserBlueprint);
					}
				}
			}
		}
	}



	// So these patches are to work around the fact that "UserBlueprintData.MakeRuntimeWwiseEvent" appears to be broken or unfinished
	[HarmonyPatch(typeof(Il2CppTLD.Gear.UserBlueprintData), nameof(Il2CppTLD.Gear.UserBlueprintData.MakeRuntimeWwiseEvent), new Type[] { typeof(string) })]
	internal class UserBlueprintData_MakeRuntimeWwiseEvent
	{
		private static void Prefix(string eventName, ref bool __runOriginal)
		{
			__runOriginal = false;
		}
		private static void Postfix(string eventName, ref bool __runOriginal, ref Il2CppAK.Wwise.Event? __result)
		{
			__result = BlueprintManager.MakeAudioEvent(eventName);
			__runOriginal = false;
		}
	}

}
