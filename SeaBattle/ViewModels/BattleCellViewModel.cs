using SeaBattle.Domain.Models;

namespace SeaBattle.ViewModels;

public class BattleCellViewModel
{
    public FieldType Type { get; set; }

    public Point Point { get; set; }
}