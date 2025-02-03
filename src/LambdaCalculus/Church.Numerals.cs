using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

// Next := λn. next n
//using Next = Func<dynamic, dynamic>;

public static partial class Church
{
    #region Delegates

    // NextNumeral := λn. next n
    public delegate dynamic NextNumeral(dynamic num);

    // Numeral := λf.λz. n times f from z // 0 f z := z, 1 f z := f z, 2 f z := f (f z), n f z := f (f .. (f z)..)
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate NextNumeral Numeral(NextNumeral next);

    #endregion

    #region Constants

    // Zero := λf.λz. z := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static NextNumeral ZeroL(NextNumeral f) => z => z;
    public static readonly Numeral Zero = ZeroL;

    // Zero := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    //public static Next ZeroL_False(Next f) => FalseL<Next, dynamic>(f);
    public static NextNumeral ZeroL_False(NextNumeral f) => z => FalseL<NextNumeral, dynamic>(f)(z);
    [EditorBrowsable(EditorBrowsableState.Never)]
    //public static readonly Num Zero_False = (Num)False;
    public static readonly Numeral Zero_False = ZeroL_False;

    // One := λf.λz. f z := λf f := Id
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static NextNumeral OneL(NextNumeral f) => f;
    public static readonly Numeral One = OneL;

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

    // Pred := λn.λf.λz. n (λg.λh. h (g f)) (λu. z) (λu. u)
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

    #endregion

    #region Logical

    // IsZero := λn. n (λx. False) True
    public static Boolean IsZero(Numeral n) => n(x => False)(True);

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

    #endregion

    #region Conversations

    // System uint to Church Num
    // AsChurch := v => b ? True : False
    public static Numeral AsChurch(this uint n)
    {
        if (n > 3)
        {
            var result = AsChurch(n / 2);
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

    #endregion
}
