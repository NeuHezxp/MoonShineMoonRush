using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] IntVariable lives;
	[SerializeField] IntVariable score;
	[SerializeField] FloatVariable health;
	[SerializeField] FloatVariable timer;

	[SerializeField] GameObject respawn;

	[Header("Events")]
	[SerializeField] VoidEvent gameStartEvent;
	[SerializeField] GameObjectEvent respawnEvent;

	public enum State
	{
		TITLE,
		START_GAME,
		PLAY_GAME,
		GAME_OVER
	}

	private State state = State.TITLE;

	void Update()
	{
		if (UIManager.Instance != null)
		{
			UIManager.Instance.Health = health.value;
			//UIManager.Instance.Timer = timer.value;
			//UIManager.Instance.Score = score.value;
			//UIManager.Instance.Lives = lives.value;
		}

		switch (state)
		{
			case State.TITLE:
				UIManager.Instance.SetActive("Title", true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				break;

			case State.START_GAME:
				UIManager.Instance.SetActive("Title", false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				// reset values
				timer.value = 60;
				lives.value = 3;
				health.value = 100;

				gameStartEvent.RaiseEvent();
				respawnEvent.RaiseEvent(respawn);

				state = State.PLAY_GAME;
				break;
			case State.PLAY_GAME:
				// game timer
				timer.value = timer - Time.deltaTime;
				if (timer <= 0)
				{
					state = State.GAME_OVER;
				}

				break;
			case State.GAME_OVER:
				break;
			default:
				break;

		}


	}

	public void OnStartGame()
	{

	}

	public void OnPlayerDead()
	{
		state = State.START_GAME;
	}

	public void OnAddPoints(int points)
	{
		score.value += points;
		Debug.Log($"New Score: {score.value}");

		// Update the score display
		if (UIManager.Instance != null)
		{
			//UIManager.Instance.Score = score.value;
		}
	}
}

