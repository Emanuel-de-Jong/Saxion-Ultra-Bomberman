using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public static bool train = true;

    public static int characterCount = 2;
    public static int characterWon = 1;

    public static int roundDuration = 90;

    public static GameController gameController;

    
}

public enum Direction
{
    None,
    Forward,
    Back,
    Left,
    Right
}

public enum Action
{
    None,
    Bomb
}