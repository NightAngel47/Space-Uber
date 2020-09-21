using UnityEngine;

namespace ACTools
{
    namespace General
    {
        [AddComponentMenu("ACTools/General/Grid System")]
        public class GridSystem : MonoBehaviour
        {
            [SerializeField] private float size = 1f;   // Distance between points on grid.
            public float Size => size;                  // Property for size.

            [SerializeField] private GridAxes axis1 = GridAxes.x;   // One axis to draw on.
            [SerializeField] private GridAxes axis2 = GridAxes.z;   // The other axis to draw on.

            [SerializeField] private Color gizmoColor = Color.yellow;           // Color of the gizmo.
            [Range(.1f, 1f)] [SerializeField] private float alpha = .5f;        // The value of the alpha of the gizmo color.
            [Range(.05f, .25f)] [SerializeField] private float gizmoSize = .1f; // Size of the gizmo points.
            [Range(1f, 50f)] [SerializeField] private float gizmoLength = 20;   // Length the gizmo will be drawn each way long the axis.

            // Booleans for tracking which axes to draw on.
            private bool axisOnX = true;
            private bool axisOnY = false;
            private bool axisOnZ = true;

            /// <summary> Finds the nearest point on this grid from the given point. </summary>
            /// <param name="position"> The original position. </param>
            /// <returns> The closet position on the grid. </returns>
            public Vector3 GetNearestPointOnGrid(Vector3 position)
            {
                position -= transform.position;

                int xCount = axisOnX ? Mathf.RoundToInt(position.x / size) : 0;
                int yCount = axisOnY ? Mathf.RoundToInt(position.y / size) : 0;
                int zCount = axisOnZ ? Mathf.RoundToInt(position.z / size) : 0;

                Vector3 result = new Vector3(xCount * size, yCount * size, zCount * size);

                result += transform.position;

                return result;
            }

            /// <summary> Calculates the distance between the center of the grid and the gizmo that is drawn furthest away. </summary>
            /// <returns> Returns the distance of the gizmo that is drawn furthest away from the center. </returns>
            public float DrawDistance()
            {
                return gizmoLength - (gizmoLength % size);
            }

            /// <summary> Determines if this grid is going along a given axis. </summary>
            /// <param name="axis"> The axis to be checked. </param>
            /// <returns> Returns true if this grid is going along the given axis. </returns>
            public bool IsAlongAxis(GridAxes axis)
            {
                return (axis1 == axis || axis2 == axis);
            }

            private void OnDrawGizmos()
            {
                Gizmos.color = gizmoColor;

                float drawnGizmoLength = DrawDistance();

                for (float x = axisOnX ? -drawnGizmoLength : 0f; x < (axisOnX ? drawnGizmoLength + 1 : .1f); x += size)
                {
                    for (float y = axisOnY ? -drawnGizmoLength : 0f; y < (axisOnY ? drawnGizmoLength + 1 : .1f); y += size)
                    {
                        for (float z = axisOnZ ? -drawnGizmoLength : 0f; z < (axisOnZ ? drawnGizmoLength + 1 : .1f); z += size)
                        {
                            Vector3 point = GetNearestPointOnGrid(new Vector3(transform.position.x + x,
                                                                              transform.position.y + y,
                                                                              transform.position.z + z));
                            Gizmos.DrawSphere(point, gizmoSize);
                        }
                    }
                }
            }

            private void OnValidate()
            {
                if (size < .1f)
                    size = .1f;

                gizmoColor.ChangeAlphaTo(alpha);

                axisOnX = IsAlongAxis(GridAxes.x);
                axisOnY = IsAlongAxis(GridAxes.y);
                axisOnZ = IsAlongAxis(GridAxes.z);
            }
        }

        /// <summary> Enumerations for the axis for the grid. </summary>
        public enum GridAxes
        {
            x,
            y,
            z,
        }
    }
}