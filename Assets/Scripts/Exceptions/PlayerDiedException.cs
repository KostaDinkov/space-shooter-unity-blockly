using System;

namespace Scripts.Exceptions
{
    public class PlayerDiedException :Exception
    {
        public PlayerDiedException():base("Player is dead.")
        {
        
        }
    }
}
