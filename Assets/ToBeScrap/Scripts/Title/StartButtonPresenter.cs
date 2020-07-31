using System;
using KanKikuchi.AudioManager;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ToBeScrap.Title
{
    public class StartButtonPresenter : MonoBehaviour
    {
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;
        
        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.BGM_TITLE);
            
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .FirstOrDefault()
                .Subscribe(_ => _zenjectSceneLoader.LoadScene("Battle"));
        }
    }
}
