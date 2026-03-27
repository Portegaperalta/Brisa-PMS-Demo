using System;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Url
    {
        public Uri Uri { get; }
        public string Value => Uri.AbsoluteUri;

        public Url(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new EmptyUrlException();

            if (!Uri.TryCreate(raw.Trim(), UriKind.Absolute, out var uri))
                throw new InvalidUrlException();

            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                throw new InvalidUrlException();

            Uri = uri;
        }
    }
}
