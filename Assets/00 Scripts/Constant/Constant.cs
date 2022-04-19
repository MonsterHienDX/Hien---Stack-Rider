using UnityEngine;
public static class Constant
{
    // ____STRING KEYS____
    public const string KEY_SAVE_COIN = "KEY_SAVE_COIN";
    public const string KEY_LEVEL = "KEY_LEVEL";

    // ____CHARACTER STATE KEYS____
    public const int LOSE = 0;
    public const int WIN = 1;
    public const int IDLE = 2;
    public const int RUN_BACKWARD = 3;
    public const int RUN_FAST = 4;

    // ____VIBRATE INTENSITY KEYS____
    public const long STRONG_VIBRATE = 90;
    public const long WEAK_VIBRATE = 35;

    // ____STANDART SYSTEM STATS____
    public const int FPS = 80;
    public const int STANDART_SCALE_MAP_LENGTH = 60;   // Sync player speed with any maps (because of using transform.Translate to move)
}
