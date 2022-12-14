using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Template.Common.Shared.Tokens;

public static class OneTimePin
{
    public static string Generate()=>GetInt32(100001, 1000000).ToString();
    public static string Hash(string str)=>BCrypt.Net.BCrypt.HashPassword(str);
    public static bool VerifyHash(string str, string hash) =>BCrypt.Net.BCrypt.Verify(str, hash);

    private static int GetInt32(int fromInclusive, int toExclusive)
    {
        if (fromInclusive >= toExclusive)
            throw new ArgumentException();

        // The total possible range is [0, 4,294,967,295).
        // Subtract one to account for zero being an actual possibility.
        uint range = (uint)toExclusive - (uint)fromInclusive - 1;

        // If there is only one possible choice, nothing random will actually happen, so return
        // the only possibility.
        if (range == 0)
        {
            return fromInclusive;
        }

        // Create a mask for the bits that we care about for the range. The other bits will be
        // masked away.
        uint mask = range;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;

        Span<uint> resultSpan = stackalloc uint[1];
        uint result;

        do
        {
            RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(resultSpan));
            result = mask & resultSpan[0];
        }
        while (result > range);

        return (int)result + fromInclusive;
    }
}