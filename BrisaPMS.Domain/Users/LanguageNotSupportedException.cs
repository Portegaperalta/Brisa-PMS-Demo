using System;

namespace BrisaPMS.Domain.Users;

public class LanguageNotSupportedException : Exception
{
    public LanguageNotSupportedException() : base("Language not supported") {}
}