namespace Bcm.Extensions;

public static class Extensions {
    public static string? NullIfWhiteSpace(this string value) {
      if (value == null) return null;

      if (String.IsNullOrWhiteSpace(value)) { return null; }
      return value;
    }
}