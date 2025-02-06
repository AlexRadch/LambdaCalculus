using System;

namespace FooNamespace;


public delegate Func<dynamic, dynamic> Boolean(dynamic @true);

public static partial class FooClass
{

    // False := λt.λf. f
    public static Func<F, F> FalseF<T, F>(T t) => f => f;
    public static readonly Boolean False = FalseF<dynamic, dynamic>;
}
