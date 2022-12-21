using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/", () => new { Name = "Diego Sousa", Age = "25"});
app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Teste", "Diego Sousa");
    return new {Texto = "Olá, olhe os headers"};
    });

app.MapPost("/SaveProduct", (Product product) => {
    ProductRepository.Add(product);
    return product;
});

app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    return product;
});

app.MapGet("/getproductinheader", (HttpRequest request) => {
    var headers = request.Headers;
    return headers["product-code"].ToString();
});

app.MapPut("/editproduct", (Product product) => {
    var p = ProductRepository.GetBy(product.Code);
    p.Name = product.Name;
});

app.MapDelete("/deleteproduct/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    
    if(product != null)
        ProductRepository.Remove(product);
});

app.Run();

public class Product {
    public string Name { get; set; }
    public string Code { get; set; }
}

public static class ProductRepository {
    private static List<Product> Products { get;set; }

    public static void Add(Product product) {
        if(Products == null)
            Products = new List<Product>();

        Products.Add(product);
    }

    public static Product? GetBy(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product){
        Products.Remove(product);
    }
}