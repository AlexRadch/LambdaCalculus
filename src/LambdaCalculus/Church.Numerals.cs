using System.ComponentModel;
using System.Diagnostics;
using static LambdaCalculus.Church;

namespace LambdaCalculus;

// Next := λn. next n
//using NextNumeral = Func<dynamic, dynamic>;

public static partial class Church
{
    #region Delegates

    // NextNumeral := λn. next n
    public delegate dynamic NextNumeral(dynamic Numeral);

    // Numeral := λf.λz. n times f from z // 0 f z := z, 1 f z := f z, 2 f z := f (f z), n f z := f (f .. (f z)..)
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate NextNumeral Numeral(NextNumeral next);

    #endregion

    #region Constants

    // Zero := λf.λz. z := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static NextNumeral ZeroF(NextNumeral f) => z => z;
    public static readonly Numeral Zero = ZeroF;
    public static readonly Func<Numeral> LazyZero = () => ZeroF;

    // Zero := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    //public static NextNumeral ZeroF_False(NextNumeral f) => FalseF<NextNumeral, dynamic>(f);
    public static NextNumeral ZeroF_False(NextNumeral f) => z => FalseF<NextNumeral, dynamic>(f)(z);
    [EditorBrowsable(EditorBrowsableState.Never)]
    //public static readonly Numeral Zero_False = False;
    //public static readonly Numeral Zero_False = f => z => False(f)(z);
    public static readonly Numeral Zero_False = f => z => FalseF<dynamic, dynamic>(f)(z);
    //public static readonly Numeral Zero_False = ZeroF_False;

    // One := λf.λz. f z := λf f := Id
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static NextNumeral OneF(NextNumeral f) => f;
    public static readonly Numeral One = OneF;
    public static readonly Func<Numeral> LazyOne = () => OneF;

    #endregion

    #region Arithmetic

    // Succ := λn.λf.λz. f (n f z)
    public static Numeral Succ(Numeral n) => f => z => f(n(f)(z));

    // Succ := λn.λf.λz. n f (f z)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Numeral SuccR(Numeral n) => f => z => n(f)(f(z));

    // Plus := λm.λn.λf.λz. m f (n f z)
    public static Func<Numeral, Numeral> Plus(Numeral m) => n => f => z => m(f)(n(f)(z));

    // Plus := λm.λn. m Succ n
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Plus_Succ(Numeral m) => n => m(x => Succ(x))(n);

    // Mult := λm.λn.λf.λz. m (n f) z := λm.λn.λf. m (n f)
    public static Func<Numeral, Numeral> Mult(Numeral m) => n => f => m(n(f));

    // Mult := λm.λn. m (λx. Plus x n) Zero
    [EditorBrowsable(EditorBrowsableState.Never)]
    //public static Func<Num, Num> Mult_Plus(Num m) => n => m(x => Plus(x)(n))(ZeroV);
    public static Func<Numeral, Numeral> Mult_Plus(Numeral m) => n => m(x => { Debug.Print($"{UnChurch(x)} + {UnChurch(n)}"); return Plus(x)(n); })(Zero);

    // Exp := λm.λn n m
    //public static Func<Num, Num> Exp_num(Num m) => n => n(m);
    public static Func<Numeral, Numeral> Exp(Numeral m) => n => fn => n(fm => m(fm))(fn);
    // Exp := λm.λn n (λx. Mult x m) One
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Exp_Mult(Numeral m) => n => n(x => Mult(x)(m))(One);

    // Pred := λn.λf.λz. n (λg.λh. h (g f)) (λ_. z) (λu. u)
    //public static Num Pred(Num n) => f => z => n(g => h => h(g(f)))(_ => z)(9641u => u);
    public static Numeral Pred(Numeral n) => f => z => n(g => (NextNumeral)(h => h(g(f))))((NextNumeral)(_ => z))((NextNumeral)(u => u));

    // Minus := λm.λn. n Pred m
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Minus(Numeral m) => n => n(x => Pred(x))(m);

    // Diff = λm.λn. Plus (Minus m n) (Minus n m)
    public static Func<Numeral, Numeral> Diff(Numeral m) => n => Plus(Minus(m)(n))(Minus(n)(m));

    // Min := λm.λn. LE m n m n
    public static Func<Numeral, Numeral> Min(Numeral m) => n => LEQ(m)(n)(m)(n);

    // Max := λm.λn. GE m n m n
    public static Func<Numeral, Numeral> Max(Numeral m) => n => GEQ(m)(n)(m)(n);

    #region Divide

    // DivideR_mn ⁡:= λm.λn. if⁡ m >= n then⁡ 1 + (m − n) / n else⁡ 0
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> DivideR_mn(Numeral m) => n =>
        LazyIf(GEQ(m)(n))
            (() => Succ(DivideR_mn(Minus(m)(n))(n)))
            (LazyZero);

