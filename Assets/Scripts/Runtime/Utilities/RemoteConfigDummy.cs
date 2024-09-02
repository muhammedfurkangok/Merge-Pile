using System.Collections.Generic;
using System.Linq;

public static class RemoteConfigDummy
{
    public static List<int> levels = new List<int> {1, 2, 3, 4, 5 ,};
    public static List<int> timers = new List<int> {60, 80, 90, 100, 120,};
    public static List<int> moves = new List<int>  {30, 35, 40, 45, 50,};

    public const int LevelLoopStart = 2;    
    public const int DefaultTimer = 60;
    public const int defaultMoveCounter = 30;
    
    public static bool hasTimer = false;
    public static bool hasMoveCounter = false;
    
}