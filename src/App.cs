using CeloIsYou.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CeloIsYou
{
    public class App : Game
    {
        private IScene _currentScene;
        private GraphicsDevice _graphicsDevice;
        private Resources _resources;

        public App()
        {
            Content.RootDirectory = "Content";

            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)(Configuration.Instance.CellWidth * Configuration.Instance.GridWidth * Configuration.Instance.RenderFactor),
                PreferredBackBufferHeight = (int)(Configuration.Instance.CellHeight * Configuration.Instance.GridHeight * Configuration.Instance.RenderFactor),
            };

            graphics.ApplyChanges();

            _graphicsDevice = graphics.GraphicsDevice;
            _resources = new Resources(Content);

            _currentScene = new Level(_graphicsDevice, _resources);
        }

        protected override void LoadContent()
        {
            _resources.Load();
            base.LoadContent();
        }

        protected void ReloadLevel(GameTime gameTime)
        {
            _currentScene?.Dispose();
            _currentScene = new Level(_graphicsDevice, _resources);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                ReloadLevel(gameTime);

            _currentScene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _currentScene.Draw(gameTime);
        }
    }
}
