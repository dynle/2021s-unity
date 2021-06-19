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
    const int ENEMY_NUM = 10;
    int[] enemy_x = new int [ENEMY_NUM];
    int[] enemy_y = new int [ENEMY_NUM];
    int[] box_speed = new int [ENEMY_NUM];
    int enemy_w = 100;
    int enemy_h = 81;

    int player_x = 360;
    int player_y = 1000;
    int player_dir = 1;
    int player_speed = 5;

    int score =0;
    int count =0;

    int gameState = 0;

    int high_score = 0;

    int background_speed = 2;

    float attack_rate = 0.1f;

    public override void InitGame()
    {
        gc.ChangeCanvasSize(720, 1280);
        gc.SetResolution(720, 1280);

        gc.TryLoad("hs",out high_score);

        resetValue();
    }

    public override void UpdateGame()
    {
        if(gameState == 0){
            if(gc.GetPointerFrameCount(0) == 1){
                gameState =1;
            }
        }
        else if(gameState == 1){
            count++;
            score = count/60;
            enemy_w = 24+count/300;
            enemy_h = 24+count/300;

            if(score>high_score){
                high_score = score;
            }

            if(gc.GetPointerFrameCount(0) ==1 ){
            player_dir = -player_dir;
            }

            //stop the flight if contacts with the wall
            if(player_x<=0 || player_x>=550){
                player_speed = 0;
                if(gc.GetPointerFrameCount(0) ==1){
                    if(player_x<=0) player_x +=1;
                    else player_x -=1;
                }
            }else{
                player_speed = 5;
                player_x += player_dir * player_speed;
            }

            for(int i =0 ; i < ENEMY_NUM ; i ++ ){
                enemy_y[i] = enemy_y[i] + box_speed[i];

                if(enemy_y[i]> 1280){
                    enemy_x[i] = gc.Random(0,696);
                    enemy_y[i] = -gc.Random(100,1280);
                    box_speed[i] = gc.Random(3,6);
                }

                if (gc.CheckHitRect (
                    player_x,player_y,170,167,
                    enemy_x[i],enemy_y[i],enemy_w,enemy_h)) {
                    gameState =2;
                    gc.Save("hs",high_score);
                }
            }
        }
        else if(gameState == 2){
            if(gc.GetPointerFrameCount(0) >=120){
                gameState=0;
                player_x = 360;
                player_y = 1100;
                score = 0;
                count = 0;
                resetValue();
            }
        }

    }

    public override void DrawGame()
    {
        if(gameState == 0){
            gc.ClearScreen();
            gc.SetColor(0,0,0);
            gc.SetFontSize(100);
            gc.DrawString("Start Screen",50,160);
            gc.SetFontSize(60);
            gc.DrawString("Tap screen to start",70,300);
        }
        else if(gameState == 1){
            gc.DrawImage(GcImage.Background,0,0);
            gc.DrawImage(GcImage.Flighter,player_x,player_y);
            for(int i =0 ; i < ENEMY_NUM ; i ++ ){
                gc.DrawImage(GcImage.Enemy,enemy_x[i],enemy_y[i]); 
            }

            // bottom bar
            gc.SetColor(255,255,255);
            gc.FillRect(0,1200,720,1280);

            gc.SetColor(0,0,0);
            gc.SetFontSize(50);
            gc.DrawString("SCORE:"+score,100,1220);
            gc.DrawString("HIGH:"+high_score,460,1220);

        }
        else if(gameState == 2){
            gc.SetColor(255,255,255);
            gc.SetFontSize(100);
            gc.DrawString("GAME OVER",140,640);
            gc.SetFontSize(50);
            gc.DrawString("Press 2 secs to restart",70,740);


            // bottom bar
            // gc.SetColor(0,0,0);
            // gc.SetFontSize(50);
            // gc.DrawString("SCORE:"+score,0,1240);
            // gc.DrawString("HIGH:"+high_score,360,1240);
        }
    }

    public void resetValue(){
        for(int i =0 ; i < ENEMY_NUM ; i ++ )
        {
            enemy_x[i] = gc.Random(0,616);
            enemy_y[i] = -gc.Random(100,480);
            box_speed[i] = gc.Random(3,6);
        }
    }

}
