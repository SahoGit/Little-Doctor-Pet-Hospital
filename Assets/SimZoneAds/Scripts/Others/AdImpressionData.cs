public struct AdImpressionData
{
    public string AdUnit;
    public AnalyticsManager.AdFormatX Format;

    public AdImpressionData(string adUnit, AnalyticsManager.AdFormatX format)
    {
        AdUnit = adUnit;
        Format = format;
    }
}