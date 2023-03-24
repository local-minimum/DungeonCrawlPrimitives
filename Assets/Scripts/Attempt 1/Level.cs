using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungAtt1
{
    public class Level : MonoBehaviour
    {
        static Level instance;
        public static Level Instance
        {
            get
            {
                if (instance == null) instance = GameObject.FindObjectOfType<Level>();
                return instance;
            }
            private set { instance = value; }
        }

        [SerializeField]
        float getVoxelPositionFudge = 0.1f;

        [SerializeField]
        float getVoxelScaleFudge = 0.2f;

        public Material WhiteBoxMaterial;

        List<Voxel> voxels = new List<Voxel>();

        void Awake()
        {
            if (Instance == null || Instance == this)
            {
                voxels.AddRange(GetComponentsInChildren<Voxel>());
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

        }

        public bool GetVoxel(float scale, Vector3 position, out Voxel voxel)
        {
            //Todo should scale magnitude by scale too...
            voxel = voxels
                .Where(vox =>
                    Mathf.Abs(1 - scale / vox.Scale) < getVoxelScaleFudge
                    && (vox.Position - position).magnitude < getVoxelPositionFudge * scale
                 )
                .FirstOrDefault();

            return voxel != null;
        }

        public Voxel FindClosestVoxel(Vector3 position)
        {
            return voxels
                .OrderBy(vox => Vector3.SqrMagnitude(vox.Position - position))
                .FirstOrDefault();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }

}