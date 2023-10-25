namespace CoreLibrary.Models.EFModels_Partial.Converter;

public static class EnumHelper
{
    public static IEnumerable<string> GetListStringFromEnum<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => e.ToString());
    }
}