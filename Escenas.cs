using System;

public class Escenas
{
	IntPtr menu = Engine.LoadImage("assets/wea_fome.png");
    IntPtr tqm= Engine.LoadImage("assets/fondo.png");
    private int width = 100;
	private int height = 100;

   public int numPantalla = 1;
    // cambia la escena al cambiar el numero 0 = inicio 1 = pantalla de juego 2 = pantalla de victoria y 3 = a pantalla de derrota

	private void dontcrash ()
	{
		if (numPantalla < 0)
		{
			numPantalla = 0;
		}
	}
		public  void Render()
		{
		if (numPantalla == 0) //renderiza el menu
		{
			Engine.Draw(menu,100,100);

        }
		if (numPantalla == 1) // renderiza el juego recorda poner que sea al apretar enter.
		{
			Engine.Draw(tqm,100,100);
		}
		
		}


	
}
