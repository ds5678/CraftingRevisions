using MelonLoader;

namespace CraftingRevisions
{
	internal class CraftingRevisionsMod : MelonMod
    {
		public override void OnApplicationStart()
		{
			Settings.instance.AddToModSettings("Crafting Revisions");
		}

		public override void OnApplicationLateStart()
		{
			BlueprintManager.RegisterPendingBlueprints();
		}
	}
}
