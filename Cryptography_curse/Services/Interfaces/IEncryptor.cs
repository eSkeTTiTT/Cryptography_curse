namespace Cryptography_curse.Services.Interfaces
{
    public interface IEncryptor
    {
        void Encrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
        bool Decrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
    }
}
