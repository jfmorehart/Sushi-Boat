using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Progress
{
	
	public static int money = 0; //regular int
	//public static int upgrades = 0; // stores boolean values, ask jack if confused
	public static int maxUnlockedLevel;
	//public static int activeSaveSlot;
	public static string scores; //32100 etc

	public static void Save() {
		PlayerPrefs.SetInt("money", money);
		PlayerPrefs.SetInt("level", maxUnlockedLevel);
		PlayerPrefs.SetString("scores", scores);

		Debug.Log("Saving:" + money + "$, " + maxUnlockedLevel + " unlocked, " + scores);
		PlayerPrefs.Save();
	}
	public static void Load() {
		money = PlayerPrefs.GetInt("money", 0);
		maxUnlockedLevel = PlayerPrefs.GetInt("level", 1);
		scores = PlayerPrefs.GetString("scores", "00000");

		Debug.Log("Loading:" + money + "$, " + maxUnlockedLevel + " unlocked, " + scores);
	}
	public static void NewLoad() {
		money = 0;
		maxUnlockedLevel = 1;
		scores = "00000";
		Save();
	}
	public static int GetScoreOnLevel(int level) {
		scores = PlayerPrefs.GetString("scores", "00000");
		if (scores.Length < 5) scores = "00000";
		Debug.Log(level + "  " + scores);
		char c = scores[level - 1];
		int score = c - '0';
		Debug.Log(level + " " + score);
		if(score > 0) {
			maxUnlockedLevel = Mathf.Max(level + 1, maxUnlockedLevel);
		}
		return (int)char.GetNumericValue(c);
	}
	public static void SetScoreOnLevel(int level, int score)
	{
		Debug.Log("setting " + level + " to " + score);
		scores = PlayerPrefs.GetString("scores", "00000");
		if (scores.Length < 5) scores = "00000";

		char[] ch = scores.ToCharArray();
		ch[level - 1] = score.ToString()[0];

		if (score > 0)
		{
			maxUnlockedLevel = Mathf.Max(level + 1, maxUnlockedLevel);
		}

		scores = new string(ch);
		Debug.Log(scores);
		PlayerPrefs.SetString("scores", scores);
		
	}
	public static bool HasBeatenTutorial() { 
		return PlayerPrefs.GetInt("tutorial?", 0) == 1;
	}
	public static void BeatTutorial() {
		PlayerPrefs.SetInt("tutorial?", 1);
	}

	//public static bool CheckValidSave(int saveToCheck)
	//{
	//	string save = PlayerPrefs.GetString(saveToCheck.ToString());
	//	if (save == null) return false;
	//	string[] strs = save.Split(",");
	//	if (strs.Length == 3)
	//	{
	//		return true;
	//	}
	//	return false;
	//}
	//public static void AddUpgrade(Upgrade up) {
	//	EditUpgrades(up, true);
 //   }
	//public static void RemoveUpgrade(Upgrade up)
	//{
	//	EditUpgrades(up, false);
	//}

	//public static void EditUpgrades(Upgrade up, bool add) {
	//	int upgr = (int)up;
	//	if (add) {
	//		//add upgrade
	//		//this is a bitwise OR operation, which makes a specific 
	//        //bit in the 'upgrades' int into a 1;
	//		// 1010 | 1000 = 1010, weve just inserted a 1 into the 8's place,
	//		// meaning we've just stored SuperReel into upgrades

	//		upgrades |= upgr;
	//	}
	//	else {
	//		//remove upgrade

	//		// this is a bitwise AND operation, which removes the 1 from
	//		// the spot by bitinverting (~) the upgrade (making it 0) in the spot.
	//		// 1010 & ~(1000) = 1010 & 0111 = 0010 = removed SuperReel
	//		upgrades &= ~upgr;
	//	}
 //   }
	//public static bool HasUpgrade(Upgrade up) {

	//	//this checks the bit corresponding to the upgrade.
	//	// the bitwise AND, (&) will return the value of the two binary numbers ANDed together
	//	// so for example 1010 & 1001 = 1000 = 8 = SuperReel, and 8 == 8 so it returns true
	//	return (upgrades & (int)up) == (int)up;
 //   }

	//public enum Upgrade { 
	//	//In order for these bitwise operations to work, these enums must
	//	// have specific integer values (corresponding to a single 1 in binary)
	//	// you can have 31 entries in this enum (one bit is wasted on the sign)
	//	// the int value of the enum should be 2^n where n is the index of the enum (eg 3 for 3rd)
	//	FastReel = 1,
	//	ExtraChef = 2, 
	//	MoreSeating = 4,
	//	SuperReel = 8
	//}

	//static string SaveString() {

	//	return GameManager.Instance.money.ToString() + "," + upgrades.ToString() + "," + maxUnlockedLevel.ToString();
 //   }

}

