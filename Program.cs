using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/", () => new { Name = "Diego Sousa", Age = "25"});
app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Teste", "Diego Sousa");
    return new {Texto = "Olá, olhe os headers"};
    });

app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});


app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);

    if(product != null)
        return Results.Ok(product);

    return Results.NotFound(code);
});

app.MapPut("/products", (Product product) => {
    var p = ProductRepository.GetBy(product.Code);
    p.Name = product.Name;

    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    
    if(product != null)
        ProductRepository.Remove(product);

    return Results.Ok();
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