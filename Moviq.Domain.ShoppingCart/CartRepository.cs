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
using Moviq.Interfaces;


namespace Moviq.Domain.Products
{
   public class CartRepository : IRepository<ICartItem>, ICartRepository
    {
        string dataType;
        string keyPattern;
        ICouchbaseClient db;
        ILocale locale;
        string searchUrl;

         public CartRepository(ICouchbaseClient db, IFactory<ICartItem> userFactory, ILocale locale, string searchUrl) 
        {
            this.db = db;
            this.dataType = ((IHelpCategorizeNoSqlData)userFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, ":c:{0}");
            this.locale = locale;
            this.searchUrl = searchUrl;
        }        

        public ICartItem GetByGuid(Guid guid)
         {
             return db.GetJson<CartItem>(String.Format(keyPattern, guid.ToString()));
         }

        public ICartItem GetByUserId(string cartId)
        {
            return db.GetJson<CartItem>(String.Format(keyPattern, cartId.ToString()));            
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

            var mainKey = String.Format(keyPattern, item.CartId.ToString());
            bool cartExist = db.KeyExists(mainKey);
            if (cartExist)
            {
                ICartItem itemOld = GetByUserId(item.CartId.ToString());
                itemOld.Uid = itemOld.Uid + "," + item.Uid;
                itemOld.Price = itemOld.Price + item.Price;
                if (SetCart(itemOld, mainKey))
                {
                    return Get(itemOld.CartId.ToString());
                }
                else
                {
                    throw new Exception(locale.UserSetFailure);
                }
            }
            else
            {
                if (SetCart(item, mainKey))
                {
                    return Get(item.CartId.ToString());
                }
                else
                {
                    throw new Exception(locale.UserSetFailure);
                }
            }
           
        }


        private bool SetCart(ICartItem item, string mainKey)
        {
           // var lookupByCartTitle = String.Format(keyPattern, item.Title);

            //return db.StoreJson(StoreMode.Set, lookupByCartTitle, mainKey)
              return db.StoreJson(StoreMode.Set, mainKey, item);
            
        }
        public ICartItem Remove(ICartItem item)
        {
            var mainKey = String.Format(keyPattern, item.CartId.ToString());
            bool cartExist = db.KeyExists(mainKey);
            if (cartExist)
            {
                ICartItem itemOld = GetByUserId(item.CartId.ToString());
                char[] charSeq = { ',' };
                string[] array = itemOld.Uid.Split(charSeq);
                string resultUid = "";
                foreach(string str in array){
                    if (!str.Equals(item.Uid))
                        resultUid = resultUid + str + ",";                   
                }
                if (!resultUid.Equals("")) {
                    resultUid = resultUid.Substring(0, resultUid.LastIndexOf(","));
                    itemOld.Uid = resultUid;
                    itemOld.Price = itemOld.Price - item.Price;
                    if (SetCart(itemOld, mainKey))
                    {
                        return Get(itemOld.CartId.ToString());
                    }
                    else
                    {
                        throw new Exception(locale.UserSetFailure);
                    }
                }               
            }
            return null;
        }
        public ICartItem EmptyCart(string userId)
        {
            var mainKey = String.Format(keyPattern, userId.ToString());
            bool cartExist = db.KeyExists(mainKey);
            if (cartExist)
            {
                ICartItem itemOld = GetByUserId(userId);                  
                    itemOld.Uid = "";
                    itemOld.Price = 0;
                    if (SetCart(itemOld, mainKey))
                    {
                        return Get(itemOld.CartId.ToString());
                    }
                    else
                    {
                        throw new Exception(locale.UserSetFailure);
                    }
                
            }
            return null;
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
