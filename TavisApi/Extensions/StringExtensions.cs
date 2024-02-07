namespace Tavis.Extensions;

public static class Extensions {
    public static string? NullIfWhiteSpace(this string? value) {
      if (value == null) return null;

      if (string.IsNullOrWhiteSpace(value)) { return null; }
      return value;
    }
}