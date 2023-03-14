using HarmonyLib;
using Il2Cpp;
using System.Reflection;
using UnityEngine;

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
			// loop over the items
			foreach (string jsonUserBlueprint in BlueprintManager.jsonUserBlueprints)
			{

				// load the blueprint into the game
				bool loaded = __instance.LoadUserBlueprint(jsonUserBlueprint);

				if (!loaded)
				{
					// validate the blueprint
					BlueprintManager.ValidateJsonBlueprint(jsonUserBlueprint);
				}
			}
		}
	}

	// These patches are to work around the fact that "UserBlueprintData.MakeRuntimeWwiseEvent" appears to be currently broken or unfinished
	[HarmonyPatch(typeof(Il2CppTLD.Gear.UserBlueprintData), nameof(Il2CppTLD.Gear.UserBlueprintData.MakeRuntimeWwiseEvent), new Type[] { typeof(string) })]
	internal class UserBlueprintData_MakeRuntimeWwiseEvent
	{
		private static void Prefix(ref bool __runOriginal)
		{
			__runOriginal = false;
		}
		private static void Postfix(string eventName, ref bool __runOriginal, ref Il2CppAK.Wwise.Event? __result)
		{
			__result = MakeAudioEvent(eventName);
			__runOriginal = false;
		}
		private static Il2CppAK.Wwise.Event? MakeAudioEvent(string? eventName)
		{
			Il2CppAK.Wwise.Event emptyEvent = new();
			emptyEvent.WwiseObjectReference = ScriptableObject.CreateInstance<WwiseEventReference>();

			if (eventName == null || eventName == "")
			{
				return emptyEvent;
			}
			uint eventId = GetAKEventIdFromString(eventName);
			if (eventId == 0U)
			{
				return emptyEvent;
			}

			Il2CppAK.Wwise.Event newEvent = new();
			newEvent.WwiseObjectReference = ScriptableObject.CreateInstance<WwiseEventReference>();
			newEvent.WwiseObjectReference.objectName = eventName;
			newEvent.WwiseObjectReference.id = eventId;
			return newEvent;
		}
		private static uint GetAKEventIdFromString(string eventName)
		{
			Type type = typeof(Il2CppAK.EVENTS);
			foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance))
			{
				if (prop.Name.ToLower() == eventName.ToLower())
				{
					return Convert.ToUInt32(prop.GetValue(null)?.ToString());
				}
			}
			return 0U;
		}
	}
}
