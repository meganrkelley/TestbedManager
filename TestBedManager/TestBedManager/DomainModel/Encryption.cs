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
			return text;

			byte[] inBlock = StringToByteAray(text);

			alg.BlockSize = 128;
			alg.KeySize = 256;
			alg.GenerateIV();
			alg.GenerateKey();

			MemoryStream ms = new MemoryStream();
			try {
				using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(inBlock, 0, inBlock.Length);
				}
				//	CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(),
				//		CryptoStreamMode.Write);

				//	cs.Close();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}

			return ByteArrayToString(ms.ToArray());

			//	ICryptoTransform xfrm = alg.CreateEncryptor();
			//	byte[] bytes = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);
		}

		public static string Decrypt(string encryptedText)
		{
			return encryptedText;

			byte[] bytes = StringToByteAray(encryptedText);

			alg.GenerateIV();

			MemoryStream ms = new MemoryStream();

			try {
				using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(bytes, 0, bytes.Length);
				}
				//CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(),
				//CryptoStreamMode.Write);
				//	cs.Write(bytes, 0, bytes.Length);
				//	cs.Close();
			} catch (Exception ex) {
				DebugLog.DebugLog.Log(ex);
			}

			return ByteArrayToString(ms.ToArray());

			//byte[] outBlock = { };

			//try {
			//	ICryptoTransform xfrm = alg.CreateDecryptor();
			//	outBlock = xfrm.TransformFinalBlock(bytes, 0, bytes.Length);
			//} catch (Exception ex) {
			//	DebugLog.DebugLog.Log("Decrypt threw an exception. Encrypted text was received as " + encryptedText + ". Exception: " + ex);
			//}

			//return ByteArrayToString(outBlock);
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