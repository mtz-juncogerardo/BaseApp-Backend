using System;
using System.Security.Cryptography.X509Certificates;
using BaseApp.Core.Helpers;
using BaseApp.Data.Requests;

namespace BaseApp.Core.Services.CommonServices
{
    public static class PriceService
    {
        public static void ValidatePrices(ArticleRequest article)
        {
            if (article.Price <= decimal.Zero || article.Discount < decimal.Zero || article.Discount >= article.Price)
            {
                CustomException.Throw("Los precios dan resultados negativos o totales a cero", 400);
            };
        }
    }
}