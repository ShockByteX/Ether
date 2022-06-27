using System.Runtime.InteropServices;

namespace Ether.Marshaling;

internal static class MarshalType
{
    public static byte[] Convert(object value)
    {
        var type = value.GetType();

        if (type.IsEnum)
        {
            type = type.GetEnumUnderlyingType();
        }

        var size = Marshal.SizeOf(value);
        var typeCode = Type.GetTypeCode(type);

        return Convert(value, type, typeCode, size);
    }

    internal static byte[] Convert(object value, Type type, TypeCode typeCode, int size)
    {
        switch (typeCode)
        {
            case TypeCode.Object: break;
            case TypeCode.Boolean: return Convert((bool)value, 1);
            case TypeCode.Byte: return new[] { (byte)value };
            case TypeCode.SByte: return new[] { (byte)(sbyte)value };
            case TypeCode.Int16: return Convert((short)value, 2);
            case TypeCode.UInt16: return Convert((ushort)value, 2);
            case TypeCode.Int32: return Convert((int)value, 4);
            case TypeCode.UInt32: return Convert((uint)value, 4);
            case TypeCode.Int64: return Convert((long)value, 8);
            case TypeCode.UInt64: return Convert((ulong)value, 8);
            case TypeCode.Single: return Convert((float)value, 4);
            case TypeCode.Double: return Convert((double)value, 8);
            case TypeCode.Decimal: return Convert((decimal)value, 16);
            case TypeCode.DateTime: return Convert((DateTime)value, 8);
            default: throw new InvalidCastException("Conversion not support");
        }

        var data = new byte[size];

        unsafe
        {
            fixed (byte* pData = data)
            {
                Marshal.StructureToPtr(value, (IntPtr)pData, false);
            }
        }

        return data;
    }

    private static byte[] Convert<TValueType>(TValueType value, int size) where TValueType : struct
    {
        var data = new byte[size];

        MemoryMarshal.Write(data, ref value);

        return data;
    }
}