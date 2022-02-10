﻿using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Player : Entity
{
	//Variables for the designers:
	//General:
	private const float PLAYER_MOVEMENT_SPEED = 1.6f;
	private const float CONTROLLER_THRESHOLD = 0.2f;

	//Animation
	private const byte IDLE_ANIMATION_DELAY = 100;
	// private const byte RUN_MIN_ANIMATION_DELAY = 100;
	// private const byte RUN_MAX_ANIMATION_DELAY = 100;

	//Double Jump:
	private const int MAX_JUMPS = 2;

	//Hold to jump higher:
	private const int MILLIS_FOR_MAX_JUMP = 500;
	private const float MIN_INSTANT_JUMP_FORCE = 30.0f; //when you jump, you always jump at least with this much force
	private const float MAX_GRADUAL_JUMP_FORCE = 5.0f; //every frame you're jumping, a fraction of this force is applied

	//Dash:
	private const float DASH_FORCE = 100.0f;
	private const int MILLIS_BETWEEN_DASHES = 4000;


	//Variables needed to track the internal state of the player
	//Double Jump:
	private bool _inAir;
	private int _jumpAmounts;

	//Hold to jump higher:
	private bool _jumping;
	private int _millisAtStartJump;
	private int _millisSinceLastDash;

	//Dash:
	private int _millisAtLastDash;

	public Player(Vector2 spawnPos) :
		base(spawnPos, "playerIdle.png", 8, 2, 12, IDLE_ANIMATION_DELAY)
	{
		//Empty
	}

	public Player(TiledObject obj) : base(new Vector2(obj.X, obj.Y), "playerIdle.png", 8, 2, 12, IDLE_ANIMATION_DELAY)
	{
		//Empty
	}

	private new void Update()
	{
		// Console.WriteLine(_vel.MagSq());
		// SetAnimationDelay((byte) Mathf.Map(_vel.MagSq(), 0, 200, 255, 50));

		Vector2 mousePos = new(Input.mouseX, Input.mouseY);
		Vector2 screenCenter = new(Game.main.width / 2.0f, Game.main.height / 2.0f);
		Vector2 direction = Vector2.Sub(mousePos, screenCenter);
		if (MyGame.DEBUG_MODE) MyGame.DebugCanvas.Line(screenCenter.x, screenCenter.y, screenCenter.x + direction.x, screenCenter.y + direction.y);

		//Basic Left/Right Movement
		const float detail = 100.0f;
		float xMovement = Mathf.Clamp(direction.x, -detail, detail) / detail;
		if(Mathf.Abs(xMovement) > CONTROLLER_THRESHOLD)
			ApplyForce(Vector2.Mult(new Vector2(xMovement, 0), PLAYER_MOVEMENT_SPEED));

		//Dashing movement
		if (MyGame.DEBUG_MODE) MyGame.DebugCanvas.Text("" + _millisSinceLastDash);
		_millisSinceLastDash = Time.time - _millisAtLastDash;
		if (Input.GetKeyDown(Key.LEFT_SHIFT))
		{
			RequestDash(direction);
		}


		//Jumping Movement
		if (_inAir && Colliding)
		{
			ResetJumps();
		}
		if (Colliding && _jumping || (Input.GetKeyUp(Key.W) || Input.GetKeyUp(Key.SPACE)))
		{
			StopJump();
		}
		if ((Colliding || _jumpAmounts < MAX_JUMPS) && (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.SPACE)))
		{
			StartJump();
		}

		if (_jumping)
		{
			int millisSinceJump = Time.time - _millisAtStartJump;

			float jumpProgress = Mathf.Clamp(Mathf.Map(MILLIS_FOR_MAX_JUMP - millisSinceJump, 0, MILLIS_FOR_MAX_JUMP, 1.0f, 0.0f), 0.0f, 1.0f);


			//jumpProgress (range: 0.0f..1.0f) represents how long the jump has been going on for.
			//0.0f: when the jump has just started
			//1.0f: when the jump has reached its maximum height
			//TODO: Construct a better formula for the jumpForce, preferably not a linear one like this, but instead an exponential one
			float jumpForce = Mathf.Map(jumpProgress, 0.0f, 1.0f, MAX_GRADUAL_JUMP_FORCE, 0.0f);


			ApplyForce(new Vector2(0, -jumpForce));
			if(MyGame.DEBUG_MODE) Console.WriteLine("jumping with force of " + jumpForce + ". jump progress: " + jumpProgress);
		}

		//Actually calculate and apply the forces that have been acting on the Player the past frame
		base.Update();
	}

	private void StartJump()
	{
		_millisAtStartJump = Time.time;
		_jumping = true;
		_inAir = true;
		_jumpAmounts++;
		ApplyForce(new Vector2(0, -MIN_INSTANT_JUMP_FORCE));
		Console.WriteLine("jump start");
	}

	private void StopJump()
	{
		_jumping = false;
		Console.WriteLine("jump end");
	}

	private void ResetJumps()
	{
		_inAir = false;
		_jumpAmounts = 0;
		Console.WriteLine("jump reset");
	}

	private void RequestDash(Vector2 direction)
	{
		Console.WriteLine(_millisSinceLastDash);
		if (_millisSinceLastDash < MILLIS_BETWEEN_DASHES) return;
		Dash(direction);
	}

	private void Dash(Vector2 direction)
	{
		_millisAtLastDash = Time.time;
		ApplyForce(Vector2.Mult(direction.Copy().Normalize(), DASH_FORCE));
	}
}