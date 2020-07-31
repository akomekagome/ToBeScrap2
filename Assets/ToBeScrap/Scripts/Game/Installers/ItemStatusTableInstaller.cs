using ToBeScrap.Game.Items;
using UnityEngine;
using Zenject;

namespace ToBeScrap.Main.Installers
{
    [CreateAssetMenu(fileName = "ItemStatusTableInstaller", menuName = "Installers/ItemStatusTableInstaller")]
    public class ItemStatusTableInstaller : ScriptableObjectInstaller<ItemStatusTableInstaller>
    {
        [SerializeField] private ItemStatusTable _itemStatusTable;
        public override void InstallBindings()
        {
            Container.BindInstance(_itemStatusTable);
        }
    }
}