using CraftingRevisions.Exceptions;
using Il2Cpp;
using System.Text.Json;

namespace CraftingRevisions
{
	public class ModUserBlueprintData
	{
		/// <summary>
		/// optional name, used for debugging
		/// </summary>
		public string? Name { get; set; } = null;
		public List<ModRequiredGearItem> RequiredGear { get; set; } = new();
		public string? RequiredTool { get; set; } = null;
		public List<string>? OptionalTools { get; set; } = new();
		public string? CraftedResult { get; set; } = null;
		public int CraftedResultCount { get; set; } = 0;
		public int DurationMinutes { get; set; } = 0;
		public string? CraftingAudio { get; set; } = null;
		public float KeroseneLitersRequired { get; set; } = 0;
		public float GunpowderKGRequired { get; set; } = 0;
		public bool RequiresLight { get; set; } = false;
		public bool Locked { get; set; } = false;
		public bool IgnoreLockInSurvival { get; set; } = true;
		public bool AppearsInStoryOnly { get; set; } = false;
		public bool AppearsInSurvivalOnly { get; set; } = false;
		public SkillType AppliedSkill { get; set; } = SkillType.None;
		public SkillType ImprovedSkill { get; set; } = SkillType.None;
		public CraftingLocation RequiredCraftingLocation { get; set; } = CraftingLocation.Anywhere;
		public bool RequiresLitFire { get; set; } = false;
		public bool CanIncreaseRepairSkill { get; set; } = false;

		#region Json
		public static ModUserBlueprintData ParseFromJson(string jsonText)
		{
			return JsonSerializer.Deserialize<ModUserBlueprintData>(jsonText);
		}
		#endregion

		#region validation
		internal void Validate()
		{
			try
			{
				var exceptions = new List<Exception>();

				if (RequiredGear == null)
					exceptions.Add(new InvalidBlueprintException($"\nRequiredGear must be set on '{Name}'"));

				if (RequiredGear != null && RequiredGear.Count <= 0)
					exceptions.Add(new InvalidBlueprintException($"\nRequiredGear must not be empty on '{Name}'"));

				if (KeroseneLitersRequired < 0)
					exceptions.Add(new InvalidBlueprintException($"\nKeroseneLitersRequired cannot be negative on '{Name}'"));

				if (GunpowderKGRequired < 0)
					exceptions.Add(new InvalidBlueprintException($"\nGunpowderKGRequired cannot be negative on '{Name}'"));

				if (string.IsNullOrWhiteSpace(CraftedResult))
					exceptions.Add(new InvalidBlueprintException($"\nCraftedResult must be set on '{Name}'"));

				if (CraftedResultCount < 1)
					exceptions.Add(new InvalidBlueprintException($"\nCraftedResultCount cannot be less than 1 on '{Name}'"));

				if (DurationMinutes < 0)
					exceptions.Add(new InvalidBlueprintException($"\nDurationMinutes cannot be negative on '{Name}'"));

				if (!IsValidEnumValue(RequiredCraftingLocation))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{Name}'"));

				if (!IsValidEnumValue(AppliedSkill))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {AppliedSkill} for AppliedSkill on '{Name}'"));

				if (!IsValidEnumValue(ImprovedSkill))
					exceptions.Add(new InvalidBlueprintException($"\nUnsupported value {ImprovedSkill} for RequiredCraftingLocation on '{Name}'"));

				if (RequiredGear != null && RequiredGear.Count > 0)
				{
					int i = 0;
					foreach (ModRequiredGearItem RequiredGearItem in RequiredGear)
					{
						if (RequiredGearItem.Item == null)
							exceptions.Add(new InvalidBlueprintException($"\nRequiredGearItem[{i}].Item must be set on '{Name}'"));
						if (RequiredGearItem.Count < 1)
							exceptions.Add(new InvalidBlueprintException($"\nRequiredGearItem[{i}].Count cannot be less than 1 on '{Name}'"));
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

	public class ModRequiredGearItem
	{
		/// <summary>
		/// String value of the gear item
		/// </summary>
		public string? Item { get; set; } = null;
		/// <summary>
		/// Count of how many are required
		/// </summary>
		public int Count { get; set; } = 0;
	}
}




