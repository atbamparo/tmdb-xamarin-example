using System;
using System.Threading.Tasks;
using TMDbExample.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace TMDbExample.Forms.ViewModels
{
    public class Paginator<T>
    {
        private readonly  Func<int, Task<Page<T>>> _fetchPageFunc;
        private int _totalPages;
        private int _currentPage;

        private bool HasMorePages => _totalPages >= _currentPage;

        public Paginator(Func<int, Task<Page<T>>> fetchPageFunc)
        {
            _fetchPageFunc = fetchPageFunc;
            ResetPages();
        }

        public void ResetPages()
        {
            _totalPages = 1;
            _currentPage = 1;
        }

        public async Task<IEnumerable<T>> GetPageAsync()
        {
            if (!HasMorePages)
            {
                return Enumerable.Empty<T>();
            }

            var moviePage = await _fetchPageFunc(_currentPage);
            _currentPage++;
            _totalPages = moviePage.TotalPages;
            return moviePage.Results;
        }
    }
}