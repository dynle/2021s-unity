using GameCanvas;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public sealed class Game : GameBase
{
    // 変数の宣言

    // Camera
    int camera_id; 
	string camera_name; 

    // string pname = "t93520mw";
    string pname = "t17501dl";
    string url = "";
    string str = "";

    const int BOX_NUM = 10;
    int[] box_x = new int [BOX_NUM];
    int[] box_y = new int [BOX_NUM];
    int[] box_speed = new int [BOX_NUM];
    int box_w = 24;
    int box_h = 24;

    int player_x = 304;
    int player_y = 400;
    int player_dir = 1;
    int player_speed = 3;

    int score =0;
    int count =0;

    int gameState = 0;

    int active_box_num = 0;

    int high_score = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        gc.SetResolution(640, 480);

        gc.TryLoad("hs",out high_score);
        
        // Camera
        // camera_id = 0; 
        // camera_name = gc.GetCameraDeviceName(camera_id); 
        // gc.StartCameraService(camera_id); 
        
        // キャンバスの大きさを設定します
        // gc.ChangeCanvasSize(720, 1280);

        resetValue();
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        // Camera
        // if (gc.GetPointerFrameCount(0) ==1 ){
        //     camera_id++;
        //     if (camera_id >= gc.CameraDeviceCount) camera_id= 0; 
        //     camera_name = gc.GetCameraDeviceName(camera_id); 
        //     gc.StartCameraService(camera_id); 
        // } 

        if(gc.GetPointerFrameCount(0) >= 120){
            resetValue();
        }
        // 起動からの経過時間を取得します
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

            active_box_num = 5 + count/600;
            if(active_box_num > BOX_NUM){
                active_box_num = BOX_NUM;
            }

            //scoreが変った時に呼ぶ
            if(score>high_score){
                high_score = score;
            }

            if(gc.GetPointerFrameCount(0) ==1 ){
                player_dir = -player_dir;
                gc.PlaySound(GcSound.Click1);
            }

            player_x += player_dir * player_speed;

            for(int i =0 ; i < active_box_num ; i ++ )
            {
                //箱を動かす処理
                box_y[i] = box_y[i] + box_speed[i];

                if(box_y[i]> 480){
                    box_x[i] = gc.Random(0,616);
                    box_y[i] = -gc.Random(100,480);
                    box_speed[i] = gc.Random(3,6);
            }

            //playerと箱の当り判定
            if (gc.CheckHitRect (
                player_x,player_y,32,32,
                box_x[i],box_y[i],box_w,box_h)) {
                //当たった時の処理
                gameState =2;
                gc.Save("hs",high_score);
                gc.PlaySound(GcSound.Click2,GcSoundTrack.BGM1,true);
                }
            }

            //playerが画面左右に着いてもgameOverになるように
            if(player_x < 0 || player_x > 608){
                gameState =2;
                gc.Save("hs",high_score);
            }
        }
        else if(gameState == 2){
            //ゲームオーバー時の処理 
            //タップしたらスコアを送信(課題２を参考に)
            if(gc.GetPointerFrameCount(0) ==1 ){
            url = "http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score="
                + score + "&name=" + pname;
                gc.GetOnlineTextAsync(url,out str);

                gc.StopSound(GcSoundTrack.BGM1);
            }       
            if(gc.GetPointerFrameCount(0) >=120){
                gameState=0;
                player_x = 304;
                player_y = 400;
                score = 0;
                count = 0;
                str="";
                resetValue();
            }
        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        //Camera
        // gc.ClearScreen();
        // gc.DrawScaledRotateCameraImage(100, 100, 25, 25, gc.CurrentCameraRotation); 
        // gc.DrawString(camera_name, 15, 15); 

        // 画面を白で塗りつぶします
        gc.ClearScreen();

        if(gameState == 0){
            //タイトル画面の処理
            gc.SetColor(0,0,0);
            gc.DrawString("Title",40,160);
        }
        else if(gameState == 1){
            //ゲーム中の処理
            gc.DrawOnlineImage("http://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/Player.png",player_x,player_y);
            for(int i =0 ; i < BOX_NUM ; i ++ ){
                gc.FillRect(box_x[i],box_y[i],box_w,box_h);  
            }
            gc.DrawString("SCORE:"+score,0,0);
            gc.DrawString("HIGH:"+high_score,0,60);
        }
        else if(gameState == 2){
            //ゲームオーバー時の処理
            gc.SetColor(0,0,0);
            gc.DrawString("GAME OVER",40,160);
            gc.DrawString(str,0,300);
            gc.DrawString("Press 2 seconds to play again", 100,400);
            gc.DrawString("SCORE:"+score,0,0);
            gc.DrawString("HIGH:"+high_score,0,60);
        }
    }

    void resetValue(){
        for(int i =0 ; i < BOX_NUM ; i ++ )
        {
            box_x[i] = gc.Random(0,616);
            box_y[i] = -gc.Random(100,480);
            box_speed[i] = gc.Random(3,6);
        }
    }
}
