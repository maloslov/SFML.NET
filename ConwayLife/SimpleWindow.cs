using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace ConwayLife
{
    public class SimpleWindow
    {
        public SimpleWindow()
        {
            _window = new RenderWindow(
                new VideoMode(800, 400),
                "Conway's Game of Life"
            );
            _window.Closed += OnClosed;
            _window.KeyPressed += OnKeyPressed;
            _window.MouseButtonPressed += OnMouseClicked;

            _cell_size = new Vector2u(5, 5);
            _life = new LifeGame(
                (int)(_window.Size.Y / _cell_size.Y),
                (int)(_window.Size.X / _cell_size.X),
                mod: 3 );
            _timer = new System.Timers.Timer
            {
                Interval=50,
                Enabled = true,
                AutoReset = true,
            };
            _timer.Elapsed += (object? sender, ElapsedEventArgs e) => { _life.Iterate(); };
            _timer.Start();
        }

        private Vector2u _cell_size { get; set; }
        private RenderWindow _window { get; set; }
        private LifeGame _life {  get; set; }
        private System.Timers.Timer _timer {  get; set; }


        public void RunLife()
        { 
            var _map = _life.GetMap();

            do
            {
                _window.Clear();
                _window.DispatchEvents();
                _map = _life.GetMap();
                for (int i = 0; i < _life.GetWidth(); i++)
                    for (int j = 0; j < _life.GetHeight(); j++)
                        _window.Draw(new SFML.Graphics.CircleShape()// RectangleShape()
                        {
                            Position = new Vector2f(i * _cell_size.X, j * _cell_size.Y),
                            //Size = new Vector2f(_cell_size.X, _cell_size.Y),
                            Radius = _cell_size.X/2,
                            FillColor = _map[i + _life.GetWidth() * j] ? Color.Green : Color.Black,
                        });
                _window.Display();
            }
            while (_window.IsOpen);
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
#if DEBUG
            Console.WriteLine($"OnKeyPressed: {e.Code}");
#endif
            if (sender == null) return;
            if (e.Code == Keyboard.Key.Space)
                _life.Paused = !_life.Paused;
            if (e.Code == Keyboard.Key.Enter)
                _life.SetMapRandom(4);
        }

        private void OnMouseClicked(object? sender, MouseButtonEventArgs e)
        { 
#if DEBUG
            Console.WriteLine($"OnKeyPressed: {e}");
#endif
            if (sender == null) return;
            var coor = new Vector2i(
                Convert.ToInt32(Math.Round((decimal)(e.X / _cell_size.X), MidpointRounding.ToPositiveInfinity)),
                Convert.ToInt32(Math.Round((decimal)(e.Y/_cell_size.Y), MidpointRounding.ToPositiveInfinity))
                );
            _life.ChangePoint(coor.X, coor.Y);


        }

        private void OnClosed(object? sender, EventArgs e)
        {
#if DEBUG 
            Console.WriteLine($"OnClosed: {e}");
#endif
            if (sender == null) return;
            var window = (SFML.Window.Window)sender;
            window.Close();
        }
    }
}