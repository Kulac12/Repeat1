﻿using Business.Abstract;
using Business.Constants;
using Business.CSS;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;


        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }

   


        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //Bir ürün eklemek istersen, eklemek istediğin ürünün kategorisinde maximum 10 ürün olabilir. 
            //business codes
           IResult result= BusinessRules.Run(
                              CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                              CheckIfProductNameBenzersiz(product.ProductName),
                              CheckCategoryCount());
            if (result != null)
            {
                return result;
            }
           
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }



        public IDataResult<List<Product>> GetAll()
        {
            //iş kodları
            //Yetkisi var mı?
            //Bana ürünleri verebilirsin çünkü 
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.Bakimda);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id), Messages.ProductsListed);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice <= min && p.UnitPrice <= max), Messages.ProductsListed);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductDetail);
        }

        [ValidationAspect(typeof(ProductValidator))]

        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        //***********************************************************************************


        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameBenzersiz(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            //Any varsa true yoksa false döndürür.
            if (result)
            {
                return new ErrorResult(Messages.SameProductNameTrue);
            }
            return new SuccessResult();
        }
  
        private IResult CheckCategoryCount()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitedExceded);
            }
            return new SuccessResult();
        }
    
    
    }
}
