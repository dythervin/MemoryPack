using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MemoryPack.Formatters {

// for unamanged types( https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/unmanaged-types )
// * sbyte, byte, short, ushort, int, uint, long, ulong, char, float, double, decimal, or bool
// * Any enum type
// * Any pointer type
// * Any user-defined struct type that contains fields of unmanaged types only
public sealed class UnmanagedFormatter<T> : MemoryPackFormatter<T>
    where T : unmanaged
{
    public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ref T value)
    {
        Unsafe.WriteUnaligned(ref writer.GetSpanReference(Unsafe.SizeOf<T>()), value);
        writer.Advance(Unsafe.SizeOf<T>());
    }

    public override void Deserialize(ref MemoryPackReader reader, ref T value)
    {
        value = Unsafe.ReadUnaligned<T>(ref reader.GetSpanReference(Unsafe.SizeOf<T>()));
        reader.Advance(Unsafe.SizeOf<T>());
    }
}

public sealed class DangerousUnmanagedFormatter<T> : MemoryPackFormatter<T>
{
    public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ref T? value)
    {
        Unsafe.WriteUnaligned(ref writer.GetSpanReference(Unsafe.SizeOf<T>()), value);
        writer.Advance(Unsafe.SizeOf<T>());
    }

    public override void Deserialize(ref MemoryPackReader reader, ref T? value)
    {
        value = Unsafe.ReadUnaligned<T>(ref reader.GetSpanReference(Unsafe.SizeOf<T>()));
        reader.Advance(Unsafe.SizeOf<T>());
    }
}

}