using System;

namespace MyGame
{
    public class GameManager
    {

        public enum GameStatus
        {
            menu, game, win, lose
        }

        private static GameManager instance;
        private GameStatus gameStatus = GameStatus.menu;  // 0 menu - 1 game - 2 gane - 3 perdi
        private IntPtr mainMenuScreen = Engine.LoadImage("assets/MainMenu.png");

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void Update()
        {
            Time.Update();

            switch (gameStatus)
            {
                case GameStatus.menu:
                    if (Engine.KeyPress(Engine.KEY_Z))
                    {
                        gameStatus = GameStatus.game;
                        Program.targetFrame = false;
                    }
                    break;
                case GameStatus.game:
                    Program.Update();
                    break;
                    //....
            }

        }

        public void Render()
        {
            switch (gameStatus)
            {
                case GameStatus.menu:
                    Engine.Clear();
                    Engine.Draw(mainMenuScreen, 0, 0);
                    Engine.Show();
                    break;
                case GameStatus.game:
                    Program.Render();
                    break;
                    //....
            }

        }

    }
}