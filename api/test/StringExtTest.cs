using NUnit.Framework;
using api;

namespace api_test
{
  public class StringExtTest
  {
    [Test]
    public void StripMargin_returns_empty_text_when_input_is_empty()
    {
      Assert.That("".StripMargin(), Is.Empty);
    }

    [TestCase(" ")]
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\t\t\t")]
    [TestCase(" \t \t \t")]
    public void StripMargin_returns_intact_text_when_input_is_whitespaces(string text)
    {
      Assert.That(text.StripMargin(), Is.EqualTo(text));
    }

    [Test]
    public void StripMargin_returns_intact_text_when_there_is_no_magin_character_at_the_beginning()
    {
      var text = " \t  \t\t   no magrgin";
      Assert.That(text.StripMargin(), Is.EqualTo(text));
    }

    [TestCase("|text", "text")]
    [TestCase("| text", " text")]
    [TestCase(" |text", "text")]
    [TestCase(" | text", " text")]
    [TestCase(" \t \t\t  |text", "text")]
    [TestCase("| \t \t\t  text", " \t \t\t  text")]
    public void StripMargin_returns_text_after_margin(string origin, string stripped)
    {
      Assert.That(origin.StripMargin(), Is.EqualTo(stripped));
    }

    [Test]
    public void StripMargin_multiple_lines_text_returns_text_with_all_lines_stripped()
    {
      Assert.That(@"
        |one 1
        |two 2
        |three 3
        |".StripMargin(), Is.EqualTo("\none 1\ntwo 2\nthree 3\n"));
    }
  }
}