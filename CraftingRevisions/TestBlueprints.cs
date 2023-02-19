
// just some json blueprints for testing 
#warning - This file and references to it to be removed before merge&release (solely here for testing)

namespace CraftingRevisions
{
	internal class TestBlueprints
	{

		public const string blueprint_1 = """
			{
			"Name": "Old_Prybar",
			"RequiredGear": ["GEAR_Cloth"],
			"RequiredGearUnits": [1],
			"KeroseneLitersRequired": 0,
			"GunpowderKGRequired": 0,
			"RequiredTool": "",
			"OptionalTools": [],
			"RequiredCraftingLocation": "Anywhere",
			"RequiresLitFire": false,
			"RequiresLight": false,
			"CraftedResult": "GEAR_Prybar",
			"CraftedResultCount": 1,
			"DurationMinutes": 15,
			"CraftingAudio": "PLAY_CraftingCloth",
			"AppliedSkill" : "None",
			"ImprovedSkill" : "None"
			}
			""";

		public const string blueprint_2 = """
			{
			"RequiredGear":[{"Item":"GEAR_Cloth","Count":1}],
			"RequiredTool":"",
			"OptionalTools":[],
			"CraftedResult":"GEAR_Prybar",
			"CraftedResultCount":1,
			"DurationMinutes":10,
			"CraftingAudio":null,
			"KeroseneLitersRequired":0,
			"GunpowderKGRequired":0,
			"RequiresLight":false,
			"Locked":false,
			"IgnoreLockInSurvival":true,
			"AppearsInStoryOnly":false,
			"AppearsInSurvivalOnly":false,
			"AppliedSkill":"None",
			"ImprovedSkill":"None",
			"RequiredCraftingLocation":"Anywhere",
			"RequiresLight":false,
			"RequiresLitFire":false,
			"CanIncreaseRepairSkill":false
			}
			""";



	}
}
