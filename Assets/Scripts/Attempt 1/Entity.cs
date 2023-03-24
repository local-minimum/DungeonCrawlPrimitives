using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt1
{
    public class Entity : MonoBehaviour
    {
        Voxel currentPosition;

        public float CurrentScale
        {
            get
            {
                if (currentPosition == null)
                {
                    Debug.LogWarning($"Assuming scale 1 for {name}");
                    return 1;
                }
                return currentPosition.Scale;
            }
        }

        public Vector3 CurrentPosition
        {
            get
            {
                if (currentPosition == null)
                {
                    Debug.Log($"Assuming origin for {name}");
                    return Vector3.zero;
                }
                return currentPosition.Position;
            }
        }

        void Start()
        {
            currentPosition = Level.Instance.FindClosestVoxel(transform.position);
            currentPosition.Occumpant = this;
            transform.position = currentPosition.Position;
        }

        public void ReleasePosition()
        {
            if (currentPosition == null)
            {
                Debug.LogWarning($"{name} is not occupying anything");
                return;
            }
            if (currentPosition.Occumpant != this)
            {
                Debug.LogWarning($"{name} is not the occupant of {currentPosition.name}, it's {currentPosition.Occumpant.name}");
                return;
            }

            currentPosition.Occumpant = null;
        }

        public void ClaimPosition(Voxel voxel)
        {
            if (voxel.Occumpant != null)
            {
                Debug.LogWarning($"{name} can not occupy {voxel.name} because it is already occupied by {voxel.Occumpant.name}");
                return;
            }

            if (currentPosition != null) ReleasePosition();

            currentPosition = voxel;
            voxel.Occumpant = this;
        }
    }
}