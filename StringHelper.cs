using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nimaime.Helper.Data
{
	public static class StringHelper
	{
		/// <summary>
		/// 删除字符串特殊符号
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string DelQuota(this string str)
		{
			string result = str;
			string[] strQuota = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "`", ";", "'", ",", ".", "/", ":", "/,", "<", ">", "?" };
			for (int i = 0; i < strQuota.Length; i++)
			{
				if (result.IndexOf(strQuota[i]) < -1)
					result = result.Replace(strQuota[i], "");
			}
			return result;
		}

		/// <summary>
		/// 提取百度分享链接
		/// </summary>
		/// <param name="str">原始文本</param>
		/// <returns>提取到的百度链接</returns>
		public static string ExtractBaiduShareLink(this string str)
		{
			Regex regBaiduURL = new Regex(@"https:\/\/pan\.baidu\.com\/s\/[\?\=a-zA-Z0-9_-]+");
			MatchCollection matches = regBaiduURL.Matches(str);
			if(matches.Count > 0)
			{
				return matches[0].Value;
			}
			return string.Empty;
		}

		/// <summary>
		/// 提取百度密码
		/// </summary>
		/// <param name="str">原始文本</param>
		/// <returns>提取到的百度密码</returns>
		public static string ExtractBaiduPassword(this string str)
		{
			Regex regBaiduPassword = new Regex(@"[提取|密]码[：|:][\W?]([A-Z|a-z|0-9]{4})", RegexOptions.Multiline);
			MatchCollection matches = regBaiduPassword.Matches(str);
			if (matches.Count > 0)
			{
				return matches[0].Groups[1].Value;
			}
			return string.Empty;
		}

		/// <summary>
		/// 从百度原始分享文本提取RecWeb V1兼容格式文本
		/// </summary>
		/// <param name="str">百度原始分享文本</param>
		/// <returns>RecWeb V1兼容格式文本</returns>
		public static string GetRecWebBaiduShareLinkString(this string str)
		{
			string shareURL = str.ExtractBaiduShareLink();
			string password = str.ExtractBaiduPassword();
			if (string.IsNullOrWhiteSpace(shareURL) || string.IsNullOrWhiteSpace(password))
			{
				return string.Empty;
			}
			return string.Format("链接: {0} 提取码: {1}", shareURL, password);
		}

		/// <summary>
		/// 提取URL
		/// </summary>
		/// <param name="str">原始文本</param>
		/// <returns>正则匹配中的项</returns>
		public static MatchCollection ExtractURLs(this string str)
		{
			//正则匹配URL
			Regex regURL = new Regex(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?", RegexOptions.Multiline);
			return regURL.Matches(str);
		}

		/// <summary>
		/// 提取第一个URL
		/// </summary>
		/// <param name="str">原始文本</param>
		/// <returns>正则匹配中的第一个匹配项</returns>
		public static string ExtractFirstURL(this string str)
		{
			MatchCollection matches = str.ExtractURLs();
			if (matches.Count > 0)
			{
				return matches[0].Value;
			}
			return string.Empty;
		}

		/// <summary>
		/// 将指定符号分割的字符串转换为List
		/// </summary>
		/// <typeparam name="T">目标类型</typeparam>
		/// <param name="str">原始文本</param>
		/// <param name="splitter">分隔符</param>
		/// <returns>拆分得到的List</returns>
		public static List<T> SplitToList<T>(this string str, char splitter)
		{
			return str.Split(splitter).Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
		}

		/// <summary>
		/// 将List转换为指定符号分割的字符串
		/// </summary>
		/// <typeparam name="T">目标类型</typeparam>
		/// <param name="list">目标列表</param>
		/// <param name="splitter">分隔符</param>
		/// <returns>组合得到的字符串</returns>
		public static string GetCombinedString<T>(this List<T> list, char splitter)
		{
			return string.Join(splitter.ToString(), list);
		}

		/// <summary>
		/// 计算字符串MD5
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string MD5Hash(this string input)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					stringBuilder.Append(hashBytes[i].ToString("x2"));
				}

				return stringBuilder.ToString().ToUpper();
			}
		}
	}
}
