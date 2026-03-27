using System;
using BrisaPMS.Domain.Enums;

namespace BrisaPMS.Domain.Exceptions;

public class LanguageNotSupportedException : Exception
{
    public LanguageNotSupportedException() : base("Language not supported") {}
}