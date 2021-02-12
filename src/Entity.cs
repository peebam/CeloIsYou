using CeloIsYou.Interpolator;
using CeloIsYou.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CeloIsYou.Extensions;
using CeloIsYou.Core;

namespace CeloIsYou
{
    public class Entity :  IEntity
    {
        private PositionInterpolator _animation;

        public bool IsDone => false;

        public bool IsControlled { get; set; }
        public bool IsKilling { get; set; }
        public bool IsPushable { get; set; }
        public bool IsStoping { get; set; }
        public bool IsWeak { get; set; }
        public bool IsWin { get; set; }

        public int DrawOrder { get; set; }

        public Coordinates Coordinates { get; private set; }
        public Vector2 Position { get; private set; }
        public Texture2D Texture { get; private set; }
        public EntityTypes Type { get; set; }

        public bool Visible { get; private set; }

        public Entity(EntityTypes type)
        {
            Type = type;
        }

        public void ClearPosition()
        {
            Visible = false;
        }

        public void ClearStates()
        {
            IsStoping = false;
            IsPushable = false;
            IsControlled = false;
            IsWeak = false;
            IsWin = false;
        }

        public void SetCoordinates(Coordinates coordinates, GameTime gameTime)
        {
            Coordinates = coordinates;
            SetPosition(coordinates.ToPosition(), gameTime);
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

        private void SetPosition(Vector2 position, GameTime gameTime)
        {
            if (!Visible)
            {
                Visible = true;
                Position = position;
                return;
            }
            _animation = new PositionInterpolator(Position, position, Configuration.Instance.GameSpeed, gameTime, pa => Position = pa.CurrentPosition);
        }
    }
}
