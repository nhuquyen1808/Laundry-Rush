using UnityEngine;

public static class VibrationManager
{
    public static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (vibrator == null) return;

            AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
            int sdkVersion = buildVersion.GetStatic<int>("SDK_INT");

            if (sdkVersion >= 26)
            {
                AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, 255);
                vibrator.Call("vibrate", effect);
            }
            else
            {
                vibrator.Call("vibrate", milliseconds);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Vibration failed: " + e.Message);
        }
#elif UNITY_IOS
        // iOS không hỗ trợ custom thời gian rung trực tiếp qua Unity
        Handheld.Vibrate();
#else
        Debug.Log("Vibration called, but not supported on this platform.");
#endif
    }
}