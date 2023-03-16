using Il2Cpp;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CraftingRevisions
{
	internal sealed class ModUserBlueprintData
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
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SkillType AppliedSkill { get; set; } = SkillType.None;
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SkillType ImprovedSkill { get; set; } = SkillType.None;
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public CraftingLocation RequiredCraftingLocation { get; set; } = CraftingLocation.Anywhere;
		public bool RequiresLitFire { get; set; } = false;
		public bool CanIncreaseRepairSkill { get; set; } = false;

		#region Json
		public static ModUserBlueprintData ParseFromJson(string jsonText)
		{
			return JsonSerializer.Deserialize<ModUserBlueprintData>(jsonText) ?? throw new ArgumentException("Could not parse blueprint data from the text.", nameof(jsonText));
		}
		#endregion

		#region validation
		internal void Validate()
		{
			StringBuilder sb = new StringBuilder();

			if (RequiredGear == null)
				sb.AppendLine($"RequiredGear must be set on '{Name}'");

			if (RequiredGear != null && RequiredGear.Count <= 0)
				sb.AppendLine($"RequiredGear must not be empty on '{Name}'");

			if (KeroseneLitersRequired < 0)
				sb.AppendLine($"KeroseneLitersRequired cannot be negative on '{Name}'");

			if (GunpowderKGRequired < 0)
				sb.AppendLine($"GunpowderKGRequired cannot be negative on '{Name}'");

			if (string.IsNullOrWhiteSpace(CraftedResult))
				sb.AppendLine($"CraftedResult must be set on '{Name}'");

			if (CraftedResultCount < 1)
				sb.AppendLine($"CraftedResultCount cannot be less than 1 on '{Name}'");

			if (DurationMinutes < 0)
				sb.AppendLine($"DurationMinutes cannot be negative on '{Name}'");

			if (!EnumValues<CraftingLocation>.Contains(RequiredCraftingLocation))
				sb.AppendLine($"Unsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{Name}'");

			if (!EnumValues<SkillType>.Contains(AppliedSkill))
				sb.AppendLine($"Unsupported value {AppliedSkill} for AppliedSkill on '{Name}'");

			if (!EnumValues<SkillType>.Contains(ImprovedSkill))
				sb.AppendLine($"Unsupported value {ImprovedSkill} for RequiredCraftingLocation on '{Name}'");

			if (RequiredGear != null && RequiredGear.Count > 0)
			{
				int i = 0;
				foreach (ModRequiredGearItem RequiredGearItem in RequiredGear)
				{
					if (RequiredGearItem.Item == null)
						sb.AppendLine($"RequiredGearItem[{i}].Item must be set on '{Name}'");
					if (RequiredGearItem.Count < 1)
						sb.AppendLine($"RequiredGearItem[{i}].Count cannot be less than 1 on '{Name}'");
					i++;
				}
			}
			if (sb.Length > 0)
			{
				MelonLoader.MelonLogger.Error(sb.ToString().Trim());
			}

		}
		#endregion

		private static class EnumValues<T> where T : struct, Enum
		{
			private static readonly T[] values = Enum.GetValues<T>();
			public static bool Contains(T value) => values.Contains(value);
		}
	}
}