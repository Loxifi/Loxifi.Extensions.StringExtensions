namespace Loxifi.Extensions.StringExtensions
{
    public class StringSplitOptions
    {
        /// <summary>
        /// If the string is quoted, the quote character
        /// </summary>
        public char QuoteCharacter { get; set; } = '"';

        /// <summary>
        /// The character to split the string on
        /// </summary>
        public char ItemDelimeter { get; set; } = ',';

        /// <summary>
        /// If true, the quote character will be removed on splitting
        /// </summary>
        public bool RemoveQuotes { get; set; } = true;
    }
}
