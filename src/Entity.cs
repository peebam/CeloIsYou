using CeloIsYou.Animations;
using CeloIsYou.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CeloIsYou.Core;

namespace CeloIsYou
{
    public class Entity : Core.IDrawable
    {
        private static readonly Vector2 NoPosition = new(-1, -1);

        private IAnimation _animation;

        public bool IsControlled { get; set; }
        public bool IsKilling { get; set; } = true;
        public bool IsPushable { get; set; }
        public bool IsStoping { get; set; }

        public int DrawOrder { get; set; }


        public Coordinates Coordinates { get; private set; }
        public Vector2 Position { get; private set; }
        public Texture2D Texture { get; private set; }
        public EntityTypes Type { get; set; }

        public Entity(EntityTypes type)
        {
            Type = type;
        }

        public void ClearPosition()
        {
            Position = NoPosition;
        }

        public void ClearStates()
        {
            IsStoping = false;
            IsPushable = false;
            IsControlled = false;
        }

        public void SetPosition(Vector2 position, GameTime gameTime)
        {
            if (Position == NoPosition)
            {
                Position = position;
                return;
            }
            _animation = new PositionAnimation(Position, position, Configuration.Instance.GameSpeed, gameTime, pa => Position = pa.CurrentPosition);
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if (_animation != null)
            {
                _animation.Update(gameTime);
                if (_animation.IsDone)
                    _animation = null;
            }
        }
    }
}
