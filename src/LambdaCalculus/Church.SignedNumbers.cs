using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Signed := CPair<Numeral, Numeral> := λf. (λp.λn. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic Signed(Boolean<Numeral, Numeral> b);

    #endregion

    #region Convertions

    // CreateSigned := λp.λn CreatePair (p - n) (n - p)
    public static Func<Numeral, Signed> CreateSigned(Numeral p) => n => b => Pair<Numeral, Numeral>(Minus(p)(n))(Minus(n)(p))(b);

    // NumeralToSigned := λx. CreatePair x 0
    public static Signed NumeralToSigned(Numeral x) => b => Pair<Numeral, Numeral>(x)(Zero)(b);

    // NumeralToNegSigned := λx. CreatePair 0 x
    public static Signed NumeralToNegSigned(Numeral x) => b => Pair<Numeral, Numeral>(Zero)(x)(b);

    // SignedToAbsNumeral := λx. x (λpx.λnx. Diff(px)(nx))
    public static Numeral SignedToAbsNumeral(Signed x) => x(px => nx => Diff(px)(nx));

    // SignedToNumeralTrunc := λx. x (λpx.λnx. px - nx)
    public static Numeral SignedToTruncNumeral(Signed x) => x(px => nx => Minus(px)(nx));

    // OneZero : ⁡= λx. x (λp.λn. CreatePair (p - n) (n - p))
    public static Signed OneZero(Signed x) => x(CreateSigned);

    #endregion

    #region Arithmetic

    // NegS = λx. pair⁡ (second⁡ x) (first⁡ x)
    public static Signed NegS(Signed x) => b => x(px => nx => Pair<Numeral, Numeral>(nx)(px)(b));

    // AbsS = λx. x (λpx.λnx. CreatePair (Diff(px)(nx)) (Zero))
    public static Signed AbsS(Signed x) => b => x(px => nx => Pair<Numeral, Numeral>(Diff(px)(nx))(Zero)(b));

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

    #region ExpS

    //// ExpS := λx.λy IsNegS y
    ////     (y (λpy.λny. DivideS OneS ((ny - py) (λr. MultS r x) (OneS))))
    ////     (y (λpy.λny. (py - ny) (λr. MultS r x) (OneS)))
    //public static Func<Signed, Signed> ExpS(Signed x) => y => 
    //    LazyIf(IsNegS(y))
    //        (() => y(py => ny => DivideS(NumeralToSigned(One))(Minus(ny)(py)(r => MultS(r)(x))(NumeralToSigned(One)))))
    //        (() => y(py => ny => Minus(py)(ny)(r => MultS(r)(x))(NumeralToSigned(One))));

    // ExpS := λx.λy IsNegS y (λpy.λny.
    //     (λd IsZero d
    //         (DivideS OneS ((ny - py) (λr. MultS r x) (OneS)))
    //         (d (r => MultS(r)(x)) OneS)
    //     )
    //     (py - ny) )
    public static Func<Signed, Signed> ExpS(Signed x) => y => y(py => ny =>
        new Func<Numeral, Signed>(d => LazyIf(IsZero⁡(d))
            (() => DivideS(NumeralToSigned(One))(Minus(ny)(py)(r => MultS(r)(x))(NumeralToSigned(One))))
            (() => d(r => MultS(r)(x))(NumeralToSigned(One)))
        )
        (Minus(py)(ny)));

    #endregion

    #endregion

    #region Logical

    // IsZeroS := λx. x (λpx.λnx. EQ p n)
    public static Boolean IsZeroS(Signed x) => x(px => nx => EQ(px)(nx));

    // IsPosS := λx. x (λpx.λnx. GT p n)
    public static Boolean IsPosS(Signed x) => x(px => nx => GT(px)(nx));

    // IsNegS := λx. x (λpx.λnx. LT p n)
    public static Boolean IsNegS(Signed x) => x(px => nx => LT(px)(nx));

    // IsZPosS := λx. x (λpx.λnx. GEQ p n)
    public static Boolean IsZPosS(Signed x) => x(px => nx => GEQ(px)(nx));

    // IsZNegS := λx. x (λpx.λnx. GEQ p n)
    public static Boolean IsZNegS(Signed x) => x(px => nx => LEQ(px)(nx));

    // GEQS := λx.λy. IsZPosS (MinusS x y)
    public static Func<Signed, Boolean> GEQS(Signed x) => y => IsZPosS(MinusS(x)(y));

    // LEQS := λx.λy. IsZNegS (MinusS x y)
    public static Func<Signed, Boolean> LEQS(Signed x) => y => IsZNegS(MinusS(x)(y));

    // GTS := λx.λy. IsPosS (MinusS x y)
    public static Func<Signed, Boolean> GTS(Signed x) => y => IsPosS(MinusS(x)(y));

    // LTS := λx.λy. IsNegS (MinusS x y)
    public static Func<Signed, Boolean> LTS(Signed x) => y => IsNegS(MinusS(x)(y));

    // EQS := λx.λy. IsZeroS (MinusS x y)
    public static Func<Signed, Boolean> EQS(Signed x) => y => IsZeroS(MinusS(x)(y));

    // NEQS := λx.λy. Not (IsZeroS (MinusS x y))
    public static Func<Signed, Boolean> NEQS(Signed x) => y => Not(IsZeroS(MinusS(x)(y)));

    // IsEvenS := λx. x (λpx.λnx. Xor (IsEven p) (IsOdd n))
    public static Boolean IsEvenS(Signed x) => x(px => nx => Xor(IsEven(px))(IsOdd(nx)));

    // IsOddS := λx. x (λpx.λnx. Xor (IsEven p) (IsEven n))
    public static Boolean IsOddS(Signed x) => x(px => nx => Xor(IsEven(px))(IsEven(nx)));

    #endregion

    #region Extensions

    // System int to Church Signed
    public static Signed ToChurch(this int x) =>
        x < 0 ? NumeralToNegSigned(ToChurch((uint)-x)) : NumeralToSigned(ToChurch((uint) x));

    // Church Signed to int
    public static int UnChurch(this Signed x) => x(px => nx => (int)px.UnChurch() - (int)nx.UnChurch());

    #endregion
}
