using UnityEngine;

public static class Utility
{
    public static float GetSqrDistanceBetween(Collider a, Collider b)
    {
        Vector3 aEdge = a.ClosestPoint(b.transform.position);
        Vector3 bEdge = b.ClosestPoint(a.transform.position);

        aEdge.y = 0;
        bEdge.y = 0;
        float distance = (aEdge - bEdge).sqrMagnitude;

        return distance;
    }
}