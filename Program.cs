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
    return String.Format("{0} - {1}", product.Code, product.Name);
});

app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
    return code;
});

app.MapGet("/getproductinheader", (HttpRequest request) => {
    var headers = request.Headers;
    return headers["product-code"].ToString();
});

app.Run();

public class Product {
    public string Name { get; set; }
    public string Code { get; set; }
}