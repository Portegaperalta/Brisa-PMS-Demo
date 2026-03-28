using System;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Url
    {
        public string Value { get; }

        public Url(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new EmptyRequiredFieldException("Url");

            if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
                throw new InvalidFieldException("Url", "must have a valid URL format");

            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                throw new InvalidFieldException("Url", "must use http or https");

            Value = url.Trim();
        }
    }
}
