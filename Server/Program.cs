using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAzureClients( b =>
{
    //connection string should be part of KeyVault or configuration
    b.AddWebPubSubServiceClient("Endpoint=https://izaan.webpubsub.azure.com;AccessKey=7+rZAXhhRR+eXQqZ3Mby1dNsulbY0JMQIeO2NdZnDXE=;Version=1.0;", "chat");
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();