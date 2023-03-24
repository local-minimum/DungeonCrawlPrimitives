using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt1
{

    [System.Serializable]
    public struct Face
    {
        public bool Passable;
        public bool HasSurface;
        public Voxel Neighbour;
        public GameObject Surface;

        public override string ToString()
        {
            var passable = Passable ? "Passable" : "Impassable";
            var hasSurface = HasSurface ? "Filled" : "Empty";

            return $"{passable} / {hasSurface}";
        }
    }


    [System.Serializable]
    public struct DirectionToFace
    {
        public FaceDirection Direction;
        public Face Face;

        public DirectionToFace(FaceDirection direction, Face face)
        {
            Direction = direction;
            Face = face;
        }

        public void GenerateWhiteBox(Transform voxelTransform, float scale)
        {
            if (Face.Surface) return;

            Face.Surface = new GameObject($"Surface - {Direction}");
            Face.Surface.transform.SetParent(voxelTransform);

            // TODO: Calculate from directon and scale
            Face.Surface.transform.localPosition = new Vector3(0, 0, 0);

            // Add a quadmesh
        }

        public override string ToString()
        {
            return $"{Direction}: {Face}";
        }
    }
}