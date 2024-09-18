using CuiLib.Commands;
using CuiLib.Options;
using SCSearch.Utilities;

namespace SCSearch.Commands
{
    /// <summary>
    /// メインのコマンドを表します。
    /// </summary>
    internal class MainCommand : Command
    {
        #region Options

        private readonly FlagOption optionHelp;
        private readonly FlagOption optionVersion;

        #endregion Options

        /// <summary>
        /// <see cref="MainCommand"/>の新しインスタンスを初期化します。
        /// </summary>
        public MainCommand() : base("scsearch")
        {
            Description = "Galaxy COMMUNITY HUBのコンテナを検索します";

            Children.Add(new SearchCommand());

            optionHelp = new FlagOption('h', "help")
            {
                Description = "ヘルプを表示します",
            }.AddTo(this);
            optionVersion = new FlagOption('v', "version")
            {
                Description = "バージョンを表示します",
            }.AddTo(this);
        }

        /// <inheritdoc/>
        protected override async Task OnExecutionAsync()
        {
            if (optionHelp.Value)
            {
                WriteHelp(Console.Out);
                return;
            }
            if (optionVersion.Value)
            {
                await Console.Out.WriteLineAsync(SR.Version);
                return;
            }
        }
    }
}
