using CraftingRevisions.Exceptions;
using MelonLoader.TinyJSON;

namespace CraftingRevisions
{
	public sealed class ModBlueprintData
	{
		/// <summary>
		/// An optional name for the blueprint. Only used in error messages.
		/// </summary>
		public string Name;

		/// <summary>
		/// The name of each gear needed to craft this item (e.g. GEAR_Line)
		/// </summary>
		public string[] RequiredGear = new string[0];

		/// <summary>
		/// How many of each item are required? <br/>
		/// This list has to match the RequiredGear list.
		/// </summary>
		public int[] RequiredGearUnits = new int[0];

		/// <summary>
		/// How many liters of kerosene are required?
		/// </summary>
		public float KeroseneLitersRequired = 0;

		/// <summary>
		/// How much gunpowder is required? (in kilograms)
		/// </summary>
		public float GunpowderKGRequired = 0;


		/// <summary>
		/// Tool required to craft (e.g. GEAR_Knife)
		/// </summary>
		public string RequiredTool;

		/// <summary>
		/// List of optional tools to speed the process of crafting or to use in place of the required tool.
		/// </summary>
		public string[] OptionalTools = new string[0];


		/// <summary>
		/// Where to craft?
		/// </summary>
		public ModCraftingLocation RequiredCraftingLocation = ModCraftingLocation.Anywhere;

		/// <summary>
		/// Requires a lit fire in the ammo workbench to craft?
		/// </summary>
		public bool RequiresLitFire = false;

		/// <summary>
		/// Requires light to craft?
		/// </summary>
		public bool RequiresLight = true;


		/// <summary>
		/// The name of the item produced.
		/// </summary>
		public string CraftedResult;

		/// <summary>
		/// Number of the item produced.
		/// </summary>
		public int CraftedResultCount = 1;


		/// <summary>
		/// Number of in-game minutes required.
		/// </summary>
		public int DurationMinutes = 5;

		/// <summary>
		/// Audio to be played.
		/// </summary>
		public string CraftingAudio;


		/// <summary>
		/// The skill associated with crafting this item. (e.g. Gunsmithing)
		/// </summary>
		public ModSkillType AppliedSkill = ModSkillType.None;

		/// <summary>
		/// The skill improved on crafting success.
		/// </summary>
		public ModSkillType ImprovedSkill = ModSkillType.None;

		#region Json
		public string DumpJson()
		{
			return JSON.Dump(this, EncodeOptions.NoTypeHints);
		}

		public string DumpJsonPretty()
		{
			return JSON.Dump(this, EncodeOptions.NoTypeHints | EncodeOptions.PrettyPrint);
		}

		public static ModBlueprintData ParseFromJson(string jsonText)
		{
			return JSON.Load(jsonText).Make<ModBlueprintData>();
		}
		#endregion

		public string GetName() => Name ?? "";

		internal void PreValidate()
		{
			if (RequiredGear == null)
				throw new InvalidBlueprintException($"RequiredGear must be set on '{GetName()}'");

			if (RequiredGearUnits == null)
				throw new InvalidBlueprintException($"RequiredGearUnits must be set on '{GetName()}'");

			if (RequiredGear.Length != RequiredGearUnits.Length)
				throw new InvalidBlueprintException($"RequiredGear length must be equal to RequiredGearUnits length on '{GetName()}'");

			if (KeroseneLitersRequired < 0)
				throw new InvalidBlueprintException($"KeroseneLitersRequired cannot be negative on '{GetName()}'");

			if (GunpowderKGRequired < 0)
				throw new InvalidBlueprintException($"GunpowderKGRequired cannot be negative on '{GetName()}'");

			if (string.IsNullOrWhiteSpace(CraftedResult))
				throw new InvalidBlueprintException($"CraftedResult must be set on '{GetName()}'");

			if (CraftedResultCount < 1)
				throw new InvalidBlueprintException($"CraftedResultCount cannot be less than 1 on '{GetName()}'");

			if (!IsValidEnumValue(RequiredCraftingLocation))
				throw new InvalidBlueprintException($"Unsupported value {RequiredCraftingLocation} for RequiredCraftingLocation on '{GetName()}'");

			if (!IsValidEnumValue(AppliedSkill))
				throw new InvalidBlueprintException($"Unsupported value {AppliedSkill} for AppliedSkill on '{GetName()}'");

			if (!IsValidEnumValue(ImprovedSkill))
				throw new InvalidBlueprintException($"Unsupported value {ImprovedSkill} for RequiredCraftingLocation on '{GetName()}'");
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
}
