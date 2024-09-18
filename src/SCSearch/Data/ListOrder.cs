namespace SCSearch.Data
{
    /// <summary>
    /// 一覧表示のソートキーを表します。
    /// </summary>
    public enum ListOrder
    {
        /// <summary>
        /// Web版と同じ順番
        /// </summary>
        Web = 0,

        /// <summary>
        /// 名前順
        /// </summary>
        Name = 1,

        /// <summary>
        /// アップロード日時順
        /// </summary>
        Uploaded = 2,

        /// <summary>
        /// サイズ順
        /// </summary>
        Size = 3,
    }
}
