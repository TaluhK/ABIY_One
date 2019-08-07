using ABIY_One.Models;
using ABIY_One.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ABIY_One.ABIY_Business_Logic
{
    public class Product_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Product> all()
        {
            return db.Products.Include(i => i.Category).ToList();
        }
        public bool add(Product model)
        {
            try
            {
                var item = db.Products.Where(x => x.Category_ID == model.Category_ID && x.Description == model.Description && x.Name == model.Name).FirstOrDefault();
                if (item != null)
                {
                    item.QuantityInStock += model.QuantityInStock;
                    item.Picture = model.Picture;
                    item.Price = model.Price;
                    item.MarkUpPercentage = model.MarkUpPercentage;
                    item.CostPrice = model.CostPrice;
                    item.ExpectedProfit = model.ExpectedProfit;
                    //db.SaveChanges();
                }
                else
                {
                    db.Products.Add(model);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Product model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool delete(Product model)
        {
            try
            {
                db.Products.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Product find_by_id(int? id)
        {
            return db.Products.Find(id);
        }
        //public List<StockCart_Item> get_cart_items(int id)
        //{
        //    //return db.StockCart_Items.
        //}


        public void updateStock_Received(int item_id, int quantity)
        {
            var item = db.Products.Find(item_id);
            item.QuantityInStock += quantity;
            db.SaveChanges();
        }
        public void updateOrder(int id, double price)
        {
            var item = db.Order_Items.Find(id);
            item.price = price;
            item.replied = true;
            item.date_replied = DateTime.Now;
            item.status = "Supplier Replied with Pricing Details";
            db.SaveChanges();
        }
    }
}