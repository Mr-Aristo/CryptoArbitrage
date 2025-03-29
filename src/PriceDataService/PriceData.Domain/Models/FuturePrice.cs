using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceData.Domain.Models;

public class FuturePrice
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public DateTime TimeStamp { get; set; }
    public decimal Price { get; set; }
}
