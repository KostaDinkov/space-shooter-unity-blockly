using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedException :Exception
{
    public PlayerDiedException():base("Player is dead.")
    {
        
    }
}
