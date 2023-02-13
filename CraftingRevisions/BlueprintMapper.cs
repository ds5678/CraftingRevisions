using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTLD.Gear;
using UnityEngine;

namespace CraftingRevisions
{
	internal static class BlueprintMapper
	{
		private static List<ModBlueprintData> blueprints = new();
		internal static void MapBlueprint(ModBlueprintData modBlueprint)
		{
			BlueprintData bpItem = new();

			bpItem.m_DurationMinutes = modBlueprint.DurationMinutes;
			// removed (temp) due to possibly moving to use UserBlueprintData
			// BlueprintData.m_CraftingAudio is now Il2CppAK.Wwise.Event, no longer string
			//bpItem.m_CraftingAudio = modBlueprint.CraftingAudio;

			bpItem.m_RequiredCraftingLocation = TranslateEnumValue<CraftingLocation, ModCraftingLocation>(modBlueprint.RequiredCraftingLocation);
			bpItem.m_RequiresLitFire = modBlueprint.RequiresLitFire;
			bpItem.m_RequiresLight = modBlueprint.RequiresLight;

			bpItem.m_Locked = false;
			bpItem.m_AppearsInStoryOnly = false;

			bpItem.m_CraftedResultCount = modBlueprint.CraftedResultCount;
			bpItem.m_CraftedResult = GetItem<GearItem>(modBlueprint.CraftedResult);

			if (!string.IsNullOrEmpty(modBlueprint.RequiredTool))
			{
				bpItem.m_RequiredTool = GetItem<ToolsItem>(modBlueprint.RequiredTool);
			}
			bpItem.m_OptionalTools = NotNull(GetItems<ToolsItem>(modBlueprint.OptionalTools));

			bpItem.m_RequiredGear = GetRequiredGearItems(modBlueprint.RequiredGear, modBlueprint.RequiredGearUnits);
			#warning bpItem.m_RequiredGearUnits is obsolete - see GetRequiredGearItems()
			//bpItem.m_RequiredGearUnits = modBlueprint.RequiredGearUnits; 
			bpItem.m_KeroseneLitersRequired = modBlueprint.KeroseneLitersRequired;
			bpItem.m_GunpowderKGRequired = modBlueprint.GunpowderKGRequired;

			bpItem.m_AppliedSkill = TranslateEnumValue<SkillType, ModSkillType>(modBlueprint.AppliedSkill);
			bpItem.m_ImprovedSkill = TranslateEnumValue<SkillType, ModSkillType>(modBlueprint.ImprovedSkill);

			// add the blueprint
			InterfaceManager.GetInstance().m_BlueprintManager.m_UserBlueprints.Add(bpItem);

		}

		internal static void MapBlueprints()
		{
			Il2CppTLD.Gear.BlueprintManager blueprintsManager = InterfaceManager.GetInstance().m_BlueprintManager;
			if (blueprintsManager == null) return;

			foreach (ModBlueprintData modBlueprint in blueprints)
			{
				MapBlueprint(modBlueprint);
			}
		}

		internal static void RegisterBlueprint(ModBlueprintData modBlueprint)
		{
			ValidateBlueprint(modBlueprint);

			blueprints.Add(modBlueprint);
		}

		private static void ValidateBlueprint(ModBlueprintData modBlueprint)
		{
			if (modBlueprint == null)
			{
				throw new ArgumentNullException(nameof(modBlueprint), "Blueprint cannot be null");
			}

			try
			{
				GetItem<GearItem>(modBlueprint.CraftedResult);

				if (!string.IsNullOrEmpty(modBlueprint.RequiredTool))
				{
					GetItem<ToolsItem>(modBlueprint.RequiredTool);
				}

				if (modBlueprint.OptionalTools != null)
				{
					GetItems<ToolsItem>(modBlueprint.OptionalTools);
				}

				GetItems<GearItem>(modBlueprint.RequiredGear);
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Validation of blueprint '{modBlueprint.GetName()}' failed. The blueprint may be out-of-date or installed incorrectly.\n{e.Message}", e);
			}
		}

		private static T TranslateEnumValue<T, E>(E value) where T : Enum where E : Enum
		{
			return (T)Enum.Parse(typeof(T), Enum.GetName(typeof(E), value));
		}

		private static T[] NotNull<T>(T[] array)
		{
			if (array == null) return new T[0];
			else return array;
		}

		/// <summary>
		/// Get a component from an item
		/// </summary>
		/// <typeparam name="T">The Component type to get</typeparam>
		/// <param name="name">The name of the object to load</param>
		/// <returns>The first component with type T</returns>
		internal static T GetItem<T>(string name) where T : UnityEngine.Component
		{
			UnityEngine.Object loadedObject = Resources.Load(name);
			if (loadedObject == null)
			{
				throw new ArgumentException($"Could not load '{name}'.");
			}
			GameObject gameObject = loadedObject.TryCast<GameObject>();
			if (gameObject == null)
			{
				throw new ArgumentException($"'{name}' is not a gameobject.");
			}

			T targetType = gameObject.GetComponent<T>();
			if (targetType == null)
			{
				throw new ArgumentException($"'{name}' does not have a component of type '{typeof(T).Name}'.");
			}

			return targetType;
		}

		internal static T[] GetItems<T>(string[] names) where T : UnityEngine.Component
		{
			T[] result = new T[names.Length];

			for (int i = 0; i < names.Length; i++)
			{
				result[i] = GetItem<T>(names[i]);
			}

			return result;
		}
		/// <summary>
		/// was previously using GetItems()
		/// </summary>
		/// <param name="names"></param>
		/// <param name="counts"></param>
		/// <returns>Il2CppReferenceArray required by BlueprintData.m_RequiredGear</returns>
		internal static Il2CppReferenceArray<BlueprintData.RequiredGearItem> GetRequiredGearItems(string[] names, int[] counts)
		{
			List<BlueprintData.RequiredGearItem> list = new();

			for (int i = 0; i < names.Length; i++)
			{
				BlueprintData.RequiredGearItem ri = new();
				ri.m_Item = GetItem<GearItem>(names[i]);
				ri.m_Count = counts[i];
				list.Add(ri);
			}

			return list.ToArray();
		}
	}
}
