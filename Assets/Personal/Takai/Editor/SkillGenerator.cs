#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Compilation;
using UnityEngine.Playables;

[CustomEditor(typeof(SkillBase))]
public class SkillGenerator : EditorWindow
{
    private string _skillName = "";
    private int _damage = 0;
    private WeaponType _weapon;
    private SkillType _type;
    private SkillDataManagement _skillDataManagement;
    private string _flaverText = "";

    private string _className = "";

    [MenuItem("Window/SkillGenerator")]
    static void Open()
    {
        var window = GetWindow<SkillGenerator>();
        window.titleContent = new GUIContent("SkillGenerator");
    }

    void OnGUI()
    {
        GUILayout.Label("SKillGenerator");

        EditorGUILayout.BeginVertical(GUI.skin.box);
        _skillName = EditorGUILayout.TextField("スキル名", _skillName);
        _damage = EditorGUILayout.IntField("ダメージ値", _damage);
        _weapon = (WeaponType)EditorGUILayout.EnumPopup("武器種類", _weapon);
        _type = (SkillType)EditorGUILayout.EnumPopup("タイプ", _type);
        _className = EditorGUILayout.TextField("クラス名", _className);
        _flaverText = EditorGUILayout.TextField("フレーバーテキスト", _flaverText);

        if (GUILayout.Button("リセット"))
        {
            Reset();
        }

        if (GUILayout.Button("クラス作成"))
        {
            CreateClass();
        }

        if (GUILayout.Button("プレハブ作成"))
        {
            CreatPrefab();
        }

        EditorGUILayout.EndVertical();
    }

    private void Reset()
    {
        _skillName = "";
        _damage = 0;
        _className = "";
        _flaverText = "";
    }

    private void CreatPrefab()
    {
        string prefabPath = $"Assets/Resources/Skills/{_className}.prefab";
        if (AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)))
        {
            Debug.LogWarning("プレハブが既に存在します " + _className);
            return;
        }

        GameObject newPrefab = new GameObject(_className);
        Type type = Type.GetType(_className + ", Assembly-CSharp");

        if (type == null)
        {
            Debug.LogError("コンポーネントが見つかりませんでした: " + _className);
            return;
        }

        newPrefab.AddComponent<PlayableDirector>();
        Component component = newPrefab.AddComponent(type);
        Debug.Log(component);

        if (component == null)
        {
            Debug.Log("コンポーネントがアタッチ出来ませんでした" + _className);
        }

        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        GameObject.DestroyImmediate(newPrefab);
    }

    private void CreateClass()
    {
        if (_skillName == "" || _className == "")
        {
            Debug.LogError("名前を入力してください");
            return;
        }

        string path = "";

        switch (_weapon)
        {
            case WeaponType.GreatSword:
                path = $"Assets/Personal/Takai/Script/Skills/GreatSword/{_className}.cs";
                break;
            case WeaponType.DualBlades:
                path = $"Assets/Personal/Takai/Script/Skills/DualBlades/{_className}.cs";
                break;
            case WeaponType.Hammer:
                path = $"Assets/Personal/Takai/Script/Skills/Hammer/{_className}.cs";
                break;
            case WeaponType.Spear:
                path = $"Assets/Personal/Takai/Script/Skills/Spear/{_className}.cs";
                break;
            case WeaponType.Any:
                path = $"Assets/Personal/Takai/Script/Skills/Any/{_className}.cs";
                break;
        }

        if (AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)))
        {
            Debug.LogWarning("コンポーネントが既に存在します " + _className);
            return;
        }

        string classCode = @"
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Playables;

    public class " + _className + @" : SkillBase
    {
        private PlayableDirector _anim;
        private PlayerController _playerStatus;

    public " + _className + @"()
    {
        SkillName = """ + _skillName + @""";
        Damage = " + _damage + @";
        Weapon = (" + typeof(WeaponType) + @")" + (int)_weapon + @";
        Type = (" + typeof(SkillType) + @")" + (int)_type + @";
        FlavorText = """ + _flaverText + @""";
    }
        
        private void Start()
        {
            _anim = GetComponent<PlayableDirector>();
        }


        public override bool IsUseCheck(PlayerController player)
        {
            return true;
        }    
    

        public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
        {
            Debug.Log(""Use Skill"");
            _playerStatus = player;
            _anim = GetComponent<PlayableDirector>();
            SkillEffect();
            await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
            Debug.Log(""Anim End"");
        }

        protected override void SkillEffect()
        {
            // スキルの効果処理を実装する
        }

        public override bool TurnEnd()
        {
            return false;
        }

        public override void BattleFinish()
        {

        }
    }";

        File.WriteAllText(path, classCode);
        CompilationPipeline.RequestScriptCompilation();
        AssetDatabase.Refresh();
    }
}
#endif