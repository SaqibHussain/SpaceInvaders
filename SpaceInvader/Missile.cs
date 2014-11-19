using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvader
{
    class Missile
    {
        Vector2 Position;

        public Missile(int XInitialPos, int YInitialPos)
        {
            Position = new Vector2(XInitialPos, YInitialPos);
        }

        public void Move()
        {
            Position.Y = Position.Y - 50;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }
    }
}
