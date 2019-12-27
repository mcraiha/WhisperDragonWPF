using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	public enum DeserializationFormat
	{
		None = 0,
		Json = 1,
		XML = 2,

		Unknown = 999999,
	}

	public static class DeserializationDefinitions
	{
		public static readonly Dictionary<DeserializationFormat, (Func<byte[], bool> isThisFormat, Func<byte[], CommonSecretsContainer> deserialize)> deserializers = new Dictionary<DeserializationFormat, (Func<byte[], bool>, Func<byte[], CommonSecretsContainer>)>()
		{
			{ DeserializationFormat.Json, (IsJsonFile, ParseJsonFile)}
		};

		private static readonly string jsonMagicStart = "{\"version\"";
		private static bool IsJsonFile(byte[] input)
		{
			if (input == null || input.Length < jsonMagicStart.Length)
			{
				return false;
			}

			string text = Encoding.UTF8.GetString(input);
			string startWithWhiteSpaceTrimmed = Regex.Replace(text.Substring(0, 20), @"\s+", "");
			return startWithWhiteSpaceTrimmed.StartsWith(jsonMagicStart);
		}

		private static CommonSecretsContainer ParseJsonFile(byte[] input)
		{
			return JsonSerializer.Deserialize<CommonSecretsContainer>(Encoding.UTF8.GetString(input));
		}
	}
}