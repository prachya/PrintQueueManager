using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintQueueManager
{
    using Data;
    public class OrderController
    {
        private dataContext db { get; set; }

        public OrderController(dataContext db)
        {
            this.db = db;  
        }

        public bool SaveOrders(IEnumerable<Order> orders, bool commit = true)
        {
            db.AddRange(orders);
            //foreach (var item in orders) {
            //    db.Add(item);
            //    db.SaveChanges();
            //}
            if(commit)
                db.SaveChanges();

            return true;
        }
    }
}
