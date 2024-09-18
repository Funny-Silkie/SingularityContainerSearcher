using CuiLib.Commands;
using CuiLib.Options;
using SCSearch.Data;
using SCSearch.Logics;
using SCSearch.Utilities;

namespace SCSearch.Commands
{
    /// <summary>
    /// ダウンロードを行うコマンドです。
    /// </summary>
    internal class DownloadCommand : Command
    {
        #region Options

        private readonly FlagOption optionHelp;
        private readonly SingleValueOption<string> optionName;
        private readonly SingleValueOption<FileInfo> optionOut;
        private readonly FlagOption optionForceUpdate;

        #endregion Options

        /// <summary>
        /// <see cref="DownloadCommand"/>の新しいインスタンスを初期化します。
        /// </summary>
        public DownloadCommand() : base("download")
        {
            Description = "コンテナをダウンロードします";

            optionHelp = new FlagOption('h', "help")
            {
                Description = "ヘルプを表示します",
            }.AddTo(this);
            optionName = new SingleValueOption<string>('n', "name")
            {
                Description = "コンテナ名（完全一致・大文字・小文字一致）",
                Required = true,
                Checker = ValueChecker.NotEmpty(),
            }.AddTo(this);
            optionOut = new SingleValueOption<FileInfo>('o', "out")
            {
                Description = "保存先のパス",
                Required = true,
                Checker = ValueChecker.VerifyDestinationFile(false, true),
            }.AddTo(this);
            optionForceUpdate = new FlagOption("force-update")
            {
                Description = "コンテナ情報を更新します（通常は直近のデータを使い回します）",
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

            var logic = new MainLogic();
            await logic.DownloadAsync(optionName.Value, optionOut.Value, optionForceUpdate.Value);
        }
    }
}
