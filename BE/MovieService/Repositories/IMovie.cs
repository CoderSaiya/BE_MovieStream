﻿using MovieService.Models;

namespace MovieService.Repositories
{
    public interface IMovie
    {
        Task<Movie?> GetMovieAsync(int id, string userId);
        Task<List<Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(int id);
        Task<List<Movie>> GetTrendingMoviesAsync();
        Task<bool> CheckVipStatusAsync(string userId);
    }
}
