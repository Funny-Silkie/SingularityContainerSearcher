# SingularityContainerSearcher

- [SingularityContainerSearcher](#singularitycontainersearcher)
  - [インストール](#インストール)
  - [メタデータの扱いについて](#メタデータの扱いについて)
  - [利用法](#利用法)
    - [コンテナの検索](#コンテナの検索)
    - [コンテナのダウンロード](#コンテナのダウンロード)


[Galaxy COMMUNICATION hub](https://depot.galaxyproject.org/singularity/)からSingularityコンテナを検索・ダウンロードできるコマンドラインツールです。

## インストール

1. [Releaseページ](https://github.com/Funny-Silkie/SingularityContainerSearcher/releases)から最新のバージョンを選択
1. 使用するコンピュータのOS・プロセッサにあったものをDL・解凍
1. 実行ファイルをパスの通った場所に配置

## メタデータの扱いについて

当ソフトウェアではGalaxy COMMUNICATION hubにアクセスして得られるHTMLの解析によりコンテナ情報を得ています。
この通信はちょっと重めなので取得したデータはキャッシュしています。
ファイルの配置場所は `~/.scsearch/` 以下です（Winは `C:\Users\{USERNAME}\` 以下）。
メタデータの取得から一時間以上経過した場合はコマンド実行時に再度取得・更新されます。
強制的に更新したい場合は `--force-update` オプションを適用します。

## 利用法

### コンテナの検索

```sh
SCSearch search [-h] [-q string] [-r] [-o string] [-c] [-d] [--force-update]
```

**オプション**
| 名前1 | 名前2            | 必須 / 既定値 |               型               | 説明                                                                                                       |
| :---- | :--------------- | :-----------: | :----------------------------: | :--------------------------------------------------------------------------------------------------------- |
| -h    | --help           |       -       |              flag              | ヘルプの表示                                                                                               |
| -q    | --query          |   - / null    |             string             | 検索文字列                                                                                                 |
| -r    | --use-regex      |       -       |              flag              | 正規表現として検索                                                                                         |
| -o    | --order          |   - / `web`   | `web` `name` `uploaded` `size` | 並び順。`web`は順にWeb版準拠，`name`は名前順，`uploaded`はコンテナアップロード日時，`size`はファイルサイズ |
| -d    | --descending     |       -       |              flag              | `-o` で指定したものを降順でソートする（`-o web`では無効）                                                  |
| -c    | --case-sensitive |       -       |              flag              | 大文字・小文字を区別して検索                                                                               |
|       | --force-update   |       -       |              flag              | メタデータを更新                                                                                           |

### コンテナのダウンロード

```sh
SCSearch download [-h] -n string -o file [--force-update]
```

**オプション**
| 名前1 | 名前2  | 必須 / 既定値 |   型   | 説明                                             |
| :---- | :----- | :-----------: | :----: | :----------------------------------------------- |
| -h    | --help |       -       |  flag  | ヘルプの表示                                     |
| -n    | --name |       +       | string | コンテナ名（完全一致且つ大文字・小文字区別アリ） |
| -o    | --out  |       +       |  file  | 出力先ファイル                                   |
