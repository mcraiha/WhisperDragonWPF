using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	public static class SerializationDefinitions
	{
		public static readonly Dictionary<DeserializationFormat, Func<CommonSecretsContainer, byte[]>> serializers = new Dictionary<DeserializationFormat, Func<CommonSecretsContainer, byte[]>>()
		{
			{ DeserializationFormat.Json, CommonSecretsContainerToJSONBytes}
		};

		// Settings for JSON serialization
		private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
		{
			WriteIndented = true
		};

		private static byte[] CommonSecretsContainerToJSONBytes(CommonSecretsContainer csc)
		{
			return JsonSerializer.SerializeToUtf8Bytes(csc, serializerOptions);
		}
	}
}