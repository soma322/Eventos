using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Eventos.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Controllers
{

    [ApiController]
    [Route("api/preguntas")]
    public class PreguntasController: ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        public PreguntasController(HttpClient httpClient, AppDbContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;

        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearPregunta([FromBody] PreguntasModelo token)
        {
            
            if (token == null)
            {
                return BadRequest(new { Message = "Token is null. Please provide a valid token." });
            }

            if (string.IsNullOrEmpty(token.token))
            {
                return BadRequest(new { Message = "Token value is null or empty." });
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "https://sensorapp.somadev.win/auth/generabase32");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
            request.Content = new StringContent("", System.Text.Encoding.UTF8, "application/json");


            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(new { Message = "Failed to fetch data.", Token = token.token });
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var pregunta = new Pregunta
            {
                base32 = jsonResponse,
                FechaDeCreacion = DateTime.UtcNow
            };
//            var preguntas = JsonSerializer.Deserialize<List<Pregunta>>(jsonResponse);

            _dbContext.Preguntas.Add(pregunta);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Base32 string saved successfully.", Base32String = jsonResponse });
        }   

    }
}
