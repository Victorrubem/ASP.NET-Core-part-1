using CasaDoCodigo.Models;
using System.Collections.Generic;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        IList<Pedido> GetPedidos();
        Pedido GetPedido();
        void AddItem(string codigo);
    }
}