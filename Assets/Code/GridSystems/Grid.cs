using System.Collections.Generic;
namespace Assets.Code.GridSystems
{
    public class Grid
    {
        public int X;
        public int Y;
        public bool Walkable = true;
        public List<Grid> Adjents = new List<Grid>();
        public bool Visited = false;
        public Grid Parent = null;
        public int Distance = 0;
        private GridStatus _status;
        public GridStatus GridStatus
        {
            get => _status;
            set
            {
                if (_status == GridStatus.Current && (value != GridStatus.None || value!=GridStatus.Target)) return;
                _status = value;
            }
        }
        public Grid(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Clear()
        {
            _status = GridStatus.None;
            Adjents = null;
            Parent = null;
            Visited = false;
            Distance = 0;
        }
    }
}