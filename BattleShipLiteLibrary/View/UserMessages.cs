using BattleShipLiteLibrary.Controller;
using BattleShipLiteLibrary.Model;

namespace BattleShipLiteLibrary.View;

public class UserMessages
{
	public static void WelcomeMessage()
	{
		Console.WriteLine("Welcome to Battleship Lite");
		Console.WriteLine("Created by Jon Arryn");
		Console.WriteLine();
	}

	public static string GetUsername()
	{
		Console.Write("What is your name: ");
		return Console.ReadLine();
	}

	public static string AskForShipPlacement(PlayerInfoModel model)
	{
		Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
		return Console.ReadLine();
		
	}
	
	public static void DisplayShotGrid(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
	
	{
		Console.WriteLine($"{activePlayer.UserName}, It's your turn.");
		Console.WriteLine("Below is your opponent's known grid");
		Console.WriteLine();
		
		string currentRow = opponent.ShotGrid[0].CoordinateLetter;
		
		foreach (var coordinate in opponent.ShotGrid)
		{
			if (coordinate.CoordinateLetter != currentRow)
			{
				Console.WriteLine();
				currentRow = coordinate.CoordinateLetter;
			}
			if (coordinate.Status == Constant.GridCoordinateStatus.Empty)
			{
				Console.Write($" {coordinate.CoordinateLetter}{coordinate.CoordinateNumber} ");
			}
			else if (coordinate.Status == Constant.GridCoordinateStatus.Hit || coordinate.Status == Constant.GridCoordinateStatus.Sunk)
			{
				Console.Write(" X  ");
			}
			else if (coordinate.Status == Constant.GridCoordinateStatus.Miss)
			{
				Console.Write(" O  ");
			}
			else
			{
				Console.Write(" ?  ");
			}
		}

		Console.WriteLine();
		Console.WriteLine();
	}

	public static string AskPlayerForShot()
	{
		Console.Write("Please enter shot coordinates: ");
		return Console.ReadLine();
	}

	public static void AnnounceWinner(PlayerInfoModel winner)
	{
		Console.WriteLine($"Congratulations to {winner.UserName} for winning!");
		Console.WriteLine($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots");
	}

	public static void InvalidShot()
	{
		Console.WriteLine("The Grid Coordinate entry you provided was not found on the opponent's grid. Please make a valid Grid entry");
	}

	public static void AnnounceShotResult(bool isAHit)
	{
		if (isAHit)
		{
			Console.WriteLine("You sunk your opponent's Battleship!");
		}
		else
		{
			Console.WriteLine("You missed, better luck next time.");
		}
	}

	public static void RepeatShot()
	{
		Console.WriteLine("You already entered that shot coordinate. Pick another coordinate to shoot on.");
	}

	public static void InvalidCoordinateEntry()
	{
		Console.WriteLine("That coordinate entry was invalid. Coordinates contain one letter followed by one number.");
	}
}