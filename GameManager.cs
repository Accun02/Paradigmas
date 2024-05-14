using System;
using System.Threading;

namespace MyGame
{
    public class GameManager
    {
        public enum GameStatus
        {
            menu, game, win, lose
        }

        private static GameManager instance;
        private GameStatus gameStatus = GameStatus.menu;
        private IntPtr mainMenuScreen = Engine.LoadImage("assets/MainMenu.png");
        private IntPtr WinScreen = Engine.LoadImage("assets/Win.png");
        private IntPtr LoseScreen = Engine.LoadImage("assets/Dead.png");
        private Enemy enemy;
        private Character player;
        private bool gameOverDelayStarted = false;
        private bool zKeyReleased = false;
        public bool ZKeyReleased { set { zKeyReleased = value; } get { return zKeyReleased; } }
        public bool GameOverDelayStarted { set { gameOverDelayStarted = value; } get { return gameOverDelayStarted; } }
        private float gameOverDelayTime = 3f;
        private float currentGameOverDelayTime = 0f;

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

        public void Update(Enemy enemy, Character player)
        {
            Time.Update();

            if (Engine.KeyPress(Engine.KEY_ESC)) // Escape
            {
                Environment.Exit(0);
            }

            if (!Engine.KeyPress(Engine.KEY_Z))  // Registra "KeyUp"
            {
                zKeyReleased = true;
            }

            switch (gameStatus)
            {
                case GameStatus.menu:
                    if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
                    {
                        zKeyReleased = false;
                        gameStatus = GameStatus.game;
                        Program.targetFrame = false;
                    }
                    break;
                case GameStatus.game:
                    Program.Update();

                    if (enemy.Health < 0)
                    {
                        gameStatus = GameStatus.win;
                    }
                    else if (player.Health <= 0)
                    {
                        if (!gameOverDelayStarted)
                        {
                            gameOverDelayStarted = true;
                            currentGameOverDelayTime = 0f;
                        }
                        else
                        {
                            currentGameOverDelayTime += Time.DeltaTime;
                            if (currentGameOverDelayTime >= gameOverDelayTime)
                            {
                                gameStatus = GameStatus.lose;
                            }
                        }
                    }
                    break;
                case GameStatus.win:

                    break;
                case GameStatus.lose:
                    if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
                    {
                        zKeyReleased = false;
                        gameOverDelayStarted = false;
                        gameStatus = GameStatus.menu;
                        Program.Restart(player, enemy);
                        Program.targetFrame = false;
                    }
                    break;
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
                case GameStatus.win:
                    Engine.Clear();
                    Engine.Draw(WinScreen, 0, 0);
                    Engine.Show();
                    break;
                case GameStatus.lose:
                    Engine.Clear();
                    Engine.Draw(LoseScreen, 0, 0);
                    Engine.Show();
                    break;
            }

        }

    }
}
