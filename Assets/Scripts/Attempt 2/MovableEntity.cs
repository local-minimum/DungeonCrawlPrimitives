using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMove(string id, Vector3Int position, FaceDirection lookDirection);


public class MovableEntity : MonoBehaviour
{
    [SerializeField, Tooltip("Leave empty to use game object name")]
    string id;

    public event OnMove OnMove;

    public void SetNewGridPosition(Vector3Int position, FaceDirection lookDirection)
    {
        OnMove?.Invoke(string.IsNullOrEmpty(id) ? name : id, position, lookDirection);
    }
}
