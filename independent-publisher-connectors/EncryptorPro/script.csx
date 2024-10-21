using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        try
        {
            // Read the request content
            string requestBody = await this.Context.Request.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(requestBody);
            string operationId = this.Context.OperationId;

            // Variables for the result
            string resultKey = "";
            string resultValue = "";

            // Encryption key and IV (should be securely stored)
            byte[] Key = Encoding.UTF8.GetBytes("9aB#fGh7!kLmNoPq1rStUvWx3yZ$2%4^");
            byte[] IV = Encoding.UTF8.GetBytes("5&7*9(0@AaBbCcDd");

            // Check the operationId to determine which logic to execute
            switch (operationId)
            {
                case "EncryptData":
                    string plainText = (string)json["text"];
                    resultValue = EncryptString(plainText, Key, IV);
                    resultKey = "encryptedText";
                    break;

                case "DecryptData":
                    string encryptedText = (string)json["encryptedText"];
                    resultValue = DecryptString(encryptedText, Key, IV);
                    resultKey = "decryptedText";
                    break;

                default:
                    return CreateErrorResponse("Invalid operation.", HttpStatusCode.BadRequest);
            }

            // Create the JSON response with the appropriate result
            JObject resultJson = new JObject
            {
                [resultKey] = resultValue
            };

            response.Content = new StringContent(resultJson.ToString(), Encoding.UTF8, "application/json");
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Internal Server Error: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    // Encryption method
    private static string EncryptString(string plainText, byte[] Key, byte[] IV)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException("The plain text cannot be null or empty.");

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                byte[] encrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(encrypted);
            }
        }
    }

    // Decryption method
    private static string DecryptString(string cipherText, byte[] Key, byte[] IV)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException("The cipher text cannot be null or empty.");

        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    private HttpResponseMessage CreateErrorResponse(string message, HttpStatusCode statusCode)
    {
        JObject errorJson = new JObject
        {
            ["error"] = message
        };

        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(errorJson.ToString(), Encoding.UTF8, "application/json")
        };
    }
}