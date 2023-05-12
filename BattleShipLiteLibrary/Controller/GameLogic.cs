using BattleShipLiteLibrary.Model;
using BattleShipLiteLibrary.View;


namespace BattleShipLiteLibrary.Controller;

public static class GameLogic
{
	public static PlayerInfoModel CreatePlayer(string playerTitle)
	{
		Console.WriteLine($"Player information for {playerTitle}");
		
		PlayerInfoModel output = new PlayerInfoModel();
		// ask user for their name
		output.UserName = UserMessages.GetUsername();
		// load up the shot grid
		InitializeGrid(output);
		// ask user for their 5 ship placements
		GetShipCoordinates(output);
		// Clear
		Console.Clear();
		
		return output;
	}

	private static void InitializeGrid(PlayerInfoModel model)
	{
		List<string> letters = new List<string>
		{
			"A", "B", "C", "D", "E"
		};

		List<int> numbers = new List<int>
		{
			1, 2, 3, 4, 5
		};

		foreach (string letter in letters)
		{
			foreach (int number in numbers)
			{
				AddGridCoordinate(model, letter, number);
			}
		}
	}

	private static void AddGridCoordinate(PlayerInfoModel model, string letter, int number)
	{
		GridCoordinateModel coordinate = new GridCoordinateModel();
		coordinate.CoordinateLetter = letter;
		coordinate.CoordinateNumber = number;
		coordinate.Status = Constant.GridCoordinateStatus.Empty;
		
		model.ShotGrid.Add(coordinate);
	}
	
	private static void GetShipCoordinates(PlayerInfoModel model)
	{
		do
		{
			string coordinate = UserMessages.AskForShipPlacement(model);
			if (coordinate.Length != 2)
			{
				UserMessages.InvalidCoordinateEntry();
				continue;
			}
			bool isValidCoordinate = GameLogic.PlaceShip(model, coordinate);

			if (isValidCoordinate == false)
			{
				UserMessages.InvalidCoordinateEntry();
			}
		} while (model.ShipLocations.Count < 5);
	}

	public static bool PlaceShip(PlayerInfoModel model, string coordinate)
	{
		bool canPlaceShip = false;
		(string row, int column) = SplitCoordinateEntry(coordinate);
		row = row.ToUpper();

		bool isValidCoordinate = GameLogic.ValidateCoordinateEntry(model, row, column);
		bool isCoordinateAvailable = GameLogic.ValidateShipCoordinateEntry(model, row, column);

		if (isValidCoordinate && isCoordinateAvailable)
		{
			model.ShipLocations.Add(new GridCoordinateModel
			{
				CoordinateLetter	= row,
				CoordinateNumber = column,
				Status = Constant.GridCoordinateStatus.Ship
			});
			canPlaceShip = true;
		}

		return canPlaceShip;
	}

	private static bool ValidateShipCoordinateEntry(PlayerInfoModel model, string shipEntryRow, int shipEntryColumn)
	{
		bool isValidLocation = true;

		foreach (var ship in model.ShipLocations)
		{
			if (ship.CoordinateLetter == shipEntryRow.ToUpper() && ship.CoordinateNumber == shipEntryColumn)
			{
				isValidLocation = false;
			}
		}
		return isValidLocation;
	}

	private static bool ValidateCoordinateEntry(PlayerInfoModel playerGridToValidate, string coordinateRow, int coordinateColumn)
	{
		bool isValidCoordinate = false;
		

		foreach (var coordinate in playerGridToValidate.ShotGrid)
		{
			if (coordinate.CoordinateLetter == coordinateRow.ToUpper() && coordinate.CoordinateNumber == coordinateColumn)
			{
				isValidCoordinate = true;
				
			}
		}
		return isValidCoordinate;
	}
	
