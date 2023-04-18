#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Compilation;

[CustomEditor(typeof(SkillBase))]
public class SkillGenerator : EditorWindow
{
    private string _skillName = "";
    private int _damage = 0;
    private WeaponType _weapon;
    private OreRarity _rarity;
    private SkillType _type;
    private PlayerSkillDataManagement _playerSkillDataManagement;

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
        _rarity = (OreRarity)EditorGUILayout.EnumPopup("レアリティ", _rarity);
        _type = (SkillType)EditorGUILayout.EnumPopup("タイプ", _type);
        _className = EditorGUILayout.TextField("クラス名", _className);
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
        _skillName = "";
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
        }

        if (AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)))
        {
            Debug.LogWarning("コンポーネントが既に存在します " + _className);
            return;
        }

        string classCode =
            "using System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing Cysharp.Threading.Tasks;\r\nusing UnityEngine.Playables;\r\n\r\npublic  class " +
            _className +
            " : SkillBase \r\n{\r\n    public string SkillName { get; set; }\r\n    public int Damage { get; set; }\r\n    public WeaponType Weapon { get; set; }\r\n    public OreRarity Rarity { get; set; }\r\n    public SkillType Type  { get; set; }\r\n    \r\n    private PlayableDirector _anim;\r\n\r\n    public override async UniTask UseSkill()\r\n    {\r\n        Debug.Log(\"Use Skill\");\r\n        _anim = GetComponent<PlayableDirector>();\r\n        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);\r\n        Debug.Log(\"Anim End\");\r\n    }\r\n\r\n    protected override void SkillEffect()\r\n    {\r\n        Debug.Log(\"Skill Effect\");\r\n    }\r\n}";
        File.WriteAllText(path, classCode);
        CompilationPipeline.RequestScriptCompilation();
        AssetDatabase.Refresh();
    }
}
#endif