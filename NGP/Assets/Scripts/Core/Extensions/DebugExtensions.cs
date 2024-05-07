using UnityEngine;

namespace Core.Extensions
{
    public static class EditorOnly
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif // UNITY_EDITOR
        }

        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.Log(message, context);
#endif // UNITY_EDITOR
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif // UNITY_EDITOR
        }

        public static void LogWarning(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message, context);
#endif // UNITY_EDITOR
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif // UNITY_EDITOR
        }

        public static void LogError(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.LogError(message, context);
#endif // UNITY_EDITOR
        }
        
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
#if UNITY_EDITOR
            bool depthTest = true;
            Debug.DrawLine(start, end, color, duration, depthTest);
#endif // UNITY_EDITOR
        }
        
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
#if UNITY_EDITOR
            bool depthTest = true;
            float duration = 0.0f;
            Debug.DrawLine(start, end, color, duration, depthTest);
#endif // UNITY_EDITOR
        }
        
        public static void DrawLine(Vector3 start, Vector3 end)
        {
#if UNITY_EDITOR
            bool depthTest = true;
            float duration = 0.0f;
            Color color = Color.white;
            Debug.DrawLine(start, end, color, duration, depthTest);
#endif // UNITY_EDITOR
        }
    }
}