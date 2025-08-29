using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Services;
using KeeperComparer.Services.Utility;
using KeeperComparer.Validators;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IEmailValidator, EmailValidator>();
builder.Services.AddScoped<IMobileNumberValidator, MobileNumberValidator>();
builder.Services.AddScoped<ILandlineNumberValidator, LandlineNumberValidator>();
builder.Services.AddScoped<IPostcodeValidator, PostcodeValidator>();
builder.Services.AddScoped<IValidator<DateTime>, DateOfBirthValidator>();

builder.Services.AddScoped<ITextComparer, TextComparer>();
builder.Services.AddScoped<IEmailComparer, EmailComparer>();
builder.Services.AddScoped<IPostcodeComparer, PostcodeComparer>();
builder.Services.AddScoped<IAddressComparer, AddressComparer>();
builder.Services.AddScoped<IDateOfBirthComparer, DateOfBirthComparer>();
builder.Services.AddScoped<INumberComparer, MobileNumberComparer>();
builder.Services.AddScoped<NameComparer>();

builder.Services.AddScoped<NumberNormalizer>();
builder.Services.AddScoped<RecordRepository>();

builder.Services.AddScoped<KeeperRecordValidator>();
builder.Services.AddScoped<RecordComparisonService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();