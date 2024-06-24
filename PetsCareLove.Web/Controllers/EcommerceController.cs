using Microsoft.AspNetCore.Mvc;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Controllers
{
    public class EcommerceController : Controller
    {
        private readonly EcommerceService _ecommerceService;
        private readonly PetService _petService;

        public EcommerceController(EcommerceService ecommerceService, PetService petService)
        {
            _ecommerceService = ecommerceService;
            _petService = petService;

        }

        public async Task<IActionResult> Index(string animal)
        {
            // retrieve all products
            var allProducts = await _ecommerceService.GetAllProductsAsync();
            var allCategories = await _ecommerceService.GetAllCategoriesAsync();
            // retrive all type of animals
            var typeAnimal = await _petService.GetAllTypeAnimalAsync();

            // retrieve Ids from Category
            var acessorios = allCategories.Where(c => c.Name == "Acessorios").Select(i => i.Id).FirstOrDefault();
            var higiene = allCategories.Where(c => c.Name == "Higiene").Select(i => i.Id).FirstOrDefault();
            var alimentos = allCategories.Where(c => c.Name == "Alimentos").Select(i => i.Id).FirstOrDefault();

            if (!string.IsNullOrEmpty(animal))
            {
                if (animal.ToLower() == "cão")
                {
                    var idAnimalDog = typeAnimal.Where(p => p.Name == "Cão").Select(i => i.Id).FirstOrDefault();
                    allProducts = allProducts.Where(p => p.PetId == idAnimalDog).ToList();
                }
                else if (animal.ToLower() == "gato")
                {
                    var idAnimalCat = typeAnimal.Where(p => p.Name == "Gato").Select(i => i.Id).FirstOrDefault();
                    allProducts = allProducts.Where(p => p.PetId == idAnimalCat).ToList();
                }
            }
            ViewBag.Animal = animal;
            ViewBag.Acessorios = acessorios;
            ViewBag.Higiene = higiene;
            ViewBag.Alimentos = alimentos;
            return View(allProducts);
        }

        public async Task<IActionResult> DetailsProduct(Guid id)
        {
            var product = await _ecommerceService.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var product = await _ecommerceService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Simples estrutura de armazenamento de carrinho na sessão
            var cart = HttpContext.Session.Get<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            cart.Add(product);
            HttpContext.Session.Set("Cart", cart);

            return Json(new { count = cart.Count });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid id)
        {
            // Lógica para remover o produto do carrinho na sessão
            var cart = HttpContext.Session.Get<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();

            // Encontra o produto pelo ID e remove
            var productToRemove = cart.FirstOrDefault(p => p.Id == id);
            if (productToRemove != null)
            {
                cart.Remove(productToRemove);
                HttpContext.Session.Set("Cart", cart);
            }

            return Ok(); // Retorna um status de sucesso
        }

        public async Task<IActionResult> ShoppCart()
        {
            var cart = HttpContext.Session.Get<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            return View(cart);
        }
    }
}
