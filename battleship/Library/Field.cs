using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace battleship.Library;

public class Field
{
    private const int CellHeight = 50;
    private const int CellWidth = 50;
    
    public int X, Y;
    public int Height, Width;
    public List<Cell> Cells = new();

    public PlayerType Owner;

    public Label FieldLabel = new();

    public GameState State;

    public int[] Ships = new int[5] {0, 0, 0, 0, 0};

    public Field(PlayerType owner, GameState state, int x, int y, int height, int width, string labelText)
    {
        Owner = owner;
        State = state;
        
        X = x;
        Y = y;
        Height = height;
        Width = width;

        for (var cellIndex = 0; cellIndex < Height * Width; ++cellIndex)
        {
            var cellRow = cellIndex / Height;
            var cellCol = cellIndex % Height;

            Cells.Add(new Cell(X + cellCol * CellWidth, Y - cellRow * CellHeight,
                cellRow, cellCol, 
                CellHeight, CellWidth,
                Owner, State));
        }

        InitializeLabel(labelText);
    }

    private void InitializeLabel(string labelText)
    {
        FieldLabel.Name = Owner + "FieldLabel";
            
        FieldLabel.Location = new Point(X, Y + 60);
        FieldLabel.Size = new Size(200, 60);
        FieldLabel.TabIndex = 0;
            
        FieldLabel.BackColor = SystemColors.Window;
        FieldLabel.Text = labelText;
    }

    public void Show()
    {
        FieldLabel.Show();
        foreach (var cell in Cells)
        {
            cell.Show();
        }
    }
    
    public void Hide()
    {
        FieldLabel.Hide();
        foreach (var cell in Cells)
        {
            cell.Hide();
        }
    }

    public void Enable()
    {
        foreach (var cell in Cells)
        {
            cell.Selected = false;
            cell.Enabled = true;

            cell.UpdateCellImage();
        }
    }
    
    public void Disable()
    {
        foreach (var cell in Cells)
        {
            cell.Selected = false;
            cell.Enabled = false;

            cell.UpdateCellImage();
        }
    }

    public int GetAmountOfShips()
    {
        return Ships[1] + Ships[2] + Ships[3] + Ships[4];
    }

    private void RollbackSelections()
    {
        foreach (var cell in Cells)
        {
            cell.RollbackSelection();
        }
    }

    private void ApplySelections()
    {
        var selectedCells = Cells.Where(cell => cell.Selected).ToList();

        foreach (var cell in selectedCells)
        {
            cell.Selected = false;
            cell.ChangeType(CellType.ShipAlive);
        }
    }

    private bool IsConnectedGraph(List<Cell> cells)
    {
        if (cells.Count == 0)
        {
            return true;
        }
        
        var queueCells = new Queue<Cell>();
        queueCells.Enqueue(cells[0]);

        var visitedCells = new HashSet<int>();
        visitedCells.Add(cells[0].Row * Height + cells[0].Col);

        while (queueCells.Count > 0)
        {
            var currentCell = queueCells.Dequeue();

            for (var dRow = -1; dRow <= 1; dRow++)
            {
                for (var dCol = -1; dCol <= 1; dCol++)
                {
                    if (Math.Abs(dRow) + Math.Abs(dCol) != 1)
                    {
                        continue;
                    }

                    var nextCellRow = currentCell.Row + dRow;
                    var nextCellCol = currentCell.Col + dCol;

                    if (0 > nextCellRow || nextCellRow >= Height ||
                        0 > nextCellCol || nextCellCol >= Width)
                    {
                        continue;
                    }

                    var nextCellIndex = nextCellRow * Width + nextCellCol;

                    if (!Cells[nextCellIndex].Selected)
                    {
                        continue;
                    }

                    if (visitedCells.Contains(nextCellIndex))
                    {
                        continue;
                    }
                    
                    visitedCells.Add(nextCellIndex);
                    queueCells.Enqueue(Cells[nextCellIndex]);
                }
            }
        }

        return visitedCells.Count == cells.Count;
    }
    
    private bool IsLinear(List<Cell> cells)
    {
        if (cells.Count <= 1)
        {
            return true;
        }

        if (cells[0].Row == cells[1].Row)
        {
            return cells.All(cell => cell.Row == cells[0].Row);
        }

        if (cells[0].Col == cells[1].Col)
        {
            return cells.All(cell => cell.Col == cells[0].Col);
        }

        return false;
    }

