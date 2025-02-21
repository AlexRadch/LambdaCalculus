namespace LambdaCalculus;

public delegate TResult SelfApplicable<TResult>(SelfApplicable<TResult> f);

public static partial class Combinators
{
    // U := λf. f f
    public static TResult U<TResult>(SelfApplicable<TResult> f) => f(f);

    //// Ω := λ. ItSelf(ItSelf);
    //public static TResult Ω<TResult>() => U<TResult>(U);

    //// Y := λf. U (λx. f (x x))
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public static Func<T, TResult> Y<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
    //    U<Func<T, TResult>>(x => f(x(x)));

    // Z := λf. U (λx. f (λv. x x v))
    public static Func<T, TResult> Z<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
        U<Func<T, TResult>>(x => f(v => x(x)(v)));

}
