namespace SCSearch.Data
{
    /// <summary>
    /// 検索パラメータを表します。
    /// </summary>
    public class SearchParameter
    {
        /// <summary>
        /// 検索文字列を取得または設定します。
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// 正規表現を使用するかどうかを表す値を取得または設定します。
        /// </summary>
        public bool UseRegex { get; set; }

        /// <summary>
        /// 表示順を取得または設定します。
        /// </summary>
        public ListOrder Order { get; set; }

        /// <summary>
        /// 降順ソートかどうかを表す値を取得または設定します。
        /// </summary>
        public bool Descending { get; set; }

        /// <summary>
        /// 大文字・小文字の区別を行うかどうかを表す値を取得または設定します。
        /// </summary>
        public bool CaseSensitive { get; set; }
    }
}
