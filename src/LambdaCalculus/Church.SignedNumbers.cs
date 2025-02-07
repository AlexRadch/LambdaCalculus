using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic Signed(Func<Numeral, Func<Numeral, dynamic>> b);

    #endregion

    #region Convertions

    // CreateSigned := λp.λn CreatePair (p - n) (n - p)
    public static Func<Numeral, Signed> CreateSigned(Numeral p) => n => b => CreatePair<Numeral, Numeral>(Minus(p)(n))(Minus(n)(p))(b);

    // NumeralToSigned := λx. CreatePair x 0
    public static Signed NumeralToSigned(Numeral x) => b => CreatePair<Numeral, Numeral>(x)(Zero)(b);

    // NumeralToNegSigned := λx. CreatePair 0 x
    public static Signed NumeralToNegSigned(Numeral x) => b => CreatePair<Numeral, Numeral>(Zero)(x)(b);

    // SignedToAbsNumeral := λx. x (λpx.λnx. Diff(px)(nx))
    public static Numeral SignedToAbsNumeral(Signed x) => x(px => nx => Diff(px)(nx));

    // SignedToNumeralTrunc := λx. x (λpx.λnx. px - nx)
    public static Numeral SignedToTruncNumeral(Signed x) => x(px => nx => Minus(px)(nx));

    // OneZero : ⁡= λx. x (λp.λn. CreatePair (p - n) (n - p))
    public static Signed OneZero(Signed x) => x(CreateSigned);

    #endregion

    #region Arithmetic

    // NegS = λx. pair⁡ (second⁡ x) (first⁡ x)
    public static Signed NegS(Signed x) => b => x(px => nx => CreatePair<Numeral, Numeral>(nx)(px)(b));

    // PlusS = λx.λy. OneZero⁡(pair⁡ (plus⁡ (first⁡ x) (first⁡ y)) (plus⁡ (second⁡ x) (second⁡ y)))
    public static Func<Signed, Signed> PlusS(Signed x) => y => x(px => nx => y(py => ny => 
        CreateSigned(Plus(px)(py))(Plus(nx)(ny))));

    // MinusS = λx.λy.OneZero⁡ (pair⁡ (plus⁡ (first⁡ x) (second⁡ y)) (plus⁡ (second⁡ x) (first⁡ y)))
    public static Func<Signed, Signed> MinusS(Signed x) => y => x(px => nx => y(py => ny => 
        CreateSigned(Plus(px)(ny))(Plus(nx)(py))));

    // MultS = λx.λy.pair⁡ (plus⁡ (mult⁡ (first⁡ x) (first⁡ y)) (mult⁡ (second⁡ x) (second⁡ y)))
    // (plus⁡ (mult⁡ (first⁡ x) (second⁡ y)) (mult⁡ (second⁡ x) (first⁡ y)))
    public static Func<Signed, Signed> MultS(Signed x) => y => x(px => nx => y(py => ny => 
        CreateSigned(Plus(Mult(px)(py))(Mult(nx)(ny)))(Plus(Mult(px)(ny))(Mult(nx)(py)))));

    // DivZ = λx.λy.IsZero⁡ y 0 (Divide⁡ x y)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> DivZ(Numeral x) => y => LazyIf(IsZero⁡(y))(() => Zero)(() => Divide⁡(x)(y));

    // DivideS = λx.λy.pair⁡(plus⁡ (DivZ⁡ (first⁡ x) (first⁡ y)) (DivZ⁡ (second⁡ x) (second⁡ y)))
    // (plus⁡ (DivZ⁡ (first⁡ x) (second⁡ y)) (DivZ⁡ (second⁡ x) (first⁡ y)))
    public static Func<Signed, Signed> DivideS(Signed x) => y => x(px => nx => y(py => ny =>
        CreateSigned(Plus(DivZ(px)(py))(DivZ(nx)(ny)))(Plus(DivZ(px)(ny))(DivZ(nx)(py)))));

    // ModZ = λx.λy.IsZero⁡ y 0 (x % y)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> ModZ(Numeral x) => y => LazyIf(IsZero⁡(y))(() => Zero)(() => Modulo(x)(y));

    // ModuloS = λx.λy.pair⁡(plus⁡ (ModZ⁡ (first⁡ x) (first⁡ y)) (ModZ⁡ (first⁡ x) (second⁡ y)))
    // (plus⁡ (ModZ⁡ (second⁡ x) (second⁡ y)) (ModZ⁡ (second⁡ x) (first⁡ y)))
    public static Func<Signed, Signed> ModuloS(Signed x) => y => x(px => nx => y(py => ny =>
        CreateSigned(Plus(ModZ(px)(py))(ModZ(px)(ny)))(Plus(ModZ(nx)(ny))(ModZ(nx)(py)))));

    #endregion

    #region Extensions

    // System int to Church Signed
    public static Signed ToChurch(this int x) =>
        x < 0 ? NumeralToNegSigned(ToChurch((uint)-x)) : NumeralToSigned(ToChurch((uint) x));

    // Church Signed to int
    public static int UnChurch(this Signed x) => x(px => nx => (int)px.UnChurch() - (int)nx.UnChurch());

    #endregion
}
