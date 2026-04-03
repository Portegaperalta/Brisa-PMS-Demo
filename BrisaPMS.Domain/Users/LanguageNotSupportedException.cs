using System;

namespace BrisaPMS.Domain.User;

public class LanguageNotSupportedException : Exception
{
    public LanguageNotSupportedException() : base("Language not supported") {}
}