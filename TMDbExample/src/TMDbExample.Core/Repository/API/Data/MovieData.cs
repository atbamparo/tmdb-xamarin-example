using System;
using System.Collections.Generic;

namespace TMDbExample.Core.Repository.API.Data
{
    public class MovieData
    {
        public string Id { get; set; }

        public bool Adult { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; }

        public string Overview { get; set; }

        public DateTime ReleaseDate { get; set; }

        public CollectionData BelongsToCollection { get; set; }

        public long Budget { get; set; }

        public List<GenreData> Genres { get; set; }

        public Uri Homepage { get; set; }

        public string ImdbId { get; set; }

        public string OriginalLanguage { get; set; }

        public string OriginalTitle { get; set; }

        public double Popularity { get; set; }

        public List<CompanyData> ProductionCompanies { get; set; }

        public List<CountryData> ProductionCountries { get; set; }

        public long Revenue { get; set; }

        public long Runtime { get; set; }

        public List<LanguageData> SpokenLanguages { get; set; }

        public string Status { get; set; }

        public string Tagline { get; set; }

        public string Title { get; set; }

        public bool Video { get; set; }

        public double VoteAverage { get; set; }

        public long VoteCount { get; set; }
    }
}
