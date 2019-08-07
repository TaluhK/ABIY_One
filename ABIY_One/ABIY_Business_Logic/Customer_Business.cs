using ABIY_One.Models;
using ABIY_One.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ABIY_One.ABIY_Business_Logic
{
    public class Customer_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Customer> all()
        {
            return db.Customers.ToList();
        }
        public bool add(Customer model)
        {
            try
            {
                db.Customers.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Customer model)
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
        public Customer find_by_id(int? id)
        {
            return db.Customers.Find(id);
        }

        public string getGender(string id_num)
        {
            if (Convert.ToInt16(id_num.Substring(7, 1)) >= 5)
                return "Male";
            else
                return "Female";
        }
    }
}