using System.Drawing;
using System.Windows.Forms;

namespace battleship.Library;

public sealed class Cell : Button
{
    public int Row, Col;
    public CellType Type;
    public bool Selected;

    public PlayerType Owner;
    public GameState State;

    public Cell(int x, int y, int row, int col, int height, int width, PlayerType owner, GameState state)
    {
        Owner = owner;
        State = state;
        
        Row = row;
        Col = col;

        Height = height;
        Width = width;
        
        Location = new Point(x, y);
        
        Name = "Cell-" + row + "-" + col; 
        
        TabStop = false;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 1;
            
        Size = new Size(Width, Height);
        TabIndex = 0;
            
        BackColor = Color.Aqua;
        Cursor = Cursors.Arrow;

        Font = new Font("DejaVu Serif", 20.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
        
        Type = CellType.Empty;

        Selected = false;
    }

    public new Response Select()
    {
        if (State == GameState.Creating)
        {
            if (Type != CellType.Empty)
            {
                return new Response(100, "Эту клетку нельзя выбрать");
            }

            Selected = !Selected;

            UpdateCellImage();

            return new Response(200);
        }
        
        // TODO //

        return new Response(100, "Часть кода не написана");
    }

    public void ChangeType(CellType type)
    {
        Type = type;

        UpdateCellImage();
    }

    public void UpdateCellImage()
    {
        if (Selected)
        {
            if (Type == CellType.Empty)
            {
                BackColor = Color.Blue;
            }
        }
        else
        {
            if (Type == CellType.Empty)
            {
                BackColor = Color.Aqua;
            }
            else if (Type == CellType.ShipAlive)
            {
                if (State == GameState.Creating)
                {
                    BackColor = Color.SaddleBrown;
                }
                else
                {
                    BackColor = Color.Aqua;
                }
            }
            else if (Type == CellType.ShipInjured)
            {
                BackColor = Color.Tomato;
            }
            else if (Type == CellType.ShipDead)
            {
                BackColor = Color.Black;
            }
            else if (Type == CellType.Miss)
            {
                BackColor = Color.LightSteelBlue;
            }
        }
    }

    public void RollbackSelection()
    {
        Selected = false;
        
        UpdateCellImage();
    }
}