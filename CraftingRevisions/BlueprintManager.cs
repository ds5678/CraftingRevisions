
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

		internal static void ValidateJsonBlueprint(string json)
		{
			ModUserBlueprintData testBluePrint = ModUserBlueprintData.ParseFromJson(json);
			testBluePrint.Validate();
		}

	}

	
}