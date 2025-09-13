var builder = DistributedApplication.CreateBuilder(args);
var constr = builder.AddConnectionString("DefaultConnection");
var blazorApp = builder.AddProject<Projects.SamsarPanel>("samsar-panel")
    .WithReference(constr);

builder.Build().Run();
