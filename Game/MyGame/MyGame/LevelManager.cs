﻿using System;
using System.Collections.Generic;
using System.IO;
using GXPEngine;
using MyGame.MyGame.Levels;
using Newtonsoft.Json.Linq;

namespace MyGame.MyGame;

public class LevelManager
{
	private readonly Dictionary<string, Level> _levels = new();
	private Level _lastLevel;
	private readonly Game _game = Game.main;

	public LevelManager()
	{
		string text = File.ReadAllText("../../assets/maps/levels.json");

		try
		{
			JObject rawLevels = JObject.Parse(text);

			foreach (var raw in rawLevels)
			{
				_levels.Add(raw.Key, new Level(raw.Value.ToString()));
			}
		}
		catch (Exception e)
		{
			Console.WriteLine($"Couldn't load levels: {e}");
		}
	}

	public void Init()
	{
		SetLevel("demo");
	}

	public void SetLevel(string levelName)
	{
		if (_lastLevel != null) _game.RemoveChild(_lastLevel);

		Level level = _levels[levelName];
		level.CreateLevel();

		_game.AddChild(level);
		foreach (GameObject gameObject in _game.GetChildren())
		{
			if (gameObject is Solid solid)
			{
				level.Solids.Add(solid);
			}
		}

		_lastLevel = _levels[levelName];
	}

	public void ClearLevels()
	{
		foreach (Level level in _levels.Values)
		{
			_game.RemoveChild(level);
		}
	}

	public Level CurrentLevel()
	{
		return _lastLevel;
	}
}