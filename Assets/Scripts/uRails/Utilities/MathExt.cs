using Squid;
using UnityEngine;


public static class MathExt
{
    public static Vector3 CatmullRom(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float time)
    {
        return 0.5f * (
                      (2 * p2) +
                      (-p1 + p3) * time +
                      (2 * p1 - 5 * p2 + 4 * p3 - p4) * time * time +
                      (-p1 + 3 * p2 - 3 * p3 + p4) * time * time * time);
    }

    public static Vector3 CatmullRom2(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float time, float tau)
    {
        Vector3 value = p2 +
                        (-tau * p1 + tau * p3) * time +
                        (2 * tau * p1 + (tau - 3) * p2 + (3 - 2 * tau) * p3 + -tau * p4) * time * time +
                        (-tau * p1 + (2 - tau) * p2 + (tau - 2) * p3 + tau * p4) * time * time * time;

        return value;
    }

    public static float Distance(this Point p1, Point p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }

    public static Point SmoothStepPoint(Point p1, Point p2, float t)
    {
        return new Point(
            (int)Mathf.SmoothStep(p1.x, p2.x, t),
            (int)Mathf.SmoothStep(p1.y, p2.y, t));
    }

    public static void GetPitchAndYaw(Vector3 center, Vector3 orbiter, out float pitch, out float yaw)
    {
        Vector3 directionOnXZ = Vector3.ProjectOnPlane(orbiter - center, Vector3.up).normalized;
        yaw = 180f * Mathf.Acos(Vector3.Dot(Vector3.forward, directionOnXZ)) / Mathf.PI;
        if (Vector3.Dot(directionOnXZ, Vector3.right) < 0)
            yaw = 360 - yaw;

        pitch = 180 * Mathf.Acos((orbiter - center).normalized.y) / Mathf.PI - 90;
    }

    public static Vector3 GetPositionFromPitchAndYaw(Vector3 center, float distance, float pitch, float yaw)
    {
        Quaternion rotation = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right);

        return rotation * (distance * Vector3.forward) + center;
    }
}

