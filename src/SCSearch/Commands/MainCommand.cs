using CuiLib.Commands;

namespace SCSearch.Commands
{
    /// <summary>
    /// メインのコマンドを表します。
    /// </summary>
    internal class MainCommand : Command
    {
        /// <summary>
        /// <see cref="MainCommand"/>の新しインスタンスを初期化します。
        /// </summary>
        public MainCommand() : base("scsearch")
        {
        }
    }
}
