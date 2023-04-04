using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using static System.Convert;
using System.Security.Principal;

namespace Packt.Shared;

public static class Protector
{
    private static readonly byte[] salt = Encoding.Unicode.GetBytes("7BANANAS");
    
    private static readonly int iterations = 150_000;

    private static Dictionary<string, User> Users = new();

    public static User Register(string username, string password, string[]? roles = null)
    {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        string saltText = ToBase64String(saltBytes);
        
        string saltedHashedPassword = SaltAndHashPassword(password, saltText);
        
        User user = new(username, saltText, saltedHashedPassword, roles);
        Users.Add(user.Name, user);

        return user;
    }

    public static string? PublicKey;
    
    // check a user's password that is stored  in the private dictionary Users
    public static bool CheckPassword(string username, string password)
    {
        if(!Users.ContainsKey(username))
        {
            return false;
        }

        User u = Users[username];
        return CheckPassword(password, u.Salt, u.SaltedHashedPassword);
    }
    
    // check a password against a salt and hashed password
    public static bool CheckPassword(string password, string saltText, string saltedHashedPassword)
    {
        string saltedHashedPassword2 = SaltAndHashPassword(password, saltText);
        return (saltedHashedPassword == saltedHashedPassword2);
    }
    
    private static string SaltAndHashPassword(string password, string saltText)
    {
        using (SHA256 sha = SHA256.Create())
        {
            string saltedPassword = password + saltText;
            return ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
        }
    }

    public static string Encrypt(string plainText, string password)
    {
        byte[] encryptedBytes;
        byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
        
        using (Aes aes = Aes.Create())
        {
            Stopwatch time = Stopwatch.StartNew();
            
            using(Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                WriteLine("PBKDF2 algorithm: {0}, Iteration count: {1:N0}", pbkdf2.HashAlgorithm, pbkdf2.IterationCount);
                
                aes.Key = pbkdf2.GetBytes(32);
                aes.IV = pbkdf2.GetBytes(16);
            }
            
            time.Stop();
            
            WriteLine("{0:N0} milliseconds to generate key and IV.", time.ElapsedMilliseconds);
            
            WriteLine("Encryption algorithm: {0}-{1}, {2} mode with {3} padding.", "AES", aes.KeySize, aes.Mode, aes.Padding);

            using (MemoryStream ms = new())
            {
                using (ICryptoTransform transformer = aes.CreateEncryptor())
                {
                    using (CryptoStream cs = new(ms, transformer, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        
                        if(!cs.HasFlushedFinalBlock)
                        {
                            cs.FlushFinalBlock();
                        }
                    }
                }
                encryptedBytes = ms.ToArray();
            }
        }

        return ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string cipherText, string password)
    {
        byte[] plainBytes;
        byte[] cryptoBytes = FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            using(Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                aes.Key = pbkdf2.GetBytes(32);
                aes.IV = pbkdf2.GetBytes(16);
            }

            using (MemoryStream ms = new())
            {
                using (ICryptoTransform transformer = aes.CreateDecryptor())
                {
                    using (CryptoStream cs = new(ms, transformer, CryptoStreamMode.Write))
                    {
                        cs.Write(cryptoBytes, 0, cryptoBytes.Length);
                        
                        if(!cs.HasFlushedFinalBlock)
                        {
                            cs.FlushFinalBlock();
                        }
                    }
                }
                plainBytes = ms.ToArray();
            }
        }
        return Encoding.Unicode.GetString(plainBytes);
    }

    public static string GenerateSignature(string data)
    {
        byte[] dataBytes = Encoding.Unicode.GetBytes(data);
        SHA256 sha = SHA256.Create();
        byte[] hashedData = sha.ComputeHash(dataBytes);
        RSA rsa = RSA.Create();
        
        PublicKey = rsa.ToXmlString(false); // exclude private key
        
        return ToBase64String(rsa.SignHash(hashedData, 
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }

    public static bool ValidateSignature(string data, string signature)
    {
        if (PublicKey == null)
        {
            return false;
        }
        
        byte[] dataBytes = Encoding.Unicode.GetBytes(data);
        SHA256 sha = SHA256.Create();
        
        byte[] hashedData = sha.ComputeHash(dataBytes);
        byte[] signatureBytes = FromBase64String(signature);
        
        RSA rsa = RSA.Create();
        rsa.FromXmlString(PublicKey);
        
        return rsa.VerifyHash(hashedData, signatureBytes, 
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
    
    public static byte[] GetRandomKeyOrIV(int size)
    {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] data = new byte[size];
        rng.GetBytes(data);
        return data;
    }

    public static void LogIn(string username, string password)
    {
        if (CheckPassword(username, password))
        {
            GenericIdentity gi = new(name: username, type: "PackAuth");
            
            GenericPrincipal gp = new(identity: gi, roles: Users[username].Roles);
            
            Thread.CurrentPrincipal = gp;
        }
    }
}