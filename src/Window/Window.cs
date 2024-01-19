using System;
using SFML.Graphics;
using SFML.Window;

public class Window
{
    private RenderWindow _renderWindow;
    private uint _width, _height, _fpsLimit;
    private String _title;

    public Window(uint _width, uint _height, uint _fpsLimit, String _title)
    {
        this._width = _width;
        this._height = _height;
        this._fpsLimit = _fpsLimit;
        this._title = _title;

        System.Threading.Thread windowThread = new System.Threading.Thread(CreateWindow);
        windowThread.Start();
    }

    private void CreateWindow()
    {
        _renderWindow = new RenderWindow(new VideoMode(_width, _height), _title);
        _renderWindow.SetFramerateLimit(_fpsLimit);
        _renderWindow.Closed += OnClosed;

        RunWindow();
    }

    private void RunWindow()
    {
        while (true)
        {
            _renderWindow.DispatchEvents();

            _renderWindow.Clear();

            foreach (var _gameObject in Scene.GameObjects)
            {
                if (_gameObject.GetType() == typeof(GameObject)) (_gameObject as GameObject).Draw(_renderWindow);
            }

            _renderWindow.Display();
        }
    }

    private void OnClosed(object sender, EventArgs e)
    {
        _renderWindow.Close();
    }
}