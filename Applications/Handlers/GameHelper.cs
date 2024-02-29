

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

public static class GameHelper
{
    public static string ParseString<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public static T ParseStruct<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    // write a function random string
    public static string RandomString(int len)
    {
        var rdn = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid() +$"{DateTime.Now}"));
        return rdn[..len];
    }

    public static string hashPasswordMD5(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}