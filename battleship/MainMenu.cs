using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace battleship;

public class MainMenu : Form
{
    private ComponentResourceManager Resources;

    private Button PlayButton = new();
    private Button ExitButton = new();
        
    private void InitializeForm()
    {   
        Icon = ((Icon)(Resources.GetObject("favicon.Icon")));
        Name = "MainMenu";
        Text = @"Морской Бой";
            
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
    }

    private void InitializeButtons()
    {
        void InitializePlayButton()
        {
            PlayButton.Name = "PlayButton";
            
            PlayButton.TabStop = false;
            
            PlayButton.Size = new Size(500, 100);
            PlayButton.Location = new Point(500, 200);
            PlayButton.TabIndex = 0;
            
            PlayButton.BackColor = SystemColors.Menu;
            PlayButton.Text = @"Играть";
            PlayButton.Cursor = Cursors.Arrow;
                
            PlayButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);

            PlayButton.Click += PlayButtonClickHandler;
                
            Controls.Add(PlayButton);
        }

        void InitializeExitButton()
        {
            ExitButton.Name = "ExitButton";
            
            ExitButton.TabStop = false;
            
            ExitButton.Size = new Size(500, 100);
            ExitButton.Location = new Point(500, 400);
            ExitButton.TabIndex = 0;
            
            ExitButton.BackColor = SystemColors.Menu;
            ExitButton.Text = @"Выйти";
            ExitButton.Cursor = Cursors.Arrow;

            ExitButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
                
            ExitButton.Click += ExitButtonClickHandler;
                
            Controls.Add(ExitButton);
        }

        InitializePlayButton();
        InitializeExitButton();
    }

    private void InitializeComponents()
    {
        SuspendLayout();
            
        InitializeForm();
        InitializeButtons();
            
        ResumeLayout();
    }
        
    private static void MainMenu_FormClosing(object sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }
    
    public MainMenu()
    {
        Resources = new ComponentResourceManager(typeof(MainMenu));
            
        InitializeComponents();
            
        Closing += MainMenu_FormClosing;
    }

    private void PlayButtonClickHandler(object sender, EventArgs eventArgs)
    {
        Hide();
        (new GameSettings()).Show();
    }
        
    private void ExitButtonClickHandler(object sender, EventArgs eventArgs)
    {
        // TEMPORARY //
        
        // var player1Ships = new List<List<Tuple<int, int>>>();
        // var player2Ships = new List<List<Tuple<int, int>>>();
        //
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 0)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 2)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 4)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 6)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 0),
        //     Tuple.Create(2, 1)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 3),
        //     Tuple.Create(2, 4)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 6),
        //     Tuple.Create(2, 7)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(4, 0),
        //     Tuple.Create(4, 1),
        //     Tuple.Create(4, 2)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(4, 4),
        //     Tuple.Create(4, 5),
        //     Tuple.Create(4, 6)
        // });
        // player1Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(6, 0),
        //     Tuple.Create(6, 1),
        //     Tuple.Create(6, 2),
        //     Tuple.Create(6, 3)
        // });
        //
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 0)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 2)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 4)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(0, 6)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 0),
        //     Tuple.Create(2, 1)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 3),
        //     Tuple.Create(2, 4)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(2, 6),
        //     Tuple.Create(2, 7)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(4, 0),
        //     Tuple.Create(4, 1),
        //     Tuple.Create(4, 2)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(4, 4),
        //     Tuple.Create(4, 5),
        //     Tuple.Create(4, 6)
        // });
        // player2Ships.Add(new List<Tuple<int, int>>()
        // {
        //     Tuple.Create(6, 0),
        //     Tuple.Create(6, 1),
        //     Tuple.Create(6, 2),
        //     Tuple.Create(6, 3)
        // });
        //
        // Hide();
        // (new Game(player1Ships, player2Ships)).Show();
        
        // TEMPORARY //
        
        Application.Exit();
    }
}