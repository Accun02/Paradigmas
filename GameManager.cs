﻿using System;

namespace MyGame
{
    public class GameManager
    {
        private static GameManager instance;
        private LevelController levelController;
        private UIMainMenu mainMenu;
        private GameStatus gameStatus = GameStatus.Menu;

        private IntPtr winScreen = Engine.LoadImage("assets/screens/win.png");
        private IntPtr loseScreen = Engine.LoadImage("assets/screens/dead.png");
        private IntPtr tutorialScreen = Engine.LoadImage("assets/screens/tutorial.png");

        private bool zKeyReleased = false;
        private bool fKeyReleased = false;
        private bool gameOverDelayStarted = false;
        private bool fullScreenToggle = false;
        private float currentDeath;
        private float waitForDeath = 2.25f;


        public delegate void GameStart();
        public delegate void GameWin();
        public delegate void GameLose();
        public delegate void GameOver();

        public event GameWin OnGameWin;
        public event GameLose OnGameLose;
        public event GameOver OnGameOver;
        public LevelController LevelController { get => levelController; set => levelController = value; }
        public bool ZKeyReleased { get => zKeyReleased; set => zKeyReleased = value; }
        public bool GameOverDelayStarted { get => gameOverDelayStarted; set => gameOverDelayStarted = value; }
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }
        public enum GameStatus { Menu, Game, Win, Lose, Tutorial }
        public void Initialize()
        {
            levelController = new LevelController();
            levelController.Initialize();
            mainMenu = new UIMainMenu();
            mainMenu.OnStartGame += GameStartManager;
            mainMenu.OnTutorial += TutorialManager;
            mainMenu.OnExit += ExitManager;
            OnGameWin += GameWinManager;
            OnGameLose += GameLoseManager;
            OnGameOver += GameOverManager;
        }
        private void GameStartManager()
        {
            Console.WriteLine("Inicio");
            gameStatus = GameStatus.Game;
        }
        private void GameOverManager()
        {
            Console.WriteLine("Game Over");
            gameStatus = GameStatus.Lose;
        }
        private void GameWinManager()
        {
            Console.WriteLine("Victoria");
            gameStatus = GameStatus.Win;
        }
        private void GameLoseManager()
        {
            Console.WriteLine("Muerto");
            gameOverDelayStarted = true;
            currentDeath = 0f;
        }
        private void TutorialManager()
        {
            Console.WriteLine("Tutorial");
            gameStatus = GameStatus.Tutorial;
        }
        private void ExitManager()
        {
            Environment.Exit(0);
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
            if (Engine.KeyPress(Engine.KEY_F) && fKeyReleased)
            {
                fKeyReleased = false;
                fullScreenToggle = !fullScreenToggle;
                Engine.ToggleFullScreen(fullScreenToggle);
            }
            if (!Engine.KeyPress(Engine.KEY_F))
            {
                fKeyReleased = true;
            }

            switch (gameStatus)
            {
                case GameStatus.Menu:   // Menú
                    mainMenu.Update();
                    break;
                case GameStatus.Game:   // Juego
                    UpdateGame();
                    break;
                case GameStatus.Win:    // Victoria
                    UpdateWin();
                    break;
                case GameStatus.Lose:   // Derrota
                    UpdateLose();
                    break;
                case GameStatus.Tutorial:   // Tutorial
                    UpdateTutorial();
                    break;
            }
        }
        private void UpdateGame()
        {
            levelController.Update();
            if (LevelController.enemy.Health <= -1) // Entrar a Victoria
            {
                OnGameWin?.Invoke();
            }
            else if (LevelController.player.Health <= 0 && !gameOverDelayStarted) // Esperar animación
            {
                OnGameLose?.Invoke();
            }
            if (gameOverDelayStarted)
            {
                currentDeath += Time.DeltaTime;
                if (currentDeath >= waitForDeath) // Entrar a Derrota
                {
                    OnGameOver?.Invoke();
                }
            }
        }
        private void UpdateWin()
        {
            if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
            {
                zKeyReleased = false;
                gameStatus = GameStatus.Menu;
                Program.targetFrame = false;
                StartGame();
            }
        }
        private void UpdateLose()
        {
            if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
            {
                zKeyReleased = false;
                gameOverDelayStarted = false;
                gameStatus = GameStatus.Menu;
                Program.targetFrame = false;
                StartGame();
            }
        }
        private void UpdateTutorial()
        {
            if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
            {
                zKeyReleased = false;
                gameStatus = GameStatus.Menu;
                Program.targetFrame = false;
                StartGame();
            }
        }
        private void StartGame()
        {
            gameOverDelayStarted = false;
            currentDeath = 0f;
            zKeyReleased = false;
        }
        public void Render()
        {
            switch (gameStatus)
            {
                case GameStatus.Menu:   // Menú Principal
                    mainMenu.Render();
                    break;
                case GameStatus.Game:   // Juego
                    levelController.Render();
                    break;
                case GameStatus.Win:    // Victoria
                    Engine.Clear();
                    Engine.Draw(winScreen, 0, 0);
                    Engine.Show();
                    break;
                case GameStatus.Lose:   // Derrota
                    Engine.Clear();
                    Engine.Draw(loseScreen, 0, 0);
                    Engine.Show();
                    break;
                case GameStatus.Tutorial:   // Tutorial
                    Engine.Clear();
                    Engine.Draw(tutorialScreen, 0, 0);
                    Engine.Show();
                    break;
            }
        }
    }
}
