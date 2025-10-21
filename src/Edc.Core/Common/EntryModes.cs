namespace Edc.Core.Common;

public static class EntryModes
{
    public const string CHIP_INSERT = "05";
    public const string CONTACTLESS_CHIP = "07";
    public const string CONTACTLESS_MAGSTRIPE = "91";
    public const string MAGSTRIPE = "02";
    public const string FALLBACK_MAGSTRIPE = "80";
    public const string MANUAL_ENTRY = "01";
    public const string QR_CODE = "03";
}