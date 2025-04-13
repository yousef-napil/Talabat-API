using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Entities.ProductModule;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public static class DataSeeding
    {
        public static void SeedData(StoreContext dbContext)
        {
            #region ProductBrand DataSeed
            if (!dbContext.ProductBrands.Any())
            {
                // Add your seed data here
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands == null) return;
                dbContext.ProductBrands.AddRange(brands);
                dbContext.SaveChanges();
            }
            #endregion

            #region ProductType DataSeed
            if (!dbContext.ProductTypes.Any())
            {
                // Add your seed data here
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var brands = JsonSerializer.Deserialize<List<ProductType>>(brandsData);
                if (brands == null) return;
                dbContext.ProductTypes.AddRange(brands);
                dbContext.SaveChanges();
            }
            #endregion

            #region Product DataSeed
            if (!dbContext.Products.Any())
            {
                // Add your seed data here
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var brands = JsonSerializer.Deserialize<List<Product>>(brandsData);
                if (brands == null) return;
                dbContext.Products.AddRange(brands);
                dbContext.SaveChanges();
            }
            #endregion

            #region DeliveryMethod DataSeed

            if (!dbContext.DeliveryMethods.Any())
            {
                // Add your seed data here
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (DeliveryMethod == null) return;
                dbContext.DeliveryMethods.AddRange(DeliveryMethod);
                dbContext.SaveChanges();
            }

            #endregion
        }
    }
}
