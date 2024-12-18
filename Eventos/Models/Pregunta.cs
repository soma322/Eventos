namespace Eventos.Models
{
    public class Pregunta
    {
        public int Id { get; set; } // Clave primaria
        public string base32 { get; set; }

        public DateTime FechaDeCreacion { get; set; } // Nueva propiedad

    }
}