	private static bool CheckForRepeatShot(PlayerInfoModel opponent, string shotRow, int shotColumn)
	{
		bool isRepeatShot = false;
		
		foreach (var coordinate in opponent.ShotGrid)
		{
			if (coordinate.CoordinateLetter == shotRow.ToUpper() && coordinate.CoordinateNumber == shotColumn)
			{
				if (coordinate.Status == Constant.GridCoordinateStatus.Hit ||
				    coordinate.Status == Constant.GridCoordinateStatus.Miss)
				{
					isRepeatShot = true;
				}
			}
			
		}

		return isRepeatShot;
	}



	public static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
	{
		bool isValidShot = false;
		bool isRepeatShot = false;
		string shotRow = "";
		int shotColumn = 0;
		do
		{
			// display opponent's known grid
			UserMessages.DisplayShotGrid(activePlayer, opponent);
			// ask for shot letter and number
			string playerShot = UserMessages.AskPlayerForShot();

			(shotRow, shotColumn) = GameLogic.SplitCoordinateEntry(playerShot);

			isValidShot = GameLogic.ValidateCoordinateEntry(opponent, shotRow, shotColumn);
			isRepeatShot = GameLogic.CheckForRepeatShot(opponent, shotRow, shotColumn);

			if (isValidShot == false)
			{
				UserMessages.InvalidShot();
			}

			if (isRepeatShot == true)
			{
				UserMessages.RepeatShot();
				isValidShot = false;
			}

			isRepeatShot = false;

		} while (isValidShot == false);

		// determine shot results
		bool isAHit = GameLogic.DetermineShotResult(opponent, shotRow, shotColumn);
		
		// Announce shot result
		Console.Clear();
		UserMessages.AnnounceShotResult(isAHit);
		Console.WriteLine();
		Console.WriteLine();
		
		// Record Results

		GameLogic.MarkShotResult(opponent, shotRow, shotColumn, isAHit);

	}
	

	private static void MarkShotResult(PlayerInfoModel opponent, string shotRow, int shotColumn, bool isAHit)
	{

		foreach (var coordinate in opponent.ShotGrid)
		{
			if (coordinate.CoordinateLetter == shotRow.ToUpper() && coordinate.CoordinateNumber == shotColumn)
			{
				if (isAHit)
				{
					coordinate.Status = Constant.GridCoordinateStatus.Hit;
				}
				else
				{
					coordinate.Status = Constant.GridCoordinateStatus.Miss;
				}
			}
		}

		foreach (var ship in opponent.ShipLocations)
		{
			if (ship.CoordinateLetter == shotRow.ToUpper() && ship.CoordinateNumber == shotColumn)
			{
				ship.Status = Constant.GridCoordinateStatus.Sunk;
			}
		}
	}

	private static bool DetermineShotResult(PlayerInfoModel opponent, string shotRow, int shotColumn)
	{
		bool isAHit = false;

		foreach (var ship in opponent.ShipLocations)
		{
			if (ship.CoordinateLetter == shotRow.ToUpper() && ship.CoordinateNumber == shotColumn)
			{
				isAHit = true;
			}
		}
		return isAHit;
	}

	

	private static (string row, int column) SplitCoordinateEntry(string playerCoordinateEntry)
	{
		string row = "";
		int column = 0;

		char[] coordinateArray = playerCoordinateEntry.ToArray();

		row = coordinateArray[0].ToString();
		int.TryParse(coordinateArray[1].ToString(), out column);

		return (row, column);
	}

	public static bool PlayerStillActive(PlayerInfoModel player)
	{
		bool isActive = false;
		foreach (var ship in player.ShipLocations)
		{
			if (ship.Status != Constant.GridCoordinateStatus.Sunk)
			{
				isActive = true;
			}
		}
		return isActive;
	}
	

	public static int GetShotCount(PlayerInfoModel player)
	{
		int shotCount = 0;

		foreach (var shot in player.ShotGrid)
		{
			if (shot.Status != Constant.GridCoordinateStatus.Empty)
			{
				shotCount += 1;
			}
		}
		return shotCount;
	}
}