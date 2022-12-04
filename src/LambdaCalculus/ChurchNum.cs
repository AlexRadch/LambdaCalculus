namespace LambdaCalculus
{
    // Num := λf.λz. n times f from x // 0 f x := x, 1 f x := f x, 2 f x := f(f x), n f x := f(f..(f x)..)
    // Num := Succ(Prev -> Next) -> Start -> Result : Succ^n from Start
    public delegate Func<dynamic, dynamic> Num(Func<dynamic, dynamic> Succ);

    public static partial class ChurchNum
    {
        #region Zero One Two

        // Zerro := λf.λz. z // the same as False := λt.λf. f
        // Zerro := (Succ -> Zerro) -> Zerro -> Zerro
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => zero => zero;
        //public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => ChurchBool.False; // should work also!!!
        //public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => ChurchBool.FalseV(succ); // should work also!!!

        public static Num ZeroV => Zero;

        //// One(f, z) := λf.λz. f z
        //// One := (Succ -> Zerro) -> Zerro -> Succ(Zerro)
        //public static Func<dynamic, dynamic> One(Func<dynamic, dynamic> succ) => zerro => succ(zerro);

        //public static Num OneV => One;

        //// Two := λf.λz. f (f z)
        //// Two := (Succ -> Zerro) -> Zerro -> Succ(Succ(Zerro)) := 
        //public static Func<dynamic, dynamic> Two(Func<dynamic, dynamic> succ) => zero => succ(succ(zero));

        //public static Num TwoV => Two;

        #endregion

        #region Add Succ Mult

        // Add := λm.λn.λf.λz. m f (n f z)
        public static Func<Num, Num> Add(this Num m) => n => f => z => m(f)(n(f)(z));
        public static Num Add(this Num m, Num n) => f => z => m(f)(n(f)(z));

        // Succ := λ1.λn.λf.λz. 1 f (n f z) := f (n f z)
        public static Num Succ(this Num n) => f => z => f(n(f)(z));

        // Mult = m ∗ n \operatorname{mult}(m, n) = m*n

        #endregion
    }
}
