using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAdmin.Models;

namespace WebAdmin.Units;
public static class Tools
{
    public static string https = "https://";
    public static async Task<bool> IsWebsiteUp(string url)
    {


        if (!url.StartsWith(https))
            url = https + url;

        using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(10) };
        HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        return response.IsSuccessStatusCode; // 2xx 视为网站正常
    }
}
