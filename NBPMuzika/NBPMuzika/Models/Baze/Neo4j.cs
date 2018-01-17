using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPMuzika.Models.Entiteti;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace NBPMuzika.Models.Baze
{
    public class Neo4j
    {
        GraphClient client;

        public Neo4j() {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "1234");
            try
            {
                client.Connect();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        Neo4jClient.Cypher.CypherQuery vratiRezultat(string query)
        {
            return new Neo4jClient.Cypher.CypherQuery(query, new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);
        }

        #region Rezultati pretrazivanja
        //vraca sve elemente koji sadrze string input u sebu (case-insensitive)
        public List<Album> vratiAlbumeSearch(/*GraphClient client,*/ string input)
        {

            string query = "match (n:Album) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }
        //public List<Bend> vratiBendoveSearch(/*GraphClient client,*/ string input)
        //{

        //    string query = "match (n:Bend) where n.name=~'.*(?i)" + input + ".*' return n";
        //    var qu = vratiRezultat(query);
        //    List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
        //    return vrati;
        //}
        //public List<Izvodjac> vratiIzvodjaceSearch(/*GraphClient client,*/ string input)
        //{

        //    string query = "match (n:Izvodjac) where n.name=~'.*(?i)" + input + ".*' return n";
        //    var qu = vratiRezultat(query);
        //    List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
        //    return vrati;
        //}
        //public List<Pesma> vratiPesmeSearch(/*GraphClient client,*/ string input)
        //{

        //    string query = "match (n:Pesma) where n.name=~'.*(?i)" + input + ".*' return n";
        //    var qu = vratiRezultat(query);
        //    List<Pesma> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Pesma>(qu).ToList();
        //    return vrati;
        //}
        //public List<Producent> vratiProducentSearch(/*GraphClient client,*/ string input)
        //{

        //    string query = "match (n:Producent) where n.name=~'.*(?i)" + input + ".*' return n";
        //    var qu = vratiRezultat(query);
        //    List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
        //    return vrati;
        //}
        public Res vratiPretraga(/*GraphClient client,*/ Strana input)
        {

            ////string query = "match (n) where n.name=~'.*(?i)" + input.search + ".*' and not n:Pesma return n.id as Id, n.name as Name, labels(n)[0] as type";
            var qu = client.Cypher.Match("(n)")
                .Where("n.name=~'.*(?i)" + input.Pretraga + ".*' and not n:Pesma")
                //.Return((n) => n.As<Pretraga>());
                .Return(n => new
                {
                    id = Return.As<long>("n.id"),
                    name = Return.As<string>("n.name"),
                    tip = n.Labels()
                });
            //var qu = query(input);
            int c = qu.Results.Count();
            qu=qu.Skip(input.Offset).Limit(input.Limit);
            var rez = qu.Results.Select(x => new Pretraga
            {
                id = x.id,
                name = x.name,
                type=x.tip.FirstOrDefault()
            }).ToList<Pretraga>();
            

            return new Res {count=c,p=(List<Pretraga>)rez};
        }

        public List<Pretraga> vratiPoc(Strana input)
        {
            var qu = client.Cypher.Match("(n)")
               .Where("n.name=~'.*(?i)" + input.Pretraga + ".*' and not n:Pesma")
               //.Return((n) => n.As<Pretraga>());
               .Return(n => new
               {
                   id = Return.As<long>("n.id"),
                   name = Return.As<string>("n.name"),
                   tip = n.Labels()
               }).OrderByDescending("n.id").Limit(7);
            var rez = qu.Results.Select(x => new Pretraga
            {
                id = x.id,
                name = x.name,
                type = x.tip.FirstOrDefault()
            }).ToList<Pretraga>();
            return rez;
        }

        //public var query(Strana input)
        //{
        //    //string query = "match (n) where n.name=~'.*(?i)" + input.search + ".*' and not n:Pesma return n.id as Id, n.name as Name, labels(n)[0] as type";
        //    var qu = client.Cypher.Match("(n)")
        //        .Where("n.name=~'.*(?i)" + input.Pretraga + ".*' and not n:Pesma")
        //        //.Return((n) => n.As<Pretraga>());
        //        .Return(n => new
        //        {
        //            id = Return.As<long>("n.id"),
        //            name = Return.As<string>("n.name"),
        //            tip = n.Labels()
        //        });
        //    return qu;
        //}
        #endregion


        #region Prikaz

        public List<Pesma> svePesmeAlbuma(/*GraphClient client,*/ int id)
        {

            string query = "match(a:Album) where a.id=" + id + " match(p:Pesma) where (p)-[:DEO]->(a) return p";
            var qu = vratiRezultat(query);
            List<Pesma> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Pesma>(qu).ToList();
            return vrati;
        }

        public List<Izvodjac> sviClanoviGrupe(/*GraphClient client,*/ int id)
        {

            string query = "match(b:Bend) where b.id=" + id + " match(i:Izvodjac) where (i)-[:CLAN]->(b) return i";
            var qu = vratiRezultat(query);
            List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
            return vrati;
        }

        public List<Album> sviAlbumiGrupe(/*GraphClient client,*/ int id)
        {
            string query = "match(b:Bend) where b.id=" + id + " match(a:Album) where (b)-[:IZDAO]->(a) return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Producent> sviProducentiAlbuma(/*GraphClient client,*/ int id)
        {
            string query = "match(a:Album) where a.id=" + id + " match(p:Producent) where (p)-[:PRODUCIRAO]->(a) return p";
            var qu = vratiRezultat(query);
            List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
            return vrati;
        }

        public List<Album> sviAlbumiProducenta(/*GraphClient client,*/ int id)
        {
            string query = "match(p:Producent) where p.id=" + id + " match(a:Album) where (p)-[:PRODUCIRAO]->(a) return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Bend> bendKojiJeIzdao(/*GraphClient client,*/ int id)
        {
            string query = "match (a:Album) where a.id=" + id + " match (b:Bend) where (b)-[:IZDAO]->(a) return b";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }

        public List<Bend> bendoviIzvodjaca(int id)
        {
            string query = "match (i:Izvodjac) where i.id=" + id + " match (b:Bend) where (i)-[:CLAN]->(b) return b";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }

        public List<Album> sviAlbumiIzvodjaca(int id)
        {
            string query = "match(i:Izvodjac) where i.id=" + id + " match(a:Album) where (i)-[:IZDAO]->(a) return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }
        
        public List<Album> vratiAlbumPoID(/*GraphClient client,*/ int id)
        {
            string query = "match (a:Album) where a.id=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Bend> vratiBendPoID(/*GraphClient client,*/ int id)
        {
            string query = "match (a:Bend) where a.id=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }

        public List<Izvodjac> vratiIzvodjacaPoID(/*GraphClient client,*/ int id)
        {
            string query = "match (a:Izvodjac) where a.id=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
            return vrati;
        }

        public List<Producent> vratiProducentaPoID(/*GraphClient client,*/ int id)
        {
            string query = "match (a:Producent) where a.id=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
            return vrati;
        }

        #endregion
    }
}