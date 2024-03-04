using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayOvermenu : MonoBehaviour
{
	public void SwitchToMap() {
        SceneManager.LoadScene(1);
    }
}
