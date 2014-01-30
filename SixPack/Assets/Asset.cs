
namespace SixPack.Assets
{
    public class Asset : IAsset
    {
        public string Url { get; set; }
        public string Content { get; set; }
        public string MinifiedContent { get; set; }
        public string ErrorContent { get; set; }
        public AssetStatus Status { get; set; }
        public int SortOrder { get; set; }
    }
}
