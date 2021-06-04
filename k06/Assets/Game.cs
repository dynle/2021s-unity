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
    float player_x;
    float player_y;
    float player_speed;
    
    const int BLOCK_NUM = 10;
    int[] block_x = new int[BLOCK_NUM];
    int[] block_y = new int [BLOCK_NUM];
    bool[] block_alive_flag = new bool[BLOCK_NUM];
    int time;
    int next_block_num;
    bool isComplete;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        gc.SetResolution(720,1280);
        
        resetValue();
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        if((player_x > 0 && player_x<720) && (player_y>0 && player_y<1280)){
            player_x += gc.AccelerationLastX * player_speed;
            player_y += gc.AccelerationLastY * player_speed;
        }else{
            //initialize the position of the ball
            player_x = 360;
            player_y = 640;
        }

        if(isComplete == false) time++;
        for(int i=0;i< BLOCK_NUM;i++){
            if(block_alive_flag [i] && i== next_block_num){
                if(gc.CheckHitRect((int)player_x,(int)player_y,24,24,block_x[i],block_y[i],40,40)){
                block_alive_flag[i] = false;
                next_block_num++;
                    if(next_block_num == BLOCK_NUM){
                    isComplete=true;  
                }
                }
            }
            }
        
        //reset game if pressing the screen for 2 sec
        if(gc.GetPointerFrameCount(0)>= 120){
            resetValue();
        }
    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.SetColor(0, 0, 0);
        gc.SetFontSize(36);
        gc.DrawString("AcceX:"+gc.AccelerationLastX,0,0);
        gc.DrawString("AcceY:"+gc.AccelerationLastY,0,40);
        gc.DrawString("AcceZ:"+gc.AccelerationLastZ,0,80);

        gc.DrawImage(GcImage.BallYellow,(int)player_x,(int)player_y);

        gc.DrawString("time:"+time,0,160);
        if (isComplete) {
            gc.DrawString("CLEAR!!",0,200);
            //stop the ball when completed
            player_speed=0;
        }

        for(int i=0;i< BLOCK_NUM;i++){
        if(block_alive_flag[i]){
            gc.SetColor(255, 0, 0);
            gc.FillRect(block_x [i], block_y [i],40,40);
            gc.SetColor(0, 0, 0);
            gc.DrawString ("" + (i + 1),block_x [i], block_y [i]);
        }
        }
    }

    void resetValue(){
        player_x = 360.0f;
        player_y = 640.0f;
        player_speed = 20.0f;

        time = 0;
        next_block_num = 0;
        isComplete = false;

        for(int i =0 ; i < BLOCK_NUM ; i ++ ){
            block_x[i] = gc.Random(0,720-40);
            block_y[i] = gc.Random(0,1280-40);
            block_alive_flag [i] = true;
        }
    }
}
