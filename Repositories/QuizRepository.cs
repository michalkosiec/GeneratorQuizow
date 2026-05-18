using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using GeneratorQuizow.Models;

namespace GeneratorQuizow.Repositories
{
    public interface IQuizRepository
    {
        void SaveEncrypted(Quiz quiz, string fileName);
        Quiz LoadDecrypted(string fileName);
        bool Exists(string fileName);
        void Delete(string fileName);
        byte[] GetRawEncryptedBytes(string fileName);
    }

    public class QuizRepository : IQuizRepository
    {
        private readonly byte[] _key = "12345678901234567890123456789012"u8.ToArray();
        private readonly byte[] _iv = "1234567890123456"u8.ToArray();

        public void SaveEncrypted(Quiz quiz, string fileName)
        {
            string json = JsonSerializer.Serialize(quiz);
            byte[] encryptedData = EncryptStringToBytes_Aes(json, _key, _iv);
            File.WriteAllBytes(fileName, encryptedData);
        }

        public Quiz LoadDecrypted(string fileName)
        {
            if (!Exists(fileName))
                throw new FileNotFoundException();

            byte[] encryptedData = File.ReadAllBytes(fileName);
            string json = DecryptStringFromBytes_Aes(encryptedData, _key, _iv);
            return JsonSerializer.Deserialize<Quiz>(json) ?? new Quiz();
        }

        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        public void Delete(string fileName)
        {
            if (Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

    private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = IV;
    
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using MemoryStream msEncrypt = new MemoryStream();
    
        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
        }
    
        return msEncrypt.ToArray(); 
    }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msDecrypt = new MemoryStream(cipherText);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
        public byte[] GetRawEncryptedBytes(string fileName)
        {
            if (!Exists(fileName))
                throw new FileNotFoundException();

            return File.ReadAllBytes(fileName);
        }
    }
    
}