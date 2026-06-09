using UnityEngine;

public static class InputDelayManager
{
    private static float lockEndTime;

    public static void Lock(float seconds)
    {
        lockEndTime = Time.unscaledTime + seconds;
    }

    public static bool  IsLocked()
    {
        return Time.unscaledTime < lockEndTime;
    }
}