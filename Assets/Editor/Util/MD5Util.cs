using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MD5Util  {
    //获取MD5的值
    public static string GetMD5Hash(byte[] data)
    {
        MD5 md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(data);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    //判断数据是否改变
    //如果md5值相同，返回true
    public static bool checkMD5Hash(byte[] date, string hash)
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        string newhash = GetMD5Hash(date);
        if (0 == comparer.Compare(newhash, hash))
        {
            return true;
        }
        return false;
    }
}
