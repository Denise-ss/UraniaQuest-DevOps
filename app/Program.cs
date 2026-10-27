
using System.Text.Json;
using UraniaQuest.Solucoes.Models;
using UraniaQuest.Solucoes.Repositorios;

// Lista que guarda as solucoes cientificas
List<SolucaoCientifica> solucoes = new List<SolucaoCientifica>();

// Cria o repositorio responsavel pelo arquivo JSON
RepositorioSolucoes repositorio = new RepositorioSolucoes();

// Controla se o arquivo pode ser utilizado com seguranca
bool arquivoDisponivel = true;

// Tenta carregar as solucoes salvas anteriormente
try
{
    solucoes = repositorio.Carregar();

    Console.WriteLine($"Solucoes carregadas: {solucoes.Count}");
}
catch (JsonException ex)
{
    arquivoDisponivel = false;
    Console.WriteLine($"Erro no formato do JSON: {ex.Message}");
}
catch (InvalidDataException ex)
{
    arquivoDisponivel = false;
    Console.WriteLine($"Erro nos dados do arquivo: {ex.Message}");
}
catch (IOException ex)
{
    arquivoDisponivel = false;
    Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
}
catch (UnauthorizedAccessException ex)
{
    arquivoDisponivel = false;
    Console.WriteLine($"Acesso negado ao arquivo: {ex.Message}");
}

// Variavel que guarda a opcao escolhida pelo usuario
int opcao = -1;

