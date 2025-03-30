namespace PriceData.Domain.Exceptions;
public class PriceNotFoundException : Exception
{
    public PriceNotFoundException(string symbol) 
        : base($"Fiyat verisi bulunamadı: {symbol}")
    { }
}

public class InvalidSymbolException : Exception
{
    public InvalidSymbolException(string symbol) 
        : base($"Invalid symbol: {symbol}")
    { }
}

public class ExternalApiException : Exception
{
    public ExternalApiException(string message) 
        : base($"API error: {message}")
    { }
}

public class DatabaseException : Exception
{
    public DatabaseException(string message)
         : base($"Database error: {message}")
    { }

    public DatabaseException(string message, Exception innerException)
        : base($"Database error: {message}", innerException)
    { }
}
