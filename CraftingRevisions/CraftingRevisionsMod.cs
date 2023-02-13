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
			BlueprintManager.RegisterPendingBlueprints();
		}

	}
}
