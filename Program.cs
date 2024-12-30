using growgreen_backend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CropRepository>();
builder.Services.AddScoped<PesticideRepository>();
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<CropsTransactionRepository>();
builder.Services.AddScoped<PesticidesConfirmRepository>();
builder.Services.AddScoped<PesticidesTransactionRepository>();
builder.Services.AddScoped<FAQRepository>();
//builder.Services.AddScoped<ContactRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
