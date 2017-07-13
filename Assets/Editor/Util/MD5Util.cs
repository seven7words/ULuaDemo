using System.Text;
using System.Security.Cryptography;



/// <summary>
/// 
/// </summary>
public class MD5Util
{
	/// <summary>
	/// 获取MD5值
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
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
	/// <summary>
	/// 判断数据是否改变
	/// </summary>
	/// <param name="date"></param>
	/// <param name="hash"></param>
	/// <returns>如果md5值相同 返回 -- true </returns>
	public static bool checkMD5Hash(byte[] date, string hash)
	{
		System.StringComparer comparer = System.StringComparer.OrdinalIgnoreCase;
		string newhash = GetMD5Hash(date);
		if (0 == comparer.Compare(newhash, hash))
		{
			return true;
		}
		return false;
	}
}

