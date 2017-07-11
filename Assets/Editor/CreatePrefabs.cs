﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//预设保存
public class CreatePrefabs : Editor
{
    #region [NGUI 预设生成插件]
    //打包模版预设
    [MenuItem("Create/Create Module Prefab")]
    static void CreateModulePrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null)
        {
            return;
        }
        string root = "Root";
        string name = go.name;
        if (!name.Contains("_view") && !name.Contains(root))
        {
            Debug.LogError("警告：UI模块必须加上后缀");
        }
        string atlasName = "";
        if (!name.Contains(root))
        {
            atlasName = go.name.Substring(0, go.name.IndexOf("_")) + "Atlas.png";

            if (AssetDatabase.LoadAssetAtPath<Object>(PathUtil.GetAssetPath(PathUtil.atlasPath + atlasName)) == null)
            {
                Debug.LogError("警告“模块图集必须喝模块名相同，是否有图集");
                //return;
            }
        }
        string path = PathUtil.modulesPath + go.name + PathUtil.SuffixPrefab;
        if (string.IsNullOrEmpty(path))
            return;
        go = NGUICreatePrefab(go, path);
        if (!name.Contains(root))
        {
            oneModuleViewAtlas(go, atlasName);
        }
    }
    //保存模块子预设
    [MenuItem("Create/Create UI Prefab")]
    static void CreateUIPrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null)
        {
            return;
        }
        string path = EditorUtility.SaveFilePanel("保存文件", PathUtil.UIPath, go.name + PathUtil.SuffixPrefab, PathUtil.Prefab);
        if (string.IsNullOrEmpty(path))
            return;
        NGUICreatePrefab(go, path);
    }
#endregion
    //生成预设
    private static GameObject NGUICreatePrefab(GameObject go,string path){
        setCommonAtlasSprite(go);
        path = path.Substring(path.LastIndexOf(PathUtil.Assets), path.Length - path.LastIndexOf(PathUtil.Assets));
        GameObject to = null;
        if(!string.IsNullOrEmpty(path)){
            string name = go.name;
            go = Instantiate(go) as GameObject;
            go.name = name;
            to = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(to == null){
                to = PrefabUtility.CreatePrefab(path, go);
            }else{
                to = PrefabUtility.ReplacePrefab(go, to, ReplacePrefabOptions.ReplaceNameBased);//覆盖原有预设 保留原有链接
            }
            Debug.Log("已生成"+path,AssetDatabase.LoadAssetAtPath<GameObject>(path));
            DestroyImmediate(go);
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return to;
    }
    //设置公共图集引用
    private static void setCommonAtlasSprite(GameObject go){
        UISprite[] sprites = go.GetComponentsInChildren<UISprite>(true);
        for (int i = 0; i < sprites.Length;i++){
            UISprite sp = sprites[i];
            if(sp.atlas!=null&&PathUtil.isCommAtlas(sp.atlas.name)){
                sp.tag = sp.atlas.name;
            }
        }
    }
    //判断是否包含模块图集以及公共图集
    public static void oneModuleViewAtlas(GameObject prefab,string moduleAtlasName){
        Debug.Log(moduleAtlasName);
        string[] paths = AssetDatabase.GetDependencies(new string[] { AssetDatabase.GetAssetPath(prefab) });
        for (int i = 0; i < paths.Length;i++){
            if(paths[i].Contains("Prefab/atlas/")&& paths[i].Contains(PathUtil.SuffixPrefab)&&!PathUtil.isCommAtlas(paths[i])&&!paths[i].Contains(moduleAtlasName)){
                Debug.LogError("警告：模块中存在其他非公共图集或者图片资源!!!" + paths[i], AssetDatabase.LoadAssetAtPath<GameObject>(paths[i].Replace(PathUtil.SuffixPng, PathUtil.SuffixPrefab)));
            }
        }
    }
}
























