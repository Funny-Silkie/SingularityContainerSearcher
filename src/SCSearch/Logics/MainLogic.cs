using SCSearch.Data;
using System.Text.RegularExpressions;

namespace SCSearch.Logics
{
    /// <summary>
    /// メインのロジックのクラスです。
    /// </summary>
    public partial class MainLogic
    {
        private readonly GalaxyLogic galaxyLogic = new();

        /// <summary>
        /// 検索を行います。
        /// </summary>
        /// <param name="parameter">検索パラメータ</param>
        /// <param name="forceUpdate">強制的にメタデータを更新するかどうかを表す値</param>
        public async Task SearchAsync(SearchParameter parameter, bool forceUpdate)
        {
            IAsyncEnumerable<ContainerMetadata> metadata = galaxyLogic.LoadMetadataAsync(forceUpdate);

            if (!string.IsNullOrEmpty(parameter.Query))
            {
                if (parameter.UseRegex)
                {
                    var options = RegexOptions.None;
                    if (!parameter.CaseSensitive) options |= RegexOptions.IgnoreCase;
                    var regex = new Regex(parameter.Query, options);

                    metadata = metadata.Where(x => regex.IsMatch(x.Name));
                }
                else
                {
                    var comparisonType = parameter.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                    metadata = metadata.Where(x => x.Name.Contains(parameter.Query, comparisonType));
                }
            }

            metadata = parameter.Descending
                ? parameter.Order switch
                {
                    ListOrder.Name => metadata.OrderByDescending(x => x.Name, parameter.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase),
                    ListOrder.Uploaded => metadata.OrderByDescending(x => x.UploadedAt),
                    ListOrder.Size => metadata.OrderByDescending(x => x.Size),
                    _ => metadata,
                }
                : parameter.Order switch
                {
                    ListOrder.Name => metadata.OrderBy(x => x.Name, parameter.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase),
                    ListOrder.Uploaded => metadata.OrderBy(x => x.UploadedAt),
                    ListOrder.Size => metadata.OrderBy(x => x.Size),
                    _ => metadata,
                };

            await ViewMetadata(metadata);
        }

        /// <summary>
        /// メタデータ一覧を表形式で表示します。
        /// </summary>
        /// <param name="metadata">表示するメタデータ一覧</param>
        private async Task ViewMetadata(IAsyncEnumerable<ContainerMetadata> metadata)
        {
            const string RowBlank = "  ";
            const int NameWidth = 30;
            const int UploadedWidth = 16;
            const int SizeWidth = 10;

            await Console.Out.WriteLineAsync();

            await Console.Out.WriteAsync("Name".PadRight(NameWidth));
            await Console.Out.WriteAsync(RowBlank);
            await Console.Out.WriteAsync("Uploaded".PadRight(UploadedWidth));
            await Console.Out.WriteAsync(RowBlank);
            await Console.Out.WriteAsync("Size".PadLeft(SizeWidth));

            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync(new string('-', NameWidth + UploadedWidth + SizeWidth + 6));

            await foreach (ContainerMetadata current in metadata)
            {
                string name = current.Name;
                await Console.Out.WriteAsync(name.PadRight(NameWidth));

                await Console.Out.WriteAsync(RowBlank);
                await Console.Out.WriteAsync(current.UploadedAt.ToString("yyyy/MM/dd HH:mm").PadRight(UploadedWidth));
                await Console.Out.WriteAsync(RowBlank);
                await Console.Out.WriteAsync(current.Size.ToString().PadLeft(SizeWidth));
                await Console.Out.WriteLineAsync();
            }
        }
    }
}
