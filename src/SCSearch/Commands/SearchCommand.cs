using CuiLib.Commands;
using CuiLib.Options;
using SCSearch.Data;
using SCSearch.Logics;
using SCSearch.Utilities;

namespace SCSearch.Commands
{
    /// <summary>
    /// 検索コマンドを表します。
    /// </summary>
    internal class SearchCommand : Command
    {
        #region Options

        private readonly FlagOption optionHelp;
        private readonly SingleValueOption<string?> optionQuery;
        private readonly FlagOption optionUseRegex;
        private readonly SingleValueOption<ListOrder> optionOrder;
        private readonly FlagOption optionDescending;
        private readonly FlagOption optionCaseSensitive;
        private readonly FlagOption optionForceUpdate;

        #endregion Options

        /// <summary>
        /// <see cref="SearchCommand"/>の新しインスタンスを初期化します。
        /// </summary>
        public SearchCommand() : base("search")
        {
            Description = "検索結果に一致するコンテナを検索します";

            optionHelp = new FlagOption('h', "help")
            {
                Description = "ヘルプを表示します",
            }.AddTo(this);
            optionQuery = new SingleValueOption<string?>('q', "query")
            {
                Description = "検索文字列",
                Required = false,
            }.AddTo(this);
            optionUseRegex = new FlagOption('r', "use-regex")
            {
                Description = "正規表現を使用します",
            }.AddTo(this);
            optionOrder = new SingleValueOption<ListOrder>('o', "order")
            {
                Description = $"表示順（{string.Join(", ", Enum.GetNames<ListOrder>().Select(x => x.ToLower()))}から選択）",
                Required = false,
                Converter = ValueConverter.StringToEnum<ListOrder>(true),
                Checker = ValueChecker.Defined<ListOrder>(),
                DefaultValue = ListOrder.Web,
            }.AddTo(this);
            optionCaseSensitive = new FlagOption('c', "case-sensitive")
            {
                Description = "大文字・小文字の区別を付けて検索します",
            }.AddTo(this);
            optionDescending = new FlagOption('d', "descending")
            {
                Description = "-cオプションで指定したキーを基に降順で表示します",
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
            await logic.SearchAsync(new SearchParameter()
            {
                Query = optionQuery.Value,
                UseRegex = optionUseRegex.Value,
                Order = optionOrder.Value,
                Descending = optionDescending.Value,
                CaseSensitive = optionCaseSensitive.Value,
            }, optionForceUpdate.Value);
        }
    }
}
