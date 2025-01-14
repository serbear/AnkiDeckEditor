using System.Collections.Generic;
using System.Linq;

namespace AnkiDeckEditor.Libs;

public static class Common
{
    public static double GetGoldenGoldenRatioOffset(double height)
    {
        var offset = -(height / 2 - height / 1.618);
        return offset;
    }

    public static bool IsAllValuesAreTrue(IEnumerable<bool> values)
    {
        return values.All(e => e.Equals(true));
    }
}