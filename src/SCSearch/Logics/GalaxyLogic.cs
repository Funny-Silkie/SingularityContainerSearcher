using CsvHelper;
using CsvHelper.Configuration;
using CuiLib;
using SCSearch.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SCSearch.Logics
{
    /// <summary>
    /// Galaxy hub関連のロジックを表します。
    /// </summary>
    internal partial class GalaxyLogic
    {
        [StringSyntax(StringSyntaxAttribute.Uri)]
        private const string HubUrl = @"https://depot.galaxyproject.org/singularity/";

        private const char RowSeparator = '\t';

        private static readonly string metadataDirectory;
        private static readonly string htmlDestPath;
        private static readonly string metadataPath;

        static GalaxyLogic()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            metadataDirectory = Path.Combine(userProfilePath, @".scsearch");
            htmlDestPath = Path.Combine(metadataDirectory, "galaxy.html");
            metadataPath = Path.Combine(metadataDirectory, "metadata.tsv");

            if (!Directory.Exists(metadataDirectory)) Directory.CreateDirectory(metadataDirectory);
        }

        /// <summary>
        /// Gaalxy COMMUNITY hubのHTMLファイルをダウンロードします。
        /// </summary>
        public async Task DownloadHtmlAsync()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, HubUrl);
            using HttpResponseMessage response = await client.SendAsync(request);

            using var htmlStream = new FileStream(htmlDestPath, FileMode.Create);
            await response.Content.CopyToAsync(htmlStream);
        }

        /// <summary>
        /// メタデータを更新します。
        /// </summary>
        public async Task UpdateMetadataAsync()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync("メタデータの更新を開始します");

                await DownloadHtmlAsync();

                using var reader = new StreamReader(htmlDestPath);
                using var writer = new StreamWriter(metadataPath);

                await writer.WriteLineAsync(string.Join(RowSeparator, ["URL", "name", "uploaded", "size"]));
                await foreach (string line in reader.IterateLinesAsync())
                {
                    Match currentMatch = GetContainerMetadataRegex().Match(line);
                    if (!currentMatch.Success) continue;

                    GroupCollection groups = currentMatch.Groups;
                    if (groups.Count != 5) continue;

                    await writer.WriteLineAsync($"{HubUrl + groups[1].Value}\t{string.Join(RowSeparator, groups.Values.Skip(2).Select(x => x.Value))}");
                }

                await Console.Out.WriteLineAsync("メタデータの更新が完了しました");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// コンテナのデータを取得するための正規表現を取得します。
        /// </summary>
        [GeneratedRegex(@"<a href=""(.+?)"">(.+?)</a>\s*?(\d{2}-\w{3}-\d{4} \d{2}:\d{2})\s*?(\d+)")]
        private static partial Regex GetContainerMetadataRegex();

        /// <summary>
        /// コンテナ情報を取得します。
        /// </summary>
        /// <param name="forceUpdate">メタデータを強制的に更新するかどうか</param>
        /// <returns>コンテナ情報の一覧</returns>
        public async IAsyncEnumerable<ContainerMetadata> LoadMetadataAsync(bool forceUpdate)
        {
            var metadataFileInfo = new FileInfo(metadataPath);

            // 前回の更新から1時間以上経過で更新
            if (forceUpdate || !metadataFileInfo.Exists || (DateTime.Now - metadataFileInfo.LastWriteTime) >= TimeSpan.FromHours(1)) await UpdateMetadataAsync();

            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = RowSeparator.ToString(),
                HasHeaderRecord = true,
                HeaderValidated = null,
            };

            using var reader = new CsvReader(metadataFileInfo.OpenText(), config, false);

            await foreach (ContainerMetadata current in reader.GetRecordsAsync<ContainerMetadata>())
            {
                yield return current;
            }
        }
    }
}
