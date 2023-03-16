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

	// These patches are to work around the fact that "UserBlueprintData.MakeRuntimeWwiseEvent" appears to be currently broken or unfinished
	[HarmonyPatch(typeof(Il2CppTLD.Gear.UserBlueprintData), nameof(Il2CppTLD.Gear.UserBlueprintData.MakeRuntimeWwiseEvent), new Type[] { typeof(string) })]
	internal class UserBlueprintData_MakeRuntimeWwiseEvent
	{
		private static readonly Dictionary<string, uint> eventIds = new();

		private static void Prefix(string eventName, ref bool __runOriginal, ref Il2CppAK.Wwise.Event? __result)
		{
			__result = MakeAudioEvent(eventName);
			__runOriginal = false;
		}
		private static Il2CppAK.Wwise.Event? MakeAudioEvent(string? eventName)
		{
			if (string.IsNullOrEmpty(eventName) || GetAKEventIdFromString(eventName) == 0)
			{
				Il2CppAK.Wwise.Event emptyEvent = new();
				emptyEvent.WwiseObjectReference = ScriptableObject.CreateInstance<WwiseEventReference>();
				emptyEvent.WwiseObjectReference.objectName = "NULL_WWISEEVENT";
				emptyEvent.WwiseObjectReference.id = GetAKEventIdFromString("NULL_WWISEEVENT");
				return emptyEvent;
			}

			Il2CppAK.Wwise.Event newEvent = new();
			newEvent.WwiseObjectReference = ScriptableObject.CreateInstance<WwiseEventReference>();
			newEvent.WwiseObjectReference.objectName = eventName;
			newEvent.WwiseObjectReference.id = GetAKEventIdFromString(eventName);
			return newEvent;
		}
		private static uint GetAKEventIdFromString(string eventName)
		{
			if (eventIds.Count == 0)
			{
				Type type = typeof(Il2CppAK.EVENTS);
				foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public))
				{
					string key = prop.Name.ToLowerInvariant();
					uint value = (uint)prop.GetValue(null)!;
					eventIds.Add(key, value);
				}
			}

			eventIds.TryGetValue(eventName.ToLowerInvariant(), out uint id);
			return id;
		}
	}
}
