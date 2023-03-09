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
		public int CraftedResultCount = 0;

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
			string exception = "";
			if (RequiredGear == null)
				exception += $"\nRequiredGear must be set on '{GetName()}'";

			if (RequiredGear != null && RequiredGear.Count <= 0)
				exception += $"\nRequiredGear must not be empty on '{GetName()}'";

			if (KeroseneLitersRequired < 0)
				exception += $"\nKeroseneLitersRequired cannot be negative on '{GetName()}'";

			if (GunpowderKGRequired < 0)
				exception += $"\nGunpowderKGRequired cannot be negative on '{GetName()}'";

			if (string.IsNullOrWhiteSpace(CraftedResult))
				exception += $"\nCraftedResult must be set on '{GetName()}'";

			if (CraftedResultCount < 1)
				exception += $"\nCraftedResultCount cannot be less than 1 on '{GetName()}'";

			if (DurationMinutes < 0)
				exception += $"\nDurationMinutes cannot be negative on '{GetName()}'";

			if (!IsValidEnumValue(RequiredCraftingLocation))
				exception += $"\nUnsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{GetName()}'";

			if (!IsValidEnumValue(AppliedSkill))
				exception += $"\nUnsupported value {AppliedSkill} for AppliedSkill on '{GetName()}'";

			if (!IsValidEnumValue(ImprovedSkill))
				exception += $"\nUnsupported value {ImprovedSkill} for RequiredCraftingLocation on '{GetName()}'";

			if (RequiredGear != null && RequiredGear.Count > 0)
			{
				int i = 0;
				foreach (RequiredGearItem RequiredGearItem in RequiredGear)
				{
					if (RequiredGearItem.Item == null)
						exception += $"\nRequiredGearItem[{i}].Item must be set on '{GetName()}'";
					if (RequiredGearItem.Count < 1)
						exception += $"\nRequiredGearItem[{i}].Count cannot be less than 1 on '{GetName()}'";
					i++;
				}
			}
			if (exception != null && exception != "")
			{
				throw new InvalidBlueprintException(exception);
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


