

namespace BattleShipLiteLibrary.Model;

public class GridCoordinateModel
{
	public string CoordinateLetter { get; set; }
	public int CoordinateNumber { get; set; }
	public Constant.GridCoordinateStatus Status { get; set; } = Constant.GridCoordinateStatus.Empty;
}