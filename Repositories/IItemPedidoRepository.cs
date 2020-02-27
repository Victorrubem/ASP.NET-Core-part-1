using CasaDoCodigo.Models;
using System.Collections.Generic;

namespace CasaDoCodigo.Repositories
{
    public interface IItemPedidoRepository
    {
        IList<ItemPedido> GetItemPedidos();
    }
}