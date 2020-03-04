using System;
using System.Collections.Generic;

public enum Exchange : byte
{
	None,
	Binance,
	Bitmex,
	Bitfinex,
}

public enum SideType : sbyte
{
	Buy = 1,

	Sell = -1,

	None = 125
}


public struct TradeData
{
	public TradeData(Exchange e, int tradeId, decimal quantity, decimal price, SideType side, DateTime date, string instrument)
	{
		Exchange = e;
		TradeId = tradeId;
		Quantity = quantity;
		Price = price;
		Side = side;
		Date = date;
		Instrument = instrument;
	}

	public Exchange Exchange { get; }
	public int TradeId { get; }
	public DateTime Date { get; }
	public decimal Quantity { get; }
	public decimal Price { get; }
	public SideType Side { get; }
	public string Instrument{ get; }

	public override string ToString()
	{
		return $"{new DateTimeOffset(Date).ToUnixTimeMilliseconds()} : {Quantity} {Price} {Side}";
	}
}


public class Config
{
	/// <summary>
	/// Интервал, за который кэшируем дату в файл
	/// </summary>
	public int FileIntervalInMinutes { get; set; }

	/// <summary>
	/// Интервал, за который собираем дату по ws и записываем в файл
	/// </summary>
	public int CacheIntervalInMinutes { get; set; }

	public List<ConnectionData> ConnectionsList { get; set; }
}

public class ConnectionData
{
	public Exchange Exchange { get; set; }

	/// <summary>
	/// Например USD_RUB
	/// </summary>
	public string InstrumentName { get; set; }
}