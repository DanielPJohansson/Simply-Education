using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminClient.Services
{
    public class SelectListBuilder
    {
        private readonly string _baseUrl;

        public SelectListBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<List<SelectListItem>> PopulateCategorySelectListAsync()
        {
            var categoriesUrl = $"{_baseUrl}/categories/list";
            using var http = new HttpClient();

            var categoriesResponseModel = await http.GetFromJsonAsync<ResponseViewModel>(categoriesUrl);
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(categoriesResponseModel!.Data);

            if (categories is not null)
            {
                return categories.Select(cat => new SelectListItem
                {
                    Value = cat.Name,
                    Text = cat.Name
                }).ToList();
            }

            return new List<SelectListItem>();
        }
    }
}