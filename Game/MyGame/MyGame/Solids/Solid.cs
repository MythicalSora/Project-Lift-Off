﻿using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Solids;

public class Solid : Sprite
{
    protected Solid(TiledObject obj) : base("colors.png")
    {
        this.collider.isTrigger = true;
        game.AddChild(this);

        // ReSharper disable once VirtualMemberCallInConstructor
        width = (int)obj.Width;
        // ReSharper disable once VirtualMemberCallInConstructor
        height = (int)obj.Height;
    }
}
