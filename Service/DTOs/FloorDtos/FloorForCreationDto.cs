using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTOs.CardDtos;

namespace Service.DTOs.FloorDtos
{
    public record FloorForCreationDto
    {
        public string Color { get; set; }
        public string Name { get; set; }
		public int CardsFrom { get; set; }
		public int CardsTo { get; set; }
	}


}
