using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 测试数据，用List模拟数据库中的数据
/// </summary>
namespace lyroge.framework.snapcodes.NOPI
{
    public class DataService
    {
        /// <summary>
        /// 课次列表
        /// </summary>
        public static List<Lesson> GetLessonList()
        {
            var list = new List<Lesson>();
            list.Add(new Lesson() { StudentCode = "BJ01", ClassCode = "C01", RoomName = "中关村V101" });
            list.Add(new Lesson() { StudentCode = "BJ01", ClassCode = "C02", RoomName = "中关村V2" });
            list.Add(new Lesson() { StudentCode = "BJ02", ClassCode = "C03", RoomName = "" });
            list.Add(new Lesson() { StudentCode = "BJ05", ClassCode = "C05", RoomName = "" });
            return list;
        }

        /// <summary>
        /// 课次
        /// </summary>
        public class Lesson
        {
            public string StudentCode { get; set; }
            public string ClassCode { get; set; }
            public string RoomName { get; set; }
        }

        /// <summary>
        /// 产品列表
        /// </summary>
        public static List<Product> GetProductList()
        {
            var list = new List<Product>();
            list.Add(new Product() { ProductId = 1, ProductName = "Surface", SupplierId = 1, ProductTypeId = 3 });
            list.Add(new Product() { ProductId = 2, ProductName = "Windows Phone 7", SupplierId = 1, ProductTypeId = 1 });
            list.Add(new Product() { ProductId = 3, ProductName = "Windows Phone 8", SupplierId = 1, ProductTypeId = 1 });
            list.Add(new Product() { ProductId = 4, ProductName = "iPhone 5", SupplierId = 2, ProductTypeId = 1 });
            list.Add(new Product() { ProductId = 5, ProductName = "Windows 7", SupplierId = 1, ProductTypeId = 5 });
            return list;
        }

        /// <summary>
        /// 分类列表
        /// </summary>
        public static List<ProductType> GetProductTypeList()
        {
            var list = new List<ProductType>();
            list.Add(new ProductType() { TypeId = 1, TypeName = "Phone" });
            list.Add(new ProductType() { TypeId = 2, TypeName = "Game Device" });
            list.Add(new ProductType() { TypeId = 3, TypeName = "Tablet" });
            return list;
        }

        /// <summary>
        /// 厂商列表
        /// </summary>
        public static List<Supplier> GetSupplierList()
        {
            var list = new List<Supplier>();
            list.Add(new Supplier() { SupplierId = 1, SupplierName = "Microsoft" });
            list.Add(new Supplier() { SupplierId = 2, SupplierName = "Apple" });
            list.Add(new Supplier() { SupplierId = 3, SupplierName = "Nokia" });
            list.Add(new Supplier() { SupplierId = 4, SupplierName = "Dell" });
            return list;
        }

        /// <summary>
        /// 产品
        /// </summary>
        public class Product
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int ProductTypeId { get; set; }
            public int SupplierId { get; set; }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public class ProductType
        {
            public int TypeId { get; set; }
            public string TypeName { get; set; }
        }

        /// <summary>
        /// 厂商
        /// </summary>
        public class Supplier
        {
            public int SupplierId { get; set; }
            public string SupplierName { get; set; }
        }
    }
}