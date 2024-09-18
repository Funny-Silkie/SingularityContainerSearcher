using CsvHelper.Configuration.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace SCSearch.Data
{
    public class ContainerMetadata
    {
        /// <summary>
        /// コンテナ名を取得または設定します。
        /// </summary>
        [Name("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// ダウンロード用URLを取得または設定します。
        /// </summary>
        [Name("URL")]
        [StringSyntax(StringSyntaxAttribute.Uri)]
        public string DownloadUrl { get; set; } = default!;

        /// <summary>
        /// アップロード日時を取得または設定します。
        /// </summary>
        [Name("uploaded")]
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// ファイルサイズを取得または設定します。
        /// </summary>
        [Name("size")]
        public ulong Size { get; set; }
    }
}
