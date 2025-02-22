using System.Diagnostics;

namespace LambdaCalculus;

public static partial class SKI
{
    #region S K I

    // I := λx. x
    public static Func<dynamic, dynamic> I = x => x;
    public static T Id<T>(T x) => x;

    // K := λx.λy. x
    public static readonly Func<dynamic, Func<dynamic, dynamic>> K = x => y => x;

    // S := λx.λy.λz. x z (y z)
    public static readonly Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>> S = x => y => z => x(z)(y(z));

    #endregion

    #region Combinators

    // SkI := SKK
    public static readonly Func<dynamic, dynamic> SkI = S(K)(K);
    public static T SkId<T>(T x) => S(K)(K)(x!);

    // Skι := λx.xSK := S(λx.xS)(λx.K) := S(S(λx.x)(λx.S))(KK) := S(SI(KS))(KK)
    public static readonly Func<dynamic, dynamic> Skι = S(S(I)(K(S)))(K(K));

    #region BCKW

    // B := λxyz.x(yz) := S(KS)K
    public static readonly Func<dynamic, dynamic> B = S(K(S))(K);

    // C := λxyz.xzy := S(S(K(S(KS)K))S)(KK)
    public static readonly Func<dynamic, dynamic> C = S(S(K(S(K(S))(K)))(S))(K(K));

    // W := λxy.xyy := SS(SK)
    public static readonly Func<dynamic, dynamic> W = S(S)(S(K));

    #endregion

    #endregion

    #region Self-application and recursion

    // U := λf.ff := S(λf.f)(λf.f) := SII
    public static readonly Func<dynamic, dynamic> U = S(I)(I);

    public static TResult Uf<TResult>(SelfApplicable<TResult> f) => U(f);

    public static Func<SelfApplicable<TResult>, TResult> Ut<TResult>() => f => U(f);

    //// H := λx.(λy.x(yy)) = λx.(S(λy.x)(λy.yy)) := λx.(S(Kx)(S(λy.y)(λy.y))) := λx.S(Kx)(SII) :=
    //// S(λx.S(Kx))(λx.U) := S(S(λx.S)(λx.Kx))(KU) := S(S(KS)K)(KU)
    //public static readonly Func<dynamic, dynamic> H = S(S(K(S))(K))(K(S(I)(I)));

    //// Yα = Uβ = U(Hα) = S(KU)H α = S(KU)(S(S(KS)K)(KU)) α
    //// λx.f(xx) = S(λx.f)(λx.xx) = S(Kf)(S(λx.x)(λx.x)) = S(Kf)(U)
    //public static readonly Func<dynamic, dynamic> Y = S(K(U))(S(S(K(S))(K))(K(U)));

    // Z = λf.U(λx.f(λv.Uxv))
    public static Func<T, TResult> Zf<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) => 
        Uf<Func<T, TResult>>(x => f(v => Uf(x)(v)));

    // Z = λf.U(λx.f(λv.Uxv)) = S(λf.U)(λf.λx.f(λv.Uxv)) = S(KU)(λf.λx.f(λv.Uxv)) = S(KU)(λf.( S(λx.f)(λx.λv.Uxv) )) =
    // S(KU)(λf.S(Kf)(λx.λv.Uxv)) = S(KU)( S(λf.S(Kf))(λf.λx.λv.Uxv) ) = S(KU)(S( λf.S(Kf) )(K(λx.λv.Uxv))) =
    // S(KU)(S( S(λf.S)(λf.Kf) )(K(λx.λv.Uxv))) = S(KU)(S(S(KS)K)(K( λx.λv.Uxv ))) =
    // S(KU)(S(S(KS)K)(K( λx.S(λv.Ux)(λv.v) ))) = S(KU)(S(S(KS)K)(K( λx.S(λv.Ux)I ))) =
    // S(KU)(S(S(KS)K)(K(S( λx.S(λv.Ux) ) (λx.I) ))) = S(KU)(S(S(KS)K)(K(S( λx.S(λv.Ux) )(KI)))) =
    // S(KU)(S(S(KS)K)(K(S(S(KS)( λx.λv.Ux ))(KI)))) = S(KU)(S(S(KS)K)(K(S(S(KS)( λx.S(λv.U)(λv.x) ))(KI)))) =
    // S(KU)(S(S(KS)K)(K(S(S(KS)( λx.S(KU)(Kx) ))(KI)))) = S(KU)(S(S(KS)K)(K(S(S(KS)( S( λx.S(KU) )( λx.Kx ) ))(KI))))
    // = S(KU)(S(S(KS)K)(K(S(S(KS)(S(K(S(KU)))K))(KI))))
    public static Func<dynamic, dynamic> Z => S(K(U))(S(S(K(S))(K))(K(S(S(K(S))(S(K(S(K(U))))(K)))(K(I)))));

    #endregion

    #region Boolean

    // Boolean := λt.λf. t|f
    [DebuggerDisplay("{LambdaCalculus.SKI.UnSki(this)}")]
    public delegate Func<dynamic, dynamic> Boolean(dynamic @true);

    // T := K
    public static readonly Func<dynamic, Func<dynamic, dynamic>> T = K;
    public static readonly Boolean TB = @true => T(@true);

    // F := SK
    public static readonly Func<dynamic, Func<dynamic, dynamic>> F = S(K);
    public static readonly Boolean FB = @true => F(@true);

    // Not := S(SI(KF))(KT) := λb. λb (SK) K
    public static readonly Func<dynamic, dynamic> Not = S(S(I)(K(F)))(K(T));
    public static readonly Func<Boolean, Boolean> NotB = a => S(S(I)(K(FB)))(K(TB))(a);

    // Or := SI(KT) := λx.λy. xTy
    public static readonly Func<dynamic, dynamic> Or = S(I)(K(T));
    public static readonly Func<Boolean, Func<Boolean, Boolean>> OrB = a => b => S(I)(K(TB))(a)(b);

    // And := SS(K(KF)) := λx.λy. xyF
    public static readonly Func<dynamic, dynamic> And = S(S)(K(K(F)));
    public static readonly Func<Boolean, Func<Boolean, Boolean>> AndB = a => b => S(S)(K(K(FB)))(a)(b);

    #endregion

    #region Convertions

    public static Boolean ToSki(this bool b) => b ? TB : FB;

    public static bool UnSki(this Boolean b) => b(K(true))(K(false))(K);

    public static bool UnSkiBoolean(dynamic b) => b(K(true))(K(false))(K);

    #endregion
}