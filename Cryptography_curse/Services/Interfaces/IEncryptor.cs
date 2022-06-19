namespace Cryptography_curse.Services.Interfaces
{
    public interface IEncryptor
    {
        public void Encrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
        public bool Decrypt(string sourcePath, string destinationPath, string password, int bufferSize = 102400);
    }
}
