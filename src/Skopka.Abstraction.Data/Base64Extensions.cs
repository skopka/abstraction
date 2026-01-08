namespace Skopka.Abstraction.Data;

public static class Base64Extensions
{
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public static string ToBase64(this ReadOnlySpan<byte> span)
    {
        return Convert.ToBase64String(span);
    }
    
    public static string ToBase64Url(this byte[] bytes)
    {
        return bytes.ToBase64().ToBase64Url();
    }
    
    public static string ToBase64Url(this ReadOnlySpan<byte> span)
    {
        return span.ToBase64().ToBase64Url();
    }

    extension(string value)
    {
        private string ToBase64Url()
        {
            return value.TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public byte[] FromBase64Url()
        {
            value = value.Replace('-', '+').Replace('_', '/');
            switch (value.Length % 4) { case 2: value += "=="; break; case 3: value += "="; break; }
            return value.FromBase64();
        }

        public byte[] FromBase64()
        {
            return Convert.FromBase64String(value);
        }
    }
}