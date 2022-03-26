using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimerCountDown : MonoBehaviour
{
	public Text textDisplay;
	public Text ScoreTextDisplay;
	public Text ResultTextDisplay;
	public float gameTime;
	private float timer;
	private bool stopTimer;

	public Button RearrangeButton;
	public Button ExitButton;
	public Image ResultImage;

	private void Start()
	{
		gameTime = GameDataManager.GameTime;
		stopTimer = false;
		timer = gameTime;
		RearrangeButton.onClick.AddListener(OnArrangeButtonClicked);
		ExitButton.onClick.AddListener(OnExitButtonClicked);
		ResultImage.enabled = false;
		ResultTextDisplay.enabled = false;
	}

	private void Update()
	{
		if (!GameDataManager.isGameFinished)
		{
			UpdateTimer();
			ScoreTextDisplay.text = (GameDataManager.Score + " / " + GameDataManager.MaxScore);
			CheckScoreTimer();
		}
	}

	private void UpdateTimer()
	{
		timer -= Time.deltaTime;
		int minutes = Mathf.FloorToInt(timer / 60);
		int seconds = Mathf.FloorToInt(timer - minutes * 60f);
		string textTime = string.Format("{0:00}:{1:00}", minutes, seconds);
		if (timer <= 0)
		{
			stopTimer = true;
			timer = 0;
		}
		if (!stopTimer)
		{
			textDisplay.text=(textTime);
		}
	}

	public void OnArrangeButtonClicked()
	{
		TileManager.Instance.gameBoard.DestroyBoard();
		TileManager.Instance.gameBoard.GenerateBoard();
	}

	public void OnExitButtonClicked()
	{
		GameDataManager.Reset();
		SceneManager.LoadScene("Start");
	}

	public void CheckScoreTimer()
	{
		if(GameDataManager.Score>= GameDataManager.MaxScore )
		{
			ResultTextDisplay.text = "Level Complete!";
			ResultImage.enabled = true;

			ResultTextDisplay.enabled = true;
			GameDataManager.isGameFinished = true;
		}
		else if(timer <= 0)
		{
			ResultTextDisplay.text = "Game Over!";
			ResultImage.enabled = true;

			ResultTextDisplay.enabled = true;
			GameDataManager.isGameFinished = true;
		}

	}
}
