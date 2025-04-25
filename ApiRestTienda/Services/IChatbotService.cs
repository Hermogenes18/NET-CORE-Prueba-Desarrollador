namespace ApiRestTienda.Services
{
    public interface IChatbotService
    {
        Task<string> ProcesarPreguntaAsync(string pregunta);
    }
}
