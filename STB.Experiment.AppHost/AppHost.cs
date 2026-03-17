var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres")
	.WithPgWeb();

var db = postgres.AddDatabase("STBExperimentDb");

builder.AddProject<Projects.STB_Experiment_API>("stb-experiment-api")
	.WithReference(db)
	.WaitFor(db);

builder.Build().Run();