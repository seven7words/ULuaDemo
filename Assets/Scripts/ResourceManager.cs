using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static Dictionary<string,string> fileDic = new Dictionary<string, string>();

    private LuaScriptMgr mgr;
	// Use this for initialization
	public void init () {
	    if (Application.isMobilePlatform)
	    {
	        
	    }else if (Application.platform == RuntimePlatform.WindowsPlayer)
	    {
	        string cfgPath = Util.DataPath + "windowAbs";
	        string content = File.ReadAllText(cfgPath);
            updateFileDic(content);
            mgr = new LuaScriptMgr();
	    }else if (Application.isEditor)
	    {
	        mgr = new LuaScriptMgr();
	    }
	}

    private void updateFileDic(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        string[] dates = content.Split(new char[] { '|' });
        foreach (string d in dates)
        {
            if (!string.IsNullOrEmpty(d))
            {
                string[] vers = d.Split(new char[] { ',' });
                fileDic.Add(vers[0],vers[1]);
            }

        }
    }

    public static byte[] getLuaAsset(string path)
    {
        if (!fileDic.ContainsKey(path))
        {
            Debug.LogError("键值不存在");
        }
        string fileName = fileDic[path];
        string filePath = Util.DataPath + fileName;
        Debug.Log(fileName);
        AssetBundle ab = AssetBundle.LoadFromFile(fileName);
        ScriptAsset sa = ab.LoadAsset<ScriptAsset>(fileName);
        ab.Unload(true);
        return null;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
