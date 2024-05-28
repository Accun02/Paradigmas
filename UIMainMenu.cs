using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Sdl;

namespace MyGame
{
    public class UIMainMenu
    {
        private string[] menuItems = { "Start", "Tutorial", "Exit" };
        private int selectedItem = 0;
        private bool arrowKeyReleased = true;

        private IntPtr mainMenuScreen = Engine.LoadImage("assets/MainMenu.png");
        private IntPtr cursorImage = Engine.LoadImage("assets/cursor.png");

        private int[] menuItemYPositions = { 440, 520, 610 };
        private int cursorBaseXPosition = 750;
        private float cursorXPosition;
        private float cursorTime = 0f;

        public delegate void MenuAction();
        public event MenuAction OnStartGame;
        public event MenuAction OnTutorial;
        public event MenuAction OnExit;
        public UIMainMenu()
        {
            cursorXPosition = cursorBaseXPosition;
        }
        public void Update()
        {
            if (!Engine.KeyPress(Engine.KEY_UP) && !Engine.KeyPress(Engine.KEY_DOWN))
            {
                arrowKeyReleased = true;
            }
            if (Engine.KeyPress(Engine.KEY_UP) && arrowKeyReleased)
            {
                arrowKeyReleased = false;
                selectedItem = (selectedItem - 1 + menuItems.Length) % menuItems.Length;
            }
            if (Engine.KeyPress(Engine.KEY_DOWN) && arrowKeyReleased)
            {
                arrowKeyReleased = false;
                selectedItem = (selectedItem + 1) % menuItems.Length;
            }
            if (Engine.KeyPress(Engine.KEY_Z) && GameManager.Instance.ZKeyReleased)
            {
                GameManager.Instance.ZKeyReleased = false;
                ExecuteItem();
            }

            cursorTime += Time.DeltaTime;
            cursorXPosition = cursorBaseXPosition + 10f * (float)Math.Sin(5f * cursorTime);
        }
        private void ExecuteItem()
        {
            switch (selectedItem)
            {
                case 0:
                    OnStartGame?.Invoke();
                    GameManager.Instance.LevelController.Restart();
                    break;
                case 1:
                    OnTutorial?.Invoke();
                    break;
                case 2:
                    OnExit?.Invoke();
                    break;
            }
        }
        public void Render()
        {
            int cursorYPosition = menuItemYPositions[selectedItem];

            Engine.Clear();
            Engine.Draw(mainMenuScreen, 0, 0);
            Engine.Draw(cursorImage, (int)cursorXPosition, cursorYPosition);
            Engine.Show();
        }
    }
}