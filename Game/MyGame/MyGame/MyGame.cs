using System;
using GXPEngine;
using MyGame.MyGame.Levels;

namespace MyGame.MyGame;


public class MyGame : Game
{
	public const bool DEBUG_MODE = false;
	public static EasyDraw DebugCanvas;
	public static Level Level;

	private MyGame() : base(1366, 768, false, false, pPixelArt: true)
	{
		targetFps = 60;
		LevelManager levelManager = new();

		Sprite background = new("background.png")
		{
			width = width,
			height = height,
		};
		AddChild(background);
    
		levelManager.Init();

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

		if (DEBUG_MODE)
		{
			DebugCanvas.ClearTransparent();
			// Console.WriteLine(GetDiagnostics());
		}
	}

	private static void Main()
	{
		new MyGame().Start();
	}
}
