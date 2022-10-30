#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using battleship.Library;

namespace battleship;

public class Game : Form
{
    private enum State
    {
        Waiting,
        Erroring,
        Ended
    };
    
    private ComponentResourceManager Resources;
    
    private Label MessageBox = new();
    
    private Label ErrorMessageBox = new();
    private Button ErrorMessageButton = new();

    private Field Player1Field;
    private Field Player2Field;
    
    private PlayerType CurrentPlayer;
    private State CurrentState;
        
    private void InitializeForm()
    {   
        Icon = ((Icon)(Resources.GetObject("favicon.Icon")));
        Name = "Game";
        Text = @"Морской Бой";
            
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
    }
    
    private void InitializeFields(List<List<Tuple<int, int>>> player1Ships, List<List<Tuple<int, int>>> player2Ships)
    {
        void InitializePlayer1Field()
        {
            Player1Field = new Field(PlayerType.Player1, GameState.Playing, 100, 600, 10, 10, "Поле выстрелов игрока 1");
            
            foreach (var cell in Player1Field.Cells)
            {
                cell.Click += CellClickHandler;
                Controls.Add(cell);
            }

            foreach (var ship in player1Ships)
            {
                Player1Field.Ships[ship.Count]++;
                
                foreach (var cell in ship)
                {
                    var cellIndex = cell.Item1 * Player1Field.Width + cell.Item2;
                    Player1Field.Cells[cellIndex].ChangeType(CellType.ShipAlive);
                }
            }
            
            Controls.Add(Player1Field.FieldLabel);
        }
        
        void InitializePlayer2Field()
        {
            Player2Field = new Field(PlayerType.Player2, GameState.Playing, 800, 600, 10, 10, "Поле выстрелов игрока 2");
            
            foreach (var cell in Player2Field.Cells)
            {
                cell.Click += CellClickHandler;
                Controls.Add(cell);
            }
            
            foreach (var ship in player2Ships)
            {
                Player2Field.Ships[ship.Count]++;
                
                foreach (var cell in ship)
                {
                    var cellIndex = cell.Item1 * Player2Field.Width + cell.Item2;
                    Player2Field.Cells[cellIndex].ChangeType(CellType.ShipAlive);
                }
            }
            
            Controls.Add(Player2Field.FieldLabel);
        }

        InitializePlayer1Field();
        InitializePlayer2Field();
        
        Player1Field.Hide();
        Player2Field.Hide();
    }
    
    private void InitializeMessageBoxes()
    {
        void InitializeErrorMessageButton()
        {
            ErrorMessageButton.Name = "ErrorMessageButton";
            
            ErrorMessageButton.Enabled = true;
            
            ErrorMessageButton.TabStop = false;
            
            ErrorMessageButton.Size = new Size(200, 50);
            ErrorMessageButton.Location = new Point(280, 480);
            ErrorMessageButton.TabIndex = 0;
            
            ErrorMessageButton.Text = @"Понятно";
            ErrorMessageButton.Cursor = Cursors.Arrow;

            ErrorMessageButton.Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);

            ErrorMessageButton.Click += ErrorMessageButtonClickHandler;
                
            Controls.Add(ErrorMessageButton);
        }
        
        void InitializeErrorMessageBox()
        {
            ErrorMessageBox.Name = "MessageBox";
            
            ErrorMessageBox.Location = new Point(200, 250);
            ErrorMessageBox.Size = new Size(300, 300);
            ErrorMessageBox.TabIndex = 0;

            ErrorMessageBox.BackColor = Color.Red;
            
            ErrorMessageBox.Padding = new Padding(5);
            
            Controls.Add(ErrorMessageBox);
            
            ErrorMessageBox.Hide();
        }

        void InitializeMessageBox()
        {
            MessageBox.Name = "MessageBox";
            
            MessageBox.Location = new Point(200, 50);
            MessageBox.Size = new Size(1000, 60);
            MessageBox.TabIndex = 0;
            
            Controls.Add(MessageBox);
        }

