using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier 
{
    public static Vector2 EvaluateQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p = Vector2.Lerp(a, b, t);
        Vector2 q = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(p, q, t);
    }
    public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p = EvaluateQuadratic(a, b, c, t);
        Vector2 q = EvaluateQuadratic(b, c, d, t);
        return Vector2.Lerp(p, q, t);
    }
}
