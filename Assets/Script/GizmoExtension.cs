using UnityEditor;
using UnityEngine;

public static class GizmoExtension
{
    static public void DrawLine(Vector3 p1, Vector3 p2, float width, Color color)
    {
        Handles.DrawBezier(p1, p2, p1, p2, color, null, width);
    }

    static public void DrawSegment(Vector3 p1, Vector3 p2, float width, float extremitySize, Quaternion extremityRotation, Color color)
    {
        Handles.DrawBezier(p1, p2, p1, p2, color, null, width);

        Vector3 p11 = p1 + extremityRotation * Vector3.up * extremitySize / 2;
        Vector3 p12 = p1 - extremityRotation * Vector3.up * extremitySize / 2;
        Handles.DrawBezier(p11, p12, p11, p12, color, null, width);

        Vector3 p21 = p2 + extremityRotation * Vector3.up * extremitySize / 2;
        Vector3 p22 = p2 - extremityRotation * Vector3.up * extremitySize / 2;
        Handles.DrawBezier(p21, p22, p21, p22, color, null, width);
    }

    static public void DrawArc(Vector3 center, Vector3 start, Vector3 end, float radius, int resolution, float width, Color color)
    {
        Vector3 up = Vector3.Cross(start, end);
        Quaternion startRot = Quaternion.LookRotation(start, up);
        Quaternion endRot   = Quaternion.LookRotation(end,   up);

        for (int i = 1; i < resolution; i++)
        {
            float progress1 = (float)(i-1) / (resolution - 1);
            float progress2 = (float) i    / (resolution - 1);

            Vector3 p1 = center + Quaternion.Lerp(startRot, endRot, progress1) * Vector3.forward * radius;
            Vector3 p2 = center + Quaternion.Lerp(startRot, endRot, progress2) * Vector3.forward * radius;

            DrawLine(p1, p2, width, color);
        }

        DrawLine(center, center + startRot * Vector3.forward * radius, width, color);
        DrawLine(center, center + endRot   * Vector3.forward * radius, width, color);
    }
}
