using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Text;
using Process = System.Diagnostics.Process;

/// <summary>
/// 版本控制器
/// </summary>
public class VersionUtil
{
	public static Dictionary<string, string[]> getVerDic
	{
		get
		{
			Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
			string filePath = PathUtil.verPath;
			if (File.Exists(filePath))
			{
				string data = File.ReadAllText(filePath);
				string[] dates = data.Split(new char[] { '|' });
				foreach (string d in dates)
				{
					string[] vers = d.Split(new char[] { ',' });
					dic[vers[0]] = vers;
				}
			}
			else
			{
				string path = Application.dataPath + "/ABs";
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				File.Create(filePath);
			}
			return dic;
		}
	}

	public static Dictionary<string, string[]> getHashDic
	{
		get
		{
			Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
			string filePath = PathUtil.verPath;
			if (File.Exists(filePath))
			{
				string data = File.ReadAllText(filePath);
				string[] dates = data.Split(new char[] { '|' });
				foreach (string d in dates)
				{
					if (!string.IsNullOrEmpty(d))
					{
						string[] vers = d.Split(new char[] { ',' });
						dic[vers[1]] = vers;
					}

				}
			}
			else
			{
				string path = Application.dataPath + "/ABs";
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				File.Create(filePath);
			}
			return dic;
		}
	}

    public static void writeVersion(Dictionary<string, string[]> dic)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string[] ver in dic.Values)
        {

            string date = string.Join(",", ver);
            if (!string.IsNullOrEmpty(date))
            {
                sb.Append(date);
                sb.Append("|");
            }
        }
        File.WriteAllText(PathUtil.verPath, sb.ToString(0,sb.Length - 1), Encoding.UTF8);
        AssetDatabase.Refresh();
    }
    public static Dictionary<string, string[]> getLowerVerDic
    {
        get
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            string filePath = PathUtil.verPath;
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                string[] dates = data.Split(new char[] { '|' });
                foreach (string d in dates)
                {
                    string[] vers = d.Split(new char[] { ',' });
                    dic[vers[0].ToLower()] = vers;
                }
            }
            else
            {
                string path = Application.dataPath + "/ABs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.Create(filePath);
            }
            return dic;
        }
    }
    /// <summary>
    /// 清理文件
    /// </summary>
    /// <param name="abDatas"></param>
    public static void ClearVersion(Dictionary<string, bool> abDatas)
    {
        Dictionary<string, string[]> hashDic = getHashDic;
        Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
        foreach (string[] datas in hashDic.Values)
        {
            if (abDatas.ContainsKey(datas[1]))
            {
                dic.Add(datas[0], datas);
            }
        }
        writeVersion(dic);
    }


}

