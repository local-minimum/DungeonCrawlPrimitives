using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DungAtt1
{

    [CustomEditor(typeof(Voxel))]
    [CanEditMultipleObjects]
    public class VoxelEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            Voxel myScript = (Voxel)target;

            myScript.CorrectFaces();

            EditorGUILayout.HelpBox("Wooop", MessageType.Info);

            if (GUILayout.Button("White-block"))
            {
                serializedObject.UpdateIfRequiredOrScript();
                myScript.WhiteBlockEmptyFaces();
                serializedObject.ApplyModifiedProperties();
            }

            DrawDefaultInspector();
        }
    }

}