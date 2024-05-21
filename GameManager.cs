using System;
using System.Threading;

namespace MyGame
{
    public class GameManager
    {
        private static GameManager instance;
        private LevelController levelController;
        public LevelController LevelController { get { return levelController; } set { levelController = value; } }
        private GameStatus gameStatus = GameStatus.menu;
        private IntPtr mainMenuScreen = Engine.LoadImage("assets/MainMenu.png");
        private IntPtr WinScreen = Engine.LoadImage("assets/Win.png");
        private IntPtr LoseScreen = Engine.LoadImage("assets/Dead.png");

        private bool gameOverDelayStarted = false;
        private bool zKeyReleased = false;
        private float waitForDeath = 2.25f;
        private float currentDeath;

        public bool ZKeyReleased { set { zKeyReleased = value; } get { return zKeyReleased; } }
        public bool GameOverDelayStarted { set { gameOverDelayStarted = value; } get { return gameOverDelayStarted; } }
        public enum GameStatus { menu, game, win, lose }

        public static GameManager Instance
        {
            get
            {
                if (instance == null) { instance = new GameManager(); } return instance;
            }
        }

        public void Initialize() 
        {
            levelController = new LevelController();
            levelController.Initialize();
        }

        public void Update()
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
                case GameStatus.menu:   //Menu

                    if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased) // Entra a Game
                    {
                        levelController.Restart();
                        gameStatus = GameStatus.game;
                        Program.targetFrame = false;
                    }

                break;

                case GameStatus.game:   //Game

                    levelController.Update();

                    if (LevelController.enemy.Health <= 0) // Entra a Win
                    {
                        gameStatus = GameStatus.win;
                    }
                    else if (LevelController.player.Health <= 0) // Espera animación
                    {
                        if (!gameOverDelayStarted)
                        {
                            gameOverDelayStarted = true;
                            currentDeath = 0f;
                        }
                        else
                        {
                            currentDeath += Time.DeltaTime;

                            if (currentDeath >= waitForDeath) // Entra a Defeat
                            {
                                gameStatus = GameStatus.lose;
                            }
                        }
                    }

                break;

                case GameStatus.win:    //Won

                break;

                case GameStatus.lose:   //Defeat

                    if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
                    {
                        zKeyReleased = false;
                        gameOverDelayStarted = false;
                        gameStatus = GameStatus.menu;
                        Program.targetFrame = false;
                    }

                break;
            }
        }

        public void Render()
        {
            switch (gameStatus)
            {
                case GameStatus.menu:   //Main Menu

                    Engine.Clear();
                    Engine.Draw(mainMenuScreen, 0, 0);
                    Engine.Show();

                break;

                case GameStatus.game:   //Game

                    levelController.Render();

                break;

                case GameStatus.win:    //Won

                    Engine.Clear();
                    Engine.Draw(WinScreen, 0, 0);
                    Engine.Show();

                break;

                case GameStatus.lose:   //Defeat

                    Engine.Clear();
                    Engine.Draw(LoseScreen, 0, 0);
                    Engine.Show();

                break;
            }
        }
    }
}
