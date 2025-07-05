using Core.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.CardDtos
{
    public class CardForReturnDto
    {
        public long Id { get; set; }
        public CardState CardStatus { get; set; }
		public string? CardNumber { get; set; }

	}
}
