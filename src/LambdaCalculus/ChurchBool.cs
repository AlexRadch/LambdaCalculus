using System.Diagnostics;
using static LambdaCalculus.ChurchUnit;

namespace LambdaCalculus
{
    // Bool := λt.λf. t|f
    // Bool := True -> False -> True|False
    public delegate dynamic Bool(dynamic @true, dynamic @false);
    public delegate Bool HiddenBool();

    public delegate dynamic HiddenValue();

    public static partial class ChurchBool
    {
        #region True False

        // True := λt.λf. t
        // True(True, False) -> True
        public static dynamic True(dynamic @true, dynamic @false) => @true;
        public static readonly Bool TrueV = True;
        public static readonly HiddenBool HiddenTrueV = () => True;

        // False := λt.λf. f
        // False(True, False) -> False
        public static dynamic False(dynamic @true, dynamic @false) => @false;
        public static readonly Bool FalseV = False;
        public static readonly HiddenBool HiddenFalseV = () => False;

        #endregion

        #region If

        // If := λp.λt.λe. p t e := λp. p
        // If(Bool, Then, Else) -> Bool(Then, Else)
        // If(Bool) -> Bool
        public static dynamic If(this Bool predicate, dynamic @then, dynamic @else) => predicate(@then, @else);
        public static Bool If(this Bool predicate) => predicate;
        public static dynamic LazyIf(this Bool predicate, HiddenValue @then, HiddenValue @else) => predicate(@then, @else)();
        public static dynamic LazyIf(this Bool predicate, dynamic @then, HiddenValue @else) => predicate(HideValue(@then), @else)();
        public static dynamic LazyIf(this Bool predicate, HiddenValue @then, dynamic @else) => predicate(@then, HideValue(@else))();

        #endregion

        #region Not

        // Not := λb. b False True
        // Not(Bool) -> Bool
        public static Bool Not(this Bool @bool) => @bool(FalseV, TrueV);
        public static HiddenBool HiddenNot(this HiddenBool @bool) => () => @bool()(FalseV, TrueV);

        #endregion

        #region Or

        // Or := λa.λb. a a b := λa.λb. a True b
        // Or(Bool, Bool) -> Bool
        public static Bool Or(this Bool a, Bool b) => a(TrueV, b);
        public static HiddenBool HiddenOr(this Bool a, HiddenBool hb) => a(HiddenTrueV, hb);
        public static HiddenBool HiddenOr(this HiddenBool ha, HiddenBool hb) => () => ha()(HiddenTrueV, hb)();
        public static Bool LazyOr(this Bool a, HiddenBool hb) => a(HiddenTrueV, hb)();
        public static Bool LazyOr(this HiddenBool ha, HiddenBool hb) => ha()(HiddenTrueV, hb)();

        // Or := λa.λb. b b a := λa.λb. b True a
        // Or(Bool, Bool) -> Bool
        public static HiddenBool HiddenOr(this HiddenBool ha, Bool b) => b(HiddenTrueV, ha);
        public static Bool LazyOr(this HiddenBool ha, Bool b) => b(HiddenTrueV, ha)();

        #endregion

        #region And

        // And := λa.λb. a b a := λa.λb. a b False
        // And(Bool, Bool) -> Bool
        public static Bool And(this Bool a, Bool b) => a(b, FalseV);
        public static HiddenBool HiddenAnd(this Bool a, HiddenBool hb) => a(hb, HiddenFalseV);
        public static HiddenBool HiddenAnd(this HiddenBool ha, HiddenBool hb) => () => ha()(hb, HiddenFalseV)();
        public static Bool LazyAnd(this Bool a, HiddenBool hb) => a(hb, HiddenFalseV)();
        public static Bool LazyAnd(this HiddenBool ha, HiddenBool hb) => ha()(hb, HiddenFalseV)();

        // And := λa.λb. b a b := λa.λb. b a False
        // And(Bool, Bool) -> Bool
        public static HiddenBool HiddenAnd(this HiddenBool ha, Bool b) => b(ha, HiddenFalseV);
        public static Bool LazyAnd(this HiddenBool ha, Bool b) => b(ha, HiddenFalseV)();

        #endregion

        #region Xor

        // Xor := λa.λb. a (Not b) b
        // Xor(Bool, Bool) -> Bool
        public static Bool Xor(this Bool a, Bool b) => a(Not(b), b);
        public static HiddenBool HiddenXor(this Bool a, HiddenBool hb) => a(HiddenNot(hb), hb);
        public static HiddenBool HiddenXor(this HiddenBool ha, HiddenBool hb) => () => ha()(HiddenNot(hb), hb)();

        // Xor := λa.λb. b (Not a) a
        // Xor(Bool, Bool) -> Bool
        public static HiddenBool HiddenXor(this HiddenBool ha, Bool b) => b(HiddenNot(ha), ha);

        #endregion

        #region Nor

        // Nor := λa.λb. a False (Not b)
        // Nor(Bool, Bool) -> Bool
        public static Bool Nor(this Bool a, Bool b) => a(FalseV, Not(b));

        // TODO: HiddenNor LazyNor

        #endregion

        #region Nand

        // Nand := λa.λb. a (Not b) True
        // Nand(Bool, Bool) -> Bool
        public static Bool Nand(this Bool a, Bool b) => a(Not(b), TrueV);

        // TODO: HiddenNand LazyNand

        #endregion

        #region Eq

        // Eq := λa.λb. a b Not(b)
        // Eq(Bool, Bool) -> Bool
        public static Bool Eq(this Bool a, Bool b) => a(b, Not(b));
        public static HiddenBool HiddenEq(this Bool a, HiddenBool hb) => a(hb, HiddenNot(hb));
        public static HiddenBool HiddenEq(this HiddenBool ha, HiddenBool hb) => () => ha()(hb, HiddenNot(hb))();

        // Eq := λa.λb. b a Not(a)
        // Eq(Bool, Bool) -> Bool
        public static HiddenBool HiddenEq(this HiddenBool ha, Bool b) => b(ha, HiddenNot(ha));

        #endregion

        #region Conversation

        // System bool to church Bool
        public static Bool Church(this bool value) => value ? TrueV : FalseV;

        // Church Bool to system bool
        // Unchurch := λp. If(p) true false := p true false
        public static bool Unchurch(this Bool p) => p(true, false); // predicate works them self
        //public static bool Unchurch(this Bool p) => p.If()(true, false); // If should work also
        //public static bool Unchurch(this Bool p) => p.If(true, false); // If should work also

        //public static dynamic ToLazy<T>(Func<T> f) => f;

        #endregion
    }
}
