using System.Text.RegularExpressions;

namespace ZipPay {
  public static class StringExt {
    public static string StripMargin(this string input, char margin='|') {
      return Regex.Replace(input, $"^[ \\t]*\\{margin}", string.Empty, RegexOptions.Multiline);
    }
  }
}