using UnityEngine;

namespace Tannern.Painting
{
    [RequireComponent(typeof(LineRenderer))]
    public class PaintStroke : MonoBehaviour
    {
        void FixedUpdate()
        {
            LineRenderer line = GetComponent<LineRenderer>();
            Vector3[] points = new Vector3[line.numPositions];
            line.GetPositions(points);
            Vector3 origin, direction;
            RaycastHit hitInfo;
            for (int i = 0; i < points.Length-1; i++)
            {
                origin = points[i];
                direction = points[i + 1] - origin;
                if (Physics.Raycast(origin, direction, out hitInfo, direction.magnitude))
                {
                    Debug.DrawRay(origin, direction, Color.magenta);
                    if (hitInfo.collider.gameObject.GetComponent<EraserTag>() != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
