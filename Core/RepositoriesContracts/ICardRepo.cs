using Core.Entities;

namespace Core.RepositoriesContracts
{
    public interface ICardRepo
    {

		IQueryable<Card> GetAllCardsInFloor(long floorId, bool trackchanges);
        IQueryable<Card> GetAllCardsAvalibaleInFloor(long floorId, bool trackchanges);
		Task<Card> GetCardById(long Id, bool tackchanges);
		void CheckInCardForVisit(Card card);
        Task CreateNewCard(Card card, long floorID);
		Task CreateCards(IEnumerable<Card> cards);
		bool ChackExistanceDeptWithTheSameNumberInSameFloor(string cardNumber,long floorId);
		void DeleteCard(Card card);

	}
}

