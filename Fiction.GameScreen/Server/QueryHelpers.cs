using System.Text;
using System.Text.Encodings.Web;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Helpers for adding query strings to URIs
    /// </summary>
    /// <remarks>
    /// Copied from https://github.com/aspnet/HttpAbstractions/blob/master/src/Microsoft.AspNetCore.WebUtilities/QueryHelpers.cs
    /// </remarks>
    public static class QueryHelpers
    {
        /// <summary>
        /// Append the given query key and value to the URI.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="name">The name of the query key.</param>
        /// <param name="value">The query value.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(string uri, string name, string value)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return AddQueryString(
                uri, new[] { new KeyValuePair<string, string>(name, value) });
        }

        /// <summary>
        /// Append the given query keys and values to the URI
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="queryString">Items to add to the URI</param>
        /// <returns></returns>
        public static string AddQueryString(
            string uri,
            IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            int anchorIndex = uri.IndexOf('#');
            string uriToBeAppended = uri;
            string anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurence.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            int queryIndex = uriToBeAppended.IndexOf('?');
            bool hasQuery = queryIndex != -1;

            StringBuilder sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (KeyValuePair<string, string> parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}
