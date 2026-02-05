using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Xpp;

public readonly struct XppDateTime
{
    private static readonly DateTime DtMinValue = new(1901, 1, 1);
    private static readonly DateTime DtMaxValue = new(2154, 12, 31);

    public static readonly XppDateTime MinValue = (XppDateTime)new DateTime(1901, 1, 1);
    public static readonly XppDateTime MaxValue = (XppDateTime)new DateTime(2154, 12, 31);

    private readonly DateTime _value = XppDateTime.MinValue;

    public XppDateTime()
    {
    }

    private XppDateTime(DateTime d)
    {
        _value = d;
    }

    public static explicit operator XppDateTime(DateTime d)
    {
        if (d < DtMinValue || d > DtMaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(d), 
                $"The value {d:O} does not fall within the acceptable range {MinValue._value:O}-{MaxValue._value:O}");
        }

        return new XppDateTime(d);
    }

    public static implicit operator DateTime(XppDateTime x)
    {
        return x._value;
    }
}
