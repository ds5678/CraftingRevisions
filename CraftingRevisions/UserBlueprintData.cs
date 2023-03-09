using CraftingRevisions.Exceptions;
using MelonLoader.TinyJSON;

namespace CraftingRevisions
{
	public class ModUserBlueprintData
	{
		/// <summary>
		/// optional name, used for debugging
		/// </summary>
		public string? Name = null;

		public List<RequiredGearItem> RequiredGear;

		// Token: 0x04007043 RID: 28739
		public string? RequiredTool = null;

		// Token: 0x04007044 RID: 28740
		public List<string>? OptionalTools = new();

		// Token: 0x04007045 RID: 28741
		public string CraftedResult;

		// Token: 0x04007046 RID: 28742
		public int CraftedResultCount = 1;

		// Token: 0x04007047 RID: 28743
		public int DurationMinutes = 0;

		// Token: 0x04007048 RID: 28744
		public string? CraftingAudio = null;

		// Token: 0x04007049 RID: 28745
		public float KeroseneLitersRequired = 0;

		// Token: 0x0400704A RID: 28746
		public float GunpowderKGRequired = 0;

		// Token: 0x0400704B RID: 28747
		public bool RequiresLight = false;

		// Token: 0x0400704C RID: 28748
		public bool Locked = false;

		// Token: 0x0400704D RID: 28749
		public bool IgnoreLockInSurvival = true;

		// Token: 0x0400704E RID: 28750
		public bool AppearsInStoryOnly = false;

		// Token: 0x0400704F RID: 28751
		public bool AppearsInSurvivalOnly = false;

		// Token: 0x04007050 RID: 28752
		public ModSkillType AppliedSkill = ModSkillType.None;

		// Token: 0x04007051 RID: 28753
		public ModSkillType ImprovedSkill = ModSkillType.None;

		// Token: 0x04007052 RID: 28754
		public ModCraftingLocation RequiredCraftingLocation = ModCraftingLocation.Anywhere;

		// Token: 0x04007053 RID: 28755
		public bool RequiresLitFire = false;

		// Token: 0x04007054 RID: 28756
		public bool CanIncreaseRepairSkill = false;

		#region Json
		public static ModUserBlueprintData ParseFromJson(string jsonText)
		{
			return JSON.Load(jsonText).Make<ModUserBlueprintData>();
		}
		#endregion

		public string GetName() => Name ?? "";

		internal void Validate()
		{
			if (RequiredGear == null)
				throw new InvalidBlueprintException($"RequiredGear must be set on '{GetName()}'");
			if (RequiredGear.Count <= 0)
				throw new InvalidBlueprintException($"RequiredGear must not be empty on '{GetName()}'");

			if (KeroseneLitersRequired < 0)
				throw new InvalidBlueprintException($"KeroseneLitersRequired cannot be negative on '{GetName()}'");

			if (GunpowderKGRequired < 0)
				throw new InvalidBlueprintException($"GunpowderKGRequired cannot be negative on '{GetName()}'");

			if (string.IsNullOrWhiteSpace(CraftedResult))
				throw new InvalidBlueprintException($"CraftedResult must be set on '{GetName()}'");

			if (CraftedResultCount < 1)
				throw new InvalidBlueprintException($"CraftedResultCount cannot be less than 1 on '{GetName()}'");

			if (DurationMinutes < 0)
				throw new InvalidBlueprintException($"DurationMinutes cannot be negative on '{GetName()}'");

			if (!IsValidEnumValue(RequiredCraftingLocation))
				throw new InvalidBlueprintException($"Unsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{GetName()}'");

			if (!IsValidEnumValue(AppliedSkill))
				throw new InvalidBlueprintException($"Unsupported value {AppliedSkill} for AppliedSkill on '{GetName()}'");

			if (!IsValidEnumValue(ImprovedSkill))
				throw new InvalidBlueprintException($"Unsupported value {ImprovedSkill} for RequiredCraftingLocation on '{GetName()}'");

			if (RequiredGear != null && RequiredGear.Count > 0)
			{
				int i = 0;
				foreach (RequiredGearItem RequiredGearItem in RequiredGear)
				{
					if (RequiredGearItem.Item == null)
						throw new InvalidBlueprintException($"RequiredGearItem[{i}].Item must be set on '{GetName()}'");
					if (RequiredGearItem.Count == 1)
						throw new InvalidBlueprintException($"RequiredGearItem[{i}].Count must be set on '{GetName()}'");
					if (RequiredGearItem.Count < 1)
						throw new InvalidBlueprintException($"RequiredGearItem[{i}].Count cannot be less than 1 on '{GetName()}'");
					i++;
				}
			}

		}

