using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppContext>();

var app = builder.Build();
ProductRepository.Init(app.Configuration);

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

if(app.Environment.IsEnvironment("Hom")){
    app.MapGet("/configuration/{configName}", (IConfiguration configuration, [FromRoute] string configName) => {
        return Results.Ok(configuration[configName]);
    });
}

app.Run();

public class Product {
    public string Name { get; set; }
    public string Code { get; set; }
}

public static class ProductRepository {
    private static List<Product> Products { get;set; }  = new List<Product>();

    public static void Init(IConfiguration configuration){
        var products = configuration.GetSection("Products").Get<List<Product>>();
        Products = products;
    }

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

public class AppContext : DbContext {
    
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=<Diego!1996>;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}