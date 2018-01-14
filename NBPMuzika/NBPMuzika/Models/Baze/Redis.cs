using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;

namespace NBPMuzika.Models.Baze
{
    public class Redis
    {
        ConnectionMultiplexer redis;
        public Redis()
        {

            redis = ConnectionMultiplexer.Connect("localhost");
        }

#region TopIPopularneVrednosti
        //format ID-ja u Redis bazi je (ID elementa iz baze).(Naziv elementa iz baze)/(tip elementa)  (bez zagrada)
        public void IncrementTopPages(string id)
        {
            var db = redis.GetDatabase();
                db.SortedSetIncrement("top", id, 1);
        }

        //format ID-ja u Redis bazi je (ID elementa iz baze).(Naziv elementa iz baze)/(tip elementa)   (bez zagrada)
        public void UpdateRecentPages(string id)
        {
            var db = redis.GetDatabase();
            //gledaju se samo top 5 skorih linkova
            if (!IDInSet(id, "recent", 5) && db.ListLength("recent") == 5)
            {
                db.ListLeftPush("recent", id);
                db.ListRightPop("recent");
            }
            else if(!IDInSet(id,"recent",2))
                db.ListLeftPush("recent", id);
        }

        bool IDInSet(string id,string setName,int iterationNum)
        {
            var db = redis.GetDatabase();
            long setlength = db.ListLength(setName);
            long l = setlength > iterationNum ? iterationNum : setlength;
            for(int i=0;i<l;i++)
            {
                if (db.ListGetByIndex(setName, i) == id)
                    return true;
            }
            return false;
        }

        //Dictionary klasa zato sto vracamo i ID i naziv (da ne bi kontaktirali bazu za naziv cvora)
        public Dictionary<int,string> vratiTopID(int broj)
        {
            var db = redis.GetDatabase();
            var vrednosti = db.SortedSetRangeByRank("top", 0, broj, Order.Descending);
            Dictionary<int, string> ret = new Dictionary<int, string>();
            for(int i = 0; i < vrednosti.Length; i++)
            {
                string zaParsiranje = vrednosti[i];
                string[] temp = zaParsiranje.Split('.');
                ret.Add(Int32.Parse(temp[0]),temp[1]);
            }
            return ret;
        }


        //gledaju se samo 5 najskorijih linkova
        //Dictionary klasa zato sto vracamo i ID i naziv (da ne bi kontaktirali bazu za naziv cvora)
        public Dictionary<int,string> vratiRecentID(/*int broj*/)
        {
            var db = redis.GetDatabase();
            var vrednosti = db.ListRange("recent", 0, 4);
            Dictionary<int, string> ret = new Dictionary<int, string>();
            for (int i = 0; i < vrednosti.Length; i++)
            {
                string zaParsiranje = vrednosti[i];
                string[] temp = zaParsiranje.Split('.');
                ret.Add(Int32.Parse(temp[0]), temp[1]);
            }
            return ret;
        }
        #endregion

#region KesiranjePretrage



#endregion

    }
}