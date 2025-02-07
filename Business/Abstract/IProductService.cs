using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();

        //void Add(Product product);
        IResult Add(Product product);
        IDataResult<Product> GetById(int productId); // bu aslında şu demek bana id sini verdiğim ilgili
        //ürün hakkındaki bilgileri getir. Geriye sadece bir adet Product nesnesi dönmesi gerek
    }
}
