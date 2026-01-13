using Ecom.API.Middleware;
using Ecomm.infrastructure;
using Ecomm.infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCors(op =>
{
    op.AddPolicy("CORSPolicy",
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
        });
});
builder.Services.AddControllers();
builder.Services.AddMemoryCache();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.infrastructureConfiguration(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = string.Empty; 
    });
}
app.UseStaticFiles();
app.UseCors("CORSPolicy");
app.UseMiddleware<ExceptionsMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
