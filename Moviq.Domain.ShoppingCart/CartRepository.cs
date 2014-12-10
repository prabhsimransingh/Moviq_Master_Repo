using Couchbase;
using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;


namespace Moviq.Domain.Products
{
   public class CartRepository : IRepository<ICartItem>, ICartRepository
    {
      

         public CartRepository(ICouchbaseClient db, IFactory<ICartItem> userFactory, ILocale locale, string searchUrl) 
        {
            this.db = db;
            this.dataType = ((IHelpCategorizeNoSqlData)userFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, "::{0}");
            this.locale = locale;
            this.searchUrl = searchUrl;
        }

         string dataType;
         string keyPattern;
         ICouchbaseClient db;
         ILocale locale;
         string searchUrl;

        public ICartItem GetByGuid(Guid guid)
         {
             return db.GetJson<CartItem>(String.Format(keyPattern, guid.ToString()));
         }

        public IEnumerable<CartItem> GetByUserId(string userId)
        {
            return db.GetJson<IEnumerable<CartItem>>(String.Format(keyPattern, userId.ToString()));
        }

        public ICartItem Get(string guid)
        {
            return db.GetJson<CartItem>(String.Format(keyPattern, guid.ToString()));
        }

        public IEnumerable<ICartItem> Get(IEnumerable<string> keys)
        {
            if (!keys.Any())
                yield break;

            var _results = db.Get(keys).Where(o => o.Value != null).Select(o => o.Value);

            if (!_results.Any())
                yield break;

            foreach (var o in _results)
                yield return JsonConvert.DeserializeObject<CartItem>(o.ToString());
        }

        public ICartItem Set(ICartItem item)
        {
            if (item.Guid == Guid.Empty) 
            {
                item.Guid = Guid.NewGuid();
            }

            var mainKey = String.Format(keyPattern, item.Guid.ToString());
            if (SetCart(item, mainKey))
            {
                return Get(item.Guid.ToString());
            }
            else
            {
                throw new Exception(locale.UserSetFailure);
            }
        }


        private bool SetCart(ICartItem item, string mainKey)
        {
           // var lookupByCartTitle = String.Format(keyPattern, item.Title);

            //return db.StoreJson(StoreMode.Set, lookupByCartTitle, mainKey)
              return db.StoreJson(StoreMode.Set, mainKey, item);
        }

        private bool SetUserId(IUser user, string mainKey)
        {
            var countKey = String.Format(keyPattern, "count");
            var id = db.Increment(countKey, 1, 1);
            var lookupById = String.Format(keyPattern, id.ToString());

            return db.StoreJson(StoreMode.Set, lookupById, mainKey);
        }

        public IEnumerable<ICartItem> List(int take, int skip)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!

            // http://localhost:8092/moviq/_design/dev_books/_view/books?stale=false&connection_timeout=60000&limit=20&skip=0
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public Task<IEnumerable<ICartItem>> Find(string searchBy)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!

            // http://localhost:8092/moviq/_design/dev_books/_view/books?stale=false&connection_timeout=60000&limit=20&skip=0
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public bool Delete(string guid)
        {
            return false;
        }

        public void Dispose()
        {
            // don't dispose the db - it's a singleton
        }

      
    }
}