    private bool IsNextToAnotherShip(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            for (var dRow = -1; dRow <= 1; dRow++)
            {
                for (var dCol = -1; dCol <= 1; dCol++)
                {
                    if (Math.Abs(dRow) + Math.Abs(dCol) == 0)
                    {
                        continue;
                    }

                    var nextCellRow = cell.Row + dRow;
                    var nextCellCol = cell.Col + dCol;

                    if (0 > nextCellRow || nextCellRow >= Height ||
                        0 > nextCellCol || nextCellCol >= Width)
                    {
                        continue;
                    }

                    var nextCellIndex = nextCellRow * Width + nextCellCol;

                    if (!Cells[nextCellIndex].Selected && Cells[nextCellIndex].Type != CellType.Empty)
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    private List<Cell> GetShipCellsList(Cell cell)
    {
        var queueCells = new Queue<Cell>();
        queueCells.Enqueue(cell);

        var visitedCells = new HashSet<int>();
        visitedCells.Add(cell.Row * Height + cell.Col);

        while (queueCells.Count > 0)
        {
            var currentCell = queueCells.Dequeue();

            for (var dRow = -1; dRow <= 1; dRow++)
            {
                for (var dCol = -1; dCol <= 1; dCol++)
                {
                    if (Math.Abs(dRow) + Math.Abs(dCol) != 1)
                    {
                        continue;
                    }

                    var nextCellRow = currentCell.Row + dRow;
                    var nextCellCol = currentCell.Col + dCol;

                    if (0 > nextCellRow || nextCellRow >= Height ||
                        0 > nextCellCol || nextCellCol >= Width)
                    {
                        continue;
                    }

                    var nextCellIndex = nextCellRow * Width + nextCellCol;

                    if (Cells[nextCellIndex].Type is CellType.Empty or CellType.Miss)
                    {
                        continue;
                    }

                    if (visitedCells.Contains(nextCellIndex))
                    {
                        continue;
                    }
                    
                    visitedCells.Add(nextCellIndex);
                    queueCells.Enqueue(Cells[nextCellIndex]);
                }
            }
        }

        return visitedCells.Select(visitedCellIndex => Cells[visitedCellIndex]).ToList();
    }
    
    public Response TryToCreateShip()
    {   
        var selectedCells = Cells.Where(cell => cell.Selected).ToList();

        if (!IsConnectedGraph(selectedCells))
        {
            RollbackSelections();
            return new Response(100, "Корабль - это связная область клеток размера 1xN, где 1 <= N <= 4");            
        }

        if (!IsLinear(selectedCells))
        {
            RollbackSelections();
            return new Response(100, "Корабль - это связная область клеток размера 1xN, где 1 <= N <= 4");
        }

        var length = selectedCells.Count;

        if (length is < 1 or > 4)
        {
            RollbackSelections();
            return new Response(100, "Длина корабля может быть от 1 до 4");
        }

        if (Ships[length] == 5 - length)
        {
            RollbackSelections();
            return new Response(100, "На поле уже достаточно кораблей длины " + length);
        }

        if (IsNextToAnotherShip(selectedCells))
        {
            RollbackSelections();
            return new Response(100, "Корабли не могут лежать рядом друг с другом");
        }

        Ships[length]++;
        ApplySelections();
        return new Response(200);
    }

    public Response TryToRemoveShip(Cell cell)
    {
        if (cell.Type != CellType.ShipAlive)
        {
            return new Response(100, "Нужно выбрать корабль");
        }

        var shipCells = GetShipCellsList(cell);

        foreach (var shipCell in shipCells)
        {
            shipCell.ChangeType(CellType.Empty);
        }

        Ships[shipCells.Count]--;

        return new Response(200);
    }

    public List<List<Tuple<int, int>>> GetListOfShips()
    {
        var ships = new List<List<Tuple<int, int>>>();

        var visitedCells = new HashSet<int>();
        
        foreach (var cell in Cells)
        {
            if (visitedCells.Contains(cell.Row * Width + cell.Col))
            {
                continue;
            }
            
            var shipCells = GetShipCellsList(cell);

            foreach (var shipCell in shipCells)
            {
                visitedCells.Add(shipCell.Row * Width + shipCell.Col);
            }
            
            ships.Add(shipCells.Select(shipCell => Tuple.Create(shipCell.Row, shipCell.Col)).ToList());
        }
        
        return ships;
    }

    private void MakeMissedNearestCells(Cell cell)
    {
        for (var dRow = -1; dRow <= 1; dRow++)
        {
            for (var dCol = -1; dCol <= 1; dCol++)
            {
                if (Math.Abs(dRow) + Math.Abs(dCol) == 0)
                {
                    continue;
                }
                
                var nextCellRow = cell.Row + dRow;
                var nextCellCol = cell.Col + dCol;

                if (0 > nextCellRow || nextCellRow >= Height ||
                    0 > nextCellCol || nextCellCol >= Width)
                {
                    continue;
                }

                var nextCellIndex = nextCellRow * Width + nextCellCol;

                if (Cells[nextCellIndex].Type == CellType.Empty)
                {
                    Cells[nextCellIndex].ChangeType(CellType.Miss);                    
                }
            }
        }
    }

    public Response TryToShot(Cell cell)
    {
        if (cell.Type != CellType.Empty && cell.Type != CellType.ShipAlive)
        {
            return new Response(100, "Нельзя стрелять в клетку дважды");
        }

        if (cell.Type == CellType.Empty)
        {
            cell.ChangeType(CellType.Miss);
            return new Response(200);
        }
        
        cell.ChangeType(CellType.ShipInjured);

        var shipCellsList = GetShipCellsList(cell);
        var aliveShipCellsList = shipCellsList.Where(shipCell => shipCell.Type == CellType.ShipAlive).ToList();

        if (aliveShipCellsList.Count > 0)
        {
            return new Response(300);
        }
        
        foreach (var shipCell in shipCellsList)
        {
            shipCell.ChangeType(CellType.ShipDead);
            MakeMissedNearestCells(shipCell);
        }

        Ships[shipCellsList.Count]--;

        return GetAmountOfShips() == 0 ? new Response(500) : new Response(400);
    }
}