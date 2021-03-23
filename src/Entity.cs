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
        private IAnimation _animation;

        private PositionInterpolator _positionInterpolator;
        
        public bool IsControlled { get; set; }
        public bool IsDone => false;
        public bool IsKilling { get; set; }
        public bool IsPushable { get; set; }
        public bool IsStoping { get; set; }
        public bool IsWeak { get; set; }
        public bool IsWin { get; set; }
        
        public Coordinates Coordinates { get; private set; }
        public int DrawOrder { get; set; }
        public Vector2 Position { get; private set; }
        public Texture2D Texture => _animation.Texture;
        public EntityTypes Type { get; private set; }
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

        public void SetAnimation(IAnimation animation)
        {
            _animation = animation;
        }

        public void Update(GameTime gameTime)
        {
            if (_positionInterpolator != null)
            {
                _positionInterpolator.Update(gameTime);
                if (_positionInterpolator.IsDone)
                    _positionInterpolator = null;
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
            _positionInterpolator = new PositionInterpolator(Position, position, Configuration.Instance.GameSpeed, gameTime, pa => Position = pa.Current);
        }
    }
}
