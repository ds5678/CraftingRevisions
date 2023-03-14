using CraftingRevisions.Exceptions;
using Il2Cpp;
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
		public string? RequiredTool = null;
		public List<string>? OptionalTools = new();
		public string CraftedResult;
		public int CraftedResultCount = 0;
		public int DurationMinutes = 0;
		public string? CraftingAudio = null;
		public float KeroseneLitersRequired = 0;
		public float GunpowderKGRequired = 0;
		public bool RequiresLight = false;
		public bool Locked = false;
		public bool IgnoreLockInSurvival = true;
		public bool AppearsInStoryOnly = false;
		public bool AppearsInSurvivalOnly = false;
		public SkillType AppliedSkill = SkillType.None;
		public SkillType ImprovedSkill = SkillType.None;
		public CraftingLocation RequiredCraftingLocation = CraftingLocation.Anywhere;
		public bool RequiresLitFire = false;
		public bool CanIncreaseRepairSkill = false;

		#region Json
		public static ModUserBlueprintData ParseFromJson(string jsonText)
		{
			return JSON.Load(jsonText).Make<ModUserBlueprintData>();
		}
		#endregion

		public string GetName() => Name ?? "";

		#region validation
		internal void Validate()
		{
			try
			{
				var exceptions = new List<Exception>();

				if (RequiredGear == null)
					exceptions.Add(new InvalidBlueprintException($"\nRequiredGear must be set on '{GetName()}'"));

				if (RequiredGear != null && RequiredGear.Count <= 0)
					exceptions.Add(new InvalidBlueprintException($"\nRequiredGear must not be empty on '{GetName()}'"));

				if (KeroseneLitersRequired < 0)
					exceptions.Add(new InvalidBlueprintException($"\nKeroseneLitersRequired cannot be negative on '{GetName()}'"));

				if (GunpowderKGRequired < 0)
					exceptions.Add(new InvalidBlueprintException($"\nGunpowderKGRequired cannot be negative on '{GetName()}'"));

				if (string.IsNullOrWhiteSpace(CraftedResult))
					exceptions.Add(new InvalidBlueprintException($"\nCraftedResult must be set on '{GetName()}'"));

				if (CraftedResultCount < 1)
					exceptions.Add(new InvalidBlueprintException($"\nCraftedResultCount cannot be less than 1 on '{GetName()}'"));

				if (DurationMinutes < 0)
					exceptions.Add(new InvalidBlueprintException($"\nDurationMinutes cannot be negative on '{GetName()}'"));

				if (!IsValidEnumValue(RequiredCraftingLocation))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{GetName()}'"));

				if (!IsValidEnumValue(AppliedSkill))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {AppliedSkill} for AppliedSkill on '{GetName()}'"));

				if (!IsValidEnumValue(ImprovedSkill))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {ImprovedSkill} for RequiredCraftingLocation on '{GetName()}'"));

				if (RequiredGear != null && RequiredGear.Count > 0)
				{
					int i = 0;
					foreach (RequiredGearItem RequiredGearItem in RequiredGear)
					{
						if (RequiredGearItem.Item == null)
							exceptions.Add(new InvalidBlueprintException($"\nRequiredGearItem[{i}].Item must be set on '{GetName()}'"));
						if (RequiredGearItem.Count < 1)
							exceptions.Add(new InvalidBlueprintException($"\nRequiredGearItem[{i}].Count cannot be less than 1 on '{GetName()}'"));
						i++;
					}
				}
				throw new AggregateException("Aggregate Exception Message", exceptions);

			}
			catch (AggregateException ae)
			{
				MelonLoader.MelonLogger.Error(ae.Message);
			}
		}
		#endregion

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
		public string? Item = null;
		/// <summary>
		/// Count of how many are required
		/// </summary>
		public int Count = 0;
	}
}


