using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Voxel))]
public class VoxelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // TODO: Make custom faces thing perhaps?
        DrawDefaultInspector();

        Voxel myScript = (Voxel)target;

        CorrectFaces(myScript);

        EditorGUILayout.HelpBox("Wooop", MessageType.Info);
    }

    static void CorrectFaces(Voxel myScript)
    {
        var faces = myScript.faces;
        var directions = System.Enum.GetValues(typeof(FaceDirection)) as FaceDirection[];

        if (faces.Count != 6 || !directions.All(direction => faces.Any(face => face.Direction == direction)))
        {
            myScript.faces = directions.Select(direction => new DirectionToFace(
                direction,
                faces.FirstOrDefault(face => face.Direction == direction).Face
                )
            ).ToList();
        }

    }
}
