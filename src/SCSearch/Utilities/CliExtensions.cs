using CuiLib.Commands;
using CuiLib.Options;

namespace SCSearch.Utilities
{
    /// <summary>
    /// コマンドライン関連の拡張を表します。
    /// </summary>
    internal static class CliExtensions
    {
        /// <summary>
        /// オプションをコマンドに追加します。
        /// </summary>
        /// <typeparam name="TOption">オプションの型</typeparam>
        /// <param name="option">対象のオプション</param>
        /// <param name="command">検索先</param>
        /// <returns><paramref name="option"/></returns>
        public static TOption AddTo<TOption>(this TOption option, Command command)
            where TOption : Option
        {
            command.Options.Add(option);
            return option;
        }
    }
}
