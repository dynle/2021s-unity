using GameCanvas;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    // 変数の宣言
    // int score=0;
    // string pname = "t93520mw";
    // string pname = "t17501dl";
    // string url = "";
    // string str = "";

    const int BOX_NUM = 10;
    int[] box_x = new int [BOX_NUM];
    int[] box_y = new int [BOX_NUM];
    int[] box_speed = new int [BOX_NUM];
    int box_w = 100;
    int box_h = 81;

    int player_x = 360;
    static int player_y = 1000;
    int player_dir = 1;
    int player_speed = 5;

    int score =0;
    int count =0;
    // string str="";

    int gameState = 0;

    int high_score = 0;

    int background_speed = 2;

    double attack_rate = 0.1;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        // キャンバスの大きさを設定します
        gc.ChangeCanvasSize(720, 1280);
        // gc.SetResolution(640, 480);

        gc.TryLoad("hs",out high_score);

        resetValue();
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        // 起動からの経過時間を取得します
        // sec = (int)gc.TimeSinceStartup;
        if(gameState == 0){
            //タイトル画面の処理
            if(gc.GetPointerFrameCount(0) == 1){
                gameState =1;
            }
        }
        else if(gameState == 1){
            //ゲーム中の処理
            count++;
            score = count/60;
            box_w = 24+count/300;
            box_h = 24+count/300;

            if(score>high_score){
                high_score = score;
            }

            if(gc.GetPointerFrameCount(0) ==1 ){
            player_dir = -player_dir;
            }

            //stop the flight if contacts with the wall
            if(player_x==0 || player_x==550){
                player_speed = 0;
                if(gc.GetPointerFrameCount(0) ==1){
                    if(player_x==0) player_x +=1;
                    else player_x -=1;
                }
            }else{
                player_speed = 5;
                player_x += player_dir * player_speed;
            }

            for(int i =0 ; i < BOX_NUM ; i ++ )
            {
            //箱を動かす処理
            box_y[i] = box_y[i] + box_speed[i];

            if(box_y[i]> 1280){
                box_x[i] = gc.Random(0,696);
                box_y[i] = -gc.Random(100,1280);
                box_speed[i] = gc.Random(3,6);
            }

            //playerと箱の当り判定
            if (gc.CheckHitRect (
                player_x,player_y,170,167,
                box_x[i],box_y[i],box_w,box_h)) {
                //当たった時の処理
                gameState =2;
                gc.Save("hs",high_score);
            }
            }

            //playerが画面左右に着いてもgameOverになるように
            // if(player_x < 0 || player_x > 608){
            //     gameState =2;
            //     gc.Save("hs",high_score);

            // }
        }
        else if(gameState == 2){
            //ゲームオーバー時の処理 
            //タップしたらスコアを送信(課題２を参考に)
            // if(gc.GetPointerFrameCount(0) ==1 ){
            // url = "http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score="
            //     + score + "&name=" + pname;
            //     gc.GetOnlineTextAsync(url,out str);
            // }       
            if(gc.GetPointerFrameCount(0) >=120){
                gameState=0;
                player_x = 360;
                player_y = 1000;
                score = 0;
                count = 0;
                // str="";
                resetValue();
            }
        }

    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        // 画面を白で塗りつぶします
        // gc.ClearScreen();

        // gc.DrawOnlineImage("http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/Player.png",320,240);

        // gc.DrawString(str,0,300);

        if(gameState == 0){
            //タイトル画面の処理
            gc.ClearScreen();
            gc.SetColor(0,0,0);
            gc.SetFontSize(100);
            gc.DrawString("Shooting Game",40,160);
        }
        else if(gameState == 1){
            gc.DrawImage(GcImage.Background,0,0);
            //ゲーム中の処理
            // gc.DrawOnlineImage("http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/Player.png",player_x,player_y);
            gc.DrawImage(GcImage.Flighter,player_x,player_y);
            for(int i =0 ; i < BOX_NUM ; i ++ ){
                // gc.FillRect(box_x[i],box_y[i],box_w,box_h); 
                gc.DrawImage(GcImage.Enemy,box_x[i],box_y[i]); 
            }
            gc.DrawString("SCORE:"+score,0,0);
            gc.DrawString("HIGH:"+high_score,0,60);

        }
        else if(gameState == 2){
            //ゲームオーバー時の処理
            gc.SetColor(0,0,0);
            gc.DrawString("GAME OVER",140,640);
            // gc.DrawString(str,0,300);
            gc.DrawString("SCORE:"+score,0,0);
            gc.DrawString("HIGH:"+high_score,0,60);

        }

        // 青空の画像を描画します
        // gc.DrawImage(GcImage.BlueSky, 0, 0);

        // // 黒の文字を描画します
        // gc.SetColor(0, 0, 0);
        // gc.SetFontSize(48);
        // gc.SetStringAnchor(GcAnchor.UpperLeft);
        // gc.DrawString("この文字と青空の画像が", 40, 160);
        // gc.DrawString("見えていれば成功です", 40, 270);
        // gc.SetStringAnchor(GcAnchor.UpperRight);
        // gc.DrawString($"{sec}s", 630, 10);
    }

    public void resetValue(){
        for(int i =0 ; i < BOX_NUM ; i ++ )
        {
            box_x[i] = gc.Random(0,616);
            box_y[i] = -gc.Random(100,480);
            box_speed[i] = gc.Random(3,6);
        }
    }

}
