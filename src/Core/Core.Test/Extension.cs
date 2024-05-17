namespace Core.Test;


internal abstract class TestBase
{
        public static void Test<TArrange, TAct>(
        Func<TArrange> arrange,
        Func<TArrange, TAct> act,
        Action<TAct> assert)
    {
        assert(act(arrange()));
    }

    public static void Test<TArrange, TAct>(
        Func<TArrange> arrange,
        Func<TArrange, TAct> act,
        Action<TArrange, TAct> assert)
    {
        var arrangeResult = arrange();
        assert(arrangeResult, act(arrangeResult));
    }
}
