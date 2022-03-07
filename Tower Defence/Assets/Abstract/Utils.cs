using System.Text;

namespace Abstract
{
    /// <summary>
    /// Global generic utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Adds spaces before capital letters to a string.
        /// </summary>
        /// <param name="text">String to space</param>
        /// <returns>Spaced <see cref="string"/></returns>
        public static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
