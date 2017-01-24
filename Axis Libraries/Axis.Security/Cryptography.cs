#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
#endregion

namespace Axis.Security
{
    /// <summary>
    /// It is used for compute hash with salt, encrypt and decrypt the data
    /// </summary>
    public class Cryptography
    {
        #region Public Methods
        /// <summary>
        /// Generates a hash for the given plain text value and returns a base64-encoded result. Before the hash is computed, a random salt is generated and appended to the plain text.
        /// </summary>
        /// <param name="plainText">
        /// Plain text value to be hashed.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", "SHA256", "SHA384", and "SHA512"
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                saltBytes = GenerateSalt();
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash value. Plain text is hashed with the same salt value as the original hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", "SHA256", "SHA384", and "SHA512"
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash matches the specified hash the function the return value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }
       
        /// <summary>
        /// Method to Encrypt data
        /// </summary>
        /// <param name="sourceData">Source String</param>
        /// <param name="keyValue">Key</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string sourceData, string keyValue)
        {
            //Allocate SymmetricAlgorithm
            SymmetricAlgorithm caSymmetricAlgorithm = caSymmetricAlgorithm = new RijndaelManaged(); ;

            //Converts source data to byte array
            byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(sourceData);
            
            //create a MemoryStream so that the process can be done without I'O files
            System.IO.MemoryStream msMemoryStream = new System.IO.MemoryStream();

            byte[] bytKey = GetKey(keyValue, caSymmetricAlgorithm);
            
            // set the private key
            caSymmetricAlgorithm.Key = bytKey;
            caSymmetricAlgorithm.IV = bytKey;

            // create an Encryptor from the Provider Service instance
            ICryptoTransform ictEncrypto = caSymmetricAlgorithm.CreateEncryptor();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream csCryptoStream = new CryptoStream(msMemoryStream, ictEncrypto, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            csCryptoStream.Write(bytIn, 0, bytIn.Length);
            csCryptoStream.FlushFinalBlock();

            //convert into Base64 so that the result can be used in xml
            var encriptedData = System.Convert.ToBase64String(msMemoryStream.ToArray());
            //var encriptedData = System.Convert.ToBase64String(msMemoryStream.ToArray()).Replace(" ", "+");
            return encriptedData;
        }
        
        /// <summary>
        /// Method to Encrypt data
        /// </summary>
        /// <param name="source">Source String</param>
        /// <param name="key">Key</param>
        /// <returns>Encrypted string</returns>
        public static string Decrypt(string sourceData, string keyValue)
        {
            SymmetricAlgorithm caSymmetricAlgorithm = caSymmetricAlgorithm = new RijndaelManaged(); ;
            
            // convert from Base64 to binary
            try
            {
                byte[] bytIn = System.Convert.FromBase64String(sourceData);
                //create a MemoryStream with the input
                System.IO.MemoryStream msMemoryStream = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

                byte[] bytKey = GetKey(keyValue, caSymmetricAlgorithm);

                //set the private key
                caSymmetricAlgorithm.Key = bytKey;
                caSymmetricAlgorithm.IV = bytKey;

                //create a Decryptor from the Provider Service instance
                ICryptoTransform ictEncrypto = caSymmetricAlgorithm.CreateDecryptor();

                //create Crypto Stream that transforms a stream using the decryption
                CryptoStream cs = new CryptoStream(msMemoryStream, ictEncrypto, CryptoStreamMode.Read);

                //read out the result from the Crypto Stream
                System.IO.StreamReader srStreamReader = new System.IO.StreamReader(cs);
                return srStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates byte array of salt.
        /// </summary>
        /// <returns></returns>
        static byte[] GenerateSalt()
        {
            byte[] saltBytes = null;
            // Define min and max salt sizes.
            int minSaltSize = 4;
            int maxSaltSize = 8;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);

            return saltBytes;
        }

        /// <summary>
        /// Method to return key
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        static byte[] GetKey(string keyValue, SymmetricAlgorithm caSymmetricAlgorithm)
        {
            string tempValue;
            if (caSymmetricAlgorithm.LegalKeySizes.Length > 0)
            {
                int lessSize = 0;
                int moreSize = caSymmetricAlgorithm.LegalKeySizes[0].MinSize;
                //key sizes are in bits
                while (keyValue.Length * 8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += caSymmetricAlgorithm.LegalKeySizes[0].SkipSize;
                }
                tempValue = keyValue.PadRight(moreSize / 8, ' ');
            }
            else
            {
                tempValue = keyValue;
            }
            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(tempValue);
        }

    
        #endregion
    }
}
