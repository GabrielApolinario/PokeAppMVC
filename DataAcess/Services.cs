using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using PokeAppMVC.Models;
using System.Net.Http.Headers;

namespace PokeAppMVC.DataAcess
{
    public class Services
    {
        HttpClient client = new HttpClient();
        private readonly Uri _baseUri = new Uri("https://pokeapi.co/api/v2/");
        public Services()
        {
            client.BaseAddress = _baseUri;
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<ResourceList> GetPokemonListAsync()
        {   
            HttpResponseMessage response = await client.GetAsync("pokemon?offset=0&limit=100");
            if (response.IsSuccessStatusCode)
            {
                var jsonList = await response.Content.ReadAsStringAsync();
                var deserializedList = JsonSerializer.Deserialize<ResourceList>(jsonList);
                return deserializedList;
            }
            return new ResourceList();
        }

        public async Task<PokemonDetail> GetPokemonByIdAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonList = await response.Content.ReadAsStringAsync();
                var deserializedList = JsonSerializer.Deserialize<PokemonDetail>(jsonList);
                return deserializedList;
            }
            return new PokemonDetail();
        }

        public async Task<Move> GetMovesByIdAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonList = await response.Content.ReadAsStringAsync();
                var deserializedList = JsonSerializer.Deserialize<Move>(jsonList);
                return deserializedList;
            }
            return new Move();
        }
    }
}
