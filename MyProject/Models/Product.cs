using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
namespace MyProject.Models {
    public class Product {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public Product() { 
            
        }
        public IEnumerable<SanPham> ListAll(int page, int pageSize) {
            return db.SanPhams.OrderByDescending(sp => sp.MaSP).ToPagedList(page, pageSize);
        }

    }
}