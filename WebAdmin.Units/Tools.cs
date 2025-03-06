using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAdmin.Units;
public static class Tools
{
    /// <summary>
    /// Https
    /// </summary>
    public static string Https = "https://";
    static public string DbName = "WebAdmin20250304";

    /// <summary>
    /// 获取并创建数据库文件路径
    /// </summary>
    /// <returns></returns>
    public static string GetAndCreatDbFilePath()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string? appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        if (string.IsNullOrWhiteSpace(appName))
        {
            _ = new Exception("应用名称不能为空");
        }

#pragma warning disable CS8604 // 引用类型参数可能为 null。
        var qrFolder = Path.Combine(folder, appName);
#pragma warning restore CS8604 // 引用类型参数可能为 null。

        // 如果 IdentifyQRcodeDownload 文件夹不存在，则创建它
        if (!Directory.Exists(qrFolder))
            Directory.CreateDirectory(qrFolder);

        string DbPath = Path.Combine(qrFolder, $"{DbName}.db");
        return DbPath;
    }


    /// <summary>
    /// 修正网址
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string CorrectWebsite(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return url;

        string newUrl = Regex.Replace(url, @"\s+", string.Empty); // 匹配所有空白字符

        if (!url.StartsWith(Https))
            newUrl = Https + url;
        return newUrl;
    }


    /// <summary>
    /// 检查网站是否正常
    /// </summary>
    /// <param name="url">网站链接 </param>
    /// <param name="timeout">超时时间</param>
    /// <returns></returns>
    public static async Task<bool> IsWebsiteUp(string url, double timeout = 5)
    {
        string newUrl = CorrectWebsite(url);
        using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(timeout) };
        HttpResponseMessage response = await client.GetAsync(newUrl, HttpCompletionOption.ResponseHeadersRead);
        return response.IsSuccessStatusCode; // 2xx 视为网站正常
    }

    /// <summary>
    /// 打开网址
    /// </summary>
    /// <param name="newUrl"></param>
    /// <returns></returns>
    public static string OpenUrl(string newUrl)
    {
        if (string.IsNullOrWhiteSpace(newUrl))
            return "地址为空，无法打开";

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = newUrl,
                UseShellExecute = true
            });
            return $"已成功打开 {newUrl}";
        }
        catch (Exception ex)
        {
            return $"打开网址失败: {ex.Message}";
        }
    }
}
