﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cryptography
{
    public class SecretKey
    {
        // Generates a random 32byte secret key
        public static string GenerateKey()
        {
            SymmetricAlgorithm symmAlgorithm = AesCryptoServiceProvider.Create();

            return ASCIIEncoding.ASCII.GetString(symmAlgorithm.Key);
        }


        public static void StoreKey(string secretKey, string outFile)
        {
            FileStream fOutput = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buffer = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                fOutput.Write(buffer, 0, buffer.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Storing key failed with errror -> {0}", e.Message);
            }
            finally
            {
                fOutput.Close();
            }
        }


        public static string LoadKey(string inFile)
        {
            FileStream fInput = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)fInput.Length];

            try
            {
                fInput.Read(buffer, 0, (int)fInput.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Loading key failed with error -> {0}", e.Message);
            }
            finally
            {
                fInput.Close();
            }

            return ASCIIEncoding.ASCII.GetString(buffer);
        }
    }
}
