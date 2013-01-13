using System;
using System.Linq;
using lyroge.framework.snapcodes.NOPI;

namespace lyroge.framework.snapcodes.Linq
{
    class LeftJoin
    {
        public void Demo()
        {
            var productList = DataService.GetProductList();
            var productTypeList = DataService.GetProductTypeList();
            var supplierList = DataService.GetSupplierList();

            //第1个列表 join
            var list1 = productList
                .Join(productTypeList, x => x.ProductTypeId, y => y.TypeId, (a, b) => new { Product = a, ProductType = b })
                .Join(supplierList, x => x.Product.SupplierId, y => y.SupplierId, (a, b) => new { a.Product.ProductId, a.Product.ProductName, b.SupplierName, a.ProductType.TypeName }).Where(x => x.SupplierName == "Microsoft" && x.TypeName == "Phone").ToList();


            //第2个列表 left join
            var list2 = from product in productList
                        join productType in productTypeList on product.ProductTypeId equals productType.TypeId
                        into joinList
                        from x in joinList.DefaultIfEmpty()
                        select new { ProductName = product.ProductName, TypeName = (x == null ? "未知分类" : x.TypeName) };

        }
    }
}
