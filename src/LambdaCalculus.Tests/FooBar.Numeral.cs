using System;

namespace FooNamespace;


public delegate dynamic NextNumeral(dynamic Numeral);

public delegate NextNumeral Numeral(NextNumeral next);

public static partial class FooClass
{
    public static readonly Numeral Zero_False = f => z => False(f)(z);
}
