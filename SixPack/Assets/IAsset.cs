
namespace SixPack.Assets
{
    /// <summary>
    /// Defines the interface of asset
    /// </summary>
    /// From: http://bundletransformer.codeplex.com/SourceControl/latest#BundleTransformer.Core/Assets/IAsset.cs
    public interface IAsset
    {
        /// <summary>
        /// The Url of the asset that will be bundled and minified
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The contents of the file, at the location of the Url
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// The minified content
        /// </summary>
        string MinifiedContent { get; set; }

        /// <summary>
        /// If any errors are experienced during processing, the content is added here
        /// </summary>
        string ErrorContent { get; set; }
        
        /// <summary>
        /// The state/status of the asset
        /// </summary>
        AssetStatus Status { get; set; }
        
        /// <summary>
        /// The order in which this asset appeared in the Request
        /// </summary>
        int SortOrder { get; set; }
    }
}
