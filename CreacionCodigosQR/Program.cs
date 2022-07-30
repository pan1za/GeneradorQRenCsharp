using QRCoder;

string policy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy, build =>
    {
        build.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();
app.UseCors(policy);

app.MapGet("/", () => "Hello World!");

app.MapGet("/qr", (string text) =>
{
    var qrGenerator = new QRCodeGenerator();
    var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
    BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeData);
    var bitMap = bitmapByteCode.GetGraphic(20);

    using var memoryStream = new MemoryStream();
    memoryStream.Write(bitMap);
    byte[] byteImage = memoryStream.ToArray();
    return Convert.ToBase64String(byteImage);
});

app.Run();
