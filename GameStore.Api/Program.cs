using GameStore.Api.Dtos;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<GameDto> games =
[
    new(1, "Game1", "Genre1", 100M, new DateOnly(2026, 12, 13)),
    new(2, "Game2", "Genre2", 200M, new DateOnly(2026, 12, 14)),
    new(3, "Game3", "Genre3", 300M, new DateOnly(2026, 12, 15))
];

//GET all games
app.MapGet("games", () => games);


//GET get game by id
const string getGameName = "GetGame";

app.MapGet("games/{id:int}", (int id) =>
{
    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);

}).WithName(getGameName);


//POST add new game
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(getGameName, new { id = game.Id }, game);

});


//PUT update the game
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{

    var index = games.FindIndex(game => game.Id == id);
    games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);

    return Results.NoContent();

});


//DELETE game by id
app.MapDelete("games/{id}", (int id) =>
{
    _ = games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
