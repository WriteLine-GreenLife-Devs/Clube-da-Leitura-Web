using ClubeDaLeitura.WebApplication.Compartilhado;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using System.Collections.Generic;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura
{
    public class RepositorioRevista : RepositorioBase<Revista>
    {
        public RepositorioRevista(ContextoDados contexto) : base(contexto)
        {
        }

        protected override List<Revista> ObterRegistros()
        {
            return contexto.Revistas;
        }
    }
}