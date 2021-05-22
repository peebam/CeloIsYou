using CeloIsYou.Interpolator;
using CeloIsYou.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CeloIsYou.Extensions;
using CeloIsYou.Core;
using CeloIsYou.src.Enumerations;

namespace CeloIsYou
{
    public class Entity :  IEntity
    {
        private IAnimation _animation;

        private PositionInterpolator _positionInterpolator;

        private Resources _resources;

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
        public EntityStates State { get; private set; }
        public EntityTypes Type { get; private set; }
        public bool Visible { get; private set; }

        public Entity(Resources resources, EntityTypes type)
        {
            _resources = resources;
            State = EntityStates.Idle;
            Type = type;
            SetDefaultAnimation();
        }

        public void Appears()
        {
            State = EntityStates.Appearing;
            _animation = _resources.GetAnimationSmoke(0.04f);
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

        public void Update(GameTime gameTime)
        {
            if (_positionInterpolator != null)
            {
                _positionInterpolator.Update(gameTime);
                if (_positionInterpolator.IsDone)
                {
                    _positionInterpolator = null;
                    State = EntityStates.Idle;
                }
            }

            _animation.Update(gameTime);
            if (State == EntityStates.Appearing && _animation.IsDone)
            {
                SetDefaultAnimation();
                State = EntityStates.Idle;
            }
        }

        private void SetPosition(Vector2 position, GameTime gameTime)
        {
            if (State != EntityStates.Idle)
                return;

            if (!Visible)
            {
                Visible = true;
                Position = position;
                return;
            }

            State = EntityStates.Moving;
            _positionInterpolator = new PositionInterpolator(Position, position, Configuration.Instance.GameSpeed, gameTime, pa => Position = pa.Current);
        }

        private void SetDefaultAnimation()
            => _animation = _resources.GetAnimation(Type.ToContentName());
        
    }
}
