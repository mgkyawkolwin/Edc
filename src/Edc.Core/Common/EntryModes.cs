namespace Edc.Core.Common;

/// <summary>
/// Defines constants representing different card entry modes used in EDC transactions.
/// These values are typically included in ISO-8583 messages to indicate how the card was presented.
/// </summary>
public static class EntryModes
{
    /// <summary>Card inserted into the chip reader.</summary>
    public const string CHIP_INSERT = "05";

    /// <summary>Contactless chip card transaction (NFC).</summary>
    public const string CONTACTLESS_CHIP = "07";

    /// <summary>Contactless transaction using magnetic stripe data.</summary>
    public const string CONTACTLESS_MAGSTRIPE = "91";

    /// <summary>Magnetic stripe card swiped through the reader.</summary>
    public const string MAGSTRIPE = "02";

    /// <summary>Fallback magnetic stripe used when chip fails.</summary>
    public const string FALLBACK_MAGSTRIPE = "80";

    /// <summary>Manual entry of card number (keyboard input).</summary>
    public const string MANUAL_ENTRY = "01";

    /// <summary>Payment via QR code scan.</summary>
    public const string QR_CODE = "03";
}