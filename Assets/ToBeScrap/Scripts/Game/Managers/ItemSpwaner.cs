using System;
using ToBeScrap.Game.Items;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ToBeScrap.Game.Managers
{
    public class ItemSpwaner : MonoBehaviour
    {
        [SerializeField] private Item itemPrefab;

        private void Start()
        {
            CreateItem();
        }

        private void CreateItem()
        {
            Instantiate(itemPrefab, new Vector2(Random.Range(-3f, 3f), 5f), quaternion.identity);
        }
    }
}