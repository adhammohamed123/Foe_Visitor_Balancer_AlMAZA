using Core.Contracts;

namespace Core.Entities
{
    public class Floor :FullAduitbaseModel
    {
       
        public string Color { get; set; }
        public string Name { get; set; }
        public int  CardsFrom { get; set; }
        public int  CardsTo { get; set; }
        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
    }

}
