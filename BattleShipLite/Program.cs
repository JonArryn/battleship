using BattleShipLiteLibrary;
using BattleShipLiteLibrary.View;
using BattleShipLiteLibrary.Model;
using BattleShipLiteLibrary.Controller;


UserMessages.WelcomeMessage();
PlayerInfoModel activePlayer = GameLogic.CreatePlayer("Player 1");
PlayerInfoModel opponent = GameLogic.CreatePlayer("Player 2");
PlayerInfoModel winner = null;

do
{
	
	
	
	GameLogic.RecordPlayerShot(activePlayer, opponent);
	// Determine if game should continue
	bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

	if (doesGameContinue == true)
	{
		// swap positions
		(activePlayer, opponent) = (opponent, activePlayer);
	}
	else
	{
		winner = activePlayer;
	}
	
} while (winner == null);

UserMessages.AnnounceWinner(winner);
Console.ReadLine();



