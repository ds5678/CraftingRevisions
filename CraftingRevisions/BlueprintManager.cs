using CraftingRevisions.Exceptions;

using Il2Cpp;
using UnityEngine;
using System.Reflection;

namespace CraftingRevisions
{
	public static class BlueprintManager
	{
		internal static HashSet<string> jsonUserBlueprints = new();

		public static void AddBlueprintFromJson(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("Blueprint text contains no information", nameof(text));
			}

			// add the blueprint to the HasSet
			jsonUserBlueprints.Add(text);
		}

		internal static Il2CppAK.Wwise.Event? MakeAudioEvent(string eventName)
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

		internal static uint GetAKEventIdFromString(string eventName)
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

		internal static void ValidateJsonBlueprint(string json)
		{
			CraftingRevisions.UserBlueprintData testBluePrint = CraftingRevisions.UserBlueprintData.ParseFromJson(json);
			testBluePrint.Validate();
		}

	}

	
}