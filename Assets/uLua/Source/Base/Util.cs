using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using LuaInterface;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util
{

    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name)
    {
        string path = Application.dataPath;
        string lowerName = name.ToLower();
        if (lowerName.EndsWith(".lua"))
        {
            int index = name.LastIndexOf('.');
            name = name.Substring(0, index);
        }
        name = name.Replace('.', '/');
        return path + "/lua/" + name + ".lua";
    }

    public static void Log(string str)
    {
        Debug.Log(str);
    }

    public static void LogWarning(string str)
    {
        Debug.LogWarning(str);
    }

    public static void LogError(string str)
    {
        Debug.LogError(str);
    }

    /// <summary>
    /// 清理内存
    /// </summary>
    public static void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
        LuaScriptMgr mgr = LuaScriptMgr.Instance;
        if (mgr != null && mgr.lua != null) mgr.LuaGC();
    }

    /// <summary>
    /// 防止初学者不按步骤来操作
    /// </summary>
    /// <returns></returns>
    static int CheckRuntimeFile()
    {
        if (!Application.isEditor) return 0;
        string sourceDir = AppConst.uLuaPath + "/Source/LuaWrap/";
        if (!Directory.Exists(sourceDir))
        {
            return -2;
        }
        else
        {
            string[] files = Directory.GetFiles(sourceDir);
            if (files.Length == 0) return -2;
        }
        return 0;
    }

    /// <summary>
    /// 检查运行环境
    /// </summary>
    public static bool CheckEnvironment()
    {
#if UNITY_EDITOR
        int resultId = Util.CheckRuntimeFile();
        if (resultId == -1)
        {
            Debug.LogError("没有找到框架所需要的资源，单击Game菜单下Build xxx Resource生成！！");
            EditorApplication.isPlaying = false;
            return false;
        }
        else if (resultId == -2)
        {
            Debug.LogError("没有找到Wrap脚本缓存，单击Lua菜单下Gen Lua Wrap Files生成脚本！！");
            EditorApplication.isPlaying = false;
            return false;
        }
#endif
        return true;
    }

	/// <summary>
	/// 添加单个按钮事件
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="click"></param>
	public static void setButtonClick(GameObject obj, LuaFunction click)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		if (btn == null)
		{
			LogError("setButtonClick 没有按钮组件 添加按钮事件失败！！！");
			return;
		}
		btn.onClick.Clear();
		EventDelegate.Add(btn.onClick, () =>
		{
			click.Call();
		});
	}
	/// <summary>
	/// 添加按钮事件
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="click"></param>
	public static void addButtonClick(GameObject obj, LuaFunction click)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		if (btn == null)
		{
			LogError("addButtonClick 没有按钮组件 添加按钮事件失败！！！");
			return;
		}
		EventDelegate.Add(btn.onClick, () =>
		{
			click.Call();
		});
	}
	public void AddPress(GameObject go, LuaFunction luafunc)
	{
		if (go == null) return;
		UIEventListener.Get(go).onPress = delegate (GameObject o, bool isPressed)
		{
			luafunc.Call(isPressed);
		};
	}

	public void AddDrag(GameObject go, LuaFunction luafunc)
	{
		if (go == null) return;
		UIEventListener.Get(go).onDrag = delegate (GameObject o, Vector2 delta)
		{
			luafunc.Call(delta);
		};
	}

	public void AddDragEnd(GameObject go, LuaFunction luafunc)
	{
		if (go == null) return;
		UIEventListener.Get(go).onDragEnd = delegate (GameObject o)
		{
			luafunc.Call();
		};
	}

	/// <summary>
	/// 获取传人的对象的子物体
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	public static GameObject[] GetObjectChild(GameObject parent)
	{
		Transform tran = parent.transform;
		if (tran.childCount == 0)
			return null;
		GameObject[] objs = new GameObject[tran.childCount];
		for (int i = 0, L = tran.childCount; i < L; i++)
		{
			objs[i] = tran.GetChild(i).gameObject;
		}
		return objs;
	}
	/// <summary>
	/// 获取传入的对象名获取所有对象
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="names"></param>
	/// <returns></returns>
	public static Transform[] GetNameTransforms(GameObject parent, string[] names)
	{
		Transform t = parent.transform;
		List<Transform> trans = new List<Transform>();
		Transform[] AllClides = t.GetComponentsInChildren<Transform>(true);
		List<string> nameList = new List<string>();
		nameList.AddRange(names);
		for (int i = 0; i < AllClides.Length; i++)
		{
			for (int y = 0; y < nameList.Count; y++)
			{
				if (AllClides[i].name == nameList[y])
				{
					trans.Add(AllClides[i]);
					nameList.RemoveAt(y);
					break;
				}
			}
			if (nameList.Count == 0)
				break;
		}
		return trans.ToArray();
	}

	//仅在编辑器下使用
	public static UnityEngine.Object LoadMainAssetAtPath(string path)
	{
#if UNITY_EDITOR
		return UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
#else
        return null;
#endif
	}
}