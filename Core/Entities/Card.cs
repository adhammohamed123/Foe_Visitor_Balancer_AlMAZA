using Core.Contracts;
using Core.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Index(nameof(CardNumber), IsUnique = true)]
	public class Card:FullAduitbaseModel
    {
        [StringLength(100)]
        public string? CardNumber { get; set; }

        public DateTime ReservedIn { get; set; }
        // public CardState CardStatus { get; set; }

        [ForeignKey(nameof(Floor))]
        public long FloorId { get; set; }
        public Floor  Floor { get; set; }

        public ICollection<Visitor> Visitors { get; set; }
    }

}
