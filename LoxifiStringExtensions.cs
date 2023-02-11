using System.Text;

namespace Loxifi.Extensions.StringExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoxifiStringExtensions
    {
        /// <summary>
        /// Splits using the specified options
        /// </summary>
        /// <param name="toSplit">The string to split</param>
        /// <param name="options">Optional options to use when splitting</param>
        /// <returns>An IEnumerable used to obtain the split values</returns>
        public static IEnumerable<string> Split(this IEnumerable<char> toSplit, StringSplitOptions? options = null)
        {
            options ??= new StringSplitOptions();

            StringBuilder currentString = new();
            bool inQuotes = false;
            bool quoteIsEscaped = false; //Store when a quote has been escaped.
            toSplit = toSplit.Concat(new List<char>() { options.ItemDelimeter }); //We add new cells at the delimiter, so append one for the parser.

            int index = -1;

            IEnumerator<char> CharEnumerator = toSplit.GetEnumerator();

            bool hasNextChar = CharEnumerator.MoveNext();

            while (hasNextChar)
            {
                char c = CharEnumerator.Current;
                hasNextChar = CharEnumerator.MoveNext();
                char nextChar = CharEnumerator.Current;

                index++;

                if (c == options.ItemDelimeter) //We hit a delimiter character...
                {
                    if (!inQuotes) //Are we inside quotes? If not, we've hit the end of a cell value.
                    {
                        yield return currentString.ToString();
                        _ = currentString.Clear();
                    }
                    else
                    {
                        _ = currentString.Append(c);
                    }
                }
                else
                {
                    if (c != ' ')
                    {
                        if (c == options.QuoteCharacter) //If we've hit a quote character...
                        {
                            if (inQuotes) //Does it appear to be a closing quote? //How does this even work? How can both of these be true? I didn't write this.. I dont know...
                            {
                                if (nextChar == c && !quoteIsEscaped) //If the character afterwards is also a quote, this is to escape that (not a closing quote).
                                {
                                    quoteIsEscaped = true; //Flag that we are escaped for the next character. Don't add the escaping quote.

                                    if (!options.RemoveQuotes) //unless we want to
                                    {
                                        _ = currentString.Append(c);
                                    }
                                }
                                else if (quoteIsEscaped)
                                {
                                    quoteIsEscaped = false; //This is an escaped quote. Add it and revert quoteIsEscaped to false.
                                    _ = currentString.Append(c);
                                }
                                else
                                {
                                    inQuotes = false;

                                    if (!options.RemoveQuotes)
                                    {
                                        _ = currentString.Append(c);
                                    }
                                }
                            }
                            else
                            {
                                if (!inQuotes)
                                {
                                    inQuotes = true;

                                    if (!options.RemoveQuotes)
                                    {
                                        _ = currentString.Append(c);
                                    }
                                }
                                else
                                {
                                    _ = currentString.Append(c); //...It's a quote inside a quote.
                                }
                            }
                        }
                        else
                        {
                            _ = currentString.Append(c);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(currentString.ToString())) //Append only if not new cell
                        {
                            _ = currentString.Append(c);
                        }
                    }
                }
            }
        }
    }
}
