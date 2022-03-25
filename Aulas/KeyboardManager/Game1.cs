using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

namespace KeyboardManager
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardManager km;
        string levelPath = "Content/SokobanLevels/level1.txt";
        char[,] map;
        const int tileSize = 64;
        private int width, height;
        private Texture2D playerTexture, wallTexture, groundTexture, boxTexture, objectiveTexture;

        //Player
        Vector2 playerPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            km = new KeyboardManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("Character4");
            wallTexture = Content.Load<Texture2D>("Wall_Black");
            groundTexture = Content.Load<Texture2D>("GroundGravel_Grass");
            boxTexture = Content.Load<Texture2D>("Crate_Beige");
            objectiveTexture = Content.Load<Texture2D>("EndPoint_Black");

            LoadLevel();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            km.Update();

            Movement();

            Vector2 teste = playerPos;
            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    char currentSymbol = map[x, y];
                    switch (currentSymbol)
                    {
                        case 'X':
                            _spriteBatch.Draw(wallTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case ' ':
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case 'B':
                            _spriteBatch.Draw(boxTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case '.':
                            _spriteBatch.Draw(objectiveTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        default:
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                    }
                }

            _spriteBatch.Draw(playerTexture, playerPos * tileSize, Color.White);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        void LoadLevel()
        {
            string[] lines = File.ReadAllLines(levelPath);
            map = new char[lines[0].Length, lines.Length];

            for(int x=0;x<lines[0].Length;x++)
            {
                for(int y=0;y<lines.Length;y++)
                {
                    string currentLine = lines[y];
                    map[x, y] = currentLine[x];

                    if (currentLine[x] == 'i')
                        playerPos = new Vector2(x, y);
                }
            }

            height = lines.Length;
            width = lines[0].Length;
            _graphics.PreferredBackBufferHeight = height * tileSize;
            _graphics.PreferredBackBufferWidth = width * tileSize;
            _graphics.ApplyChanges();
        }

        void Movement()
        {
            Vector2 newPos = playerPos;
            Vector2 dir = Vector2.Zero;

            if (km.IsKeyPressed(Keys.W))
            {
                newPos -= Vector2.UnitY;
                dir = -Vector2.UnitY;
            }
                
            if (km.IsKeyPressed(Keys.S))
            {
                newPos += Vector2.UnitY;
                dir = Vector2.UnitY;
            }
                
            if (km.IsKeyPressed(Keys.A))
            {
                newPos -= Vector2.UnitX;
                dir = -Vector2.UnitX;
            }
                
            if (km.IsKeyPressed(Keys.D))
            {
                newPos += Vector2.UnitX;
                dir = Vector2.UnitX;
            }
                

            if (map[(int)newPos.X, (int)newPos.Y] == 'X')
                newPos = playerPos;

            else if(map[(int)newPos.X, (int)newPos.Y] == 'B')
            {
                if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == ' ' || map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.')
                {
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                    map[(int)(newPos.X), (int)(newPos.Y)] = ' ';
                    
                }
                else newPos = playerPos;
            }

            map[(int)playerPos.X, (int)playerPos.Y] = ' ';
            playerPos = newPos;
            map[(int)playerPos.X, (int)playerPos.Y] = 'i';
        }
    }
}
