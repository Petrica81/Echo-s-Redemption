using UnityEngine;

public static class DebugDraw
{
    public static void DrawCircle(Vector3 position, float radius, Color color, int segments = 6)
    {
        float angleDifference = 360f/segments;
        Vector3 previousPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleDifference);
            Vector3 currentPoint = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + position;
            
            if (i > 0)
                Debug.DrawLine(previousPoint, currentPoint, color);

            previousPoint = currentPoint;
        }
    }
}
