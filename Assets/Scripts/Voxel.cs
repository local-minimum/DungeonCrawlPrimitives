using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Voxel : MonoBehaviour
{
    public Entity Occumpant { get; set; }

    [SerializeField]
    bool blocked;

    [SerializeField]
    Transform overridePositionAnchor;

    public bool Blocked
    {
        get { return blocked; }
    }

    public Vector3 Position
    {
        get
        {
            if (overridePositionAnchor == null) return transform.position;
            return overridePositionAnchor.position;
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

    Color GizmoColor
    {
        get
        {
            if (Blocked) return Color.black;
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
}
