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
        private IntPtr pauseScreen = Engine.LoadImage("assets/screens/pause.png");
        private IntPtr quitImage = Engine.LoadImage("assets/HUD/quit.png");

        private bool zKeyReleased = false;
        private bool fKeyReleased = false;
        private bool escKeyReleased = false;
        private bool quitHold = false;
        private const float escHoldTime = 0.75f;
        private float escHoldTimer = 0f;

        private bool hardMode = false;
        private bool hardModeWon = false;
        private bool gameOverDelayStarted = false;
        private bool fullScreenToggle = false;
        private float currentDeath;
        private float waitForDeath = 2.25f;

        private bool paused = false;
        private int pauseCounter = 0;

        public delegate void GameStart();
        public delegate void GameWin();
        public delegate void GameLose();
        public delegate void GameOver();

        public event GameWin OnGameWin;
        public event GameLose OnGameLose;
        public event GameOver OnGameOver;

        private Sound music;
        private Sound click;

        public AudioMixer audioMixer;

        public LevelController LevelController
        {
            get => levelController;
            set => levelController = value;
        }
        public bool Paused
        {
            get => paused;
            set => paused = value;
        }
        public int PauseCounter
        {
            get => pauseCounter;
            set => pauseCounter = value;
        }
        public bool ZKeyReleased
        {
            get => zKeyReleased;
            set => zKeyReleased = value;
        }
        public bool HardMode
        {
            get => hardMode;
            set => hardMode = value;
        }
        public bool HardModeWon
        {
            get => hardModeWon;
            set => hardModeWon = value;
        }
        public bool GameOverDelayStarted
        {
            get => gameOverDelayStarted;
            set => gameOverDelayStarted = value;
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public enum GameStatus
        {
            Menu,
            Game,
            Win,
            Lose,
            Tutorial
        }

        public void Initialize()
        {
            audioMixer = new AudioMixer();
            levelController = new LevelController();
            levelController.Initialize();
            mainMenu = new UIMainMenu();
            mainMenu.OnStartGame += GameStartManager;
            mainMenu.OnTutorial += TutorialManager;
            mainMenu.OnExit += ExitManager;
            OnGameWin += GameWinManager;
            OnGameLose += GameLoseManager;
            OnGameOver += GameOverManager;

            music = new Sound("music.wav");
            click = new Sound("clickUI.wav");
        }

        private void GameStartManager()
        {
            Console.WriteLine("Inicio");
            click.PlayOnce(audioMixer.UIClickChannel);
            gameStatus = GameStatus.Game;

            music.PlayOnce(audioMixer.MusicChannel);
        }

        private void GameOverManager()
        {
            Console.WriteLine("Game Over");
            gameStatus = GameStatus.Lose;
        }

        private void GameWinManager()
        {
            Console.WriteLine("Victoria");
            if (hardMode)
            {
                hardModeWon = true;
            }
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
            click.PlayOnce(audioMixer.UIClickChannel);
            gameStatus = GameStatus.Tutorial;
        }

        private void ExitManager()
        {
            Environment.Exit(0);
        }

        public void Update()
        {
            Time.Update();

            if (Engine.KeyPress(Engine.KEY_ESC))
            {
                escHoldTimer += Time.DeltaTime;

                if (escHoldTimer > 0.2)
                {
                    quitHold = true;
                }
                if (escHoldTimer >= escHoldTime)
                {
                    Environment.Exit(0);
                }
            }

            if (Engine.KeyPress(Engine.KEY_ESC) && escKeyReleased && gameStatus == GameStatus.Game && LevelController.player.Health > 0)
            {
                escKeyReleased = false;
                paused = !paused;

                if (paused)
                {
                    Program.targetFrame = true;
                    pauseCounter++;
                }
                else
                {
                    Program.targetFrame = false;
                    pauseCounter = 0;
                }
            }

            if (!Engine.KeyPress(Engine.KEY_ESC))
            {
                quitHold = false;
                escKeyReleased = true;
                escHoldTimer = 0f;
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
                    if (paused)
                        goBackToMenu();
                    UpdateGame();
                    break;
                case GameStatus.Win:        // Victoria
                case GameStatus.Tutorial:  // Tutorial
                case GameStatus.Lose:       // Derrota
                    goBackToMenu();
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
                music.Stop();
                currentDeath += Time.DeltaTime;
                if (currentDeath >= waitForDeath) // Entrar a Derrota
                {
                    OnGameOver?.Invoke();
                }
            }
        }

        private void goBackToMenu()
        {
            if (Engine.KeyPress(Engine.KEY_Z) && zKeyReleased)
            {
                music.Stop();
                click.PlayOnce(audioMixer.UIClickChannel);
                zKeyReleased = false;
                if (gameStatus == GameStatus.Lose)
                    gameOverDelayStarted = false;
                gameStatus = GameStatus.Menu;
                Program.targetFrame = false;
                StartGame();
            }
        }

        private void StartGame()
        {
            escKeyReleased = true;
            paused = false;
            gameOverDelayStarted = false;
            currentDeath = 0f;
            zKeyReleased = false;
        }

        public void Render()
        {
            switch (gameStatus)
            {
                case GameStatus.Menu:   // Menú Principal
                    Engine.Clear();
                    mainMenu.Render();
                    Engine.Show();
                    break;
                case GameStatus.Game:   // Juego
                    Engine.Clear();
                    levelController.Render();
                    if (paused)
                    {
                        Engine.Draw(pauseScreen, 0, 0);
                        BlinkingImage.ShowBlinkingText(288, 593);
                    }
                    if (quitHold)
                    {
                        Engine.Draw(quitImage, 1060, 20);
                    }
                    Engine.Show();
                    break;
                case GameStatus.Win:    // Victoria
                    Engine.Clear();
                    Engine.Draw(winScreen, 0, 0);
                    BlinkingImage.ShowBlinkingText(288, 593);
                    Engine.Show();
                    break;
                case GameStatus.Lose:   // Derrota
                    Engine.Clear();
                    Engine.Draw(loseScreen, 0, 0);
                    BlinkingImage.ShowBlinkingText(288, 593);
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
