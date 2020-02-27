using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor httpContext;
        public PedidoRepository(ApplicationContext contexto, IHttpContextAccessor httpContext) : base(contexto)
        {
            this.httpContext = httpContext;
        }

        public IList<Pedido> GetPedidos()
        {
            return dbSet.ToList();
        }

        /**
         * Obtém o id do pedido que está armazenado na sessão
         */
        public int? GetPedidoIdDaSessao() {
            return httpContext.HttpContext.Session.GetInt32("PedidoId");
        }

        /**
         * Adiciona um id do Pedido na Sessão
         * 
         */
        public void SetPedidoId(int pedidoId) {
            httpContext.HttpContext.Session.SetInt32("PedidoId", pedidoId);
        }

        public Pedido GetPedido()
        {
            var pedidoId = this.GetPedidoIdDaSessao();
            var pedido = dbSet
                .Include(p => p.Itens)                  //Include é como se fosse um inner join, ele irá trazer os itens também
                    .ThenInclude(i => i.Produto)        //ThenInclude é para trazer também o produto do item
                .Where(p => p.Id == pedidoId).SingleOrDefault();

            if (pedido == null) {
                pedido = new Pedido();
                dbSet.Add(pedido);
                contexto.SaveChanges();
                this.SetPedidoId(pedido.Id);
            }
            return pedido;
        }

        public void AddItem(string codigo)
        {
            var produto = contexto.Set<Produto>().Where(p => p.Codigo == codigo).SingleOrDefault();
            if (produto == null) {
                throw new ArgumentException(" Produto não encontrado! ");
            }

            var pedido = this.GetPedido();
            var itemPedido = contexto.Set<ItemPedido>()
                .Where(i => i.Produto.Codigo == codigo 
                    && i.Pedido.Id == pedido.Id).SingleOrDefault();

            if(itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido,produto, 1, produto.Preco);
                contexto.Set<ItemPedido>().Add(itemPedido);
                contexto.SaveChanges();
            }

        }
    }
}
