## 概要 ##
tumblrから画像を一括でダウンロードできたらなーと思って作成。

  * C#で書き直し、Tumblr API v2に対応。
  * TumblrPhotoの実行にはWindowsと.net Framework 4.0が必要です。
  * 1万枚以上の画像を安定してダウンロード。

## 既知問題 ##

  * ダンロード済みのファイルに対してもダウンロード中のメッセージが表示される。(ダウンロードはされない)

## 更新 ##

[TumblrPhoto\_0.2.0.0](http://code.google.com/p/tumblr-photo/downloads/detail?name=TumblrPhoto_0.2.0.0.zip)で下記に対応。

  1. Textでポストされた画像のダウンロードに対応。

[TumblrPhoto\_0.1.2.1](http://code.google.com/p/tumblr-photo/downloads/detail?name=TumblrPhoto_0.1.2.1.zip)で下記に対応。

  1. ダウンロードファイルの拡張子が正常に設定されない場合に正しく設定されるように修正。(jpg、png、gif、bmpに対応。)
  1. 拡張子の判定処理に伴いダウンロード済みのファイル判定方法を修正。
  1. 入力した設定が稀に保存されないバグを修正。(これに伴いバージョンアップ後に今までの設定が消える場合があります。)


TumblrPhoto\_0.1.1.0で下記に対応。

  1. 複数画像のポストが有る場合ダウンロードリスト生成に失敗する。
  1. 画像が404の場合にダウンロードが中断される。

## その他 ##

  * ソースはSVNのリポジトリに格納してあります。