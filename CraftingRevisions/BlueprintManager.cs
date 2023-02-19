namespace CraftingRevisions
{
	public static class BlueprintManager
	{
		private static readonly List<ModBlueprintData> pendingBlueprints = new List<ModBlueprintData>();
		private static bool registeredPendingBlueprints;

		internal static HashSet<string> jsonUserBlueprints = new();

		internal static void RegisterPendingBlueprints()
		{
			foreach (var blueprint in pendingBlueprints)
			{
				BlueprintMapper.RegisterBlueprint(blueprint);
			}
			pendingBlueprints.Clear();
			registeredPendingBlueprints = true;
		}

		public static void AddBlueprint(ModBlueprintData blueprint, bool validateEarly)
		{
			if (blueprint == null)
				throw new ArgumentNullException(nameof(blueprint));

			blueprint.PreValidate();

			if (registeredPendingBlueprints || validateEarly)
				BlueprintMapper.RegisterBlueprint(blueprint);
			else
				pendingBlueprints.Add(blueprint);
		}

		public static void AddBlueprintFromJson(string text, bool validateEarly)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("Blueprint text contains no information", nameof(text));
			}

			// add the blueprint to the HasSet
			jsonUserBlueprints.Add(text);
		}
	}
}