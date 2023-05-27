public class FlavorTextData
{
    #region 各ランクのボスText
    private string _flavorTwin
        = "二振りの剣を用いて戦う鍛冶士。\n持ち前の俊敏さを活かした攻撃をする。";
    private string _flavorHammer
        = "大きなハンマーを持った大柄の鍛冶士。\n攻撃とデバフに特化した戦いをする。";
    private string _flavorSpear
        = "槍を巧みに扱う鍛冶士。\n長期戦になるほど攻撃が苛烈になる。";
    #endregion

    private string[] _flavorWeakEnemyRankA = default;
    private string[] _flavorWeakEnemyRankB = default;
    private string[] _flavorWeakEnemyRankC = default;

    public string FlavorTwin => _flavorTwin;
    public string FlavorHammer => _flavorHammer;
    public string FlavorSpear => _flavorSpear;

    public string[] FlavorWeakEnemyRankA => _flavorWeakEnemyRankA;
    public string[] FlavorWeakEnemyRankB => _flavorWeakEnemyRankB;
    public string[] FlavorWeakEnemyRankC => _flavorWeakEnemyRankC;


    public void Start()
    {
        string blueA = "大きな剣を扱う上級鍛冶士。\n極めて素早く攻撃を繰り出してくる。";
        string redA = "大きな剣を扱う上級鍛冶士。\n極めて攻撃力の高い技を繰り出してくる。";
        string greenA = "大きな剣を扱う上級鍛冶士。\n全体的に極めて高い技量を持つ。";

        string blueB = "大きな剣を扱う中級鍛冶士。\nとても素早く攻撃を繰り出してくる。";
        string redB = "大きな剣を扱う中級鍛冶士。\nとても攻撃力の高い技を繰り出してくる。";
        string greenB = "大きな剣を扱う中級鍛冶士。\n全体的に高い技量を持つ。";

        string blueC = "大きな剣を扱う低級鍛冶士。\n素早く攻撃を繰り出してくる。";
        string redC = "大きな剣を扱う低級鍛冶士。\n攻撃力の高い技を繰り出してくる。";
        string greenC = "大きな剣を扱う低級鍛冶士。\n全体のバランスの取れた技量を持つ。";

        _flavorWeakEnemyRankA = new string[] { blueA, redA, greenA };
        _flavorWeakEnemyRankB = new string[] { blueB, redB, greenB };
        _flavorWeakEnemyRankC = new string[] { blueC, redC, greenC };
    }
}
