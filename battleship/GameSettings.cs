using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using battleship.Library;

namespace battleship;

public class GameSettings : Form
{
    private enum State
    {
        Creating,
        Removing,
        Waiting,
        Erroring
    };
    
    private ComponentResourceManager Resources;

    private Field Player1Field;
    private Field Player2Field;
    
    private Label MessageBox = new();
    
    private Label ErrorMessageBox = new();
    private Button ErrorMessageButton = new();

    private Button NewShipButton = new();
    private Button AddShipButton = new();
    private Button RemoveShipButton = new();
    private Button FinishCreatingButton = new();
    
    private PlayerType CurrentPlayer;
    private State CurrentState;
    
    private void InitializeForm()
    {   
        Icon = ((Icon)(Resources.GetObject("favicon.Icon")));
        Name = "GameSettings";
        Text = @"Морской Бой";
            
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
    }

    private void InitializeFields()
    {
        void InitializePlayer1Field()
        {
            Player1Field = new Field(PlayerType.Player1, GameState.Creating, 100, 600, 10, 10, "Поле игрока 1");
            
            foreach (var cell in Player1Field.Cells)
            {
                cell.Click += CellClickHandler;
                Controls.Add(cell);
            }
            
            Controls.Add(Player1Field.FieldLabel);
        }
        
        void InitializePlayer2Field()
        {
            Player2Field = new Field(PlayerType.Player2, GameState.Creating, 100, 600, 10, 10, "Поле игрока 2");
            
            foreach (var cell in Player2Field.Cells)
            {
                cell.Click += CellClickHandler;
                Controls.Add(cell);
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
            
            ErrorMessageButton.Size = new Size(150, 50);
            ErrorMessageButton.Location = new Point(330, 480);
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
            MessageBox.Size = new Size(500, 60);
            MessageBox.TabIndex = 0;
            
            Controls.Add(MessageBox);
        }

        InitializeErrorMessageButton();
        InitializeErrorMessageBox();
        InitializeMessageBox();
    }

    private void InitializeButtons()
    {
        void InitializeNewShipButton()
        {
            NewShipButton.Name = "NewShipButton";
            
            NewShipButton.TabStop = false;
            
            NewShipButton.Size = new Size(300, 80);
            NewShipButton.Location = new Point(700, 200);
            NewShipButton.TabIndex = 0;
            
            NewShipButton.BackColor = SystemColors.Menu;
            NewShipButton.Text = @"Новый корабль";
            NewShipButton.Cursor = Cursors.Arrow;

            NewShipButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
                
            NewShipButton.Click += NewShipButtonClickHandler;
                
            Controls.Add(NewShipButton);
        }
        
        void InitializeAddShipButton()
        {
            AddShipButton.Name = "AddShipButton";
            
            AddShipButton.TabStop = false;
            
            AddShipButton.Size = new Size(300, 80);
            AddShipButton.Location = new Point(700, 300);
            AddShipButton.TabIndex = 0;
            
            AddShipButton.BackColor = SystemColors.Menu;
            AddShipButton.Text = @"Добавить корабль";
            AddShipButton.Cursor = Cursors.Arrow;

            AddShipButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
                
            AddShipButton.Click += AddShipButtonClickHandler;
                
            Controls.Add(AddShipButton);
        }
        
        void InitializeRemoveShipButton()
        {
            RemoveShipButton.Name = "RemoveShipButton";
            
            RemoveShipButton.TabStop = false;
            
            RemoveShipButton.Size = new Size(300, 80);
            RemoveShipButton.Location = new Point(700, 400);
            RemoveShipButton.TabIndex = 0;
            
            RemoveShipButton.BackColor = SystemColors.Menu;
            RemoveShipButton.Text = @"Удалить корабль";
            RemoveShipButton.Cursor = Cursors.Arrow;

            RemoveShipButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
                
            RemoveShipButton.Click += RemoveShipButtonClickHandler;
                
            Controls.Add(RemoveShipButton);
        }

        void InitializeFinishCreatingButton()
        {
            FinishCreatingButton.Name = "RemoveShipButton";
            
            FinishCreatingButton.TabStop = false;
            
            FinishCreatingButton.Size = new Size(300, 80);
            FinishCreatingButton.Location = new Point(700, 500);
            FinishCreatingButton.TabIndex = 0;
            
            FinishCreatingButton.BackColor = SystemColors.Menu;
            FinishCreatingButton.Text = @"Завершить расстановку";
            FinishCreatingButton.Cursor = Cursors.Arrow;

            FinishCreatingButton.Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
                
            FinishCreatingButton.Click += FinishCreatingButtonClickHandler;
                
            Controls.Add(FinishCreatingButton);
        }
        
        InitializeNewShipButton();
        InitializeAddShipButton();
        InitializeRemoveShipButton();
        InitializeFinishCreatingButton();
    }
    
    
    private void InitializeComponents()
    {
        SuspendLayout();
            
        InitializeForm();
        InitializeMessageBoxes();
        InitializeFields();
        InitializeButtons();
            
        ResumeLayout();
    }
    
    private static void GameSettings_FormClosing(object sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }
    
    public GameSettings()
    {
        Closing += GameSettings_FormClosing;
        
        Resources = new ComponentResourceManager(typeof(MainMenu));
            
        InitializeComponents();
        
        StartCreating(PlayerType.Player1);
        
        ShowError("В случае необходимости будет появляться это окно с подсказками");
    }

    private void ErrorMessageButtonClickHandler(object sender, EventArgs eventArgs)
    {
        HideError();
    }

    private void ChangeCurrentState(State newState)
    {
        if (newState == State.Waiting)
        {
            EnableField();
            
            NewShipButton.Enabled = true;
            AddShipButton.Enabled = false;
            RemoveShipButton.Enabled = AlreadyCreatedShips() > 0;
            FinishCreatingButton.Enabled = AlreadyCreatedShips() == 10;
        }
        else if (newState == State.Creating)
        {
            EnableField();
            
            NewShipButton.Enabled = false;
            AddShipButton.Enabled = true;
            RemoveShipButton.Enabled = false;
            FinishCreatingButton.Enabled = false;
        }
        else if (newState == State.Removing)
        {
            EnableField();
            
            NewShipButton.Enabled = false;
            AddShipButton.Enabled = false;
            RemoveShipButton.Enabled = false;
            FinishCreatingButton.Enabled = false;
        }
        else if (newState == State.Erroring)
        {
            DisableField();
            
            NewShipButton.Enabled = false;
            AddShipButton.Enabled = false;
            RemoveShipButton.Enabled = false;
            FinishCreatingButton.Enabled = false;
        }

        CurrentState = newState;
    }

    private void ShowError(string errorMessage)
    {
        ErrorMessageBox.Text = errorMessage;
        ErrorMessageBox.Show();
        ErrorMessageButton.Show();

        ChangeCurrentState(State.Erroring);
    }
    
    private void HideError()
    {
        ErrorMessageBox.Text = "";
        ErrorMessageBox.Hide();
        ErrorMessageButton.Hide();

        ChangeCurrentState(State.Waiting);
    }

    private void StartCreating(PlayerType player)
    {
        CurrentPlayer = player;

        ChangeCurrentState(State.Waiting);
        
        if (player == PlayerType.Player1)
        {
            Player1Field.Show();
            MessageBox.Text = "Игрок 1 ставит корабли";
        }
        else
        {
            Player2Field.Show();
            MessageBox.Text = "Игрок 2 ставит корабли";
        }
    }
    
    private void FinishCreating(PlayerType player)
    {
        if (player == PlayerType.Player1)
        {
            Player1Field.Hide();
            MessageBox.Text = "";
            
            StartCreating(PlayerType.Player2);
        }
        else
        {
            Player2Field.Hide();
            MessageBox.Text = "";

            Hide();

            (new Game(Player1Field.GetListOfShips(), Player2Field.GetListOfShips())).Show();
        }
    }

    private void NewShipButtonClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState != State.Waiting)
        {
            return;
        }
        
