using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt2
{
    public class LootTable : MonoBehaviour
    {
        static LootTable instance;
        public static LootTable Instance { 
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<LootTable>();
                }
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
