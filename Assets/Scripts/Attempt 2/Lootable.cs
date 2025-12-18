using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt2
{
    public delegate void LootDropEvent(string itemId, int level, Vector3Int coordinates);

    public delegate void LootPickupEvent(string itemId, string looter);

    public class Lootable : MonoBehaviour
    {
        [SerializeField]
        public string ItemId;

        public static event LootDropEvent OnLootDrop;
        public static event LootPickupEvent OnLootPickup;

        public void Drop(Vector3Int coorinates)
        {
            OnLootDrop?.Invoke(ItemId, Level.instance.lvl, coorinates);
        }

        public void Pickup(string looter)
        {
            OnLootPickup?.Invoke(ItemId, looter);
        }
    }
}