using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PokeAppMVC.DataAcess;
using System.Dynamic;
using Newtonsoft.Json;

namespace PokeAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Services _dataAcess = new Services();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Pokemon> pokemonList = _dataAcess.GetPokemonListAsync().GetAwaiter().GetResult().PokemonList.OrderBy(v => v.Name).ToList();

            return View(pokemonList);
        }

        [HttpPost]
        public IActionResult PokemonSearch(string pokemonSearched)
        {
            List<Pokemon> pokemonList = new List<Pokemon>();

            if (!string.IsNullOrWhiteSpace(pokemonSearched))
                pokemonList = _dataAcess.GetPokemonListAsync().GetAwaiter().GetResult().PokemonList
                    .Where(p => p.Name.Contains(pokemonSearched))
                    .OrderBy(v => v.Name).ToList().ToList();
            else
                pokemonList = _dataAcess.GetPokemonListAsync().GetAwaiter().GetResult().PokemonList.OrderBy(v => v.Name).ToList();

            return PartialView("_IndexForSearch", pokemonList);
        }

        [HttpPost]
        public PartialViewResult PokemonDetails(string pokemonDetail)
        {
            PokemonDetail details = _dataAcess.GetPokemonByIdAsync(pokemonDetail).GetAwaiter().GetResult();
            List<Ability> abilities = _dataAcess.GetPokemonByIdAsync(pokemonDetail).GetAwaiter().GetResult().Abilities;

            // dynamic multipleModel = new ExpandoObject();
            // multipleModel.Moves = moves;
            // multipleModel.Abilities = abilities;


            return PartialView("_PokemonDetails", details);
        }

        [HttpPost]
        public PartialViewResult PokemonMoves(string movesList)
        {
            List<Move> movesResult = JsonConvert.DeserializeObject<List<Move>>(movesList);

            return PartialView("_PokemonMoves", movesResult);
        }

        [HttpPost]
        public PartialViewResult PokemonAbilities(string abilitiesList)
        {
            List<Ability> abilitiesResult = JsonConvert.DeserializeObject<List<Ability>>(abilitiesList);

            return PartialView("_PokemonAbilities", abilitiesResult);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
