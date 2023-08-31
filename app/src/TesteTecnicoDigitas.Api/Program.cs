namespace TesteTecnicoDigitas.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddServices();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseWebSockets();

            var scope = app.Services.CreateScope();
            var startupBtc = scope.ServiceProvider.GetRequiredService<StartupBtc>();
            var startupEth = scope.ServiceProvider.GetRequiredService<StartupEth>();

            app.Run();
        }
    }
}