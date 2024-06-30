namespace ConwayLife
{
    internal class LifeGame
    {
        public bool Paused = true;
        private bool[] _map;
        private bool[] _buf;
        private bool[] _prev;
        private bool[] _prev_prev;
        private int _height;
        private int _width;

        public LifeGame(int height, int width, int mod=3)
        {
            _height = height;
            _width = width;
            _map = new bool[_height * _width];
            _buf = new bool[_height * _width];
            _prev = new bool[_height * _width];
            _prev_prev = new bool[_height * _width];

            SetMapRandom(mod);
        }

        public void Iterate()
        {
            if(Paused) return;

            for (int i = 0; i < _width * _height; i++)
            {
                _prev_prev[i] = _prev[i];
                _prev[i] = _buf[i];
            }

            for(int i = 0 ; i < _width*_height; i++)
            {
                _buf[i] = _isAlive(i, _map[i]);
            }


            for (int i = 0; i < _width * _height; i++)
            {
                _map[i] = _buf[i];
            }
        }

        private bool _isAlive(int i, bool isAlive)
        {
            var a = 0;

            a += (i > 0) ? (_map[i - 1] ? 1 : 0) : 0;
            a += (i + 1 < _width * _height) ? (_map[i + 1] ? 1 : 0) : 0;

            a += (i - _width > 0) ? (_map[i - _width] ? 1 : 0) : 0;
            a += (i - _width - 1 > 0) ? (_map[i - _width - 1] ? 1 : 0) : 0;
            a += (i - _width + 1 > 0) ? (_map[i - _width + 1] ? 1 : 0) : 0;

            a += (i + _width < _width * _height - 1) ? (_map[i + _width] ? 1 : 0) : 0;
            a += (i + _width - 1 < _width * _height - 1) ? (_map[i + _width - 1] ? 1 : 0) : 0;
            a += (i + _width + 1 < _width * _height - 1) ? (_map[i + _width + 1] ? 1 : 0) : 0;
            return isAlive ? (a == 3 || a == 2) : a == 3;
        } 

        public bool IsActive()
        {
            var a = false;
            var b = false;
            for (int i = 0; i < _width*_height; i++)
            {
                if (_map[i])
                {
                    a = true;
                    break;
                }
            }

            for(int i = 0; i < _width * _height; i++)
            {
                if (_prev_prev[i] != _map[i])
                {
                    b = true;
                }
            }

            return a && b;
        }

        public void SetMap(bool[] new_map, int width, int height)
        {
            _map = new_map;
            UpdateSize(width, height);
        }

        public void SetMapRandom(int mod)
        {
            var rand = new Random();

            for (int i =0;i< _buf.Length;i++)
            {
                _buf[i] = rand.Next() % mod == 1 ? true : false;
            }

            for (int i = 0; i < _width*_height; i++)
                _map[i] = _buf[i];
        }

        public void SetPoint(int index, bool val)
        {
            _map[index] = val;
        }

        public void ChangePoint(int x, int y)
        {
            if (x*y < _map.Length)
                _map[x + _width * y] = !_map[x + _width * y];
        }

        public void UpdateSize(int width, int height)
        {
            _width = width;
            _height = height;
            _buf = new bool[_width * _height];

            for(int i = 0; i < _width*_height; i++)
            {
                _buf[i] = _map[i];
            }

            for (int i = 0; i < _width * _height; i++)
                _map[i] = _buf[i];
        }

        public bool[] GetMap()
        {
            return _map;
        }

        public int GetHeight()
        {
            return _height;
        }
        public int GetWidth()
        {
            return _width;
        }
    }
}