// Mantem o menu funcionando enquanto o usuario nao escolher sair
while (opcao != 0)
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("            URANIAQUEST");
    Console.WriteLine("  Gerenciamento de Solucoes Cientificas");
    Console.WriteLine("====================================");
    Console.WriteLine("1 - Cadastrar solucao cientifica");
    Console.WriteLine("2 - Listar solucoes cadastradas");
    Console.WriteLine("3 - Buscar solucoes por categoria");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("====================================");

    Console.Write("Escolha uma opcao: ");

    // Verifica se o usuario digitou um numero valido
    if (!int.TryParse(Console.ReadLine(), out opcao))
    {
        Console.WriteLine("Digite um numero valido!");
        opcao = -1;
        continue; // Volta ao inicio do menu
    }

    // Executa uma acao conforme a opcao escolhida
    switch (opcao)
    {
        case 1:
            CadastrarSolucao();
            break;

        case 2:
            ListarSolucoes();
            break;

        case 3:
            BuscarSolucoesPorCategoria();
            break;

        case 0:
            Console.WriteLine("Encerrando o UraniaQuest...");
            break;

        default:
            Console.WriteLine("Opcao invalida! Tente novamente.");
            break;
    }

    // ============================================
    // METODO 1 - CADASTRAR SOLUCAO CIENTIFICA
    // ============================================

    void CadastrarSolucao()
    {

        if (!arquivoDisponivel)
        {
            Console.WriteLine("Cadastro bloqueado: verifique o arquivo JSON.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== CADASTRO DE SOLUCAO CIENTIFICA ===");

        Console.Write("Codigo: ");

        // Verifica se o codigo e um numero maior que zero
        if (!int.TryParse(Console.ReadLine(), out int codigo) || codigo <= 0)
        {
            Console.WriteLine("Codigo invalido!");
            return;
        }

        // Verifica se ja existe uma solucao com esse codigo
        foreach (SolucaoCientifica item in solucoes)
        {
            if (item.Codigo == codigo)
            {
                Console.WriteLine("Ja existe uma solucao com esse codigo!");
                return;
            }
        }

        // Le os campos obrigatorios do cadastro
        string nome = LerCampoObrigatorio("Nome");

        // Permite somente as cinco categorias do UraniaQuest
        string categoria = LerCategoriaValida();

        string problema = LerCampoObrigatorio("Problema");

        string aplicacao = LerCampoObrigatorio("Aplicacao");

        string instituicao = LerCampoObrigatorio("Instituicao");

        string referencia = LerCampoObrigatorio("Referencia");

        // Verifica se o link possui um formato valido
        string link = LerLinkValido();

        // Cria um objeto usando a classe SolucaoCientifica
        SolucaoCientifica novaSolucao = new SolucaoCientifica
        {
            Codigo = codigo,
            Nome = nome,
            Categoria = categoria,
            Problema = problema,
            Aplicacao = aplicacao,
            Instituicao = instituicao,
            Referencia = referencia,
            Link = link
        };
        
        // Adiciona a nova solucao na lista
        solucoes.Add(novaSolucao);

        try
        {
            // Salva a lista atualizada no arquivo JSON
            repositorio.Salvar(solucoes);

            Console.WriteLine();
            Console.WriteLine("Solucao cadastrada e salva com sucesso!");
        }
        catch (IOException ex)
        {
            // Desfaz a inclusao na lista se nao conseguir salvar
            solucoes.Remove(novaSolucao);

            Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            solucoes.Remove(novaSolucao);

            Console.WriteLine($"Acesso negado ao arquivo: {ex.Message}");
        }

    }

    // ============================================
    // METODO 2 - LISTAR SOLUCOES CADASTRADAS
    // ============================================

    void ListarSolucoes()
    {
        Console.WriteLine();
        Console.WriteLine("=== SOLUCOES CIENTIFICAS CADASTRADAS ===");

        // Verifica se a lista esta vazia
        if (solucoes.Count == 0)
        {
            Console.WriteLine("Nenhuma solucao cadastrada.");
            return;
        }

        // Percorre todas as solucoes cadastradas
        foreach (SolucaoCientifica item in solucoes)
        {
            Console.WriteLine();
            Console.WriteLine($"Codigo: {item.Codigo}");
            Console.WriteLine($"Nome: {item.Nome}");
            Console.WriteLine($"Categoria: {item.Categoria}");
            Console.WriteLine($"Problema: {item.Problema}");
            Console.WriteLine($"Aplicacao: {item.Aplicacao}");
            Console.WriteLine($"Instituicao: {item.Instituicao}");
            Console.WriteLine($"Referencia: {item.Referencia}");
            Console.WriteLine($"Link: {item.Link}");
            Console.WriteLine("------------------------------------");
        }
    }

    // ============================================
    // METODO 3 - BUSCAR POR CATEGORIA
    // ============================================

    void BuscarSolucoesPorCategoria()
    {
        Console.WriteLine();
        Console.WriteLine("=== BUSCAR SOLUCOES POR CATEGORIA ===");

        // Verifica se existe alguma solucao cadastrada
        if (solucoes.Count == 0)
        {
            Console.WriteLine("Nenhuma solucao cadastrada.");
            return;
        }

        Console.Write("Digite a categoria: ");

        // Remove espacos extras da categoria digitada
        string categoriaBusca = (Console.ReadLine() ?? "").Trim();

        // Impede a pesquisa com categoria vazia
        if (categoriaBusca == "")
        {
            Console.WriteLine("A categoria nao pode ficar vazia!");
            return;
        }

        // Controla se alguma solucao foi encontrada
        bool encontrou = false;

        // Percorre as solucoes procurando pela categoria
        foreach (SolucaoCientifica item in solucoes)
        {
            // ToUpper permite comparar letras maiusculas e minusculas
            if (item.Categoria.Trim().ToUpper() == categoriaBusca.ToUpper())
            {
                Console.WriteLine();
                Console.WriteLine($"Codigo: {item.Codigo}");
                Console.WriteLine($"Nome: {item.Nome}");
                Console.WriteLine($"Categoria: {item.Categoria}");
                Console.WriteLine($"Problema: {item.Problema}");
                Console.WriteLine($"Aplicacao: {item.Aplicacao}");
                Console.WriteLine($"Instituicao: {item.Instituicao}");
                Console.WriteLine($"Referencia: {item.Referencia}");
                Console.WriteLine($"Link: {item.Link}");
                Console.WriteLine("------------------------------------");

                encontrou = true;
            }
        }

        // Informa quando nao existem solucoes na categoria
        if (!encontrou)
        {
            Console.WriteLine("Nenhuma solucao encontrada nessa categoria.");
        }
    }

    // ============================================
    // METODO 4 - VALIDAR CAMPOS OBRIGATORIOS
    // ============================================

    string LerCampoObrigatorio(string campo)
    {
        string valor = "";

        // Repete a pergunta enquanto o campo estiver vazio
        while (valor == "")
        {
            Console.Write($"{campo}: ");

            // Le o valor e remove os espacos extras
            valor = (Console.ReadLine() ?? "").Trim();

            if (valor == "")
            {
                Console.WriteLine("Este campo nao pode ficar vazio!");
            }
        }

        // Devolve o valor preenchido
        return valor;
    }

    // ============================================
    // METODO 5 - VALIDAR CATEGORIAS
    // ============================================

    string LerCategoriaValida()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Categorias disponiveis:");
            Console.WriteLine("ALIMENTACAO");
            Console.WriteLine("SAUDE");
            Console.WriteLine("ENERGIA");
            Console.WriteLine("INDUSTRIA");
            Console.WriteLine("AMAZONIA_AZUL");

            // Le a categoria e converte para letras maiusculas
            string categoria = LerCampoObrigatorio("Categoria").ToUpper();

            // Verifica se pertence aos cinco setores do UraniaQuest
            if (categoria == "ALIMENTACAO" ||
                categoria == "SAUDE" ||
                categoria == "ENERGIA" ||
                categoria == "INDUSTRIA" ||
                categoria == "AMAZONIA_AZUL")
            {
                return categoria;
            }

            Console.WriteLine("Categoria invalida! Escolha uma das opcoes.");
        }
    }

    // ============================================
    // METODO 6 - VALIDAR LINK CIENTIFICO
    // ============================================

    string LerLinkValido()
    {
        while (true)
        {
            // Solicita o endereco da referencia cientifica
            string link = LerCampoObrigatorio("Link");

            // Verifica se e um endereco completo com HTTP ou HTTPS
            if (Uri.TryCreate(link, UriKind.Absolute, out Uri? endereco) &&
                endereco != null &&
                (endereco.Scheme == "http" || endereco.Scheme == "https"))
            {
                return link;
            }

            Console.WriteLine("Link invalido! Informe um endereco com http:// ou https://.");
        }
    }
}
