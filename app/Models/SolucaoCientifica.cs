using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UraniaQuest.Solucoes.Models // Organiza nossa classe dentro do projeto
{
    public class SolucaoCientifica //Cria a classe chamada SolucaoCientifica
    {
        public int Codigo { get; set; } //Guarda um número inteiro para identificar a solução

        public string Nome { get; set; } = ""; //Guarda o nome da solução científica

        public string Categoria { get; set; } = ""; //Guarda o setor do UraniaQuest

        public string Problema { get; set; } = ""; //Descreve o problema enfrentado naquele mundo

        public string Aplicacao { get; set; } = ""; //Explica como a tecnologia nuclear pode contribuir

        public string Instituicao { get; set; } = ""; //Identifica a instituição responsável pela fonte

        public string Referencia { get; set; } = ""; //Guarda o título da publicação científica

        public string Link { get; set; } = ""; //Guarda o endereço da referência
    }
}

