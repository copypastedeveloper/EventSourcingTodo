using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MedArchon.Common.Utilities
{
    public static class CloneableHelper
    {
        /// <summary>
        /// Implementation of deep copy.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object Clone(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            if (!obj.GetType().IsSerializable) throw new ArgumentException("The type must be serializable.");

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }
    }
}