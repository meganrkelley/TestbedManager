﻿using System;
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

			alg.Padding = PaddingMode.None;
			alg.GenerateIV();
			alg.GenerateKey();

			MemoryStream stream = new MemoryStream();
			try {
				using (CryptoStream cs = new CryptoStream(stream, alg.CreateEncryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(inBlock, 0, inBlock.Length);
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Encryption error: " + ex);
			}

			return ByteArrayToString(stream.ToArray());
		}

		public static string Decrypt(string encryptedText)
		{
			return encryptedText;

			byte[] bytes = StringToByteAray(encryptedText);

			MemoryStream stream = new MemoryStream();
			
			try {
				using (CryptoStream cs = new CryptoStream(stream, alg.CreateDecryptor(),
					CryptoStreamMode.Write)) {
					cs.Write(bytes, 0, bytes.Length);
				}
			} catch (Exception ex) {
				DebugLog.DebugLog.Log("Decryption error: " + ex);
			}

			return ByteArrayToString(stream.ToArray());
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