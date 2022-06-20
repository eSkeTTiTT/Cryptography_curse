using Cryptography_curse.Services.Interfaces;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography_curse.Services.Realization
{
    public class Rfc2898Encryptor : IEncryptor
    {
        private static readonly byte[] __Salt =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };

        private static ICryptoTransform GetEncryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);            
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateEncryptor();
        }

        private static ICryptoTransform GetDecryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateDecryptor();
        }

        public bool Decrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400)
        {
            var decryptor = GetDecryptor(password);

            using var destinatiobDecrypted = File.Create(destinationPath, bufferSize);
            using var destination = new CryptoStream(destinatiobDecrypted, decryptor, CryptoStreamMode.Write);
            using var source = File.OpenRead(sourcePath);

            int readed = 0;
            var buffer = new byte[bufferSize];
            do
            {
                readed = source.Read(buffer, 0, bufferSize);
                destination.Write(buffer, 0, readed);
            }
            while (readed > 0);

            try
            {
                destination.FlushFinalBlock();
            }
            catch (CryptographicException)
            {
                return false;
            }

            return true;
        }

        public void Encrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400)
        {
            var encryptor = GetEncryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            using var destinationEncrypted = File.Create(destinationPath, bufferSize);
            using var destination = new CryptoStream(destinationEncrypted, encryptor, CryptoStreamMode.Write);
            using var source = File.OpenRead(sourcePath);

            int readed = 0;
            var buffer = new byte[bufferSize];
            do
            {
                readed = source.Read(buffer, 0, bufferSize);
                destination.Write(buffer, 0, readed);
            }
            while (readed > 0);

            destination.FlushFinalBlock();
        }
    }
}
