using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HobitShorten.Models.Dtos
{
    public class URLDto
    {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public string ShortURL { get; set; }
        public string Token { get; set; }
    }
}
