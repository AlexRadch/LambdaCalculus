﻿
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // CPair := λb. (λf.λs. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic CPair(Boolean b);

    #endregion

    #region Operations

    // Pair := λf.λs.λb. b f s
    public static Func<dynamic, CPair> Pair(dynamic f) => s => b => b(f)(s);

    // First := λp. p (λf.λs. f) := λp. p True
    public static dynamic First(CPair p) => p(f => s => f);

    // Second := λp. p (λf.λs. s) := λp. p False
    public static dynamic Second(CPair p) => p(f => s => s);

    #endregion

    #region Extensions

    // Church CPair to (dynamic, dynamic) value tupple
    // UnChurch := λp. (λf.λs. (f, s))
    public static (dynamic First, dynamic Second) UnChurch(this CPair p) => p(f => s => (f, s));

    #endregion
}