        InitializeErrorMessageButton();
        InitializeErrorMessageBox();
        InitializeMessageBox();
    }

    private void InitializeComponents()
    {
        SuspendLayout();
            
        InitializeForm();
        InitializeMessageBoxes();
            
        ResumeLayout();
    }
        
    private static void Game_FormClosing(object sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }
    
    public Game(List<List<Tuple<int, int>>> player1Ships, List<List<Tuple<int, int>>> player2Ships)
    {
        Resources = new ComponentResourceManager(typeof(Game));
            
        InitializeComponents();
        InitializeFields(player1Ships, player2Ships);
            
        Closing += Game_FormClosing;

        StartGame();

        ShowError("В случае необходимости будет появляться это окно с подсказками");
    }
    
    private void ErrorMessageButtonClickHandler(object sender, EventArgs eventArgs)
    {
        HideError();
    }
    
    private void ChangeCurrentState(State newState)
    {
        DisableFields();
        if (newState == State.Waiting)
        {
            EnableField();
        }

        CurrentState = newState;
    }

    private void ChangeCurrentPlayer(PlayerType newPlayer)
    {
        CurrentPlayer = newPlayer;
        
        ChangeCurrentState(State.Waiting);

        ErrorMessageBox.Location = CurrentPlayer == PlayerType.Player1 ?
            new Point(200, 250) : new Point(900, 250);
        
        ErrorMessageButton.Location = CurrentPlayer == PlayerType.Player1 ? 
            new Point(280, 480) : new Point(980, 480);

        MessageBox.Text = CurrentPlayer == PlayerType.Player1 ?
            "Ходит игрок 1 (нажимайте на клетку в левом поле, в которую хотите выстрелить)" :
            "Ходит игрок 2 (нажимайте на клетку в правом поле, в которую хотите выстрелить)";
    }
    
    private void ShowError(string? errorMessage = null)
    {
        ErrorMessageBox.Show();
        ErrorMessageButton.Show();
        
        if (CurrentState == State.Ended)
        {
            ErrorMessageBox.Text = CurrentPlayer == PlayerType.Player1
                ? "Поздравляем с победой игрока 1!"
                : "Поздравляем с победой игрока 2!";
            ErrorMessageBox.BackColor = Color.Chartreuse;
            ErrorMessageButton.Text = "Выйти в меню";
            
            return;
        }
        
        ErrorMessageBox.Text = errorMessage;

        ChangeCurrentState(State.Erroring);
    }
    
    private void HideError()
    {
        ErrorMessageBox.Hide();
        ErrorMessageButton.Hide();
        ErrorMessageBox.Text = "";
        
        if (CurrentState == State.Ended)
        {
            Hide();
            (new MainMenu()).Show();
            
            return;
        }

        ChangeCurrentState(State.Waiting);
    }
    
    private void StartGame()
    {
        ChangeCurrentPlayer(PlayerType.Player1);
        
        Player1Field.Show();
        Player2Field.Show();
    }

    private void EndGame()
    {
        ChangeCurrentState(State.Ended);
        ShowError();
    }

    private void CellClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState != State.Waiting)
        {
            return;
        }
        
        var cell = (Cell)sender;

        if (!cell.Enabled)
        {
            return;
        }

        var response = TryToShotCell(cell);
        
        if (response.Status == 100)
        {
            ShowError(response.Error);
        }
        else if (response.Status == 200)
        {
            ChangeCurrentPlayer(CurrentPlayer == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1);
        }
        else if (response.Status == 300)
        {
            MessageBox.Text = CurrentPlayer == PlayerType.Player1 ?
                "Продолжает ходить игрок 1, так как произошло попадание в корабль игрока 2" :
                "Продолжает ходить игрок 2, так как произошло попадание в корабль игрока 1";
        }
        else if (response.Status == 400)
        {
            MessageBox.Text = CurrentPlayer == PlayerType.Player1 ?
                "Продолжает ходить игрок 1, так как произошло потопление корабля игрока 2" :
                "Продолжает ходить игрок 2, так как произошло потопление корабля игрока 1";
        }
        else
        {
            EndGame();
        }
    }
    
    private void EnableField()
    { 
        if (CurrentPlayer == PlayerType.Player1)
        {
            Player1Field.Enable();
        }
        else
        {
            Player2Field.Enable();
        }
    }
    
    private void DisableFields()
    {
        Player1Field.Disable();
        Player2Field.Disable();
    }

    private Response TryToShotCell(Cell cell)
    {
        return CurrentPlayer == PlayerType.Player1 ?
            Player1Field.TryToShot(cell) : Player2Field.TryToShot(cell);
    }
}