    // Divide1R ⁡:= λm.λn. if⁡ n <= m then⁡ 0 else⁡ 1 + (m − n) / n 
    // Divide1R ⁡:= λm.λn. if⁡ IsZero(m - n) then⁡ 0 else⁡ 1 + (m − n) / n 
    // Divide1R ⁡:= λm.λn. (λd. if⁡ (IsZero d) then⁡ 0 else⁡ 1 + d / n) (m - n)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Divide1R(Numeral m) => n => new Func<Numeral, Numeral>(d => 
        LazyIf(IsZero(d))
            (LazyZero)
            (() => Succ(Divide1R(d)(n)))
        )
        (Minus(m)(n));

    // DivideR ⁡:= λm. Divide1R (m + 1)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> DivideR(Numeral m) => Divide1R(Succ(m));

    // Div1 :⁡= λf.λm.λn (λd.IsZero⁡ d 0 (1 + (f d n))) (Minus⁡ m n)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Func<Numeral, Numeral>> Div1(Func<Numeral, Func<Numeral, Numeral>> f) => m => n =>
        new Func<Numeral, Numeral>(d =>
        LazyIf(IsZero(d))
            (LazyZero)
            (() => Succ(f(d)(n)))
        )
        (Minus(m)(n));

    // Divide1 ⁡:= Z Div1
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Func<Numeral, Numeral>> Divide1 = Combinators.Z<Numeral, Func<Numeral, Numeral>>(Div1);

    // Divide ⁡:= λm. Divide1 (m + 1)
    public static Func<Numeral, Numeral> Divide(Numeral m) => Divide1(Succ(m));

    //public static Func<Numeral, Numeral> Modulo(Numeral m) => Divide1(Succ(m));

    #endregion

    #region Modulo

    // ModuloR_mn ⁡:= λm.λn. if⁡ m >= n then⁡ (m − n) % n else⁡ m
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> ModuloR_mn(Numeral m) => n =>
        LazyIf(GEQ(m)(n))
            (() => ModuloR_mn(Minus(m)(n))(n))
            (() => m);

    // Mod1 :⁡= λf.λm.λn (λd.IsZero⁡ d (m - 1) (f d n)) (Minus⁡ m n)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Func<Numeral, Numeral>> Mod1(Func<Numeral, Func<Numeral, Numeral>> f) => m => n =>
        new Func<Numeral, Numeral>(d =>
        LazyIf(IsZero(d))
            (() => Pred(m))
            (() => f(d)(n))
        )
        (Minus(m)(n));

    // Modulo ⁡:= λm. Z Div1 (m + 1)
    public static Func<Numeral, Numeral> Modulo(Numeral m) => Combinators.Z<Numeral, Func<Numeral, Numeral>>(Mod1)(Succ(m));

    #endregion

    #endregion

    #region Functions

    // FactorialR := λx. If (x == 0) (1) (x * (FactorialR (x - 1)))
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Numeral FactorialR(Numeral x) => LazyIf(IsZero(x))(LazyOne)(() => Mult(x)(FactorialR(Pred(x))));

    // Fact := λf. λx. If (x == 0) (1) (x * (f (x - 1)))
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Fact(Func<Numeral, Numeral> f) => x => 
        LazyIf(IsZero(x))(LazyOne)(() => Mult(x)(f(Pred(x))));

    // Factorial ⁡:= Z Fact
    public static readonly Func<Numeral, Numeral> Factorial = Combinators.Z<Numeral, Numeral>(Fact);

    // FibonacciR := λx. If (x <= 1) (1) (FibonacciR(x - 1) + FibonacciR(x - 2))
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Numeral FibonacciR(Numeral x) =>
        LazyIf(LEQ(x)(One))(() => x)(() => Plus(FibonacciR(Pred(x)))(FibonacciR(Pred(Pred(x)))));

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Numeral, Numeral> Fib(Func<Numeral, Numeral> f) => x =>
        LazyIf(LEQ(x)(One))(() => x)(() => Plus(f(Pred(x)))(f(Pred(Pred(x)))));

    // Factorial ⁡:= Z Fact
    public static readonly Func<Numeral, Numeral> Fibonacci = Combinators.Z<Numeral, Numeral>(Fib);

    #endregion

    #region Logical

    // IsZero := λn. n (λx. False) True
    public static Boolean IsZero(Numeral n) => n(f => False)(True);

    // GEQ := λm.λn. IsZero (Minus n m)
    public static Func<dynamic, Boolean> GEQ(Numeral m) => n => IsZero(Minus(n)(m));

    // LEQ := λm.λn. IsZero (Minus m n)
    public static Func<dynamic, Boolean> LEQ(Numeral m) => n => IsZero(Minus(m)(n));

    // GT := λm.λn. Not (LEQ m n)
    public static Func<dynamic, Boolean> GT(Numeral m) => n => Not(LEQ(m)(n));

    // LT := λm.λn. Not (GEQ m n)
    public static Func<dynamic, Boolean> LT(Numeral m) => n => Not(GEQ(m)(n));

    // EQ := λm.λn. And (LEQ m n) (LEQ n m)
    public static Func<dynamic, Boolean> EQ(Numeral m) => n => And(LEQ(m)(n))(LEQ(n)(m));

    // NEQ := λm.λn. Not (EQ m n)
    public static Func<dynamic, Boolean> NEQ(Numeral m) => n => Not(EQ(m)(n));

    // IsEven := λn. n Not True
    public static Boolean IsEven(Numeral n) => n(f => Not(f))(True);

    // IsOdd := λn. n Not False
    public static Boolean IsOdd(Numeral n) => n(f => Not(f))(False);

    #endregion

    #region Extensions

    // System uint to Church Num
    // ToChurch := v => b ? True : False
    public static Numeral ToChurch(this uint n)
    {
        if (n > 3)
        {
            var result = ToChurch(n / 2);
            result = Plus(result)(result);
            if (n % 2 == 1)
                result = Succ(result);
            return result;
        }
        {
            var result = Zero;
            for (int i = 0; i < n; i++)
                result = Succ(result);
            return result;
        }
    }

    public static uint UnChurch(this Numeral n) => n(x => x + 1)(0u);

    public static uint UnChurch(this Func<Numeral> n) => n()(x => x + 1)(0u);

    #endregion
}
