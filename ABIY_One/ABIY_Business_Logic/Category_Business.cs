
using ABIY_One.Models;
using ABIY_One.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ABIY_One.ABIY_Business_Logic
{
    public class Category_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Category> all()
        {
            return db.Categories.ToList();
        }
        public bool add(Category model)
        {
            try
            {
                db.Categories.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Category model)
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
        public bool delete(Category model)
        {
            try
            {
                db.Categories.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Category find_by_id(int? id)
        {
            return db.Categories.Find(id);
        }
        public List<Product> category_items(int? id)
        {
            return find_by_id(id).Items.ToList();
        }
    }
}