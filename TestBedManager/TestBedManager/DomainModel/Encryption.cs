using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace TestBedManager
{
	public class Encryption
	{
		private static SymmetricAlgorithm alg = Rijndael.Create();

		public static string Encrypt(string text)
		{
			byte[] inBlock = StringToByteAray(text);

			alg.GenerateIV();
			alg.GenerateKey();

			MemoryStream ms = new MemoryStream();
			try {
				using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(inBlock, 0, inBlock.Length);
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}

			return ByteArrayToString(ms.ToArray());
		}

		public static string Decrypt(string encryptedText)
		{
			byte[] bytes = StringToByteAray(encryptedText);

			MemoryStream ms = new MemoryStream();

			try {
				using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(bytes, 0, bytes.Length);
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}

			return ByteArrayToString(ms.ToArray());
		}

		private static string ByteArrayToString(byte[] bytes)
		{
			return UnicodeEncoding.Unicode.GetString(bytes);
		}

		private static byte[] StringToByteAray(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
	}
}