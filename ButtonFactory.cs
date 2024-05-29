using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public enum ButtonType
    {
        Start,
        Tutorial,
        Exit
    }
    public enum ButtonState
    {
        On,
        Off
    }

    public class ButtonFactory
    {
        private Dictionary<ButtonType, string> baseSpritePaths = new Dictionary<ButtonType, string>
        {
            { ButtonType.Start, "assets/mainMenu/start_" },
            { ButtonType.Tutorial, "assets/mainMenu/tutorial_" },
            { ButtonType.Exit, "assets/mainMenu/exit_" }
        };

        private Dictionary<ButtonType, Dictionary<ButtonState, string>> spritePaths = new Dictionary<ButtonType, Dictionary<ButtonState, string>>();

        public ButtonFactory()
        {
            foreach (ButtonType type in Enum.GetValues(typeof(ButtonType)))
            {
                var paths = new Dictionary<ButtonState, string>();
                switch (type)
                {
                    case ButtonType.Start:
                        paths.Add(ButtonState.On, baseSpritePaths[type] + "on.png");
                        paths.Add(ButtonState.Off, baseSpritePaths[type] + "off.png");
                        break;
                    case ButtonType.Tutorial:
                        paths.Add(ButtonState.On, baseSpritePaths[type] + "on.png");
                        paths.Add(ButtonState.Off, baseSpritePaths[type] + "off.png");
                        break;
                    case ButtonType.Exit:
                        paths.Add(ButtonState.On, baseSpritePaths[type] + "on.png");
                        paths.Add(ButtonState.Off, baseSpritePaths[type] + "off.png");
                        break;
                }
                spritePaths.Add(type, paths);
            }
        }

        public Button CreateButton(ButtonType type, Vector2 position)
        {
            string imagePath = spritePaths[type][ButtonState.On];
            return new Button(position, imagePath);
        }

        public Button CreateButton(ButtonType type, ButtonState state, Vector2 position)
        {
            string imagePath = spritePaths[type][state];
            return new Button(position, imagePath);
        }
    }
}
