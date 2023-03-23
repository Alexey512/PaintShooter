using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Core.Utility
{
	public static class ObjectCloner
	{
		public static T Clone<T>(this T source)
		{
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}

		public static T CloneJson<T>(object source)
		{            
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}

			return (T)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(source), source.GetType());
		}
	}
}
