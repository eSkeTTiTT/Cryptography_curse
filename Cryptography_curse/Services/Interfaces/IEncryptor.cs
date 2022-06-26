using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cryptography_curse.Services.Interfaces
{
    public interface IEncryptor
    {
        void Encrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
        bool Decrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
        Task EncryptAsync(string sourcePath, string destinationPath, string password, int bufferSize = 102400, IProgress<double> progress = null, CancellationToken cancel = default);
        Task<bool> DecryptAsync(string sourcePath, string destinationPath, string password, int bufferSize = 102400, IProgress<double> progress = null, CancellationToken cancel = default);
    }
}
