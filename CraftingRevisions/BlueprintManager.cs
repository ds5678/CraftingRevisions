using Il2Cpp;

namespace CraftingRevisions
{
	public static class BlueprintManager
	{
		internal static HashSet<string> jsonUserBlueprints = new();

		public static void AddBlueprintFromJson(string text, bool validateEarly)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("Blueprint text contains no information", nameof(text));
			}

			// add the blueprint to the HasSet
			jsonUserBlueprints.Add(text);
		}

		public static Il2CppAK.Wwise.Event? MakeAudioEvent(string eventName)
		{
			if (eventName == null)
			{
				return null;
			}
			uint eventId = AkSoundEngine.GetIDFromString(eventName);
			if (eventId <=0 || eventId  >= 4294967295)
			{
				return null;
			}

			Il2CppAK.Wwise.Event newEvent = new();
			newEvent.WwiseObjectReference = new WwiseEventReference();
			newEvent.WwiseObjectReference.objectName = eventName;
			newEvent.WwiseObjectReference.id = eventId;
			return newEvent;
		}
	}
}