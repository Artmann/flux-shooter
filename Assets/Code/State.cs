using System;

namespace Code
{
    public abstract class State
    {
        public string id = Guid.NewGuid().ToString();
    }
}