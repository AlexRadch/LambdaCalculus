
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // CPair := λb. (λf.λs. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic CPair<F, S>(Boolean<F, S> b);

    #endregion

    #region Operations

    // Pair := λf.λs.λb. b f s
    public static Func<S, CPair<F, S>> Pair<F, S>(F f) => s => b => b(f)(s);

    // First := λp. p (λx.λy. x) := λp. p True
    public static F First<F, S>(CPair<F, S> p) => p(f => s => f!);

    // Second := λp. p (λx.λy. y) := λp. p False
    public static S Second<F, S>(CPair<F, S> p) => p(f => s => s!);

    #endregion

    #region Extensions

    // Church CPair to (F, S) value tupple
    // UnChurch := λb. If b true false := b true false
    public static (F First, S Second) UnChurch<F, S>(this CPair<F, S> p) => p(f => s => (f, s));

    #endregion
}
