using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataManager 
{
	public static int NumofMatches = 3;
	public static int Score = 0;
	public static float GameTime = 120;
	public static int MaxScore = 1200;
	public static bool isGameFinished = false;
	public static void Reset()
	{
		Score = 0;
		isGameFinished = false;
	}

	public static void SetData(int matches, int mxScore, float timer)
	{
		GameTime = timer;
		NumofMatches = matches;
		MaxScore = mxScore;
	}
}
