using MelonLoader;

namespace CraftingRevisions
{
	internal class CraftingRevisionsMod : MelonMod
	{
		public override void OnInitializeMelon()
		{
			Settings.instance.AddToModSettings("Crafting Revisions");
		}

		public override void OnApplicationLateStart()
		{
#warning - All of this to be removed before merge & release
			// Load a couple of TEST user json blueprints
			// (blueprint_1 is the old format | blueprint_2 is the new UserBlueprintData format)
			BlueprintManager.AddBlueprintFromJson(TestBlueprints.blueprint_1, false);
			BlueprintManager.AddBlueprintFromJson(TestBlueprints.blueprint_2, true);

		}

	}
}
