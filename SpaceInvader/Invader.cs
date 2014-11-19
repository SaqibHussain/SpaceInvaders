using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvader
{
    class Invader
    {
        Vector2 AlienPos;

        public Invader()
        {
            AlienPos = new Vector2();

            AlienPos.X = 0;
            AlienPos.Y = 0;
        }

        public void MoveHorizontal(int amount)
        {
            AlienPos.X = AlienPos.X + amount;
        }

        public void MoveVertical(int amount)
        {
            AlienPos.Y = AlienPos.Y + amount;
        }

        public void SetXPos(int pos)
        {
            AlienPos.X = pos;
        }

        public int GetXPos()
        {
            return (int)AlienPos.X;
        }

        public void SetYPos(int pos)
        {
            AlienPos.Y = pos;
        }

        public int GetYPos()
        {
            return (int)AlienPos.Y;
        }

        public Vector2 GetPos()
        {
            return AlienPos;
        }
    }
}
