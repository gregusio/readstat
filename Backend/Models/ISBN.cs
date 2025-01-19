using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Backend.Models;

public class ISBN : IEquatable<ISBN>
{
    public string Value { get; private set;}

    private ISBN()
    {
        Value = string.Empty;
    }

    private ISBN(string value)
    {
        Value = value;
    }

    public static ISBN Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !IsValid(value))
        {
            return new ISBN(string.Empty);
        }

        return new ISBN(value);
    }

    private static bool IsValid(string value)
    {
        if (value.Length == 10)
        {
            return IsValidISBN10(value);
        }
        else if (value.Length == 13)
        {
            return IsValidISBN13(value);
        }

        return false;
    }

    private static bool IsValidISBN10(string value)
    {
        var regex = new Regex(@"^\d{9}[\d|X]$");
        if (regex.IsMatch(value))
        {
            return true;
        }

        return false;
    }

    private static bool IsValidISBN13(string value)
    {
        var regex = new Regex(@"^\d{13}$");
        if (regex.IsMatch(value))
        {
            return true;
        }

        return false;
    }

    public bool Equals(ISBN? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ISBN);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
