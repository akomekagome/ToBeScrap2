using KanKikuchi.AudioManager;
using ToBeScrap.Game.Players;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ToBeScrap.Game.Managers
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;

        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.BGM_BATTLE);
            for (var i = 0; i < 2; i++)
                CreatePlayer((PlayerId)i);
        }

        private void CreatePlayer(PlayerId playerId)
        {
            var player = Instantiate(playerPrefab, new Vector2((int)playerId, 5f), quaternion.identity);
            player.Init(playerId);
        }
    }
}