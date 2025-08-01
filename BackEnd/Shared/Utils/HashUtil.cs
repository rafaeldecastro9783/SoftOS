using System.Security.Cryptography;
using System.Text;

namespace SoftOS.Shared.Utils
{
    public static class HashUtil
    {
        private const string _salt =
            $@"
Putting on my black dress
Gotta clean up this mess
Black lips
Pale skin and cut crease";

        public static string ComputarPBKDF2(string str)
        {
            byte[] password = Encoding.UTF8.GetBytes(str);
            byte[] salt = Encoding.UTF8.GetBytes(_salt);
            return Convert.ToBase64String(
                Rfc2898DeriveBytes.Pbkdf2(password, salt, 1000, HashAlgorithmName.SHA512, 48)
            );
        }
    }
}
