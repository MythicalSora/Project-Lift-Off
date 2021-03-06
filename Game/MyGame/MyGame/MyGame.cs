using System;
using System.IO;
using GXPEngine;

namespace MyGame.MyGame;


public class MyGame : Game
{
	public const int PLAYER_HEALTH = 3;

	public const bool DEBUG_MODE = false;
	public static EasyDraw DebugCanvas;
	public static LevelManager LevelManager;

	private static int _score;

	private MyGame() : base(1408, 768, true, false, pPixelArt: true)
	{
		targetFps = 60;
		LevelManager = new LevelManager();
		SoundManager.LoadAllSounds();

		Sprite background = new("background0.png")
		{
			width = width,
			height = height,
		};
		AddChild(background);

		StartGame();

		if (DEBUG_MODE)
		{
			DebugCanvas = new EasyDraw(width, height);
			AddChild(DebugCanvas);
		}

		SoundManager.music.Play(volume: 0.5f);

		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	private void Update()
	{
		// Console.WriteLine("------------------");
		Gamepad.Update();
		UI.Update();

		// Console.WriteLine(SoundManager.Channel.IsPlaying);

		if (DEBUG_MODE)
		{
			DebugCanvas.ClearTransparent();
			// Console.WriteLine(GetDiagnostics());
		}

		AddScore(1);

		if (Input.GetKeyDown(Key.R))
		{
			StartGame();
		}

		UI.Text("Score: " + _score, width, 0);
		if (Input.GetKeyDown(Key.B))
		{
			Console.WriteLine(GetDiagnostics());
			foreach (GameObject gameObject in GetChildren())
			{
				Console.WriteLine("D: " + gameObject);
			}
		}
	}

	private void StartGame()
	{
		LevelManager.Init();

		_score = 0;
		UI.Init();
	}

	public static void AddScore(int additionAmount)
	{
		_score += additionAmount;
	}

	private static void Main()
	{
		new MyGame().Start();
	}
}
