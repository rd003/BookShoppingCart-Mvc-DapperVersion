using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories;

public interface IGenreRepository
{
    Task AddGenre(Genre genre);
    Task UpdateGenre(Genre genre);
    Task<Genre?> GetGenreById(int id);
    Task DeleteGenre(Genre genre);
    Task<IEnumerable<Genre>> GetGenres();
}
public class GenreRepository : IGenreRepository
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public GenreRepository(IConfiguration configuration)
    {
        _config = configuration;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    public async Task AddGenre(Genre genre)
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string query = @"
            INSERT INTO Genre (GenreName)
            VALUES (@GenreName);
         ";
        await connection.ExecuteAsync(query, genre);
    }
    public async Task UpdateGenre(Genre genre)
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string query = @"
          UPDATE Genre 
          SET GenreName= @GenreName
          WHERE Id= @Id;
        ";
        await connection.ExecuteAsync(query, genre);
    }

    public async Task DeleteGenre(Genre genre)
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string query = @"
         DELETE FROM Genre
         WHERE Id = @Id;
        ";
        await connection.ExecuteAsync(query, new { genre.Id });
    }

    public async Task<Genre?> GetGenreById(int id)
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string query = @"
          select
           g.Id,
           g.GenreName 
          from Genre g
          where Id=@id;
        ";

        Genre? genre = await connection.QueryFirstOrDefaultAsync<Genre>(query, new { id });
        return genre;
    }

    public async Task<IEnumerable<Genre>> GetGenres()
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string query = @"
            select 
              g.Id,
              g.GenreName
            from Genre g
            order by 
              g.GenreName asc;  
        ";
        IEnumerable<Genre> genres = await connection.QueryAsync<Genre>(query);
        return genres;
    }


}
