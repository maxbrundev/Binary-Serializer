using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Serializer
{
    class BinarySerializer
    {
        public byte[] Serialize(object instance)
        {
            if (instance == null)
                return null;

            Type type = instance.GetType();

            if (type == typeof(string))
                return Encoding.UTF8.GetBytes((string)instance);
            if (type.IsPrimitive == true)
            {
                if (type == typeof(System.Char) ||
                    type == typeof(System.Byte) ||
                    type == typeof(System.SByte))
                {
                    return new byte[] { (System.Byte)instance };
                }

                if (type == typeof(System.Int16))
                    return BitConverter.GetBytes((System.Int16)instance);
                if (type == typeof(System.Int32))
                    return BitConverter.GetBytes((System.Int32)instance);
                if (type == typeof(System.Int64))
                    return BitConverter.GetBytes((System.Int64)instance);
                if (type == typeof(System.UInt16))
                    return BitConverter.GetBytes((System.UInt16)instance);
                if (type == typeof(System.UInt32))
                    return BitConverter.GetBytes((System.UInt32)instance);
                if (type == typeof(System.UInt64))
                    return BitConverter.GetBytes((System.UInt64)instance);

                if (type == typeof(System.Single))
                    return BitConverter.GetBytes((System.Single)instance);
                if (type == typeof(System.Double))
                    return BitConverter.GetBytes((System.Double)instance);

                if (type == typeof(System.Boolean))
                    return new byte[] { (bool)instance == true ? (byte)1 : (byte)0 };

                throw new Exception("Unhandled type " + type);
            }
            if (type == typeof(System.Decimal))
                return Encoding.ASCII.GetBytes(((System.Decimal)instance).ToString());

            if (type.IsClass == true)
            {
                using (MemoryStream stream = new MemoryStream(1024))
                {
                    FieldInfo[] fields = type.GetFields();

                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i].IsDefined(typeof(NonSerializedAttribute), false) == true)
                            continue;

                        byte[] raw = this.Serialize(fields[i].Name);
                        stream.Write(raw, 0, raw.Length);
                        raw = this.Serialize(fields[i].GetValue(instance));
                        stream.Write(raw, 0, raw.Length);
                    }

                    return stream.ToArray();
                }
            }
            return null;
        }

        public T Deserialize<T>(byte[] raw)
        {
            if (raw == null)
                return default(T);

            Type type = typeof(T);

            if (type == typeof(string))
                return (T)(object)Encoding.UTF8.GetString(raw);
            if (type.IsPrimitive == true)
            {
                if (type == typeof(System.Char) ||
                    type == typeof(System.Byte) ||
                    type == typeof(System.SByte))
                {
                    return (T)(object)raw[0];
                }

                if (type == typeof(System.Int16))
                    return (T)(object)BitConverter.ToInt16(raw, 0);
                if (type == typeof(System.Int32))
                    return (T)(object)BitConverter.ToInt32(raw, 0);
                if (type == typeof(System.Int64))
                    return (T)(object)BitConverter.ToInt64(raw, 0);

                if (type == typeof(System.UInt16))
                    return (T)(object)BitConverter.ToUInt16(raw, 0);
                if (type == typeof(System.UInt32))
                    return (T)(object)BitConverter.ToUInt32(raw, 0);
                if (type == typeof(System.UInt64))
                    return (T)(object)BitConverter.ToUInt64(raw, 0);

                if (type == typeof(System.Single))
                    return (T)(object)BitConverter.ToSingle(raw, 0);
                if (type == typeof(System.Double))
                    return (T)(object)BitConverter.ToDouble(raw, 0);

                if (type == typeof(System.Boolean))
                    return (T)(object)(raw[0] == 1 ? true : false);

                throw new Exception("Unhandled type " + type);
            }

            if (type == typeof(System.Decimal))
                return (T)(object)Decimal.Parse(Encoding.ASCII.GetString(raw));

            if (type.IsClass == true)
            {
               //TODO
            }

            throw new Exception("Unhandled object");
        }
    }
}