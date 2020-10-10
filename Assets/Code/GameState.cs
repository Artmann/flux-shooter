using System;
using System.Collections.Generic;

namespace Code
{
    [Serializable]
    public class GameState
    {
        public List<PlayerState> players = new List<PlayerState>();
    }
}