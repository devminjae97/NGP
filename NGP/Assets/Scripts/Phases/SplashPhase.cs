using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SplashPhase : PhaseBase
{
    [SerializeField] private Image _splashImage;

    protected override void Initialize()
    {
        _splashImage.color = new (1,1,1,0);
    }

    protected override void StartPhase()
    {
        if (_splashImage == null)
        {
            return;
        }
        
        AsyncShowSplash().Forget();
    }

    private async UniTaskVoid AsyncShowSplash()
    {
        await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
        
        // Appear Logo (Tween)
        var appearTween = _splashImage.DOFade(1, 1);
        await appearTween.Play();

        // wait
        await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());

        // Disappear Logo (Tween)
        var disappearTween = _splashImage.DOFade(0, 1);
        await disappearTween.Play();

        await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());

        SceneManager.LoadScene("LobbyScene");
    }
}
