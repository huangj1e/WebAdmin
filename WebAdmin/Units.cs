using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAdmin;
public static class Units
{
    public static async Task<bool> IsWebsiteUp(string url)
    {
        using HttpClient client = new();
        try
        {
            HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode; // 2xx 视为网站正常
        }
        catch
        {
            return false; // 请求失败，网站可能不可用
        }
    }
}
