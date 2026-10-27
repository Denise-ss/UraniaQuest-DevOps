
using System.Text.Json;
using UraniaQuest.Solucoes.Models;
namespace UraniaQuest.Solucoes.Repositorios
{
    public class RepositorioSolucoes
    {
        // Define o nome e o local do arquivo JSON
        private readonly string caminhoArquivo =
            Path.Combine(AppContext.BaseDirectory, "solucoes.json");

        // Carrega as solucoes que foram salvas anteriormente
        public List<SolucaoCientifica> Carregar()
        {
            // Se o arquivo ainda nao existe, retorna uma lista vazia
            if (!File.Exists(caminhoArquivo))
            {
                return new List<SolucaoCientifica>();
            }

            // Le o conteudo do arquivo
            string json = File.ReadAllText(caminhoArquivo);

            // Verifica se o arquivo esta vazio
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidDataException("O arquivo JSON esta vazio.");
            }

            // Converte o JSON em uma lista de solucoes
            List<SolucaoCientifica>? lista =
                JsonSerializer.Deserialize<List<SolucaoCientifica>>(json);

            if (lista == null)
            {
                throw new InvalidDataException("O arquivo JSON nao contem uma lista valida.");
            }

            return lista;
        }

        // Salva as solucoes no arquivo JSON
        public void Salvar(List<SolucaoCientifica> solucoes)
        {
            // Organiza o JSON para facilitar a leitura
            JsonSerializerOptions opcoes = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Converte a lista de objetos em texto JSON
            string json = JsonSerializer.Serialize(solucoes, opcoes);

            // Grava o texto no arquivo
            File.WriteAllText(caminhoArquivo, json);
        }
    }
}
