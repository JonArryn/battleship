namespace BattleShipLiteLibrary.Model;

public class PlayerInfoModel
{
	public string UserName { get; set; }
	public List<GridCoordinateModel> ShotGrid { get; set; } = new List<GridCoordinateModel>();
	public List<GridCoordinateModel> ShipLocations { get; set; } = new List<GridCoordinateModel>();
}