namespace Skopka.Abstraction.Data.Tests;

public class Base64ExtensionsTests
{
    [Fact]
    public void ToBase64_ByteArray_EqualsConvert()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };

        var s = bytes.ToBase64();

        Assert.Equal(Convert.ToBase64String(bytes), s);
    }

    [Fact]
    public void ToBase64_Span_EqualsConvert()
    {
        var bytes = new byte[] { 10, 20, 30, 40, 50 };

        var s = bytes.AsSpan().ToBase64();

        Assert.Equal(Convert.ToBase64String(bytes), s);
    }

    [Fact]
    public void FromBase64_Roundtrip()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5, 250, 251, 252, 253, 254, 255 };

        var s = bytes.ToBase64();
        var decoded = s.FromBase64();

        Assert.Equal(bytes, decoded);
    }

    [Fact]
    public void ToBase64Url_Roundtrip()
    {
        var bytes = new byte[] { 0, 1, 2, 3, 10, 11, 12, 250, 251, 252, 253, 254, 255 };

        var s = bytes.ToBase64Url();
        var decoded = s.FromBase64Url();

        Assert.Equal(bytes, decoded);
    }

    [Fact]
    public void ToBase64Url_DoesNotContainPlusSlashOrPadding()
    {
        var bytes = new byte[]
        {
            0xFB, 0xFF, 0xEF, 0x00, 0x10, 0x20, 0x30, 0x40,
            0x50, 0x60, 0x70, 0x80, 0x90, 0xA0, 0xB0, 0xC0
        };

        var s = bytes.ToBase64Url();

        Assert.DoesNotContain('+', s);
        Assert.DoesNotContain('/', s);
        Assert.DoesNotContain('=', s);
    }

    [Fact]
    public void FromBase64Url_RestoresPadding_Mod4Equals2()
    {
        // 1 byte => Base64 "AA==", Base64Url "AA" (len % 4 == 2)
        var bytes = new byte[] { 0x00 };

        var s = bytes.ToBase64Url();
        Assert.Equal(2, s.Length % 4);

        var decoded = s.FromBase64Url();

        Assert.Equal(bytes, decoded);
    }

    [Fact]
    public void FromBase64Url_RestoresPadding_Mod4Equals3()
    {
        // 2 bytes => Base64 "AAE=", Base64Url "AAE" (len % 4 == 3)
        var bytes = new byte[] { 0x00, 0x01 };

        var s = bytes.ToBase64Url();
        Assert.Equal(3, s.Length % 4);

        var decoded = s.FromBase64Url();

        Assert.Equal(bytes, decoded);
    }

    [Fact]
    public void FromBase64Url_Invalid_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => "!!".FromBase64Url());
    }

    [Fact]
    public void FromBase64_Invalid_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => "!!!".FromBase64());
    }

    [Fact]
    public void EmptyBase64_Roundtrip()
    {
        var bytes = Array.Empty<byte>();

        var s = bytes.ToBase64();
        var decoded = s.FromBase64();

        Assert.Equal(bytes, decoded);
    }

    [Fact]
    public void EmptyBase64Url_Roundtrip()
    {
        var bytes = Array.Empty<byte>();

        var s = bytes.ToBase64Url();
        var decoded = s.FromBase64Url();

        Assert.Equal(bytes, decoded);
    }
}