using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheJam
{
    class Tile : Entity
    {
        public Tile(int x, int y, Texture2D sprite, bool collision, bool interactable) : base(x, y, -10, collision, interactable, sprite)
        {
        }
    }
}
