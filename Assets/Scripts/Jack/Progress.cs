using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Progress
{
	public static int money = 0; //regular int
	public static int upgrades = 0; // stores boolean values, ask jack if confused
	public static int maxUnlockedLevel = 1;
	public static int activeSaveSlot;

	public static void Save() {
		
		Debug.Log("Saving..." + SaveString());
		PlayerPrefs.SetString(activeSaveSlot.ToString(), SaveString());
	}
	public static void Load() {
		string save = PlayerPrefs.GetString(activeSaveSlot.ToString());
		if (save.Length < 1) {
			NewLoad();
			return;
		}
		string[] strs = save.Split(",");
		if (strs.Length != 3)
		{
			Debug.LogError("Incorrect save format!");
			NewLoad();
			return;
		}
		money = int.Parse(strs[0]);
		upgrades = int.Parse(strs[1]);
		maxUnlockedLevel = Mathf.Max(1, int.Parse(strs[2]));

		Debug.Log("Loading monayyy: " + strs[0]);
		Debug.Log("Loading upgrades: " + strs[1]);
		Debug.Log("Loading level: " + strs[2]);
	}
	public static void NewLoad() {
		Debug.Log("No Save in Selected Slot! Overrwiting.");
		money = 0;
		upgrades = 0;
		maxUnlockedLevel = 1;
		Save();
	}
	public static bool CheckValidSave(int saveToCheck)
	{
		string save = PlayerPrefs.GetString(saveToCheck.ToString());
		if (save == null) return false;
		string[] strs = save.Split(",");
		if (strs.Length == 3)
		{
			return true;
		}
		return false;
	}
	public static void AddUpgrade(Upgrade up) {
		EditUpgrades(up, true);
    }
	public static void RemoveUpgrade(Upgrade up)
	{
		EditUpgrades(up, false);
	}

	public static void EditUpgrades(Upgrade up, bool add) {
		int upgr = (int)up;
		if (add) {
			//add upgrade
			//this is a bitwise OR operation, which makes a specific 
	        //bit in the 'upgrades' int into a 1;
			// 1010 | 1000 = 1010, weve just inserted a 1 into the 8's place,
			// meaning we've just stored SuperReel into upgrades

			upgrades |= upgr;
		}
		else {
			//remove upgrade

			// this is a bitwise AND operation, which removes the 1 from
			// the spot by bitinverting (~) the upgrade (making it 0) in the spot.
			// 1010 & ~(1000) = 1010 & 0111 = 0010 = removed SuperReel
			upgrades &= ~upgr;
		}
    }
	public static bool HasUpgrade(Upgrade up) {

		//this checks the bit corresponding to the upgrade.
		// the bitwise AND, (&) will return the value of the two binary numbers ANDed together
		// so for example 1010 & 1001 = 1000 = 8 = SuperReel, and 8 == 8 so it returns true
		return (upgrades & (int)up) == (int)up;
    }

	public enum Upgrade { 
		//In order for these bitwise operations to work, these enums must
		// have specific integer values (corresponding to a single 1 in binary)
		// you can have 31 entries in this enum (one bit is wasted on the sign)
		// the int value of the enum should be 2^n where n is the index of the enum (eg 3 for 3rd)
		FastReel = 1,
		ExtraChef = 2, 
		MoreSeating = 4,
		SuperReel = 8
	}

	static string SaveString() {

		return GameManager.Instance.money.ToString() + "," + upgrades.ToString() + "," + maxUnlockedLevel.ToString();
    }

}

