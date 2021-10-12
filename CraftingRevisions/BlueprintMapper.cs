using System;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingRevisions
{
	internal static class BlueprintMapper
	{
		private static List<ModBlueprintData> blueprints = new List<ModBlueprintData>();
		internal static void MapBlueprint(ModBlueprintData modBlueprint)
		{
			BlueprintItem bpItem = GameManager.GetBlueprints().AddComponent<BlueprintItem>();
			if (bpItem == null)
			{
				throw new NullReferenceException("Error creating Blueprint");
			}

			bpItem.m_DurationMinutes = modBlueprint.DurationMinutes;
			bpItem.m_CraftingAudio = modBlueprint.CraftingAudio;

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

			bpItem.m_RequiredGear = NotNull(GetItems<GearItem>(modBlueprint.RequiredGear));
			bpItem.m_RequiredGearUnits = modBlueprint.RequiredGearUnits;
			bpItem.m_KeroseneLitersRequired = modBlueprint.KeroseneLitersRequired;
			bpItem.m_GunpowderKGRequired = modBlueprint.GunpowderKGRequired;

			bpItem.m_AppliedSkill = TranslateEnumValue<SkillType, ModSkillType>(modBlueprint.AppliedSkill);
			bpItem.m_ImprovedSkill = TranslateEnumValue<SkillType, ModSkillType>(modBlueprint.ImprovedSkill);
		}

		internal static void MapBlueprints()
		{
			GameObject blueprintsManager = GameManager.GetBlueprints();
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
				throw new ArgumentException($"Validation of blueprint '{modBlueprint.GetName()}' failed: {e.Message}\nThe blueprint may be out-of-date or installed incorrectly.");
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

		internal static T GetItem<T>(string name) where T : UnityEngine.Component
		{
			GameObject gameObject = Resources.Load(name).TryCast<GameObject>();
			if (gameObject == null)
			{
				throw new ArgumentException($"Could not load '{name}'.");
			}

			T targetType = gameObject?.GetComponent<T>();
			if (targetType == null)
			{
				throw new ArgumentException($"'{name}' is not a '{typeof(T).Name}'.");
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
	}
}
