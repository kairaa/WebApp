﻿using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using Services.Abstract;
using Shared.Utilities.Results.ComplexTypes;

namespace Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var searchResult = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            if(searchResult.ResultStatus == ResultStatus.Success)
            {
                return View (new ArticleSearchViewModel
                {
                    ArticleListDto = searchResult.Data,
                    //keyword'ün tutulma sebebi, arama sonucunda dönen makaleler için de sayfalama yapılmasıdır
                    Keyword = keyword
                });
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int articleId)
        {
            var articleResult = await _articleService.GetAsync(articleId);
            if(articleResult.ResultStatus == ResultStatus.Success)
            {
                var userArticles = await _articleService.GetAllByUserIdOnFilter(articleResult.Data.Article.UserId,
                    Entities.ComplexTypes.FilterBy.Category, Entities.ComplexTypes.OrderBy.Date, false, 10, articleResult.Data.Article.CategoryId,
                    DateTime.Now, DateTime.Now, 0, 100000, 0, 100000);
                await _articleService.IncreaseViewCountAsync(articleId);
                return View(new ArticleDetailViewModel
                {
                    ArticleDto = articleResult.Data,
                    ArticleDetailRightSideBarViewModel = new ArticleDetailRightSideBarViewModel
                    {
                        ArticleListDto = userArticles.Data,
                        Header = "Kullanıcının Aynı Kategori Üzerindeki En Çok Okunan Makaleleri",
                        User = articleResult.Data.Article.User
                    }
                });
            }
            return NotFound();
        }
    }
}
