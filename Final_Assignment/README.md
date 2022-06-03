## アプリ名
Save the Earth

## 想定している端末の機種名、、縦横の解像度
LGV32, 2560*1440, 16:9

## 動作確認方法
（１：apkで実機確認、２：xcodeで実行ファイルを作成して実機で確認、３：UnityRemoteで確認、４：その他）
1, 3

## 作ったアプリの内容
地球に近付く敵を戦闘機の攻撃で殺して地球を守るシューティングゲームを作りました。戦闘機の攻撃は無限に発射されて、スコアが50点増加するごとにステージが変わったら敵の速度が増加することに設定しました。ゲームは、敵とプレイヤーが接触した時、または地球のHPが0になった時にゲームオーバーになります。地球のHPは敵がスレイヤーを通り過ぎたら20が削減されて、５つの敵が通り過ぎたら地球のHPが0になります。

## 工夫した点、苦労した点、ＰＲポイント（あれば）
工夫した点は戦闘機が無限に攻撃を発射する処理でした。配列を使うと複雑になってしまって、他の色々な方法を考えたら一つの攻撃だけ使うことになりました。攻撃が敵と接触するかy座標が0より小さくなって画面の外に出るかによってbullet_onscreen変数を変えて、攻撃の位置が最初の位置に戻る処理が一番簡単だと思ってその方法で具現しました。苦労した点は特にないです。

## 備考
流用したリソース、アセットがあれば記載してください。
（自作リソースで、来期以降の授業で使っても良い、
という場合は、書いておいてください）
コード参考：授業k09コード
アセット：
1. BackgroundFirst：https://www.pinterest.co.kr/pin/275564070935985360/ 
2. BackgroundFirst_Earth: https://www.pinterest.co.kr/pin/824581012994021169/ 
3. Background: https://hdwallpaperim.com/space-pixel-art-horizon-stars/
4. Enemy: https://www.pngfind.com/download/oiohmR_space-ship-small-small-spaceship-png-transparent-png/
5. Flighter: https://pngtree.com/element/down?id=Mzc3NDgyNQ==&type=1&time=1624086309&token=NGEyZTAyNDU5OGNmMDZmZmExZjVkODRlZTk3M2ZkN2E=.
6. Bullet: https://www.deviantart.com/kidpaddleetcie/art/Attack-Ball-Effect-1-509267686
7. SoundEffects (Bling, Dead, Shooting, Stage): https://mixkit.co/free-sound-effects/game/
