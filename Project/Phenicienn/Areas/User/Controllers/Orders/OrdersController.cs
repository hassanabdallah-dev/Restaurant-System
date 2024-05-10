using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.User.Controllers.Orders
{
    /*[Route("api/[controller]")]
    [ApiController]*/
    [Area("User")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public void Send(List<int> B, List<int> A, string remarque)

        {
            
            int i = 0;
            float sum = 0;
            var items = _context.Item.Where(m => B.Contains(m.ItemId)).ToList();
            for (i=0;i<B.Count;i++)
            {
                var item = items.Where(m => m.ItemId == B[i]).FirstOrDefault();
                float price = item.Price;
                sum += price * A[i];
            }
            Order order = new Order();
            order.Commment = remarque;
            order.Date = DateTime.Now;
            order.TotalPrice = sum;
            order.Status = 0;
            var tableID= HttpContext.Session.GetInt32("table_id");
            table tablee = _context.tables.FirstOrDefault(m=>m.id == tableID);

            order.table = tablee;
            order.session = tablee.session;
            order.OrdersItems =new List<OrdersItems>();
            for (i=0;i<B.Count;i++)
            {
                var item = items.Where(m => m.ItemId == B[i]).FirstOrDefault();
                OrdersItems orderItems = new OrdersItems();
                orderItems.Item = item;
                orderItems.Quantity = A[i];
                order.OrdersItems.Add(orderItems);
            }
            _context.Orders.Add(order);
            var billexists = _context.Bills.Any(m => m.session == tablee.session);
            if (!billexists)
            {
                Bills bills = new Bills() { 
                    session = tablee.session,
                    Table = tablee,
                    Amount = order.TotalPrice,
                    Paid = false,
                };
                _context.Bills.Add(bills);
            }
            else
            {
                Bills bills = _context.Bills.Where(m => m.session == tablee.session).FirstOrDefault();
                bills.Amount += order.TotalPrice;
                _context.Bills.Update(bills);
            }
            _context.SaveChanges();
        }
    }
}