using Cysharp.Threading.Tasks;
public interface IAddDamage
{
    /// <summary>
    /// ダメージを受けた時の処理を書くメソッド
    /// </summary>
    public async UniTask AddDamage(float damage, float criticalRate) { }
}
