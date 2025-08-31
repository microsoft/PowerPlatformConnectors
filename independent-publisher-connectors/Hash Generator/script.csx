public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        JObject json = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
        string method = json.GetValue("type").ToString();
        string valueToHash = json.GetValue("string").ToString();
        string hashedValue = Hash(valueToHash, method);

        var responseContent = new JObject
        {
            ["hash"] = hashedValue,
            ["valueToHash"] = valueToHash
        };

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

        response.Content = CreateJsonContent(responseContent.ToString());
        return response;
    }

    static string Hash(string input, string method) =>
        method switch
        {
            "md5" => CreateMD5(input),
            "sha1" => CreateSHA1(input),
            "sha256" => CreateSHA256(input),
            "sha512" => CreateSHA512(input),
            _ => CreateSHA1(input)
        };

    public static string CreateMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.Unicode.GetBytes(input));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public static string CreateSHA1(string input)
    {
        using (SHA1Managed sha1 = new SHA1Managed())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public static string CreateSHA256(string input)
    {
        using (SHA256Managed sha256 = new SHA256Managed())
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public static string CreateSHA512(string input)
    {
        using (SHA512Managed sha512 = new SHA512Managed())
        {
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}