        ChangeCurrentState(State.Creating);
    }
    
    private void AddShipButtonClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState != State.Creating)
        {
            return;
        }

        var response = TryToCreateShip();
        
        if (response.Status != 200)
        {
            ShowError(response.Error);
            return;
        }
        
        ChangeCurrentState(State.Waiting);
    }

    private void RemoveShipButtonClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState != State.Waiting)
        {
            return;
        }

        ChangeCurrentState(State.Removing);
    }
    
    private void FinishCreatingButtonClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState != State.Waiting)
        {
            return;
        }

        FinishCreating(CurrentPlayer);
    }
    
    private void CellClickHandler(object sender, EventArgs eventArgs)
    {
        if (CurrentState == State.Waiting)
        {
            ShowError("Чтобы поставить корабль, нажмите на кнопку 'Новый корабль'\n" +
                      "Чтобы удалить корабль, нажмите на кнопку 'Удалить корабль'");
            return;
        }
        
        var cell = (Cell)sender;

        if (!cell.Enabled)
        {
            return;
        }

        if (CurrentState == State.Creating)
        {
            var response = cell.Select();

            if (response.Status != 200)
            {
                ShowError(response.Error);
            }
        }
        else if (CurrentState == State.Removing)
        {
            var response = TryToRemoveShip(cell);
            
            if (response.Status != 200)
            {
                ShowError(response.Error);
                return;
            }

            ChangeCurrentState(State.Waiting);
        }
    }

    private int AlreadyCreatedShips()
    {
        return CurrentPlayer == PlayerType.Player1 ?
            Player1Field.GetAmountOfShips() : Player2Field.GetAmountOfShips();
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
    
    private void DisableField()
    {
        if (CurrentPlayer == PlayerType.Player1)
        {
            Player1Field.Disable();
        }
        else
        {
            Player2Field.Disable();
        }
    }

    private Response TryToCreateShip()
    {
        return CurrentPlayer == PlayerType.Player1 ?
            Player1Field.TryToCreateShip() : Player2Field.TryToCreateShip();
    }

    private Response TryToRemoveShip(Cell cell)
    {
        return cell.Owner == PlayerType.Player1
            ? Player1Field.TryToRemoveShip(cell)
            : Player2Field.TryToRemoveShip(cell);
    }
}