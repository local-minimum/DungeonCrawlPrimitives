using UnityEngine;

public enum FaceDirection
{
    North, South, West, East, Up, Down
}

public static class FaceExtentions
{
    public static Vector3 AsVector(this FaceDirection faceDirection)
    {
        switch (faceDirection)
        {
            case FaceDirection.Up:
                return Vector3.up;
            case FaceDirection.Down:
                return Vector3.down;
            case FaceDirection.North:
                return Vector3.forward;
            case FaceDirection.South:
                return Vector3.back;
            case FaceDirection.West:
                return Vector3.left;
            case FaceDirection.East:
                return Vector3.right;
            default:
                throw new System.ArgumentException($"{faceDirection} is not a known as a vector");
        }
    }

    public static FaceDirection Invert(this FaceDirection faceDirection)
    {
        switch (faceDirection)
        {
            case FaceDirection.Up:
                return FaceDirection.Down;
            case FaceDirection.Down:
                return FaceDirection.Up;
            case FaceDirection.North:
                return FaceDirection.South;
            case FaceDirection.South:
                return FaceDirection.North;
            case FaceDirection.West:
                return FaceDirection.East;
            case FaceDirection.East:
                return FaceDirection.West;
            default:
                throw new System.ArgumentException($"{faceDirection} has no inverse");
        }
    }
}
