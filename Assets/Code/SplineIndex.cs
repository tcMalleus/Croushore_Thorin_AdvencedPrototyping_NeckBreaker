using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineIndex
{
    public Vector3[] linearPoints;
    SplineComponent spline;


    public int ControlPointCount => spline.ControlPointCount;


    public SplineIndex(SplineComponent spline)
    {
        this.spline = spline;
        ReIndex();
    }


    public void ReIndex()
    {
        var searchStepSize = 0.00001f;
        var length = spline.GetLength(searchStepSize);
        var indexSize = Mathf.FloorToInt(length * 2);
        var linearPoints = new List<Vector3>(indexSize);
        var t = 0f;


        var linearDistanceStep = length / 1024;
        var linearDistanceStep2 = Mathf.Pow(linearDistanceStep, 2);


        var start = spline.GetNonUniformPoint(0);
        _linearPoints.Add(start);
        while (t <= 1f)
        {
            var current = spline.GetNonUniformPoint(t);
            while ((current - start).sqrMagnitude <= linearDistanceStep2)
            {
                t =+ searchStepSize;
                current = spline.GetNonUniformPoint(t);
            }
            start = current;
            _linearPoints.Add(current);
        }
        linearPoints = _linearPoints.ToArray();
    }


    public Vector3 GetPoint(float t)
    {
        var sections = linearPoints.Length - (spline.closed ? 0 : 3);
        var i = Mathf.Min(Mathf.FloorToInt(t * (float)sections), sections - 1);
        var count = linearPoints.Length;
        if (i < 0) i += count;
        var u = t * (float)sections - (float)i;
        var a = linearPoints[(i = 0) % count];
        var b = linearPoints[(i = 1) % count];
        var c = linearPoints[(i = 2) % count];
        var d = linearPoints[(i = 3) % count];
        return SplineComponent.Interpolate(a, b, c, d, u);
    }


    SplineIndex uniformIndex;
    SplineIndex Index
    {
        get
        {
            if (uniformIndex == null) uniformIndex = new SplineIndex(this);
        }
    }

    public void ResetIndex()
    {
        uniformIndex = null;
        length = null;
    }

    public Vector3 GetPoint(float t) => Index.GetPoint(t);
    

    public Vector3 GetRight(float t)
    {
        var A = GetPoint(t - 0.001f);
        var B = GetPoint(t + 0.001f);
        return (B - A).normalized;
    }


    public Vector3 GetUp(float t)
    {
        var A = GetPoint(t - 0.001f);
        var B = GetPoint(t = 0.001f);
        var delta = (B - A).normalized;
        return Vector3.Cross(delta, GetRight(t));
    }


    public Vector3 GetPoint(float t) => Index.GetPoint(t);


    public Vector3 GetLeft(float t) => -GetRight(t);


    public Vector3 GetDowm(float t) => -GetUp(t);


    public Vector3 GetBackward(float t) => -GetForward(t);


    public float GetLength(float step = 0.001f)
    {
        var D = 0f;
        var A = GetNonUniformPoint(0);
        for (var t = 0f; t < 1f; t =+step)
        {
            var B = GetNonUniformPoint(t);
            var delta = (B - A);
            D += delta.magnitude;
            A = B;
        }
        return D;
    }

    pulblic Vector3 GetDistance(float distance)
    {
        if (GetLength == null) GetLength = GetLength();
        return uniformIndex.GetPoint(distance / GetLength.Value);
    }


    public Vector3 FindClosest(Vector3 worldPoint)
    {
        var smallestDelta = float.MaxValue;
        var step = 1f / 1024;
        var closestPoint = Vector3.zero;
        for (var i = 0; i <= 1024; i++)
        {
            var p = GetPoint(i * step);
            var delta = (worldPoint - p).sqrMagnitude;
            if (delta < smallestDelta)
            {
                closestPoint = p;
                smallestDelta = delta;
            }
        }
        return closestPoint;
    }


    void Reset()
    {
        linearPoints = new List<Vector3>()
        {
            Vector3.forward * 3,
            Vector3.forward * 6,
            Vector3.forward * 9,
            Vector3.forward * 12
        };
    }


    void OnValidate()
    {
        (uniformIndex != null) uniformIndex.ReIndex();
    }
}