		private static T[] GetEnumValues<T>() where T : Enum
		{
			return (T[])(Enum.GetValues(typeof(T)));
		}

		private static bool IsValidEnumValue<T>(T value) where T : Enum
		{
			return GetEnumValues<T>().Contains(value);
		}

	}

	public class RequiredGearItem
	{
		/// <summary>
		/// String value of the gear item
		/// </summary>
		public string Item;
		/// <summary>
		/// Count of how many are required
		/// </summary>
		public int Count;
	}

	public enum ModSkillType
	{
		None = -1,
		Firestarting,
		CarcassHarvesting,
		IceFishing,
		Cooking,
		Rifle,
		Archery,
		ClothingRepair,
		ToolRepair,
		Revolver,
		Gunsmithing
	}

	public enum ModCraftingLocation
	{
		Anywhere,
		Workbench,
		Forge,
		AmmoWorkbench
	}
}




	//public const string blueprint_1 = """
	//		{
	//		    "Name": "blueprint_bearLeggings",
	//			"RequiredGear":[{"Item":"GEAR_BearHideDried","Count":1},{"Item":"GEAR_GutDried","Count":2}],
	//			"RequiredTool":"GEAR_SewingKit",
	//			"OptionalTools":["GEAR_HookAndLine"],
	//			"CraftedResult":"GEAR_BearskinLeggings",
	//			"CraftedResultCount":1,
	//			"DurationMinutes":900,
	//			"CraftingAudio":"Play_RepairingLeatherHide",
	//			"KeroseneLitersRequired":0,
	//			"GunpowderKGRequired":0,
	//			"RequiresLight":false,
	//			"Locked":false,
	//			"IgnoreLockInSurvival":true,
	//			"AppearsInStoryOnly":false,
	//			"AppearsInSurvivalOnly":false,
	//			"AppliedSkill":"None",
	//			"ImprovedSkill":"None",
	//			"RequiredCraftingLocation":"Workbench",
	//			"RequiresLight":false,
	//			"RequiresLitFire":false,
	//			"CanIncreaseRepairSkill":false

	//		}
	//		""";

	//public const string blueprint_2 = """
	//		{
	//		"RequiredGear":[{"Item":"GEAR_Cloth","Count":1}],
	//		"RequiredTool":"",
	//		"OptionalTools":[],
	//		"CraftedResult":"GEAR_Prybar",
	//		"CraftedResultCount":1,
	//		"DurationMinutes":10,
	//		"CraftingAudio":null,
	//		"KeroseneLitersRequired":0,
	//		"GunpowderKGRequired":0,
	//		"RequiresLight":false,
	//		"Locked":false,
	//		"IgnoreLockInSurvival":true,
	//		"AppearsInStoryOnly":false,
	//		"AppearsInSurvivalOnly":false,
	//		"AppliedSkill":"None",
	//		"ImprovedSkill":"None",
	//		"RequiredCraftingLocation":"Anywhere",
	//		"RequiresLight":false,
	//		"RequiresLitFire":false,
	//		"CanIncreaseRepairSkill":false
	//		}
	//		""";


