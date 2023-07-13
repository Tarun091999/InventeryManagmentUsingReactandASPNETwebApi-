using Inventory.Dal.Repository;
using Inventory.Modals;
using Inventory.Modals.ModalDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace InvoiceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {

        IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {

            var List = _productRepo.GetProducts();
            return Ok(List);

        }


        [HttpPost ("AddProduct")]
        [Authorize (Roles="Admin")]
        public async Task <IActionResult> AddProduct(AddProductDTO product)
        {

            _productRepo.AddProduct(product);
             return Ok("Product Added");    
        }
        [HttpPost("DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id )
        {

            _productRepo.DeleteProduct(id);
            _productRepo.Save();
            return Ok("Product deleted");
        }

       
       
        
        [HttpPost("checkout")]
        public async Task<IActionResult> ReqProduct(RequestOrder rqOrder)
        {
            Order order1 = new Order
            {
                OrderId = Guid.NewGuid(),
                Id = HttpContext.User.FindFirstValue(ClaimTypes.Name),
                Total = rqOrder.Total,
               
            };
            order1.orderDetails = rqOrder.ReqProducts.Select(x => new OrderDetails
            {
                OrderDetailsId = Guid.NewGuid(),
                OrderId = order1.OrderId,
                Product_Id = x.ReqProductId,
                Qty = x.ReqQty
            }).ToList();
            _productRepo.AddOrderCh(order1);
            _productRepo.Save();

            
            
          
            return Ok("done");
        }

        [HttpPost("getinvoices")]
        public async Task <IActionResult> GetInvoices()
        {
            
            var list = _productRepo.GetAllOrder();
            
          
            return Ok(list);

        }
       
        //public async Task<IActionResult> GetCreatedInvoice()
        //{

        //}
    
    }
 }

