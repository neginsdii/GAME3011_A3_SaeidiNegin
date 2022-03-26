using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSigns : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Easy"))
		{
			Debug.Log("Collsiion");
			GameDataManager.SetData(3, 220, 120);
			SceneManager.LoadScene("Match3");
		}
		else if (other.gameObject.CompareTag("Normal"))
		{
			Debug.Log("Collsiion");
			GameDataManager.SetData(4, 250, 90);
			SceneManager.LoadScene("Match3");
		}
		else if (other.gameObject.CompareTag("Hard"))
		{
			Debug.Log("Collsiion");
			GameDataManager.SetData(5, 280, 70);
			GameDataManager.NumofMatches = 5;
			SceneManager.LoadScene("Match3");
		}
	}
}
