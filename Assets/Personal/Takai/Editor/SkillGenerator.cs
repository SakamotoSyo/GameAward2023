using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillBase))]
public class SkillGenerator : EditorWindow
{
    private string _skillName = "";
    private int _damage = 0;
    private WeaponType _weapon;
    private OreRarity _rarity;
    private SkillType _type;
    private PlayerSkillDataManagement _playerSkillDataManagement;

    private string _className;

    [MenuItem("Window/SkillGenerator")]
    static void Open()
    {
        var window = GetWindow<SkillGenerator>();
        window.titleContent = new GUIContent("SkillGenerator");
    }

    void OnGUI()
    {
        GUILayout.Label("スキルステータス設定");

        EditorGUILayout.BeginVertical(GUI.skin.box);
        _skillName = EditorGUILayout.TextField("スキル名", _skillName);
        _damage = EditorGUILayout.IntField("ダメージ", _damage);
        _weapon = (WeaponType)EditorGUILayout.EnumPopup("武器種類", _weapon);
        _rarity = (OreRarity)EditorGUILayout.EnumPopup("レアリティ", _rarity);
        _type = (SkillType)EditorGUILayout.EnumPopup("タイプ", _type);
        _className = EditorGUILayout.TextField("スキルのクラス名", _className);
        if (GUILayout.Button("リセット"))
        {
            Reset();
        }

        if (GUILayout.Button("作成"))
        {
            CreatSkill();
        }

        EditorGUILayout.EndVertical();
    }

    private void Reset()
    {
        _skillName = "";
        _damage = 0;
        _className = "";
    }

    private void CreatSkill()
    {
        if(_skillName == "" || _className == "") {return;}
        string prefabPath = $"Assets/Resources/Skills/{_skillName}.prefab";
        GameObject newPrefab = new GameObject("NewPrefab");

        
        
        
        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        AssetDatabase.Refresh();
        GameObject.DestroyImmediate(newPrefab);
    }
}