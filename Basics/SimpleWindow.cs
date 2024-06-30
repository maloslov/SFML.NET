using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace Basics
{
    public class SimpleWindow
    {
        public SimpleWindow()
        {
            WINDOW_SIZE = (800, 800);
            _window = new RenderWindow(
                new VideoMode(WINDOW_SIZE.Item1, WINDOW_SIZE.Item2),
                "SFML works!"
            );
            _circle = new CircleShape
            {
                FillColor = Color.Yellow,
                Radius = 100f
            };
            _shapes = new List<Shape>
            {
                _circle
            };
            _timer_ui = new System.Timers.Timer
            {
                Interval = 50,
                Enabled = true,
                AutoReset = true
            };
            MOVE_SPEED = 10.0f;
            _inputKeys = new List<Keyboard.Key>();
            _ui_controls = new Dictionary<string, List<Keyboard.Key>>
            {
                {"ui_up", new List<Keyboard.Key>{ Keyboard.Key.Up, Keyboard.Key.W } },
                {"ui_down", new List<Keyboard.Key>{ Keyboard.Key.Down, Keyboard.Key.S } },
                {"ui_left", new List<Keyboard.Key>{ Keyboard.Key.Left, Keyboard.Key.A } },
                {"ui_right", new List<Keyboard.Key>{ Keyboard.Key.Right, Keyboard.Key.D } },
                {"ui_esc", new List<Keyboard.Key>{ Keyboard.Key.Escape } },
            };
            ConfigEvents();
        }

        private CircleShape _circle { get; set; }
        private List<Shape> _shapes { get; set; }
        private System.Timers.Timer _timer_ui { get; set; }
        private Dictionary<string, List<Keyboard.Key>> _ui_controls { get; set; }
        private List<Keyboard.Key> _inputKeys { get; set; }
        private RenderWindow _window { get; set; }
        private float MOVE_SPEED { get; set; }
        private (uint, uint) WINDOW_SIZE { get; set; }

        public void Run()
        {
            _timer_ui.Start();
            

            while (_window.IsOpen)
            {
                _window.Clear();
                _window.DispatchEvents();
                DrawShapes();
                _window.Display();

                Thread.Sleep(10);
            }   
        }

        private void ConfigEvents()
        {
            _window.Closed += Window_OnClosed;
            _window.Resized += Window_OnResized;
            _window.KeyPressed += Window_OnKeyPressed;
            _window.KeyReleased += Window_OnKeyReleased;

            _timer_ui.Elapsed += CheckMovement;
        }

        private void DrawShapes()
        {
            foreach (var shape in _shapes)
            {
                shape.Draw(_window, RenderStates.Default);
                _window.SetTitle(shape.Position.ToString());
            }
        }

        private void CheckMovement(object? sender, ElapsedEventArgs e)
        {
            var movement = new Vector2f();

            foreach (var key in _inputKeys)
            {
                //MOVEMENT KEYS
                if (_ui_controls["ui_up"].Contains(key))
                    movement.Y += -MOVE_SPEED;
                if (_ui_controls["ui_down"].Contains(key))
                    movement.Y += MOVE_SPEED;
                if (_ui_controls["ui_left"].Contains(key))
                    movement.X += -MOVE_SPEED;
                if (_ui_controls["ui_right"].Contains(key))
                    movement.X += MOVE_SPEED;
                //SPECIAL KEYS
                if (_ui_controls["ui_esc"].Contains(key))
                    _window.Close();
            }

            _circle.Position += movement;
        }

        private void Window_OnKeyPressed(object? sender, SFML.Window.KeyEventArgs e)
        {
#if DEBUG 
            Console.WriteLine($"KeyPressed: {e.Code}");
#endif
            if (sender == null) return;
            if (!_inputKeys.Contains(e.Code))
                _inputKeys.Add(e.Code);
        }

        private void Window_OnKeyReleased(object? sender, SFML.Window.KeyEventArgs e)
        {
#if DEBUG 
            Console.WriteLine($"KeyReleased: {e.Code}");
#endif
            if (sender == null) return;
            if (_inputKeys.Contains(e.Code))
                _inputKeys.Remove(e.Code);
        }

        private void Window_OnClosed(object? sender, EventArgs e)
        {
#if DEBUG 
            Console.WriteLine($"OnClosed: {e}");
#endif
            if (sender == null) return;
            var window = (SFML.Window.Window)sender;
            window.Close();
        }

        private void Window_OnResized(object? sender, SizeEventArgs e)
        {
#if DEBUG 
            Console.WriteLine($"OnResized: {e}");
#endif
            var center = new Vector2u(
                _window.Size.X / 2,
                _window.Size.Y / 2
                );
            var visibleArea = new FloatRect(
                center.X - e.Width / 2,
                center.Y - e.Height / 2,
                center.X + e.Width / 2,
                center.Y + e.Height / 2
                );
            _window.SetView(new View(visibleArea));
        }
    }
}