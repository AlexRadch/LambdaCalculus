namespace LambdaCalculus;

public delegate TResult SelfApplicable<TResult>(SelfApplicable<TResult> f);

public static partial class Combinators
{
    // U := λf. f f
    public static TResult U<TResult>(SelfApplicable<TResult> f) => f(f);

    //// Ω := λ. ItSelf(ItSelf);
    //public static TResult Ω<TResult>() => U<TResult>(U);

    //// Y := λf. (λx.f (x x)) (λx.f (x x))
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public static Func<T, TResult> Y<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
    //    ((SelfApplicable<Func<T, TResult>>)(x => f(x(x))))(x => f(x(x)));

    //// Z := λf. (λx. f(λv. x x v)) (λx. f (λv. x x v))
    //public static Func<T, TResult> Z1<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
    //    ((SelfApplicableFunc<Func<T, TResult>>)(x => f(v => x(x)(v))))(x => f(v => x(x)(v)));

    // Z := λf. (λx.λv. f (x x) v) (λx.λv. f (x x) v)
    public static Func<T, TResult> Z<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
        ((SelfApplicable<Func<T, TResult>>)(x => v => f(x(x))(v)))(x => v => f(x(x))(v));
}
