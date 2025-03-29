﻿namespace Arbitrage.Application.DTOs;

public record FuturePriceDto(
    string Symbol, 
    decimal Price, 
    DateTime TimeStamp
    );
