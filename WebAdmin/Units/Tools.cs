using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAdmin.Models;

namespace WebAdmin.Units;
public static class Tools
{
    public static string Https = "https://";

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
