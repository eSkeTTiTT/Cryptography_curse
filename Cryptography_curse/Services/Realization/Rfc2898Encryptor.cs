using Cryptography_curse.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

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

        #region Async

        public async Task EncryptAsync(string sourcePath, string destinationPath, string password, int bufferSize = 102400, IProgress<double> progress = null, CancellationToken cancel = default)
        {
            #region Check for exceptions

            if (!(File.Exists(sourcePath)))
            {
                throw new FileNotFoundException(nameof(sourcePath));
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            #endregion

            cancel.ThrowIfCancellationRequested();

            var encryptor = GetEncryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            try
            {
                await using var destinationEncrypted = File.Create(destinationPath, bufferSize);
                await using var destination = new CryptoStream(destinationEncrypted, encryptor, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var fileLength = source.Length;
                var lastPercent = 0.0;

                int readed = 0;
                var buffer = new byte[bufferSize];
                do
                {
                    readed = await source.ReadAsync(buffer, 0, bufferSize, cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer, 0, readed, cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / fileLength;

                    // чтобы постоянно не уведомлять
                    if (percent - lastPercent >= 0.01)
                    {
                        progress?.Report(percent);
                        lastPercent = percent;
                    }

                    if (cancel.IsCancellationRequested)
                    {
                        cancel.ThrowIfCancellationRequested();
                    }
                }
                while (readed > 0);

                destination.FlushFinalBlock();

                // если все прошло ок
                progress?.Report(1.0);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in EncryptAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DecryptAsync(string sourcePath, string destinationPath, string password, int bufferSize = 102400, IProgress<double> progress = null, CancellationToken cancel = default)
        {
            #region Check for exceptions

            if (!(File.Exists(sourcePath)))
            {
                throw new FileNotFoundException(nameof(sourcePath));
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            #endregion

            cancel.ThrowIfCancellationRequested();

            var decryptor = GetDecryptor(password);

            try
            {
                await using var destinationDecrypted = File.Create(destinationPath, bufferSize);
                await using var destination = new CryptoStream(destinationDecrypted, decryptor, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var fileLength = source.Length;
                var lastPercent = 0.0;

                int readed = 0;
                var buffer = new byte[bufferSize];
                do
                {
                    readed = await source.ReadAsync(buffer, 0, bufferSize, cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer, 0, readed, cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / fileLength;

                    // чтобы постоянно не уведомлять
                    if (percent - lastPercent >= 0.01)
                    {
                        progress?.Report(percent);
                        lastPercent = percent;
                    }

                    cancel.ThrowIfCancellationRequested();
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

                // если все прошло ок
                progress?.Report(1.0);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in DecryptAsync: {0}", ex.Message);
                throw;
            }

            return true;
        }

        #endregion
    }
}
