#nullable enable
using GameCanvas;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    // 変数の宣言
    int ball_x;
    int ball_y;
    int ball_speed_x;
    int ball_speed_y;

    int player_x;
    int player_y;
    int player_w;
    int player_h;

    const int BLOCK_NUM = 50;
    int[] block_x = new int [BLOCK_NUM];
    int[] block_y = new int [BLOCK_NUM];
    bool[] block_alive_flag = new bool [BLOCK_NUM];
    int block_w = 64;
    int block_h = 20;
    int time; 

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        gc.SetResolution(640,480);
        ball_x=0;
        ball_y=0;
        ball_speed_x=3;
        ball_speed_y=3;

        player_x=270;
        player_y=460;
        player_w=100;
        player_h=20;

        for(int i=0;i<BLOCK_NUM;i++){
            block_x[i]=(i%10)*block_w;
            block_y[i]=(i/10)*block_h;
            block_alive_flag[i]=true;
        }
        time=0;
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        if(countBlock()>0) time++;

        if(gc.GetPointerFrameCount(0)>0){
            player_x=(int)(gc.GetPointerX(0) - player_w/2);
            player_y=(int)(gc.GetPointerY(0) - player_h/2);
        }

        ball_x=ball_x+ball_speed_x;
        ball_y=ball_y+ball_speed_y;

        if(ball_x<0){
            ball_x=0;
            ball_speed_x=-ball_speed_x;
        }

        if( ball_y < 0 ) {
        ball_y = 0;
        ball_speed_y = -ball_speed_y;
        }

        if( ball_x > 616 ) {
        ball_x = 616;
        ball_speed_x = -ball_speed_x;
        }

        if(gc.CheckHitRect(ball_x,ball_y,24,24,player_x,player_y,player_w,player_h)){
            if(ball_speed_y>0){
                ball_speed_y=-ball_speed_y;
            }
        }

        for(int i=0;i<BLOCK_NUM;i++){
            if(gc.CheckHitRect(ball_x,ball_y,24,24,block_x[i],block_y[i],block_w,block_h)){
                block_alive_flag[i]=false;
            }
        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.DrawImage(GcImage.BlueSky, 0, 0);
        gc.DrawImage(GcImage.BallYellow,ball_x,ball_y);

        gc.SetColor(0,0,255);
        gc.FillRect(player_x,player_y,player_w,player_h);

        for(int i=0;i<BLOCK_NUM;i++){
            if(block_alive_flag[i]) gc.FillRect(block_x[i],block_y[i],block_w,block_h);
        }

        gc.SetColor(255,0,0);
        gc.DrawString("time:"+time,0,0);

        if(countBlock()==0){

            gc.DrawString("Clear!",270, 240);

            //Stop the ball
            ball_speed_x=0;
            ball_speed_y=0;

            //Move the player back to the original position
            player_x=270;
            player_y=460;
            player_w=100;
            player_h=20;
        }
    }

    int countBlock(){
        int num=0;
        for(int i=0;i<BLOCK_NUM;i++){
            if(block_alive_flag[i]){
                num++;
            }
        }
        return num;
    }
}
