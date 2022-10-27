using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class IK : MonoBehaviour
{
    [SerializeField] Transform[] knees;
    [SerializeField] Transform feet;
    [SerializeField] Transform target;

    List<(float angle, float dist)> anglesDistances = new List<(float, float)>();
    [SerializeField] float angleStart = 0;
    [SerializeField] float angleEnd = 120;
    [SerializeField] float angleStep = 5;

    void OnValidate()
    {
        CacheAnglesDistances();
    }

    void Awake()
    {
        CacheAnglesDistances();
    }

    void Update()
    {
        MatchTarget();
    }

    void CacheAnglesDistances()
    {
        anglesDistances.Clear();

        if (angleStep <= 0 || !feet)
            return;

        for (float angle = angleStart; angle <= angleEnd; angle += angleStep)
            anglesDistances.Add((angle, DistanceToFeet(angle)));

        //PrintAnglesDistances();
    }

    void PrintAnglesDistances()
    {
        print("\nPrintAnglesDistances\n");

        foreach ((float angle, float dist) e in anglesDistances)
            print("Angle : " + e.angle + "\t\tDist : " + e.dist);
    }

    float DistanceToFeet(float angle)
    {
        SetAngle(angle);

        return (transform.position - feet.position).magnitude;
    }

    void SetAngle(float angle)
    {
        foreach (Transform knee in knees)
            knee.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    void MatchTarget()
    {
        if (!feet || !target)
            return;

        Vector3 toTarget = target.position - transform.position;
        float distToTarget = toTarget.magnitude;
        float angle = FindAngle(distToTarget);
        SetAngle(angle);
        transform.forward = toTarget;
        transform.rotation *= Quaternion.Euler(-Vector3.Angle(transform.forward, feet.position - transform.position), 0, 0);
    }

    float FindAngle(float dist)
    {
        if (anglesDistances.Count == 0)
            return 0;

        for (int i = 0; i < anglesDistances.Count; i++)
        {
            (float angle, float dist) e1 = anglesDistances[i];

            if (e1.dist < dist)
            {
                if (i == 0)
                    return e1.angle;

                (float angle, float dist) e2 = anglesDistances[i-1];

                return Mathf.Lerp(e1.angle, e2.angle, Mathf.InverseLerp(e1.dist, e2.dist, dist));
            }
        }

        return anglesDistances.Last().angle;
    }
}
