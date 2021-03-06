﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using Newtonsoft.Json;
using NBPMuzika.Models.Entiteti;

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
            var vrednosti = db.ListRange("recent", 0, 100);
            Dictionary<int, string> ret = new Dictionary<int, string>();
            int j = 0;
            for (int i = 0; i < vrednosti.Length; i++)
            {
               
                string zaParsiranje = vrednosti[i];
                string[] temp = zaParsiranje.Split('.');
                if (!ret.ContainsKey(Int32.Parse(temp[0])))
                {
                    ret.Add(Int32.Parse(temp[0]), temp[1]);
                    j++;
                }
                if (j > 4)
                    break;
            }
            return ret;
        }
        #endregion

#region KesiranjePretrage
        

        //public void KesirajAlbume(string query, List<Album> vrednosti)
        //{
        //    var db = redis.GetDatabase();
        //    db.StringSet(query + ".Album", JsonConvert.SerializeObject(vrednosti), new TimeSpan(0,5,0));
        //}

        //public List<Album> KesiraniAlbumi(string query)
        //{
        //    var db = redis.GetDatabase();
        //    var vrednost = db.StringGet(query + ".Album");
        //    if (!vrednost.IsNull)
        //        return JsonConvert.DeserializeObject<List<Album>>(vrednost);
        //    else
        //        return null;
        //}

        //public void KesirajBend(string query,List<Bend> vrednosti)
        //{
        //    var db = redis.GetDatabase();
        //    db.StringSet(query + ".Bend", JsonConvert.SerializeObject(vrednosti), new TimeSpan(0, 5, 0));
        //}

        //public List<Bend> KesiraniBendovi(string query)
        //{
        //    var db = redis.GetDatabase();
        //    var vrednost = db.StringGet(query + ".Bend");
        //    if (!vrednost.IsNull)
        //        return JsonConvert.DeserializeObject<List<Bend>>(vrednost);
        //    else
        //        return null;
        //}

        //public void KesirajIzvodjaca(string query,List<Izvodjac> vrednosti)
        //{
        //    var db = redis.GetDatabase();
        //    db.StringSet(query + ".Izvodjac", JsonConvert.SerializeObject(vrednosti), new TimeSpan(0, 5, 0));
        //}

        //public List<Izvodjac> KesiraniIzvodjaci(string query)
        //{
        //    var db = redis.GetDatabase();
        //    var vrednost = db.StringGet(query + ".Izvodjac");
        //    if (!vrednost.IsNull)
        //        return JsonConvert.DeserializeObject<List<Izvodjac>>(vrednost);
        //    else
        //        return null;
        //}

        //public void KesirajProducente(string query,List<Producent> vrednosti)
        //{
        //    var db = redis.GetDatabase();
        //    db.StringSet(query + ".Producent", JsonConvert.SerializeObject(vrednosti), new TimeSpan(0, 5, 0));
        //}

        //public List<Producent> KesiraniProducenti(string query)
        //{
        //    var db = redis.GetDatabase();
        //    var vrednost = db.StringGet(query + ".Producent");
        //    if (!vrednost.IsNull)
        //        return JsonConvert.DeserializeObject<List<Producent>>(vrednost);
        //    else
        //        return null;
        //}

        public void KesirajRezultat(string query, Res vrednosti, int pagenum)
        {
            var db = redis.GetDatabase();
            //string zaKesiranje = query;
            //if (zaKesiranje != null)
            //    db.StringSet("queryPretraga", query);
            //if (zaKesiranje == null)
            //    zaKesiranje = db.StringGet("queryPretraga");
            db.StringSet(query + "." + pagenum, JsonConvert.SerializeObject(vrednosti.p), new TimeSpan(0, 5, 0));
            db.StringSet(query + "." + pagenum + ".count", vrednosti.count, new TimeSpan(0, 5, 0));
        }

        public Res KesiraneVrednosti(string query, int pagenum)
        {
            var db = redis.GetDatabase();
            //string rezultat = query;
            //if (rezultat == null)
            //    rezultat = db.StringGet("queryPretraga");
            Res vrednosti = new Res();
            var rez = db.StringGet(query + "." + pagenum);
            var c = db.StringGet(query + "." + pagenum + ".count");
            if (!rez.IsNull)
            {
                vrednosti.p = JsonConvert.DeserializeObject<List<Pretraga>>(rez);
                vrednosti.count = Int32.Parse(c);
                return vrednosti;
            }
            else
                return null;
        }

        #endregion

    }
}