using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungAtt1
{
    public class Voxel : MonoBehaviour
    {
        private static Vector3[] corners = new Vector3[]
        {
        new Vector3(1, 1, 1),   // 0 E U N
        new Vector3(-1, 1, 1),  // 1 W U N
        new Vector3(-1, -1, 1), // 2 W D N
        new Vector3(1, -1, 1),  // 3 E D N
        new Vector3(1, 1, -1),  // 4 E U S
        new Vector3(-1, 1, -1), // 5 W U S
        new Vector3(-1, -1, -1),// 6 W D S
        new Vector3(1, -1, -1), // 7 E D S
        };

        public static Vector3[] Corners(FaceDirection direction, float scale)
        {
            switch (direction)
            {
                case FaceDirection.Up:
                    return new Vector3[] { corners[1] * scale, corners[5] * scale, corners[4] * scale, corners[0] * scale };
                case FaceDirection.Down:
                    return new Vector3[] { corners[2] * scale, corners[3] * scale, corners[7] * scale, corners[6] * scale };
                case FaceDirection.North:
                    return new Vector3[] { corners[1] * scale, corners[0] * scale, corners[3] * scale, corners[2] * scale };
                case FaceDirection.South:
                    return new Vector3[] { corners[4] * scale, corners[5] * scale, corners[6] * scale, corners[7] * scale };
                case FaceDirection.West:
                    return new Vector3[] { corners[1] * scale, corners[2] * scale, corners[6] * scale, corners[5] * scale };
                case FaceDirection.East:
                    return new Vector3[] { corners[4] * scale, corners[7] * scale, corners[3] * scale, corners[0] * scale };
                default:
                    return new Vector3[] { };
            }
        }

        public List<DirectionToFace> faces = new List<DirectionToFace>();

        public Entity Occumpant { get; set; }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        [SerializeField]
        float scale = 3f;

        public float Scale
        {
            get { return scale; }
        }

        [SerializeField, Range(0, 1)]
        float gizmoScale = 0.99f;

        public Vector3 LocalCenter
        {
            get
            {
                return Vector3.up * scale * 0.5f;
            }
        }

        Color GizmoColor
        {
            get
            {
                if (Occumpant != null) return Color.cyan;
                return Color.magenta;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Vector3 origin = transform.position;
            var drawOrigin = origin + Vector3.up * 0.5f * scale;
            Gizmos.DrawWireCube(drawOrigin, Vector3.one * scale * gizmoScale);
        }

        public void CorrectFaces()
        {
            var directions = System.Enum.GetValues(typeof(FaceDirection)) as FaceDirection[];

            if (faces.Count != 6 || !directions.All(direction => faces.Any(face => face.Direction == direction)))
            {
                faces = directions.Select(direction => new DirectionToFace(
                    direction,
                    faces.FirstOrDefault(face => face.Direction == direction).Face
                    )
                ).ToList();
            }

        }

        public void WhiteBlockEmptyFaces()
        {
            var origin = LocalCenter;

            for (int i = 0, l = faces.Count; i < l; i++)
            {
                var face = faces[i];
                if (face.Face.Surface || !face.Face.HasSurface) continue;

                var go = new GameObject($"Face {face.Direction}");
                go.transform.SetParent(transform);
                go.transform.localPosition = origin + face.Direction.AsVector() * scale * 0.5f;
                MakeFaceQuad(go, face.Direction);
                face.Face.Surface = go;
                face.Face.Passable = false;
            }
        }

        private static Vector3 VMul(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

        private void MakeFaceQuad(GameObject faceGO, FaceDirection direction)
        {
            var normal = direction.Invert().AsVector();
            var side = normal.x + normal.y + normal.z;
            var planeVector = new Vector3(normal.x == 0 ? 1 : 0, normal.y == 0 ? 1 : 0, normal.z == 0 ? 1 : 0) * side;

            var corners = Corners(direction, scale * 0.5f).Select(v => VMul(v, planeVector)).ToArray();

            var mr = faceGO.AddComponent<MeshRenderer>();
            var mf = faceGO.AddComponent<MeshFilter>();
            var m = new Mesh();
            m.vertices = corners;
            m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            m.RecalculateNormals();
            m.name = $"WhiteBox-Quad-{direction}";
            mf.mesh = m;
            mr.material = Level.Instance.WhiteBoxMaterial;
        }
    }
}