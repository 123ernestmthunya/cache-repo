using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Models
{
    public class StarWarsCharacter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Height { get; set; }
        public string? Mass { get; set; }
        public string? HairColor { get; set; }
        public string? SkinColor { get; set; }
        public string? EyeColor { get; set; }
        public string? BirthYear { get; set; }
        public string? Gender { get; set; }
        public string? Homeworld { get; set; }
        public string[]? Films { get; set; }
        public string[]? Starships { get; set; }
        public string[]? Vehicles { get; set; }
        public string[]? Species { get; set; }
        public string? Created { get; set; }
        public string   ? Edited { get; set; }
        public string? Url { get; set; }
    }
}
