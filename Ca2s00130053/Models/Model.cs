﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ca2s00130053.Models
{
    public class MovieDbInitializer : DropCreateDatabaseAlways<MovieDb>
    {
        protected override void Seed(MovieDb context)
        {
            var directors = new List<Director>()
            {
                new Director() {FirstName = "Steven", LastName = "Spielberg"},
                new Director() {FirstName = "George", LastName = "Clooney"},
                new Director() {FirstName = "Michael", LastName = "Bay"},
                new Director() {FirstName = "Ken", LastName = "Loach"},
                new Director() {FirstName = "Frank", LastName = "Darabont"}
            };

            directors.ForEach(dir => context.Directors.Add(dir));
            context.SaveChanges();

            var movies = new List<Movie>()
            {
                new Movie() {Title = "Saving Private Ryan", ReleaseDate = DateTime.Parse("24/07/1998"), 
                    Director = context.Directors.SingleOrDefault(d=>d.FirstName=="Steven"),
                    Genre = Genre.War,
                    Actors = new List<Actor>()
                            {
                                new Actor()
                                {   Name = "Tom Hanks",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Matt Damon",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Vin Diesel",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Bryan Cranston",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Adam Goldberg",
                                    Sex = Sex.Male
                                }
                    }},
                new Movie() {Title = "Gravity", ReleaseDate = DateTime.Parse("03/10/2014"),
                    Director = context.Directors.Where(d=>d.FirstName=="George").FirstOrDefault(),
                    Genre = Genre.SciFi,
                    Actors = new List<Actor>()
                            {
                                new Actor()
                                {   Name = "George Clooney",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Sandra Bullok",
                                    Sex = Sex.Female
                                }
                    }},
               new Movie() {Title = "Transformers", ReleaseDate = DateTime.Parse("3/05/2007"),
                    Director = context.Directors.Where(d=>d.FirstName=="Michael").FirstOrDefault(),
                    Genre = Genre.SciFi,
                    Actors = new List<Actor>()
                            {
                                new Actor()
                                {   Name = "Shia LaBeouf",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   Name = "Megan Fox",
                                    Sex = Sex.Female
                                },
                                new Actor()
                                {   Name = "Peter Cullen",
                                    Sex = Sex.Male
                                }
                    }},

                    new Movie() {Title = "Wind That Shakes The Barley", ReleaseDate = DateTime.Parse("23/06/2006"),
                    Director = context.Directors.Where(d=>d.FirstName=="Ken").FirstOrDefault(),
                    Genre = Genre.War

                    },

                    new Movie() {Title = "The Shawshank Redemption", ReleaseDate = DateTime.Parse("14/10/1994"),
                    Director = context.Directors.Where(d=>d.FirstName=="Frank").FirstOrDefault(),
                    Genre = Genre.SciFi

                    }

            };

            movies.ForEach(mov => context.Movies.Add(mov));
            context.SaveChanges();

            base.Seed(context);
        }
    }

    public class MovieDb : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public MovieDb()
            : base("MovieDb")
        { }
    }
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        [Display(Name = "Movie Name"), Required]
        [StringLength(30, ErrorMessage = "Must be between " +
                       "{2} and {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }
        [DisplayName("Releasing"), DataType(DataType.Date),
                    DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DirectorID { get; set; }

        public virtual Director Director { get; set; }
        public virtual List<Actor> Actors { get; set; }
    }

    public enum Sex
    {
        Male, Female
    }

    public enum Genre
    {
        Drama, War, Comedy, SciFi, Documentary
    }

    public class Actor
    {
        public int ActorID { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }

        public object Actors { get; set; }
    }

    public class Director
    {
        [Key]
        public int DirectorID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }
    }
}