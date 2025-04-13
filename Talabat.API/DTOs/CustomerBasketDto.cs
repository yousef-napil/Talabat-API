﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.BasketModule;

namespace Talabat.API.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
