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
                throw new EmptyRequiredFieldException("Url");

            if (!Uri.TryCreate(raw.Trim(), UriKind.Absolute, out var uri))
                throw new InvalidFieldException("Url", "must have a valid URL format");

            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                throw new InvalidFieldException("Url", "must use http or https");

            Uri = uri;
        }
    }
}
