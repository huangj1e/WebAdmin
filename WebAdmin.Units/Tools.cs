using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAdmin.Units;
public static class Tools
{
    public static string Https = "https://";
    static public string DbName = "WebAdmin20250304";

    public static string GetAndCreatDbFilePath()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        var qrFolder = Path.Combine(folder, appName);

        // 如果 IdentifyQRcodeDownload 文件夹不存在，则创建它
        if (!Directory.Exists(qrFolder))
            Directory.CreateDirectory(qrFolder);

        string DbPath = Path.Combine(qrFolder, $"{DbName}.db");
        return DbPath;
    }



    public static string CorrectWebsite(string url)
    {
        if (string.IsNullOrEmpty(url)) return url;

        string newUrl = url;

        if (!url.StartsWith(Https))
            newUrl = Https + url;
        return newUrl;
    }


    public static async Task<bool> IsWebsiteUp(string url)
    {
        string newUrl = CorrectWebsite(url);
        using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) };
        HttpResponseMessage response = await client.GetAsync(newUrl, HttpCompletionOption.ResponseHeadersRead);
        return response.IsSuccessStatusCode; // 2xx 视为网站正常
    }

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
