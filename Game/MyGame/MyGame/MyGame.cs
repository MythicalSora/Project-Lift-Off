using System;
using GXPEngine;
using MyGame.MyGame.Levels;

namespace MyGame.MyGame;


public class MyGame : Game
{
	public const int PLAYER_HEALTH = 3;

	public const bool DEBUG_MODE = true;
	public static EasyDraw DebugCanvas;
	// ReSharper disable once InconsistentNaming
	public static Level Level;

	private static int _score;

	private MyGame() : base(1408, 768, false, false, pPixelArt: true)
	{
		targetFps = 60;

		Sprite background = new("background.png")
		{
			width = width,
			height = height,
		};
		AddChild(background);
		Level = new Level();
		AddChild(Level);

		UI.Init();

		if (DEBUG_MODE)
		{
			DebugCanvas = new EasyDraw(width, height);
			AddChild(DebugCanvas);
		}

		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	private void Update()
	{
		Gamepad.Update();
		UI.Update();

		if (DEBUG_MODE)
		{
			DebugCanvas.ClearTransparent();
			// Console.WriteLine(GetDiagnostics());
		}

		AddScore(1);

		UI.Canvas.Text("Score: " + _score, width - 100, 100); //TODO: Designer
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
