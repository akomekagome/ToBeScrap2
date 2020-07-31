using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToBeScrap.Game.Items
{
    [System.Serializable]
    public struct ItemStatusData
    {
        public ItemType itemType;
        public float itemWeight;
        public float itemSpeed;
    }

    [CreateAssetMenu(fileName = "ItemStatusTable", menuName = "ToBeScrap/Data/ItemStatusTable")]
    public class ItemStatusTable : ScriptableObject
    {
        public List<ItemStatusData> itemStatusTable;

        public ItemStatusData GetItemStatusData(ItemType itemType)
            => itemStatusTable.FirstOrDefault(x => x.itemType == itemType);
    }
}