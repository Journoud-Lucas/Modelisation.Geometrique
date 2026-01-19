using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Chaikin : MonoBehaviour
{
    private List<Vector3> points;
    private int iterations = 10;
    private void OnDrawGizmos()
    {
        points = new List<Vector3>()
{
    new Vector3(5.000f, 2.500f, 0),
    new Vector3(4.268f, 4.268f, 0),
    new Vector3(2.500f, 5.000f, 0),
    new Vector3(0.732f, 4.268f, 0),
    new Vector3(0.000f, 2.500f, 0),
    new Vector3(0.732f, 0.732f, 0),
    new Vector3(2.500f, 0.000f, 0),
    new Vector3(4.268f, 0.732f, 0)
};
        Gizmos.color = Color.white;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[(i + 1) % points.Count];
            Gizmos.DrawLine(start, end);
        }

        // Chaikin
        Gizmos.color = Color.red;

        List<Vector3> refinedPoints = points;
        for (int iteration = 0; iteration < iterations; ++iteration)
        {
            refinedPoints = ChaikinSubdivision(refinedPoints);
        }
        for (int i = 0; i < refinedPoints.Count; ++i)
        {
            Vector3 start = refinedPoints[i];
            Vector3 end = refinedPoints[(i + 1) % refinedPoints.Count];
            Gizmos.DrawLine(start, end);
        }

    }

    private List<Vector3> ChaikinSubdivision(List<Vector3> inputPoints)
    {
        List<Vector3> outputPoints = new List<Vector3>();
        for (int i=0; i < inputPoints.Count; ++i)
        {
            /*
            Q.x = 0.75*x0 + 0.25*x1
            Q.y = 0.75*y0 + 0.25*y1

            R.x = 0.25*x0 + 0.75*x1
            R.y = 0.25*y0 + 0.75*y1 
            */
            Vector3 p0 = inputPoints[i];
            Vector3 p1 = inputPoints[(i + 1) % inputPoints.Count];
            Vector3 Q = new Vector3(
                0.75f * p0.x + 0.25f * p1.x,
                0.75f * p0.y + 0.25f * p1.y,
                0f
            );
            Vector3 R = new Vector3(
                0.25f * p0.x + 0.75f * p1.x,
                0.25f * p0.y + 0.75f * p1.y,
                0f
            );
            outputPoints.Add(Q);
            outputPoints.Add(R);
        }
        return outputPoints;
    }
}
