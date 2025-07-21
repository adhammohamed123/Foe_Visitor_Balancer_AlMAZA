using Core.Entities;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Threading.Tasks;

namespace Repository
{
	public class CardRepo : BaseRepository<Card>, ICardRepo
	{
		public CardRepo(FoeVisitContext context) : base(context)
		{
			
		}

		public void CheckInCardForVisit(Card card)
		=> card.CardStatus = Core.Entities.Enum.CardState.NotAvailable;

		public async Task CreateCards(IEnumerable<Card> cards)
		=> await context.Cards.AddRangeAsync(cards);


		public async Task CreateNewCard(Card card, long floorId)
		{
			card.FloorId = floorId;
			await Create(card);
		}
		public bool ChackExistanceDeptWithTheSameNumberInSameFloor(string cardNumber, long floorId)
		  => FindByCondition(c => c.CardNumber.ToLower().Equals(cardNumber.ToLower().Trim()) && c.FloorId == floorId, false).Any();

		public IQueryable<Card> GetAllCardsAvalibaleInFloor(long floorId, DateTime In, bool trackchanges)
		=> FindByCondition(c => c.FloorId.Equals(floorId) && c.CardStatus == Core.Entities.Enum.CardState.Available, trackchanges);

		public IQueryable<Card> GetAllCardsInFloor(long floorId, bool trackchanges)
		  => FindByCondition(c => c.FloorId.Equals(floorId), trackchanges);

		public async Task<Card> GetCardById(long Id, bool tackchanges)
		=> await FindByCondition(c => c.Id.Equals(Id), tackchanges).SingleOrDefaultAsync();

		public void DeleteCard(Card card)
		=> SoftDelete(card);
	}

	//public class VisitorInVisitRepo : BaseRepository<VisitorInVisit>, IVisitorInVisitRepo
	//{
	//	public VisitorInVisitRepo(FoeVisitContext context) : base(context)
	//	{
	//	}

	//	public async Task addVisitorToVisit(VisitorInVisit visitorInVisit)
	//		=> await Create(visitorInVisit);

	//}
}
